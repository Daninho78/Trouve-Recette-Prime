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


    private Guid recipeId;

    public async void ShowDetails(Recipe recipe)//string titre, Guid id)
    {
        panelDetails.SetActive(true);
        titreRecette.text = recipe.Title;
        recipeId = recipe.Id;

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

        ingredientsText.text = "";
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
        
    //tempsPrepaText.text = $" {recipe.PrepTimeMinutes} min";
    nbrPartsText.text = $" {recipe.Serving} personnes";
    //tempsCuissonText.text = $" {recipe.CookTimeMinutes}";
    pageText.text = recipe.Page != null ? $"{recipe.Page}" : "—";
    rateText.text = $"{recipe.Rate}/5";
    difficultyText.text = $" {recipe.Difficulty}";

    remarqueText.text = $"Remarque : {recipe.Remarque?.Trim() ?? ""}";
}

private string BuildIngredientLine(IngredientLine ingr)
{
    // priorité à QuantityText (ex: "1 pincée", "un peu", "à volonté")
    if (!string.IsNullOrWhiteSpace(ingr.QuantityText))
        return $"{ingr.QuantityText} {ingr.Name}".Trim();

    // sinon Quantity + Unity (ex: 200 g)
    if (ingr.Quantity != null)
    {
        string unit = ingr.Unity ?? "";
        return $"{ingr.Quantity} {unit} {ingr.Name}".Trim();
    }

    // sinon juste le nom
    return ingr.Name;
}
    
    public void FermerPanel()
{
    panelDetails.SetActive(false);
}
}