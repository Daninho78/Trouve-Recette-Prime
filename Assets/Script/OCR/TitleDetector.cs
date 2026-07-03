public static class TitleDetector
{
    public static string Find(string[] lines)
    {
        if (lines == null || lines.Length == 0)
            return "";

        return lines[0].Trim();
    }
}