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
            if (flags.Contains("--h") || flags.Contains("-help"))
            {
                PrintHelp(flags.Contains("-help"));
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
            if (fromInput)
            {
                if (positional.Count < 2)
                {
                    Console.WriteLine("Missing arguments. Specify at least one input and an output directory.");
                    return 1;
                }

                string outputDir = positional[^1];
                var inputItems = positional.Take(positional.Count - 1).ToList();

                string fullOutputDir = Path.GetFullPath(outputDir);
                Directory.CreateDirectory(fullOutputDir);

                foreach (string item in inputItems)
                {
                    string fullItemPath = Path.GetFullPath(item);

                    if (Directory.Exists(fullItemPath))
                    {
                        string folderName = new DirectoryInfo(fullItemPath).Name;
                        string targetDir = Path.Combine(fullOutputDir, folderName);

                        Directory.CreateDirectory(targetDir);

                        foreach (string file in Directory.GetFiles(fullItemPath, "*", SearchOption.AllDirectories))
                        {
                            string relativePath = Path.GetRelativePath(fullItemPath, file);
                            string targetFile = Path.Combine(targetDir, relativePath);

                            Directory.CreateDirectory(Path.GetDirectoryName(targetFile)!);
                            File.Copy(file, targetFile, overwrite: true);
                        }

                        continue;
                    }

                    if (File.Exists(fullItemPath))
                    {
                        string targetFile = Path.Combine(fullOutputDir, Path.GetFileName(fullItemPath));
                        File.Copy(fullItemPath, targetFile, overwrite: true);
                        continue;
                    }

                    Console.WriteLine($"Error: File or folder does not exist: {item}");
                    return 1;
                }

                Console.WriteLine("Input mode (-fi) completed successfully.");
                return 0;
            }

            Console.WriteLine("Invalid arguments. Use bfc -help for help.");
            return 1;
        }

        static void PrintHelp(bool full)
        {
            Console.WriteLine("Usage: bfc -c -fi <input1> [input2 ...] <output-directory>");
            Console.WriteLine("       bfc -c -fi -o <file>");
            Console.WriteLine("       bfc -c -fi");
            Console.WriteLine("       bfc -c -fs <mask-file> [output-directory]");
            Console.WriteLine();
            if (!full)
            {
                Console.WriteLine("Use bfc -help for full manual.");
                return;
            }
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