using System.Collections.Generic;
using UnityEngine;

public static class IngredientDetector
{
    private static readonly string[] IngredientUnits =
    {
        "g", "kg", "mg",
        "ml", "cl", "l"
    };

    public static List<string> Find(string[] lines, List<string> ignoredLines)
    {
        List<string> ingredients = new List<string>();

        foreach (string line in lines)
        {
            string trimmedLine = line.Trim();

            if (string.IsNullOrWhiteSpace(trimmedLine))
                continue;

            if (ignoredLines.Contains(trimmedLine))
            {
                Debug.Log("[OCR_IGNORED] " + trimmedLine);
                continue;
            }

            if (LooksLikeIngredient(trimmedLine))
            {
                ingredients.Add(trimmedLine);
            }
        }

        return ingredients;
    }

    private static bool LooksLikeIngredient(string line)
    {
        if (LooksLikePreparationStep(line))
            return false;

        if (line.StartsWith("•"))
            return true;

        if (char.IsDigit(line[0]))
            return true;

        foreach (string unit in IngredientUnits)
        {
            if (line.Contains(" " + unit + " "))
                return true;
        }

        return false;
    }

    private static bool LooksLikePreparationStep(string line)
    {
        string lowerLine = line.ToLower();

        string[] preparationPatterns =
        {
            "ajoutez",
            "mélangez",
            "versez",
            "faites",
            "laissez",
            "coupez",
            "découpez",
            "épluchez",
            "râpez",
            "écrasez",
            "chauffez",
            "cuisez",
            "réservez",
            "dans un",
            "dans une",
            "dans le",
            "dans la",
            "avec ",
            "une cocotte",
            "un saladier",
            "un bol",
            "une casserole",
            "une poêle"
        };

        foreach (string pattern in preparationPatterns)
        {
            if (lowerLine.StartsWith(pattern))
                return true;
        }

        return false;
    }
}