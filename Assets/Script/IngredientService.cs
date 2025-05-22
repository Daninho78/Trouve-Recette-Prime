using System;
using Postgrest.Attributes;
using Postgrest.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

[Table("ingredients")]
public class Ingredient : BaseModel
{
    [PrimaryKey("id")]
    public Guid Id { get; set; }

    [Column("recipe_id")]
    public Guid RecipeId { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

public static class IngredientService
{
    public static async Task InsertIngredient(Guid recipeId, string name)
    {
        var ingredient = new Ingredient
        {
            RecipeId = recipeId,
            Name = name,
            CreatedAt = DateTime.UtcNow
        };

        var client = SupabaseManager.Instance.GetClient();
        await client.From<Ingredient>().Insert(ingredient);
    }

    public static async Task<List<Ingredient>> GetIngredientsForRecipe(Guid recipeId)
    {
        var client = SupabaseManager.Instance.GetClient();

        var response = await client
            .From<Ingredient>()
            .Filter("recipe_id", Postgrest.Constants.Operator.Equals, recipeId)
            .Get();

        return response.Models;
    }
}
