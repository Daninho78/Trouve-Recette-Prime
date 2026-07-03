public static class PortionDetector
{
    public static string Find(string[] lines)
    {
        foreach (string line in lines)
        {
            if (line.ToLower().Contains("personne"))
            {
                return line.Trim();
            }
        }

        return "";
    }
}
