using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class RecipeItem : MonoBehaviour
{
    private Recipe recipe;
    private RecipeDetailUI recipeDetailUI;
    public TextMeshProUGUI bookText;

    public void Setup(Recipe r, RecipeDetailUI detailUI)
    {
        recipe = r;
        recipeDetailUI = detailUI;


        GetComponent<Button>().onClick.AddListener(OnClick);

        SetBookName();
    }

    private async void SetBookName()
    {
        var book = await BookService.GetBookById(recipe.BookId);

        if (book != null && bookText != null)
        {
            bookText.text = "(" + book.Title + ")";
        }
    }

    private void OnClick()
    {
        recipeDetailUI.ShowDetails(recipe);
    }
}