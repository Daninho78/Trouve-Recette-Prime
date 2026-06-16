using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class IngredientSuggestionManager : MonoBehaviour
{
    private List<Ingredient> allIngredients = new List<Ingredient>();
    

    private async void Start()
    {
        await LoadIngredients();
    }

    private async Task LoadIngredients()
    {
        allIngredients = await IngredientService.GetAllIngredients();
        Debug.Log("Suggestions ingrédients prêtes : " + allIngredients.Count);

    }

    public List<Ingredient> GetSuggestions(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return new List<Ingredient>();

        string search = NormalizeSearchText(searchText);

        string[] searchWords = search
            .Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

        List<Ingredient> results = allIngredients
            .Where(i =>
            {
                string ingredientName = NormalizeSearchText(i.Name);

                return searchWords.All(word => ingredientName.Contains(word));
            })
            .OrderBy(i =>
            {
                string ingredientName = NormalizeSearchText(i.Name);

                return ingredientName.StartsWith(search) ? 0 : 1;
            })
            .ThenBy(i => i.Name)
            .Take(10)
            .ToList();

        return results;
    }

    private string NormalizeSearchText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "";

        string normalized = text.ToLower().Trim();

        normalized = normalized
            .Replace("'", " ")
            .Replace("’", " ")
            .Replace("`", " ")
            .Replace("´", " ")
            .Replace("ʼ", " ")
            .Replace("-", " ")
            .Replace(",", " ")
            .Replace(".", " ")
            .Replace(";", " ")
            .Replace(":", " ")
            .Replace("/", " ");

        while (normalized.Contains("  "))
        {
            normalized = normalized.Replace("  ", " ");
        }

        return normalized.Trim();
    }


}