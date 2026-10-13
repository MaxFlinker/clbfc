using System;
using System.Globalization;

namespace clbfc;

public static class fnfCertify
{
    public static CreationDecision Ask(string rawLine, string? forcedLang)
    {
        var lang = forcedLang ?? CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        return lang switch
        {
            "pirate" => AskPirate(rawLine),
            "de" => AskGerman(rawLine),
            "nl" => AskDutch(rawLine),
            "el" => AskGreek(rawLine),
            "la" => AskLatin(rawLine),
            "ar" => AskArabic(rawLine),
            "he" => AskHebrew(rawLine),
            "sa" => AskSanskrit(rawLine),
            "zh" => AskChinese(rawLine),
            "ja" => AskJapanese(rawLine),
            "tr" => AskTurkish(rawLine),
            "es" => AskSpanish(rawLine),
            "pl" => AskPolish(rawLine),
            "pt" => AskPortuguese(rawLine),
            "sv" => AskSwedish(rawLine),
            "no" => AskNorwegian(rawLine),
            "ch" => AskSwiss(rawLine),
            "at" => AskAustrian(rawLine),
            _ => AskEnglish(rawLine)
        };
    }

    private static CreationDecision AskPirate(string rawLine)
    {
        Console.WriteLine($"Arrr! Ye entered a shady entry: \"{rawLine}\"");
        Console.WriteLine("Shall I forge a file or carve a folder, ye scallywag?");
        Console.WriteLine("[F] File (fer yer booty)");
        Console.WriteLine("[D] Directory (fer yer treasure chests)");
        Console.WriteLine("[A] Abandon ship!");

        return AskLoop("F", "D", "A", "Arrr! Choose F, D, or A, matey.");
    }

    private static CreationDecision AskEnglish(string rawLine)
    {
        Console.WriteLine($"Unclear entry: \"{rawLine}\"");
        Console.WriteLine("Should I create a file or a folder?");
        Console.WriteLine("[F] File");
        Console.WriteLine("[D] Directory");
        Console.WriteLine("[A] Abort");

        return AskLoop("F", "D", "A", "Please enter F, D, or A.");
    }

    private static CreationDecision AskGerman(string rawLine)
    {
        Console.WriteLine($"Unklarer Eintrag: \"{rawLine}\"");
        Console.WriteLine("Soll ich eine Datei oder einen Ordner erzeugen?");
        Console.WriteLine("[D] Datei");
        Console.WriteLine("[O] Ordner");
        Console.WriteLine("[A] Abbrechen");

        return AskLoop("D", "O", "A", "Bitte D, O oder A eingeben.");
    }

    private static CreationDecision AskDutch(string rawLine)
    {
        Console.WriteLine($"Onduidelijke invoer: \"{rawLine}\"");
        Console.WriteLine("Moet ik een bestand of een map maken?");
        Console.WriteLine("[B] Bestand");
        Console.WriteLine("[M] Map");
        Console.WriteLine("[A] Afbreken");

        return AskLoop("B", "M", "A", "Voer alstublieft B, M of A in.");
    }

    private static CreationDecision AskGreek(string rawLine)
    {
        Console.WriteLine($"Ασαφής καταχώρηση: \"{rawLine}\"");
        Console.WriteLine("Να δημιουργήσω αρχείο ή φάκελο;");
        Console.WriteLine("[A] Αρχείο");
        Console.WriteLine("[F] Φάκελος");
        Console.WriteLine("[X] Ακύρωση");

        return AskLoop("A", "F", "X", "Παρακαλώ εισάγετε A, F ή X.");
    }

    private static CreationDecision AskSwiss(string rawLine)
    {
        Console.WriteLine($"Unklarer Eintrag: \"{rawLine}\"");
        Console.WriteLine("Was söll i mache? Datei oder Ordner aalegge?");
        Console.WriteLine("[D] Datei");
        Console.WriteLine("[O] Ordner");
        Console.WriteLine("[A] Abbräche");

        return AskLoop("D", "O", "A", "Bitte D, O oder A iigee.");
    }

    private static CreationDecision AskAustrian(string rawLine)
    {
        Console.WriteLine($"Unklarer Eintrag: \"{rawLine}\"");
        Console.WriteLine("Wos soi i do jetzt mochn? A Datei oder an Ordner?");
        Console.WriteLine("[D] Datei");
        Console.WriteLine("[O] Ordner");
        Console.WriteLine("[A] Abbrechen");

        return AskLoop("D", "O", "A", "Bitte D, O oder A eingeben.");
    }

    private static CreationDecision AskLatin(string rawLine)
    {
        Console.WriteLine($"Entry obscura: \"{rawLine}\"");
        Console.WriteLine("Fasciculum an tabulam creare?");
        Console.WriteLine("[T] Tabula");
        Console.WriteLine("[F] Fasciculus");
        Console.WriteLine("[A] Abortus");

        return AskLoop("T", "F", "A", "Quaeso T, F vel A introduce.");
    }

    private static CreationDecision AskArabic(string rawLine)
    {
        Console.WriteLine($"مدخل غير واضح: \"{rawLine}\"");
        Console.WriteLine("هل يجب إنشاء ملف أم مجلد؟");
        Console.WriteLine("[M] ملف");
        Console.WriteLine("[D] مجلد");
        Console.WriteLine("[A] إلغاء");

        return AskLoop("M", "D", "A", "الرجاء إدخال M أو D أو A.");
    }

