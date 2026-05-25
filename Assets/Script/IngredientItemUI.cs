using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientItemUI : MonoBehaviour
{
    public TMP_Dropdown unitDropdown;
    public GameObject customQuantityInput;

    public GameObject suggestionButtonPrefab;
    public Transform suggestionsPanel;

    private Ingredient firstSuggestion;

    public void OnUnitChanged()
    {
        string selected = unitDropdown.options[unitDropdown.value].text;
        customQuantityInput.SetActive(selected == "Autre");
    }

    public void RemoveItem()
    {
        Destroy(gameObject);
    }

    //affiche la suggestion d'ingredients
    public void OnIngredientInputChanged(string value)
    {

        Debug.Log("Input ingrédient changé : " + value);

        IngredientInputData data = GetComponent<IngredientInputData>();

        if (data != null && data.selectedIngredient != null && value == data.selectedIngredient.Name)
        {
            foreach (Transform child in suggestionsPanel)
            {
                Destroy(child.gameObject);
            }

            suggestionsPanel.gameObject.SetActive(false);
            return;
        }
        foreach (Transform child in suggestionsPanel)
        {
            Destroy(child.gameObject);
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            suggestionsPanel.gameObject.SetActive(false);
            return;
        }

        var manager = FindObjectOfType<IngredientSuggestionManager>();

        if (manager == null)
        {
            Debug.LogWarning("IngredientSuggestionManager introuvable.");
            return;
        }

        var suggestions = manager.GetSuggestions(value);
        firstSuggestion = suggestions.Count > 0 ? suggestions[0] : null;

        if (suggestions.Count == 0)
        {
            suggestionsPanel.gameObject.SetActive(false);
            return;
        }

        suggestionsPanel.gameObject.SetActive(true);

        foreach (var ingredient in suggestions)
        {
            GameObject buttonObject = Instantiate(suggestionButtonPrefab, suggestionsPanel);

            TextMeshProUGUI text = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = ingredient.Name;
            }
            Button button = buttonObject.GetComponent<Button>();
            if (button != null)
            {
                Ingredient capturedIngredient = ingredient;
                button.onClick.AddListener(() => SelectSuggestion(capturedIngredient));
            }
        }
    }

    private void SelectSuggestion(Ingredient ingredient)
    {
        IngredientInputData data = GetComponent<IngredientInputData>();

        data.selectedIngredient = ingredient;
        data.nameInput.text = ingredient.Name;

        suggestionsPanel.gameObject.SetActive(false);

        if (data.quantityInput != null)
        {
            data.quantityInput.Select();
            data.quantityInput.ActivateInputField();
        }

        IngredientInputManager2 manager = GetComponentInParent<IngredientInputManager2>();

        if (manager != null)
        {
            manager.EnsureEmptyLineAtEnd();
        }


    }

    public void OnIngredientEndEdit(string value)
    {
        Debug.Log("End edit ingrédient : " + value);
        if (firstSuggestion != null)
        {
            SelectSuggestion(firstSuggestion);
        }
    }

    public void OnQuantityEndEdit(string value)
    {
        IngredientInputData data = GetComponent<IngredientInputData>();

        if (data == null || data.unitInput == null)
            return;

        data.unitInput.Select();
        data.unitInput.ActivateInputField();
    }

    public void OnUnitEndEdit(string value)
    {
        IngredientInputManager2 manager = GetComponentInParent<IngredientInputManager2>();

        if (manager != null)
        {
            manager.EnsureEmptyLineAtEnd();

            Transform lastItem = manager.transform.GetChild(manager.transform.childCount - 1);
            IngredientInputData lastData = lastItem.GetComponent<IngredientInputData>();

            if (lastData != null && lastData.nameInput != null)
            {
                lastData.nameInput.Select();
                lastData.nameInput.ActivateInputField();
            }
        }
    }
}