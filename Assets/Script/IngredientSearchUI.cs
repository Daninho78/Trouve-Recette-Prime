using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Threading.Tasks;

public class IngredientSearchUI : MonoBehaviour
{
    public TMP_InputField searchInput;
    public Transform contentRecettes;
    public GameObject prefabRecipeItem;
    public RecipeDetailUI recipeDetailUI;
    public GameObject panelResultats;

    public async void OnSearchClicked()
    {
        string saisie = searchInput.text;

        // Séparer et nettoyer
        List<string> noms = saisie
            .Split(',')
            .Select(s => s.Trim().ToLower())
            .Where(s => !string.IsNullOrEmpty(s))
            .ToList();

        if (noms.Count == 0)
        {
            Debug.LogWarning("Aucun ingrédient saisi.");
            return;
        }

        // Lancer la recherche
        List<Recipe> recettes = await RecipeService.GetRecipesByIngredients(noms);

        Debug.Log($"🔍 Recettes trouvées : {recettes.Count}");

        // Supprimer les anciens résultats
        foreach (Transform child in contentRecettes)
            Destroy(child.gameObject);

        // Afficher les nouvelles recettes
        foreach (var recette in recettes)
        {
            GameObject item = Instantiate(prefabRecipeItem, contentRecettes);
            item.GetComponentInChildren<TextMeshProUGUI>().text = recette.Title;

            item.GetComponent<RecipeItem>().Setup(recette, recipeDetailUI);
        }
        panelResultats.SetActive(true);
    }

    public void FermerResultats()
{
    panelResultats.SetActive(false);
}
}