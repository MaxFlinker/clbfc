public static class StructureFormatDetector
{
    public static string DetectFormat(List<string> lines)
    {
        bool hasTree = false;
        bool hasMask = false;

        foreach (var raw in lines)
        {
            if (string.IsNullOrWhiteSpace(raw))
                continue;

            string line = raw.Trim();

            // TREE-Symbole
            if (line.Contains("├──") || line.Contains("└──") || line.Contains("│"))
                hasTree = true;
                hasMask = false;
            // MASK-Symbole (NUR echte Masken!)
            if (hasTree !=true && line.EndsWith("/"))
                hasMask = true;

            // Dateien sind KEIN MASK-Indikator!
            // line.Contains('.') wird NICHT mehr ausgewertet
        }

        if (hasTree && hasMask)
            throw new Exception("Mixed TREE and MASK format detected.");

        if (hasTree)
            return "TREE";

        if (hasMask)
            return "MASK";

        // Fallback: Wenn weder Tree noch Mask eindeutig → Tree bevorzugen
        return "TREE";
    }
}
