using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Supabase;
using Postgrest.Models;
using JetBrains.Annotations;

public class UIAjoutBasique : MonoBehaviour
{
    public TMP_InputField titreLivreInput;
    public TMP_InputField auteurInput;
    public TMP_InputField collectionInput;
    public TMP_InputField titreRecetteInput;
    public TMP_InputField ingredientsInput;
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
        if (currentBookId == Guid.Empty)
        {
            Debug.LogWarning("Aucun livre sélectionné.");
            return;
        }

        string titreRecette = titreRecetteInput.text;
        if (string.IsNullOrWhiteSpace(titreRecette))
        {
            Debug.LogWarning("Titre de la recette vide.");
            return;
        }

        var recetteId = await SupabaseRPC.InsertRecipeRPC(currentBookId, titreRecette, 0);

        if (recetteId == Guid.Empty)
        {
            Debug.LogWarning("❌ La recette n'a pas été insérée.");
            return;
        }

        Debug.Log("✅ Recette insérée avec ID : " + recetteId);

        string[] ingredients = ingredientsInput.text.Split(',');

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


    }


}