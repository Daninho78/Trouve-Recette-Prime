public static class IngredientParser
{
    private static readonly string[] Units =
{
    "g", "kg", "mg",
    "ml", "cl", "l",
    "cuil. à soupe",
    "cuil. à café"
};
    public static RecipeOCRIngredient Parse(string line)
    {
        RecipeOCRIngredient ingredient = new RecipeOCRIngredient();

        ingredient.OriginalLine = line;
        string[] parts = line.Split(' ');

        if (parts.Length > 0)
        {
            ingredient.Quantity = parts[0];
            string textAfterQuantity = "";

            if (line.Length > ingredient.Quantity.Length)
            {
                textAfterQuantity = line.Substring(ingredient.Quantity.Length).Trim();
            }

            foreach (string unit in Units)
            {
                if (textAfterQuantity.ToLower().StartsWith(unit.ToLower()))
                {
                    ingredient.Unit = unit;
                    break;
                }
            }
        }
        int startIndex = 0;

        if (!string.IsNullOrEmpty(ingredient.Quantity))
        {
            startIndex++;
        }

        if (!string.IsNullOrEmpty(ingredient.Unit))
        {
            startIndex++;
        }

        string name = line;

        if (!string.IsNullOrEmpty(ingredient.Quantity))
        {
            name = name.Substring(ingredient.Quantity.Length).Trim();
        }

        if (!string.IsNullOrEmpty(ingredient.Unit))
        {
            if (name.StartsWith(ingredient.Unit))
            {
                name = name.Substring(ingredient.Unit.Length).Trim();
            }
        }

        if (name.StartsWith("de "))
        {
            name = name.Substring(3);
        }
        if (name.StartsWith("d'"))
        {
            name = name.Substring(2);
        }

        ingredient.Name = name;


        return ingredient;
    }
}