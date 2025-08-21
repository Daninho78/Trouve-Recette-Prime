using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class BookDetailsUI : MonoBehaviour
{
    public GameObject panelDetails;
    public TextMeshProUGUI titreLivre;
    public Transform contentRecette;
    public GameObject prefabRecipeItem;
    public RecipeDetailUI recipeDetailUI;
    private Guid bookId;

    public async void ShowDetails(string titre, Guid id)
    {
        panelDetails.SetActive(true);           // On affiche le panel
        titreLivre.text = titre;                // On met à jour le titre
        bookId = id;                            // On garde l'ID du livre sélectionné

        await LoadRecipesForBook();             // On charge les recettes (voir ci-dessous)
    }

private async Task LoadRecipesForBook()
{
    // On supprime les anciennes recettes affichées
    foreach (Transform child in contentRecette)
    {
        Destroy(child.gameObject);
    }

    // Chargement
    List<Recipe> recettes = await RecipeService.GetRecipesByBook(bookId);
    Debug.Log("Nombre de recettes trouvées : " + recettes.Count);

        foreach (Recipe recette in recettes)
        {
            Debug.Log("📜 Recette trouvée : " + recette.Title);

            GameObject item = Instantiate(prefabRecipeItem, contentRecette); // on crée une case recette
            if (item == null)
            {
                Debug.LogError("❌ Échec lors de l'instanciation du prefab !");
                continue;
            }
            TextMeshProUGUI titre = item.GetComponentInChildren<TextMeshProUGUI>();
            if (titre == null)
            {
                Debug.LogError("❌ Aucun TextMeshProUGUI trouvé dans le prefab !");
            }
            else
            {
                titre.text = recette.Title;
                Debug.Log("✅ Texte mis à jour : " + recette.Title);
            }
            item.GetComponent<RecipeItem>().Setup(recette, recipeDetailUI);
            
        }
}





}