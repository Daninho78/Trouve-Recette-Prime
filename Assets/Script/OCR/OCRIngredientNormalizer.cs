using System.Collections.Generic;

public static class OCRIngredientNormalizer
{
    private static readonly Dictionary<string, string> BusinessRules = new()
    {
        { "farine", "farine (au choix)" },
        { "farine de blé", "farine de blé (au choix)" }
    };

    public static List<string> GetCandidates(string ingredientName)
    {
        List<string> candidates = new();

        if (string.IsNullOrWhiteSpace(ingredientName))
            return candidates;

        string original = ingredientName
            .ToLower()
            .Trim();

        AddCandidate(candidates, original);

        string normalized = original
            .Replace("œ", "oe")
            .Replace("’", "'");

        while (normalized.Contains("  "))
        {
            normalized = normalized.Replace("  ", " ");
        }

        AddCandidate(candidates, normalized);

        string singular = null;

        if (normalized.EndsWith("s") && normalized.Length > 1)
        {
            singular = normalized.Substring(0, normalized.Length - 1);
        }
        else if (normalized.EndsWith("x") && normalized.Length > 1)
        {
            singular = normalized.Substring(0, normalized.Length - 1);
        }

        if (singular != null)
        {
            AddCandidate(candidates, singular);

            if (BusinessRules.TryGetValue(singular, out string singularBusinessRule))
            {
                AddCandidate(candidates, singularBusinessRule);
            }
        }

        if (BusinessRules.TryGetValue(normalized, out string businessRule))
        {
            AddCandidate(candidates, businessRule);
        }

        return candidates;
    }

    private static void AddCandidate(List<string> candidates, string candidate)
    {
        if (!string.IsNullOrWhiteSpace(candidate) && !candidates.Contains(candidate))
        {
            candidates.Add(candidate);
        }
    }
}