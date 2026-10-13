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
        var stack = new List<(int Depth, string Path)>();

        foreach (var raw in lines)
        {
            if (string.IsNullOrWhiteSpace(raw))
                continue;

            string line = raw.TrimEnd();

            // Tree-Zeilen anhand der Darstellung zerlegen.
            int marker = line.IndexOf("├──", StringComparison.Ordinal);
            int lastMarker = line.IndexOf("└──", StringComparison.Ordinal);

            if (lastMarker >= 0 && (marker < 0 || lastMarker < marker))
                marker = lastMarker;

            int depth;
            string name;

            if (marker >= 0)
            {
                string prefix = line[..marker];
                name = line[(marker + 3)..].Trim();

                // Jede vollständige "│   "-Gruppe entspricht einer Ebene.
                depth = 1;
                for (int i = 0; i + 3 < prefix.Length; i += 4)
                {
                    if (prefix.AsSpan(i).StartsWith("│   "))
                        depth++;
                    else if (prefix.AsSpan(i).StartsWith("    "))
                        depth++;
                }
            }
            else
            {
                // Wurzeleintrag, z. B. mfboxsys/
                name = line.Trim();
                depth = 0;
            }

            if (string.IsNullOrWhiteSpace(name))
                continue;

            // Vorherige Ebenen verlassen.
            while (stack.Count > 0 && stack[^1].Depth >= depth)
                stack.RemoveAt(stack.Count - 1);

            string relativePath = stack.Count == 0
                ? name
                : Path.Combine(stack[^1].Path, name);

            bool isDirectory =
                name.EndsWith("/", StringComparison.Ordinal) ||
                name.EndsWith("\\", StringComparison.Ordinal);

            if (isDirectory)
            {
                relativePath = relativePath.TrimEnd('/', '\\');
                result.Add(relativePath + Path.DirectorySeparatorChar);
                stack.Add((depth, relativePath));
            }
            else
            {
                result.Add(relativePath);
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
