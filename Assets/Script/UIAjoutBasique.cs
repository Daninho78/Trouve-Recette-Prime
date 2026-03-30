using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Supabase;
using Postgrest.Models;
using JetBrains.Annotations;
using System.Linq;

public class UIAjoutBasique : MonoBehaviour
{
    public TMP_InputField titreLivreInput;
    public TMP_InputField auteurInput;
    public TMP_InputField collectionInput;
    public TMP_InputField titreRecetteInput;
    public TMP_Dropdown servingsDropdown;
    public TMP_InputField tempsPreparationInput;
    public TMP_InputField tempsCuissonInput;
    public TMP_InputField pageInput;
    public TMP_Dropdown difficulteDropdown;
    public TMP_Dropdown rateDropdown;
    public IngredientInputManager2 ingredientInputManager;
    public TMP_InputField remarqueInput;
    public GameObject panelAddRecipe;

    private Guid currentBookId;

    public GameObject bibliothèque;

    public async void CreerLivre()
    {
        string titre = titreLivreInput.text;
        string auteur = auteurInput.text;
        string collection = collectionInput.text;
        if (string.IsNullOrWhiteSpace(titre))
        {
            Debug.LogWarning("Titre du livre vide.");
            return;
        }

        currentBookId = await SupabaseRPC.InsertBookRPC(titre, auteur, collection);
        Debug.Log("Livre créé avec ID : " + currentBookId);

        panelAddRecipe.SetActive(true);
    }

    public async void AjouterRecette()
    {
        Debug.Log("currentBookId = " + currentBookId);
        if (currentBookId == Guid.Empty)
        {
            Debug.LogWarning("Aucun livre sélectionné.");
            return;
        }

        string titreRecette = titreRecetteInput.text;
        string pageText = pageInput.text;
        string prepTimeText = tempsPreparationInput.text;
        string cookTimeText = tempsCuissonInput.text;
        string difficulty = difficulteDropdown.options[difficulteDropdown.value].text;
        string rateText = rateDropdown.options[rateDropdown.value].text;
        string remark = remarqueInput.text;
        string servingText = servingsDropdown.options[servingsDropdown.value].text;

        int.TryParse(pageText, out int page);
        int.TryParse(prepTimeText, out int prepTime);
        int.TryParse(cookTimeText, out int cookTime);
        int.TryParse(servingText, out int serving);
        int.TryParse(rateText, out int rate);

        if (string.IsNullOrWhiteSpace(titreRecette))
        {
            Debug.LogWarning("Titre de la recette vide.");
            return;
        }

        List<string> ingredients = ingredientInputManager.GetAllIngredients();
        //verifie qu'il n'y a pas doublons dans la liste des ingredients
        var ingredientsNormalises = ingredients
    .Select(i => i.Trim().ToLower())
    .ToList();

        bool hasDuplicate = ingredientsNormalises.Count != ingredientsNormalises.Distinct().Count();

        if (hasDuplicate)
        {
            Debug.LogWarning("La recette contient des ingrédients en double.");
            return;
        }

        var recetteId = await SupabaseRPC.InsertRecipeRPC(
            currentBookId,
            titreRecette,
            page,
            prepTime,
            cookTime,
            serving,
            difficulty,
            rate,
            remark);

        if (recetteId == Guid.Empty)
        {
            Debug.LogWarning("❌ La recette n'a pas été insérée.");
            return;
        }

        Debug.Log("✅ Recette insérée avec ID : " + recetteId);

        

        

        foreach (string ingredientBrut in ingredients)
        {
            string nomNettoye = ingredientBrut.Trim().ToLower();

            if (!string.IsNullOrEmpty(nomNettoye))
            {
                Debug.Log("⏳ Envoi RPC ingrédient : " + nomNettoye);
                var ingredientId = await SupabaseRPC.InsertIngredientRPC(nomNettoye);

                if (ingredientId != Guid.Empty)
                {
                    await SupabaseRPC.InsertRecipeIngredientRPC(recetteId, ingredientId);
                    Debug.Log("🔗 Ingrédient lié : " + nomNettoye);
                }
                else
                {
                    Debug.LogWarning("❌ Erreur lors de l'insertion de l'ingrédient : " + nomNettoye);
                }
            }
        }

        Debug.Log("🎉 Recette et ingrédients enregistrés.");

        ingredientInputManager.ClearInputs();

        titreRecetteInput.text = "";
        tempsPreparationInput.text = "";
        tempsCuissonInput.text = "";
        pageInput.text = "";
        remarqueInput.text = "";
        servingsDropdown.value = 0;
        difficulteDropdown.value = 0;
        rateDropdown.value = 0;


    }


}