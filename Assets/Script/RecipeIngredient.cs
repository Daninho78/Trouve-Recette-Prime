using Postgrest.Attributes;
using Postgrest.Models;
using System;

[Table("recipe_ingredient")]
public class RecipeIngredient : BaseModel
{
    [PrimaryKey("recipe_id", false)]
    public Guid RecipeId { get; set; }

    [PrimaryKey("ingredient_id", false)]
    public Guid IngredientId { get; set; }

    [Column("quantity")]
    public decimal? Quantity { get; set; }

    [Column("unity")]
    public string Unity { get; set; }

    [Column("quantity_text")]
    public string QuantityText { get; set; }
}