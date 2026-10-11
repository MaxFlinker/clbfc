using System;
using System.Globalization;
using System.IO;

namespace clbfc;

class Program
{
    public const string Version = "1.09.0";

    static int Main(string[] args)
    {
        return CommandLineHandler.Run(args);
    }

    private static bool IsKnownFlag(string arg) =>
        arg is "-c" or "-fs" or "-fi" or "-o" or "--lang" or "-h" or "--help" or "-?" or "/?" or "-version";

    private static bool IsHelpFlag(string arg) =>
        arg is "-h" or "--help" or "-?" or "/?";

    private static StringComparison GetPathComparison() =>
        OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

    private static bool IsSameOrChildPath(string candidatePath, string parentPath)
    {
        string normalizedCandidate = Path.TrimEndingDirectorySeparator(Path.GetFullPath(candidatePath));
        string normalizedParent = Path.TrimEndingDirectorySeparator(Path.GetFullPath(parentPath));

        if (string.Equals(normalizedCandidate, normalizedParent, GetPathComparison()))
        {
            return true;
        }

        string parentWithSeparator = normalizedParent + Path.DirectorySeparatorChar;
        return normalizedCandidate.StartsWith(parentWithSeparator, GetPathComparison());
    }

    private static void PrintFolderError(string? lang)
    {
        lang ??= CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        switch (lang)
        {
            case "tr": Console.WriteLine("Hata: Bu bir klasör, dosya değil."); break;
            case "de": Console.WriteLine("Fehler: Dies ist ein Ordner, keine Datei."); break;
            case "es": Console.WriteLine("Error: Esto es una carpeta, no un archivo."); break;
            case "pl": Console.WriteLine("Błąd: To jest folder, nie plik."); break;
            case "pt": Console.WriteLine("Erro: Isto é uma pasta, não um arquivo."); break;
            case "sv": Console.WriteLine("Fel: Detta är en mapp, inte en fil."); break;
            case "no": Console.WriteLine("Feil: Dette er en mappe, ikke en fil."); break;
            default: Console.WriteLine("Error: This is a folder, not a file."); break;
        }
    }

    private static void PrintFileMissingError(string? lang)
    {
        lang ??= CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        switch (lang)
        {
            case "tr": Console.WriteLine("Hata: Dosya mevcut değil."); break;
            case "de": Console.WriteLine("Fehler: Datei existiert nicht."); break;
            case "es": Console.WriteLine("Error: El archivo no existe."); break;
            case "pl": Console.WriteLine("Błąd: Plik nie istnieje."); break;
            case "pt": Console.WriteLine("Erro: O arquivo não existe."); break;
            case "sv": Console.WriteLine("Fel: Filen finns inte."); break;
            case "no": Console.WriteLine("Feil: Filen finnes ikke."); break;
            case "sa": Console.WriteLine("दोषः: फाइल् नास्ति."); break;
            default: Console.WriteLine("Error: File does not exist."); break;
        }
    }

    private static void PrintHelp()
    {
        Console.WriteLine("BatchFileCreator (bfc)");
        Console.WriteLine($"Version: {Version}");
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
}
