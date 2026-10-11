using System;
using System.IO;

namespace clbfc;

    public static class StructureCreator
    {
        public static void CreateFromMask(string maskText, string targetFolder, string? forcedLang = null)
        {
            // string[] → List<string>
            var lines = maskText
                .Replace("\r", "")
                .Split('\n')
                .Select(x => x.Trim())
                .ToList();

            string format = StructureFormatDetector.DetectFormat(lines);

            List<string> paths;

            if (format == "TREE")
            {
                paths = ParseTreeStructure(lines);
            }
            else
            {
                paths = ParseMaskStructure(lines);
            }

            // Jetzt die Struktur erzeugen
            foreach (var p in paths)
            {
                if (p.EndsWith("/"))
                {
                    Directory.CreateDirectory(Path.Combine(targetFolder, p.TrimEnd('/')));
                }
                else
                {
                    string fullPath = Path.Combine(targetFolder, p);
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
                    File.WriteAllText(fullPath, "");
                }
            }
        }

    public static List<string> ParseTreeStructure(List<string> lines)
    {
        var result = new List<string>();
        var stack = new Stack<string>();

        foreach (var raw in lines)
        {
            if (string.IsNullOrWhiteSpace(raw))
                continue;

            string line = raw.Trim();

            // Entferne Tree-Symbole
            line = line
                .Replace("├───", "")
                .Replace("├──", "")
                .Replace("└───", "")
                .Replace("└──", "")
                .Replace("│", "")
                .Trim();

            if (string.IsNullOrWhiteSpace(line))
                continue;

            // Ordner oder Datei?
            bool isFile = line.Contains('.');

            // Hierarchie bestimmen
            int indent = raw.TakeWhile(c => c == ' ' || c == '\t').Count();

            while (stack.Count > indent)
                stack.Pop();

            string path = (stack.Count == 0)
                ? line
                : Path.Combine(stack.Peek(), line);

            if (!isFile)
            {
                // Ordner
                result.Add(path + "/");
                stack.Push(path);
            }
            else
            {
                // Datei
                result.Add(path);
            }
        }

        return result;
    }


    public static List<string> ParseMaskStructure(List<string> lines)
    {
        var result = new List<string>();

        foreach (var raw in lines)
        {
            if (string.IsNullOrWhiteSpace(raw))
                continue;

            string line = raw.Trim();

            // Entferne Tree-Symbole (falls versehentlich drin)
            line = line
                .Replace("├──", "")
                .Replace("└──", "")
                .Replace("│", "")
                .Replace("─", "")
                .Replace("—", "")
                .Trim();

            // Ordner
            if (line.EndsWith("/") || !line.Contains('.'))
            {
                string folder = line.TrimEnd('/', '\\');
                result.Add(folder + "/");
                continue;
            }

            // Datei
            if (line.Contains('.'))
            {
                result.Add(line);
                continue;
            }

            throw new Exception($"Unclear entry in mask: \"{raw}\"");
        }

        return result;
    }
    public static int ExecuteTree(List<string> paths, string? outputDirectory = null)
    {
        string outputDir = Path.GetFullPath(
            outputDirectory ?? Directory.GetCurrentDirectory());

        Directory.CreateDirectory(outputDir);

        foreach (string path in paths)
        {
            string relativePath = path.TrimEnd('/', '\\');

            if (string.IsNullOrWhiteSpace(relativePath))
                continue;

            string fullPath = Path.GetFullPath(
                Path.Combine(outputDir, relativePath));

            string rootPrefix = Path.EndsInDirectorySeparator(outputDir)
                ? outputDir
                : outputDir + Path.DirectorySeparatorChar;

            if (!fullPath.Equals(
                    outputDir,
                    StringComparison.OrdinalIgnoreCase)
                && !fullPath.StartsWith(
                    rootPrefix,
                    StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine(
                    $"Error: Path escapes output directory: {path}");
                return 1;
            }

            if (path.EndsWith("/") || path.EndsWith("\\"))
            {
                Directory.CreateDirectory(fullPath);
            }
            else
            {
                string? parent = Path.GetDirectoryName(fullPath);

                if (!string.IsNullOrEmpty(parent))
                    Directory.CreateDirectory(parent);

                // Don't overwrite existing files.
                if (!File.Exists(fullPath))
                    File.WriteAllText(fullPath, "");
            }
        }

        Console.WriteLine("Tree structure created successfully.");
        return 0;
    }

}
