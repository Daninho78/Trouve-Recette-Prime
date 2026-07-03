using System.Text.RegularExpressions;

public static class TimeDetector
{
    public static string FindPreparationTime(string[] lines)
    {
        return FindTimeAfterKeyword(lines, "préparation");
    }

    public static string FindCookingTime(string[] lines)
    {
        return FindTimeAfterKeyword(lines, "cuisson");
    }

    private static string FindTimeAfterKeyword(string[] lines, string keyword)
    {
        foreach (string line in lines)
        {
            string lowerLine = line.ToLower();
            int keywordIndex = lowerLine.IndexOf(keyword);

            if (keywordIndex == -1)
                continue;

            string textAfterKeyword = line.Substring(keywordIndex);

            Match match = Regex.Match(
                textAfterKeyword,
                @"\d+\s*(h|heure|heures|min|mn|minute|minutes|s|sec|seconde|secondes)",
                RegexOptions.IgnoreCase);

            if (match.Success)
                return match.Value;
        }

        return "";
    }
}