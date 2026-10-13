using System;
using System.Globalization;
using System.IO;

namespace clbfc;

class Program
{
    public const string Version = "1.12.2";
    static int Main(string[] args)
    {
        return CommandLineHandler.Run(args);
    }

    public static bool IsKnownFlag(string arg) =>
        arg is "-c" or "-fs" or "-fi" or "-o" or "--lang" or "-h" or "--help" or "-?" or "/?" or "-version";

    public static bool IsHelpFlag(string arg) =>
        arg is "-h" or "--help" or "-?" or "/?";

    public static StringComparison GetPathComparison() =>
        OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

    public static bool IsSameOrChildPath(string candidatePath, string parentPath)
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

    public static void PrintFolderError(string? lang)
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

    public static void PrintFileMissingError(string? lang)
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
}