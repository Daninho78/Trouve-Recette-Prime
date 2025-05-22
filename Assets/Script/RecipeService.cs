using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Supabase;
using Postgrest;
using Postgrest.Models;
using Postgrest.Responses;
using Postgrest.Attributes;
using UnityEngine;
using Unity.VisualScripting;


[Table("recipes")]
public class Recipe : BaseModel
{
    [PrimaryKey("id")]
    public Guid Id { get; set; }

    [Column("book_id")]
    public Guid BookId { get; set; }

    [Column("title")]
    public string Title { get; set; }

    [Column("duration")]
    public int Duration { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

public static class RecipeService
{
    public static async Task InsertRecipe(Guid bookId, string title, int duration)
    {
        var recipe = new Recipe
        {
            BookId = bookId,
            Title = title,
            Duration = duration,
            CreatedAt = DateTime.UtcNow
        };

        var client = SupabaseManager.Instance.GetClient();
        var response = await client.From<Recipe>().Insert(recipe);
        Debug.Log("Recette ajoutée : " + title);
    }

    public static async Task<List<Recipe>> GetRecipesForBook(Guid bookId)
    {
        var client = SupabaseManager.Instance.GetClient();
        var response = await client.From<Recipe>().Filter("book_id", Postgrest.Constants.Operator.Equals, bookId).Get();
        return response.Models;
    }
}
