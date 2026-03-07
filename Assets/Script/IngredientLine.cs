using System;

[Serializable]
public class IngredientLine
{
    public Guid IngredientId;      // id de l’ingrédient (table ingredients)
    public string Name;
    public decimal? Quantity;     // ex: 200
    public string Unity;          // ex: "g"
    public string QuantityText;   // ex: "une pincée", "1/2", "à volonté"
}