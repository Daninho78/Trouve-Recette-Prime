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

    private Guid recipeId;

    public async void ShowDetails(string titre, Guid id)
    {
        panelDetails.SetActive(true);
        titreRecette.text = titre;
        recipeId = id;
        await LoadIngredientsForRecipe();
    }

    private async Task LoadIngredientsForRecipe()
    {
        Debug.Log("Chargement des ingredients pour la recette ID : " + recipeId);
        // Supprimer les anciens ingrédients affichés
        foreach (Transform child in contentIngredients)
            Destroy(child.gameObject);

        // Récupérer les ingrédients via Supabase
        List<Ingredient> ingredients = await RecipeService.GetIngredientsByRecipe(recipeId);
        
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


        foreach (var ingr in ingredients)
        {
            Debug.Log("✅ Ingrédient : " + ingr.Name);
            GameObject item = Instantiate(prefabIngredientItem, contentIngredients);
            item.GetComponentInChildren<TextMeshProUGUI>().text = ingr.Name;


        }

    }
    
    public void FermerPanel()
{
    panelDetails.SetActive(false);
}
}