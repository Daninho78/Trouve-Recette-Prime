using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

public static class RecipeService
{
    public static async Task<List<Recipe>> GetRecipesByBook(Guid bookId)
    {
        try
        {
            var result = await Supabase.Client.Instance
                .From<Recipe>()
                .Filter("book_id", Postgrest.Constants.Operator.Equals, bookId.ToString())
                .Get();

            return result.Models;
        }
        catch (Exception ex)
        {
            Debug.LogError("Erreur lors du chargement des recettes : " + ex.Message);
            return new List<Recipe>();
        }
    }

    public static async Task<List<IngredientLine>> GetIngredientsByRecipe(Guid recipeId)
    {
        try
        {
            // 1. On récupère toutes les liaisons recipe_ingredient pour la recette donnée
            var liaisons = await Supabase.Client.Instance
                .From<RecipeIngredient>()
                .Filter("recipe_id", Postgrest.Constants.Operator.Equals, recipeId.ToString())
                .Get();

            // 2. On prépare une liste d'ingrédients à retourner
            List<IngredientLine> ingredients = new List<IngredientLine>();

            foreach (var liaison in liaisons.Models)
            {
                // 3. Pour chaque liaison, on récupère l'ingrédient correspondant
                var ingredientResult = await Supabase.Client.Instance
                    .From<Ingredient>()
                    .Filter("id", Postgrest.Constants.Operator.Equals, liaison.IngredientId.ToString())
                    .Get();

                // 4. On ajoute l'ingrédient (il devrait y en avoir qu’un par ID)
                if (ingredientResult.Models.Count > 0)
{
    var ing = ingredientResult.Models[0];

    ingredients.Add(new IngredientLine
    {
        IngredientId = ing.Id,
        Name = ing.Name,

      
        Quantity = liaison.Quantity,
        Unity = liaison.Unit,
        QuantityText = liaison.QuantityText
    });
}
            }

            return ingredients;
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ Erreur lors du chargement des ingrédients : " + ex.Message);
            return new List<IngredientLine>();
        }
    }

    public static async Task<List<Recipe>> GetRecipesByIngredients(List<string> nomsIngredients)
    {
        List<Recipe> recettesTrouvées = new();

        try
        {
            Debug.Log($"🔍 Étape 1 : Ingrédients recherchés : {string.Join(", ", nomsIngredients)}");

            List<Ingredient> ingredients = new();

            foreach (var nom in nomsIngredients)
            {
                var result = await Supabase.Client.Instance
                    .From<Ingredient>()
                    .Filter("name", Postgrest.Constants.Operator.Equals, nom)
                    .Get();

                ingredients.AddRange(result.Models);
            }

            Debug.Log($"✅ Étape 1 : Ingrédients trouvés : {ingredients.Count}");

            if (ingredients.Count == 0)
                return recettesTrouvées;

            List<Guid> ingredientIds = ingredients.Select(i => i.Id).ToList();

            List<RecipeIngredient> liaisons = new();

            foreach (var id in ingredientIds)
            {
                var liaisonResult = await Supabase.Client.Instance
                    .From<RecipeIngredient>()
                    .Filter("ingredient_id", Postgrest.Constants.Operator.Equals, id.ToString())
                    .Get();

                liaisons.AddRange(liaisonResult.Models);
            }

            Debug.Log($"✅ Étape 2 : Liaisons trouvées : {liaisons.Count}");

            var recipeIds = liaisons.Select(l => l.RecipeId).Distinct().ToList();

            if (recipeIds.Count == 0)
                return recettesTrouvées;

            Debug.Log($"🔍 Étape 2 : IDs des recettes à récupérer : {recipeIds.Count}");

            foreach (var recipeId in recipeIds)
            {
                var recipeResult = await Supabase.Client.Instance
                    .From<Recipe>()
                    .Filter("id", Postgrest.Constants.Operator.Equals, recipeId.ToString())
                    .Get();

                recettesTrouvées.AddRange(recipeResult.Models);
            }

            Debug.Log($"✅ Étape 3 : Recettes récupérées : {recettesTrouvées.Count}");
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ Erreur lors de la recherche par ingrédients : " + ex.Message);
        }

        return recettesTrouvées;
    }
}