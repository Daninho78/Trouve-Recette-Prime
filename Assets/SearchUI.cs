using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SearchUI : MonoBehaviour
{
    public TMP_InputField searchInput;

    public GameObject recipeItemPrefab;
    public Transform resultsContent;
    public RecipeDetailUI recipeDetailUI;

    public GameObject noResultText;

    public async void OnSearchClicked()
    {
        var recipes = await RecipeService.GetAllRecipes();

        string search = searchInput.text.ToLower();

        var mots = search
            .Split(' ')
            .Where(m => !string.IsNullOrWhiteSpace(m))
            .ToArray();

        var resultats = new List<Recipe>();

        foreach (var recipe in recipes)
        {
            var book = await BookService.GetBookById(recipe.BookId);
            var ingredients = await RecipeService.GetIngredientsByRecipe(recipe.Id);

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
}
