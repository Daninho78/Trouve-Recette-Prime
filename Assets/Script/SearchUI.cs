using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class SearchUI : MonoBehaviour
{
    public TMP_InputField searchInput;

    public GameObject recipeItemPrefab;
    public Transform resultsContent;
    public RecipeDetailUI recipeDetailUI;

    public GameObject noResultText;

    private List<Recipe> cachedRecipes = new List<Recipe>();
    private Dictionary<Guid, Book> cachedBooks = new Dictionary<Guid, Book>();
    private Dictionary<Guid, List<IngredientLine>> cachedIngredients = new Dictionary<Guid, List<IngredientLine>>();

    private async void Start()
    {
        await LoadCache();
    }
    public void OnSearchClicked()
    {
        var recipes = cachedRecipes;

        string search = searchInput.text.ToLower();

        var mots = search
            .Split(' ')
            .Where(m => !string.IsNullOrWhiteSpace(m))
            .ToArray();

        var resultats = new List<Recipe>();

        foreach (var recipe in recipes)
        {
            Book book = cachedBooks.ContainsKey(recipe.BookId)
    ? cachedBooks[recipe.BookId]
    : null;
            var ingredients = cachedIngredients.ContainsKey(recipe.Id)
    ? cachedIngredients[recipe.Id]
    : new List<IngredientLine>();

            string texteRecherche = recipe.Title.ToLower();

            if (book != null)
            {
                texteRecherche += " " + book.Title.ToLower();
            }

            foreach (var ingr in ingredients)
            {
                texteRecherche += " " + ingr.Name.ToLower();

                if (!string.IsNullOrWhiteSpace(ingr.QuantityText))
                    texteRecherche += " " + ingr.QuantityText.ToLower();
            }

            if (mots.All(m => texteRecherche.Contains(m)))
            {
                resultats.Add(recipe);
            }
        }
        foreach (Transform child in resultsContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var recipe in resultats)
        {
            GameObject item = Instantiate(recipeItemPrefab, resultsContent);

            RecipeItem recipeItem = item.GetComponent<RecipeItem>();
            recipeItem.Setup(recipe, recipeDetailUI);

            TextMeshProUGUI titreTexte = item.GetComponentInChildren<TextMeshProUGUI>();
            if (titreTexte != null)
            {
                titreTexte.text = recipe.Title;
            }
        }

        noResultText.SetActive(resultats.Count == 0);

        Debug.Log("Résultats trouvés : " + resultats.Count);
    }

    private async Task LoadCache()
    {
        cachedRecipes = await RecipeService.GetAllRecipes();

        foreach (var recipe in cachedRecipes)
        {
            if (!cachedBooks.ContainsKey(recipe.BookId))
            {
                var book = await BookService.GetBookById(recipe.BookId);
                if (book != null)
                    cachedBooks.Add(recipe.BookId, book);
            }

            if (!cachedIngredients.ContainsKey(recipe.Id))
            {
                var ingredients = await RecipeService.GetIngredientsByRecipe(recipe.Id);
                cachedIngredients.Add(recipe.Id, ingredients);
            }
        }

        Debug.Log("Cache chargé : " + cachedRecipes.Count + " recettes");
    }
}
