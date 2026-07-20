using System.Collections.Generic;

public static class OCRUnitNormalizer
{
    public static IEnumerable<string> GetCandidates(string unitName)
    {
        if (string.IsNullOrWhiteSpace(unitName))
            yield break;

        yield return unitName.Trim().ToLower();

        string normalized = unitName.Trim().ToLower();

        switch (normalized)
        {
            case "gramme":
            case "grammes":
            case "gr":
                yield return "g";
                break;

            case "cuil. à soupe":
            case "cuillère à soupe":
            case "cuillerée à soupe":
            case "cas":
                yield return "c. à soupe";
                break;
        }
    }
}