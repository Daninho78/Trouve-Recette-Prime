using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class IngredientService
{
    public static async Task<List<Ingredient>> GetAllIngredients()
    {
        List<Ingredient> allIngredients = new List<Ingredient>();

        try
        {
            var firstPage = await Supabase.Client.Instance
                .From<Ingredient>()
                .Range(0, 999)
                .Get();

            allIngredients.AddRange(firstPage.Models);

            var secondPage = await Supabase.Client.Instance
                .From<Ingredient>()
                .Range(1000, 1999)
                .Get();

            allIngredients.AddRange(secondPage.Models);

            return allIngredients;
        }
        catch (Exception ex)
        {
            Debug.LogError("Erreur lors du chargement des ingrédients : " + ex.Message);
            return new List<Ingredient>();
        }
    }

    
}