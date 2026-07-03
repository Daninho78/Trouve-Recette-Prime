using UnityEngine;
using System.Collections.Generic;

public class RecipeOCRResult
{
    public string title;
    public string preparationTime;
    public string cookingTime;
    public string portions;
    public List<string> ingredients = new List<string>();

    public int ingredientCount;
}

public class RecipeOCRParser
{
    public static RecipeOCRResult Parse(string ocrText)
    {
        RecipeOCRResult result = new RecipeOCRResult();

        if (!string.IsNullOrWhiteSpace(ocrText))
        {
            string[] lines = ocrText.Split('\n');

            foreach (string line in lines)
            {
                Debug.Log("[OCR_LINE] " + line);
            }

            result.title = TitleDetector.Find(lines);
result.portions = PortionDetector.Find(lines);
result.preparationTime = TimeDetector.FindPreparationTime(lines);
result.cookingTime = TimeDetector.FindCookingTime(lines);

List<string> ignoredLines = new List<string>();

if (!string.IsNullOrWhiteSpace(result.title))
    ignoredLines.Add(result.title);

if (!string.IsNullOrWhiteSpace(result.portions))
    ignoredLines.Add(result.portions);

if (!string.IsNullOrWhiteSpace(result.preparationTime))
    ignoredLines.Add(result.preparationTime);

if (!string.IsNullOrWhiteSpace(result.cookingTime))
    ignoredLines.Add(result.cookingTime);

result.ingredients = IngredientDetector.Find(lines, ignoredLines);
result.ingredientCount = result.ingredients.Count;
        }

        Debug.Log("Titre détecté : " + result.title);

        return result;
    }
}
