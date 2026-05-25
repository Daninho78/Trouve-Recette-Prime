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

        string search = searchText.ToLower().Trim();

        List<Ingredient> results = allIngredients
        .Where(i => i.Name.ToLower().Contains(search))
        .OrderBy(i => i.Name.ToLower().StartsWith(search) ? 0 : 1)
        .ThenBy(i => i.Name)
        .Take(10)
        .ToList();

        return results;
    }


}