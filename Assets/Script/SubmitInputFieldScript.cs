using TMPro;
using UnityEngine;

public class SubmitInputFieldScript : MonoBehaviour
{
    public enum SubmitType
    {
        Ingredient,
        Unit
    }

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private IngredientItemUI ingredientItemUI;
    [SerializeField] private SubmitType submitType;
    [SerializeField] private AddIngredientsPanelUI addIngredientsPanelUI;

    private void Awake()
    {
        if (inputField == null)
            inputField = GetComponent<TMP_InputField>();

        if (ingredientItemUI == null)
            ingredientItemUI = GetComponentInParent<IngredientItemUI>();

        if (addIngredientsPanelUI == null)
            addIngredientsPanelUI = GetComponentInParent<AddIngredientsPanelUI>();
    }

    private void OnEnable()
    {
        if (inputField != null)
            inputField.onSubmit.AddListener(OnSubmit);
    }

    private void OnDisable()
    {
        if (inputField != null)
            inputField.onSubmit.RemoveListener(OnSubmit);
    }

    private void OnSubmit(string text)
    {
        Debug.Log("SUBMIT INPUT : " + text);

        if (ingredientItemUI == null)
            return;

        if (submitType == SubmitType.Ingredient)
        {
            ingredientItemUI.ValidateIngredientFirstSuggestion();
        }
        else if (submitType == SubmitType.Unit)
        {
            ingredientItemUI.ValidateUnitFirstSuggestion();

            if (addIngredientsPanelUI != null)
            {
                addIngredientsPanelUI.AddIngredientToList();
            }
        }
    }
}