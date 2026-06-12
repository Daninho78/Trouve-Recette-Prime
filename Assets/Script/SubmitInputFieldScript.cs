using TMPro;
using UnityEngine;

public class SubmitInputFieldScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private IngredientItemUI ingredientItemUI;

    private void Awake()
    {
        if (inputField == null)
            inputField = GetComponent<TMP_InputField>();

        if (ingredientItemUI == null)
            ingredientItemUI = GetComponentInParent<IngredientItemUI>();
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

        if (ingredientItemUI != null)
        {
            ingredientItemUI.ValidateIngredientFirstSuggestion();
        }
    }
}