    private static CreationDecision AskHebrew(string rawLine)
    {
        Console.WriteLine($"ערך לא ברור: \"{rawLine}\"");
        Console.WriteLine("ליצור קובץ או תיקייה?");
        Console.WriteLine("[K] קובץ");
        Console.WriteLine("[T] תיקייה");
        Console.WriteLine("[A] ביטול");

        return AskLoop("K", "T", "A", "אנא הזן K, T או A.");
    }

    private static CreationDecision AskSanskrit(string rawLine)
    {
        Console.WriteLine($"अस्पष्ट प्रविष्टि: \"{rawLine}\"");
        Console.WriteLine("किं फाइलं वा सञ्चिका निर्मातव्या?");
        Console.WriteLine("[F] फाइल");
        Console.WriteLine("[D] सञ्चिका");
        Console.WriteLine("[A] निरसनम्");

        return AskLoop("F", "D", "A", "कृपया F, D, वा A इति टंकयतु।");
    }

    private static CreationDecision AskChinese(string rawLine)
    {
        Console.WriteLine($"不明确的条目: \"{rawLine}\"");
        Console.WriteLine("要创建文件还是文件夹?");
        Console.WriteLine("[W] 文件");
        Console.WriteLine("[J] 文件夹");
        Console.WriteLine("[A] 取消");

        return AskLoop("W", "J", "A", "请输入 W、J 或 A。");
    }

    private static CreationDecision AskJapanese(string rawLine)
    {
        Console.WriteLine($"不明な項目: \"{rawLine}\"");
        Console.WriteLine("ファイルまたはフォルダを作成しますか?");
        Console.WriteLine("[F] ファイル");
        Console.WriteLine("[D] フォルダ");
        Console.WriteLine("[A] 中止");

        return AskLoop("F", "D", "A", "F、D、または A を入力してください。");
    }

    private static CreationDecision AskTurkish(string rawLine)
    {
        Console.WriteLine($"Belirsiz giriş: \"{rawLine}\"");
        Console.WriteLine("Dosya mı klasör mü oluşturayım?");
        Console.WriteLine("[D] Dosya");
        Console.WriteLine("[K] Klasör");
        Console.WriteLine("[A] İptal");

        return AskLoop("D", "K", "A", "Lütfen D, K veya A girin.");
    }

    private static CreationDecision AskSpanish(string rawLine)
    {
        Console.WriteLine($"Entrada poco clara: \"{rawLine}\"");
        Console.WriteLine("¿Debo crear un archivo o una carpeta?");
        Console.WriteLine("[A] Archivo");
        Console.WriteLine("[C] Carpeta");
        Console.WriteLine("[X] Cancelar");

        return AskLoop("A", "C", "X", "Por favor ingrese A, C o X.");
    }

    private static CreationDecision AskPolish(string rawLine)
    {
        Console.WriteLine($"Niejasny wpis: \"{rawLine}\"");
        Console.WriteLine("Utworzyć plik czy folder?");
        Console.WriteLine("[P] Plik");
        Console.WriteLine("[F] Folder");
        Console.WriteLine("[A] Anuluj");

        return AskLoop("P", "F", "A", "Proszę wpisać P, F lub A.");
    }

    private static CreationDecision AskPortuguese(string rawLine)
    {
        Console.WriteLine($"Entrada indefinida: \"{rawLine}\"");
        Console.WriteLine("Criar um arquivo ou uma pasta?");
        Console.WriteLine("[A] Arquivo");
        Console.WriteLine("[P] Pasta");
        Console.WriteLine("[C] Cancelar");

        return AskLoop("A", "P", "C", "Por favor, digite A, P ou C.");
    }

    private static CreationDecision AskSwedish(string rawLine)
    {
        Console.WriteLine($"Otydlig post: \"{rawLine}\"");
        Console.WriteLine("Ska jag skapa en fil eller en mapp?");
        Console.WriteLine("[F] Fil");
        Console.WriteLine("[M] Mapp");
        Console.WriteLine("[A] Avbryt");

        return AskLoop("F", "M", "A", "Ange F, M eller A.");
    }

    private static CreationDecision AskNorwegian(string rawLine)
    {
        Console.WriteLine($"Uklart element: \"{rawLine}\"");
        Console.WriteLine("Skal jeg lage en fil eller en mappe?");
        Console.WriteLine("[F] Fil");
        Console.WriteLine("[M] Mappe");
        Console.WriteLine("[A] Avbryt");

        return AskLoop("F", "M", "A", "Vennligst skriv inn F, M eller A.");
    }

    private static CreationDecision AskLoop(string fileKey, string dirKey, string abortKey, string invalidMessage)
    {
        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine()?.Trim().ToUpperInvariant();

            if (input == fileKey) return CreationDecision.File;
            if (input == dirKey) return CreationDecision.Directory;
            if (input == abortKey) return CreationDecision.Abort;

            Console.WriteLine(invalidMessage);
        }
    }
}

public enum CreationDecision
{
    File,
    Directory,
    Abort
}