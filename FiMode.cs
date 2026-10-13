using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace clbfc
{
    public static class FiMode
    {
        public static int Run(
    string version,
    string? forcedLang,
    string? openFile,
    string? outputDirectory = null)


        {
            var lines = new List<StringBuilder>();
            int currentLineIndex = 0;
            int cursorIndex = 0;
            int viewportTop = 0;
            const int EditorTop = 8;

            int EditorBottom()
            {
                return Console.WindowHeight - 2;
            }

            int EditorHeight()
            {
                return Math.Max(
                    1,
                    Console.WindowHeight - EditorTop - 1);
            }

            int SafeMessageRow()
            {
                return Math.Clamp(
                    EditorTop + lines.Count + 2,
                    0,
                    Console.BufferHeight - 1);
            }

            void SetCursorFromIndices()
            {
                int maxVisibleLines = EditorHeight();

                if (currentLineIndex < viewportTop)
                    viewportTop = currentLineIndex;

                if (currentLineIndex >= viewportTop + maxVisibleLines)
                    viewportTop = currentLineIndex - maxVisibleLines + 1;

                int lineRow = EditorTop + currentLineIndex - viewportTop;

                int col = Math.Min(
                    2 + cursorIndex,
                    Console.BufferWidth - 1);

                Console.SetCursorPosition(
                    col,
                    Math.Clamp(lineRow, EditorTop, EditorBottom()));
            }

            // Initial content
            if (!string.IsNullOrEmpty(openFile))
            {
                foreach (var rawLine in openFile.Split('\n'))
                    lines.Add(new StringBuilder(rawLine.Replace("\r", "")));
            }
            if (lines.Count == 0)
                lines.Add(new StringBuilder());

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            Console.WriteLine("Comman Line Batch FileCreator fi mode");
            Console.WriteLine();
            Console.WriteLine($"clbfc v. {version}");
            Console.WriteLine();

            Console.WriteLine("execute with Ctrl+R      leave with Ctrl+Q       save .using Ctrl+M" );
            Console.WriteLine("");
            Console.WriteLine($"current time/date: {DateTime.Now:HH:mm:ss dd.MM.yyyy}");
            Console.WriteLine();
            Console.WriteLine("-----File starts below-----");
            Console.WriteLine("####");
            void DrawStatusLine()
            {
                int bottom = Console.WindowHeight - 1;
                int cx = Console.CursorLeft;
                int cy = Console.CursorTop;

                Console.SetCursorPosition(0, bottom);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
                Console.Write("Ctrl+(R)un    Ctrl+(Q)uit    (M)ake/save file".PadRight(Console.BufferWidth));

                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(cx, cy);
            }

            void RedrawAll()
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;

                int editorHeight = EditorHeight();

                for (int screenRow = 0; screenRow < editorHeight; screenRow++)
                {
                    int fileRow = viewportTop + screenRow;

                    Console.SetCursorPosition(0, EditorTop + screenRow);

                    if (fileRow >= lines.Count)
                    {
                        Console.Write(
                            "".PadRight(Console.BufferWidth));
                        continue;
                    }

                    string text =
                        "> " + lines[fileRow].ToString();

                    if (text.Length > Console.BufferWidth)
                        text = text[..Console.BufferWidth];

                    Console.Write(
                        text.PadRight(Console.BufferWidth));
                }

                SetCursorFromIndices();
                DrawStatusLine();
            }

            RedrawAll();

            while (true)
            {
                var key = Console.ReadKey(intercept: true);

                // Exit
                if (key.Modifiers == ConsoleModifiers.Control && key.Key == ConsoleKey.Q)
                {
                    Console.SetCursorPosition(0, SafeMessageRow());
                    Console.WriteLine();
                    Console.WriteLine("Exiting fi mode.");
                    Console.Clear();
                    return 0;
                }

                // Save as .mask
                if (key.Modifiers == ConsoleModifiers.Control && key.Key == ConsoleKey.M)
                {
                    string suggested = $"bfc_input_{DateTime.Now:yyyyMMdd_HHmmss}.mask";
                    string filename = PromptEditable("Save as:", suggested);

                    var outLines = lines.Select(sb => sb.ToString()).ToArray();
                    File.WriteAllLines(filename, outLines);

                    Console.SetCursorPosition(0, SafeMessageRow());
                    Console.WriteLine($"Saved current input as {filename}");
                    DrawStatusLine();
                    SetCursorFromIndices();
                    continue;
                }

                // Execute
                if (key.Modifiers == ConsoleModifiers.Control && key.Key == ConsoleKey.R)
                {
                    // Editor buffer (List<StringBuilder>) → List<string>
                    var stringLines = lines.Select(sb => sb.ToString()).ToList();

                    // Use the output directory supplied by CommandLineHandler.
                     string outputDir = Path.GetFullPath(
                        outputDirectory ?? Directory.GetCurrentDirectory());

                    Directory.CreateDirectory(outputDir);

                    // Format erkennen
                    string format = StructureFormatDetector.DetectFormat(stringLines);

                    // Struktur parsen
                    List<string> paths;

                    Console.SetCursorPosition(0, SafeMessageRow());
                    Console.WriteLine("Executing fi mode with collected input...");

                    if (format == "TREE")
                    {
                        paths = StructureCreator.ParseTreeStructure(stringLines);

                        int exitCode = StructureCreator.ExecuteTree(paths, outputDir);

                        if (exitCode != 0)
                        {
                            Console.WriteLine("TREE execution failed.");
                        }
                    }
                    else
                    {
                        string fullInput = string.Join("\n", stringLines);

                        int exitCode = CommandLineHandler.ExecuteMask(
                            fullInput,
                            outputDir);

                        if (exitCode != 0)
                        {
                            Console.WriteLine("MASK execution failed.");
                        }
                    }

                    Console.WriteLine("fi mode (full input) completed successfully.");
                    DrawStatusLine();
                    SetCursorFromIndices();
                    continue;
                }


                var currentLine = lines[currentLineIndex];

                // Arrow keys
                if (key.Key == ConsoleKey.LeftArrow && key.Modifiers == 0)
                {
                    if (cursorIndex > 0)
                        cursorIndex--;
                    SetCursorFromIndices();
                    continue;
                }

                if (key.Key == ConsoleKey.RightArrow && key.Modifiers == 0)
                {
                    if (cursorIndex < currentLine.Length)
                        cursorIndex++;
                    SetCursorFromIndices();
                    continue;
                }

                if (key.Key == ConsoleKey.UpArrow && key.Modifiers == 0)
                {
                    if (currentLineIndex > 0)
                    {
                        currentLineIndex--;
                        currentLine = lines[currentLineIndex];
                        if (cursorIndex > currentLine.Length)
                            cursorIndex = currentLine.Length;
                    }
                    RedrawAll();
                    continue;
                }

                if (key.Key == ConsoleKey.DownArrow && key.Modifiers == 0)
                {
                    if (currentLineIndex < lines.Count - 1)
                    {
                        currentLineIndex++;
                        currentLine = lines[currentLineIndex];
                        if (cursorIndex > currentLine.Length)
                            cursorIndex = currentLine.Length;
                    }
                    RedrawAll();
                    continue; 
                }

                // Enter → neue Zeile
                if (key.Key == ConsoleKey.Enter && key.Modifiers == 0)
                {
                    var newLine = new StringBuilder();
                    if (cursorIndex < currentLine.Length)
                    {
                        // Split line at cursor
                        var rest = currentLine.ToString().Substring(cursorIndex);
                        currentLine.Remove(cursorIndex, currentLine.Length - cursorIndex);
                        newLine.Append(rest);
                    }

                    lines.Insert(currentLineIndex + 1, newLine);
                    currentLineIndex++;
                    cursorIndex = 0;

                    RedrawAll();
                    continue;
                }

                // Backspace
                if (key.Key == ConsoleKey.Backspace && key.Modifiers == 0)
                {
                    if (cursorIndex > 0)
                    {
                        currentLine.Remove(cursorIndex - 1, 1);
                        cursorIndex--;
                    }
                    else if (cursorIndex == 0 && currentLineIndex > 0)
                    {
                        // Merge with previous line
                        var prev = lines[currentLineIndex - 1];
                        int oldLen = prev.Length;
                        prev.Append(currentLine.ToString());
                        lines.RemoveAt(currentLineIndex);
                        currentLineIndex--;
                        cursorIndex = oldLen;
                    }
                    RedrawAll();
                    continue;
                }

                // Normal characters
                if (!char.IsControl(key.KeyChar))
                {
                    currentLine.Insert(cursorIndex, key.KeyChar);
                    cursorIndex++;
                    
                    RedrawAll();
                    continue;
                }
            }
        }

        private static string PromptEditable(string label, string suggested)
        {
            Console.Write($"{label} {suggested}");

            var input = new StringBuilder(suggested);
            int cursor = input.Length;

            Console.SetCursorPosition(label.Length + 1 + cursor, Console.CursorTop);

            while (true)
            {
                var key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return input.ToString();
                }

                if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine();
                    return suggested;
                }

                if (key.Key == ConsoleKey.Backspace && cursor > 0)
                {
                    input.Remove(cursor - 1, 1);
                    cursor--;
                    Console.Write("\b \b");
                    continue;
                }

                if (!char.IsControl(key.KeyChar))
                {
                    input.Insert(cursor, key.KeyChar);
                    cursor++;
                    Console.Write(key.KeyChar);
                    continue;
                }
            }
        }


        public static string? SuggestedFilename { get; private set; }

        public static void SetSuggestedFilename(string name)
        {
            SuggestedFilename = name;
        }

    }
}
