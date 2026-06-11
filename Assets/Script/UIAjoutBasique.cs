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
    public GameObject miniPanelAdd;

    private Guid currentRecipeId = Guid.Empty;
    private bool isEditing = false;

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

        ClearBookInputs();

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

        var ingredients = ingredientInputManager.GetAllIngredients();
        //verifie qu'il n'y a pas doublons dans la liste des ingredients
        var ingredientsNormalises = ingredients
    .Select(i => i.ingredient.Name.Trim().ToLower())
    .ToList();

        bool hasDuplicate = ingredientsNormalises.Count != ingredientsNormalises.Distinct().Count();

        if (hasDuplicate)
        {
            Debug.LogWarning("La recette contient des ingrédients en double.");
            return;
        }

        if (isEditing)
        {

            var ingredientsEdit = ingredientInputManager.GetAllIngredients();

            Debug.Log("Ingrédients récupérés en édition : " + ingredientsEdit.Count);

            foreach (var ingr in ingredientsEdit)
            {
                Debug.Log($"EDIT INGREDIENT -> name: {ingr.ingredient.Name}, quantity: {ingr.quantity}, unit: {ingr.unit}, quantityText: {ingr.quantityText}");
            }

            bool success = await SupabaseRPC.UpdateRecipeRPC(
                currentRecipeId,
                titreRecette,
                page,
                prepTime,
                cookTime,
                serving,
                difficulty,
                rate,
                remark
            );

            await SupabaseRPC.DeleteRecipeIngredientsRPC(currentRecipeId);

            foreach (var ingr in ingredientsEdit)
            {
                var ingredientId = ingr.ingredient.Id;

                if (ingredientId != Guid.Empty)
                {
                    await SupabaseRPC.InsertRecipeIngredientRPC(
                        currentRecipeId,
                        ingredientId,
                        ingr.quantity,
                        ingr.unit,
                        ingr.quantityText
                    );
                }
            }

            if (!success)
            {
                Debug.LogWarning("La recette n'a pas été mise à jour.");
                return;
            }

            Debug.Log("✅ Recette mise à jour avec succès");

            isEditing = false;
            currentRecipeId = Guid.Empty;
            ClearInputs();
            panelAddRecipe.SetActive(false);

            BookDetailsUI bookDetails = FindObjectOfType<BookDetailsUI>();

            if (bookDetails != null)
            {
                bookDetails.ShowDetails(new Book
                {
                    Id = currentBookId
                });
            }
            else
            {
                SearchUI searchUI = FindObjectOfType<SearchUI>();

                if (searchUI != null)
                {
                    await searchUI.LoadCache();
                    searchUI.OnSearchClicked();
                }
            }

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

        

        

        foreach (var ingredient in ingredients)
        {
            var ingredientId = ingredient.ingredient.Id;

            if (ingredientId != Guid.Empty)
            {
                await SupabaseRPC.InsertRecipeIngredientRPC(
                    recetteId,
                    ingredientId,
                    ingredient.quantity,
                    ingredient.unit,
                    ingredient.quantityText
                );

                Debug.Log("🔗 Ingrédient lié : " + ingredient.ingredient.Name);
            }
        }

        Debug.Log("🎉 Recette et ingrédients enregistrés.");

        ClearInputs();

        


    }

    public void OuvrirAjoutRecette()
    {
        if (currentBookId == Guid.Empty)
        {
            Debug.LogWarning("Aucun livre sélectionné.");
            return;
        }

        panelAddRecipe.SetActive(true);
        miniPanelAdd.SetActive(false);
    }

    public void ClearInputs()
    {
        titreRecetteInput.text = "";
        pageInput.text = "";
        tempsPreparationInput.text = "";
        tempsCuissonInput.text = "";
        remarqueInput.text = "";

        servingsDropdown.value = 0;
        difficulteDropdown.value = 0;
        rateDropdown.value = 0;

        // Reset ingrédients
        ingredientInputManager.ClearInputs();
    }

    private void ClearBookInputs()
    {
        titreLivreInput.text = "";
        auteurInput.text = "";
        collectionInput.text = "";
    }

    public void RetourAjoutLivre()
    {
        ClearInputs();
        currentBookId = Guid.Empty;
        panelAddRecipe.SetActive(false);
    }

    public void SetCurrentBookId(Guid bookId)
    {
        currentBookId = bookId;
    }

    public async void EditRecipe(Recipe recipe)
    {
        SetCurrentBookId(recipe.BookId);

        isEditing = true;
        currentRecipeId = recipe.Id;

        // Remplir les champs
        titreRecetteInput.text = recipe.Title;
        pageInput.text = recipe.Page.ToString();
        tempsPreparationInput.text = recipe.PrepTimeMinutes.ToString();
        tempsCuissonInput.text = recipe.CookTimeMinutes.ToString();
        remarqueInput.text = recipe.Remarque;

        // Dropdowns
        int serving = recipe.Serving ?? 0;
        servingsDropdown.value = serving;
        difficulteDropdown.value = difficulteDropdown.options
            .FindIndex(o => o.text == recipe.Difficulty);
        rateDropdown.value = recipe.Rate;

        

        panelAddRecipe.SetActive(true);

        Debug.Log("EditRecipe appelée pour : " + recipe.Title);

        var ingredients = await RecipeService.GetIngredientsByRecipe(recipe.Id);
        Debug.Log("Nombre d'ingrédients récupérés : " + ingredients.Count);

        ingredientInputManager.ClearAllInputs();

        foreach (var ingr in ingredients)
        {
            Debug.Log("QUANTITE CHARGEE : " + ingr.Name + " = " + ingr.Quantity);
            ingredientInputManager.CreateInputFromData(
                new Ingredient
                {
                    Id = ingr.IngredientId,
                    Name = ingr.Name
                },
                ingr.Quantity.HasValue ? ingr.Quantity.Value.ToString() : "",
                ingr.Unity


            );
        }


    }




}