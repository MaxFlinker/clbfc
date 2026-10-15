using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace clbfc
{
    public class CommandLineHandler
    {
        private static string _suggestedFilename = string.Empty;

        public static int Run(string[] args)
        {
            var flags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var positional = new List<string>();

            foreach (var arg in args)
            {
                if (arg.StartsWith("-", StringComparison.Ordinal))
                    flags.Add(arg);
                else
                    positional.Add(arg);
            }

            // Help
            if (flags.Contains("--help") ||
    flags.Contains("-help") ||
    flags.Contains("-h") ||
    flags.Contains("--h") ||
    flags.Contains("-?") ||
    flags.Contains("/?"))
            {
                PrintHelp();
                return 0;
            }

            // Version (standalone or combined with -c)
            if (flags.Contains("-version"))
            {
                Console.WriteLine("Command Line Batch FileCreator (clbfc)");
                Console.WriteLine($"Version: {Program.Version}");
                return 0;
            }

            bool create = flags.Contains("-c");
            bool fromInput = flags.Contains("-fi");
            bool fromFile = flags.Contains("-fs");
            bool openFile = flags.Contains("-o");

            if (!create)
            {
                Console.WriteLine("Invalid arguments. The -c flag is required.");
                return 1;
            }

            // ============================
            // MASK MODE (-fs)
            // ============================
            if (fromFile)
            {
                if (positional.Count < 1)
                {
                    Console.WriteLine("Need <mask-file>.");
                    return 1;
                }

                string maskFile = positional[0];

                if (!File.Exists(maskFile))
                {
                    Console.WriteLine($"File not found: {maskFile}");
                    return 1;
                }

                string outputDir = positional.Count > 1 ? positional[1] : Directory.GetCurrentDirectory();
                string fullOutputDir = Path.GetFullPath(outputDir);
                Directory.CreateDirectory(fullOutputDir);

                string maskText = File.ReadAllText(maskFile);
                return ExecuteMask(maskText, fullOutputDir);
            }

            // ============================
            // FI MODE OPEN (-fi -o)
            // ============================
            if (fromInput && openFile)
            {
                if (positional.Count < 1 || positional.Count > 2)
                {
                    Console.WriteLine(
                        "Usage: clbfc -c -fi -o <input-file> [output-directory]");
                    return 1;
                }

                string inputFile = Path.GetFullPath(positional[0]);

                string outputDir = positional.Count == 2
                    ? Path.GetFullPath(positional[1])
                    : Directory.GetCurrentDirectory();

                if (!File.Exists(inputFile))
                {
                    Console.WriteLine($"Input file not found: {inputFile}");
                    return 1;
                }

                Directory.CreateDirectory(outputDir);

                SetSuggestedFilename(inputFile);

                string content = File.ReadAllText(inputFile);

                return FiMode.Run(
                    Program.Version,
                    null,
                    content,
                    outputDir);
            }

            // ============================
            // FI MODE NORMAL (-fi)
            // ============================

            if (fromInput && positional.Count == 0)
            {
                return FiMode.Run(Program.Version, null, null, null);
            }

            if (fromInput)
            {
                if (positional.Count > 2)
                {
                    Console.WriteLine(
                        "Usage: clbfc -c -fi [input-file] [output-directory]");
                    return 1;
                }

                // One argument: open the input file in the editor.
                if (positional.Count == 1)
                {
                    string inputFile = Path.GetFullPath(positional[0]);

                    if (!File.Exists(inputFile))
                    {
                        Console.WriteLine($"Input file not found: {inputFile}");
                        return 1;
                    }

                    SetSuggestedFilename(inputFile);

                    string content = File.ReadAllText(inputFile);

                    return FiMode.Run(
                        Program.Version,
                        null,
                        content,
                        Directory.GetCurrentDirectory());
                }

                // Two arguments: input file + output directory.
                string fullOutputDir = Path.GetFullPath(positional[1]);
                string fullInputFile = Path.GetFullPath(positional[0]);

                if (!File.Exists(fullInputFile))
                {
                    Console.WriteLine($"Input file not found: {fullInputFile}");
                    return 1;
                }

                Directory.CreateDirectory(fullOutputDir);

                SetSuggestedFilename(fullInputFile);

                string fileContent = File.ReadAllText(fullInputFile);

                return FiMode.Run(
                    Program.Version,
                    null,
                    fileContent,
                    fullOutputDir);
            }
        
        Console.WriteLine("Invalid arguments. Use clbfc --help for help.");
        return 1;
    }

        public static void PrintHelp()
        {
            Console.WriteLine("Command Line Batch FileCreator (clbfc)");
            Console.WriteLine($"Version: {Program.Version}");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  bfc -c -fi <input1> [input2 ...] <outputdir> [--lang xx]");
            Console.WriteLine("  bfc -c -fi");
            Console.WriteLine("  bfc -c -fs <maskfile> [outputdir] [--lang xx]");
            Console.WriteLine();
            Console.WriteLine("Modes:");
            Console.WriteLine("  -c                Create mode (required)");
            Console.WriteLine("  -fi               Use to paste multiline Folderstructures into fiMode");
            Console.WriteLine("  -fi -o            Open a .mask or any file in fiMode");
            Console.WriteLine("  -fs               Create structure from a mask/script file");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  --lang xx         Force language (for example: en, de, tr, sa, zh)");
            Console.WriteLine("  -version          Show version");
            Console.WriteLine("  -h, --help, -?, /? Show this help");
            Console.WriteLine();
            Console.WriteLine(@"  bfc -c -fi file1.txt folder1 output");
            Console.WriteLine(@"  bfc -c -fs structure.mask output --lang de");
        }

        public static int ExecuteMask(string maskText, string outputDir)
        {
            var lines = maskText.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (var raw in lines)
            {
                string line = raw.Trim();

                if (line.Length == 0)
                    continue;

                // DIRECTORY RULE
                if (line.EndsWith("/"))
                {
                    string dirPath = Path.Combine(outputDir, line.TrimEnd('/'));
                    Directory.CreateDirectory(dirPath);
                    continue;
                }

                // FILE RULE
                if (line.Contains('.') && !line.EndsWith("/"))
                {
                    string filePath = Path.Combine(outputDir, line);

                    string? parentDir = Path.GetDirectoryName(filePath);
                    if (parentDir != null)
                        Directory.CreateDirectory(parentDir);

                    File.WriteAllText(filePath, string.Empty);
                    continue;
                }

                // UNCLEAR ENTRY
                Console.WriteLine($"Unclear entry in mask: \"{line}\"");
                Console.WriteLine("Rules:");
                Console.WriteLine(" - directory: ends with '/'");
                Console.WriteLine(" - file: contains '.' but does not end with '/'");
                return 1;
            }

            Console.WriteLine("Mask mode (-fs) completed successfully.");
            return 0;
        }

        public static void EnsureOutputDirectory(string outputDir)
        {
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
        }

        public static void SetSuggestedFilename(string name)
        {
            _suggestedFilename = name;
        }
    }
}