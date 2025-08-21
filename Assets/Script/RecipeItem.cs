using UnityEngine;
using UnityEngine.UI;


public class RecipeItem : MonoBehaviour
{
    private Recipe recipe;
    private RecipeDetailUI recipeDetailUI;

    public void Setup(Recipe r, RecipeDetailUI detailUI)
    {
        recipe = r;
        recipeDetailUI = detailUI;


        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        recipeDetailUI.ShowDetails(recipe.Title, recipe.Id);
    }
}