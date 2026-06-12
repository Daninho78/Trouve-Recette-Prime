using TMPro;
using UnityEngine;

public class InputEnterValidator : MonoBehaviour
{
    public TMP_InputField inputField;
    public IngredientItemUI ingredientItemUI;

    public enum ValidationType
    {
        Ingredient,
        Unit
    }

    public ValidationType validationType;

    private void Update()
    {
        if (inputField == null || ingredientItemUI == null)
            return;

        if (!inputField.isFocused)
            return;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (validationType == ValidationType.Ingredient)
            {
                ingredientItemUI.ValidateIngredientFirstSuggestion();
            }
            /*else if (validationType == ValidationType.Unit)
            {
                ingredientItemUI.ValidateUnitFirstSuggestion();
            }*/
        }
    }
}