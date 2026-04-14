using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Threading.Tasks;
using UnityEngine.UI;


public class RecipeDetailUI : MonoBehaviour
{
    public GameObject panelDetails;
    public TextMeshProUGUI titreRecette;
    public Transform contentIngredients;
    public GameObject prefabIngredientItem;
    [SerializeField] private TextMeshProUGUI ingredientsText;
    [SerializeField] private TMP_Text tempsText;
    //[SerializeField] private TMP_Text tempsPrepaText;
    //[SerializeField] private TMP_Text tempsCuissonText;
    [SerializeField] private TMP_Text nbrPartsText;
    [SerializeField] private TMP_Text rateText;
    [SerializeField] private TMP_Text pageText;
    [SerializeField] private TMP_Text difficultyText;
    [SerializeField] private TMP_Text remarqueText;
    private Recipe currentRecipe;


    private Guid recipeId;

    public async void ShowDetails(Recipe recipe)//string titre, Guid id)
    {
        panelDetails.SetActive(true);
        titreRecette.text = recipe.Title;
        recipeId = recipe.Id;
        currentRecipe = recipe;

        DisplayInfo(recipe);

        await LoadIngredientsForRecipe();
    }

    private async Task LoadIngredientsForRecipe()
    {
        Debug.Log("Chargement des ingredients pour la recette ID : " + recipeId);
        // Supprimer les anciens ingrédients affichés
        //foreach (Transform child in contentIngredients)
           // Destroy(child.gameObject);

        // Récupérer les ingrédients via Supabase
        List<IngredientLine> ingredients = await RecipeService.GetIngredientsByRecipe(recipeId);
        
        if (ingredients == null)
    {
        Debug.LogWarning("❌ Aucun ingrédient récupéré (null).");
        return;
    }

    if (ingredients.Count == 0)
    {
        Debug.LogWarning("⚠️ Aucun ingrédient trouvé pour cette recette.");
        return;
    }

        ingredientsText.text = $"Ingrédients\n\n";
        foreach (var ingr in ingredients)
        {
            string line = BuildIngredientLine(ingr);
            ingredientsText.text += "- " + line + "\n";
            //Ancienne methode
            //GameObject item = Instantiate(prefabIngredientItem, contentIngredients);
            //item.GetComponentInChildren<TextMeshProUGUI>().text = ingr.Name;


        }
        

    }

    private void DisplayInfo(Recipe recipe)
    {
    string temps = "";

    temps += $"Préparation : {recipe.PrepTimeMinutes ?? 0} min\n";
    temps += $"Cuisson : {recipe.CookTimeMinutes ?? 0} min";

    if (recipe.RestTimeMinutes.HasValue && recipe.RestTimeMinutes.Value > 0)
        {
            temps += $"\nRepos : {recipe.RestTimeMinutes.Value} min";
        }

        tempsText.text = temps;
        
    nbrPartsText.text = $"Pour {recipe.Serving} personnes";
    pageText.text = $"Page : {recipe.Page ?? 0}";
    rateText.text = $"Note : {recipe.Rate}/5";
    difficultyText.text = $"Difficulté : {recipe.Difficulty}";

    remarqueText.text = $"Remarque : {recipe.Remarque?.Trim() ?? ""}";
    }

    private string BuildIngredientLine(IngredientLine ingr)
    {
        // Cas spécial : "une pincée sel"
        if (!string.IsNullOrWhiteSpace(ingr.QuantityText))
            return $"{ingr.QuantityText} {ingr.Name}".Trim();

        // Cas normal avec quantité + unité : "200 g farine"
        if (ingr.Quantity > 0 && !string.IsNullOrWhiteSpace(ingr.Unity))
            return $"{ingr.Quantity} {ingr.Unity} {ingr.Name}".Trim();

        // Cas quantité sans unité : "200 farine"
        if (ingr.Quantity > 0)
            return $"{ingr.Quantity} {ingr.Name}".Trim();

        // Sinon juste le nom
        return ingr.Name;
    }

    public void FermerPanel()
    {
    panelDetails.SetActive(false);
    }

    public void ModifierRecette()
    {
        panelDetails.SetActive(false);
        FindObjectOfType<UIAjoutBasique>().EditRecipe(currentRecipe);
    }

    public async void SupprimerRecette()
    {
        await SupabaseRPC.DeleteRecipeIngredientsRPC(currentRecipe.Id);
        await SupabaseRPC.DeleteRecipeRPC(currentRecipe.Id);

        panelDetails.SetActive(false);
        FindObjectOfType<BookDetailsUI>().ShowDetails(new Book
        {
            Id = currentRecipe.BookId
        });
    }
}