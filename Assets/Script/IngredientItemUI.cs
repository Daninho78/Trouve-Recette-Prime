using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IngredientItemUI : MonoBehaviour
{
    public GameObject customQuantityInput;

    public GameObject suggestionButtonPrefab;
    public Transform suggestionsPanel;

    public GameObject suggestionRow;

    private Ingredient firstSuggestion;
    private Unit firstUnitSuggestion;

    public Transform unitSuggestionsPanel;

    public bool suppressSuggestions = false;

   


    private void Update()
    {
        IngredientInputData data = GetComponent<IngredientInputData>();

        if (data == null || data.nameInput == null)
            return;

        if (data.nameInput.isFocused && Input.GetKeyDown(KeyCode.Return))
        {
            if (firstSuggestion != null)
            {
                SelectSuggestion(firstSuggestion);
            }
        }
    }


    public void RemoveItem()
    {
        Destroy(gameObject);
    }

    //affiche la suggestion d'ingredients
    public void OnIngredientInputChanged(string value)
    {
        if (suppressSuggestions)
            return;

        Debug.Log("Input ingrédient changé : " + value);

        IngredientInputData data = GetComponent<IngredientInputData>();

        if (data != null && data.selectedIngredient != null && value == data.selectedIngredient.Name)
        {
            foreach (Transform child in suggestionsPanel)
            {
                Destroy(child.gameObject);
            }

            suggestionRow.SetActive(false);
            suggestionsPanel.gameObject.SetActive(false);
            return;
        }
        foreach (Transform child in suggestionsPanel)
        {
            Destroy(child.gameObject);
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            suggestionRow.SetActive(false);
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
            suggestionRow.SetActive(false);
            suggestionsPanel.gameObject.SetActive(false);
            return;
        }

        suggestionRow.SetActive(true);
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
                button.onClick.AddListener(() =>
                {
                 
                    SelectSuggestion(capturedIngredient);
                });
            }
        }
    }

    private void SelectSuggestion(Ingredient ingredient)
    {
        IngredientInputData data = GetComponent<IngredientInputData>();

        data.selectedIngredient = ingredient;
        data.nameInput.text = ingredient.Name;

        suggestionRow.SetActive(false);
        suggestionsPanel.gameObject.SetActive(false);

        if (data.quantityInput != null)
        {
            data.quantityInput.Select();
            data.quantityInput.ActivateInputField();
        }

        

    }

    public void OnIngredientEndEdit(string value)
    {
        Debug.Log("End edit ingrédient : " + value);
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
        if (firstUnitSuggestion != null)
        {
            SelectUnitSuggestion(firstUnitSuggestion);
        }

        
    }

    public void OnUnitInputChanged(string value)
    {
        foreach (Transform child in unitSuggestionsPanel)
        {
            Destroy(child.gameObject);
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            unitSuggestionsPanel.gameObject.SetActive(false);
            CheckSuggestionRowVisibility();
            return;
        }

        var manager = FindObjectOfType<UnitSuggestionManager>();

        if (manager == null)
        {
            Debug.LogWarning("UnitSuggestionManager introuvable.");
            return;
        }

        var suggestions = manager.GetSuggestions(value);
        firstUnitSuggestion = suggestions.Count > 0 ? suggestions[0] : null;

        if (suggestions.Count == 0)
        {
            unitSuggestionsPanel.gameObject.SetActive(false);
            CheckSuggestionRowVisibility();
            return;
        }

        suggestionRow.SetActive(true);
        unitSuggestionsPanel.gameObject.SetActive(true);

        foreach (var unit in suggestions)
        {
            GameObject buttonObject = Instantiate(suggestionButtonPrefab, unitSuggestionsPanel);

            TextMeshProUGUI text = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = unit.Name;
            }

            Button button = buttonObject.GetComponent<Button>();
            if (button != null)
            {
                Unit capturedUnit = unit;
                button.onClick.AddListener(() => SelectUnitSuggestion(capturedUnit));
            }
        }
    }

    private void CheckSuggestionRowVisibility()
    {
        bool ingredientPanelActive = suggestionsPanel.gameObject.activeSelf;
        bool unitPanelActive = unitSuggestionsPanel.gameObject.activeSelf;

        suggestionRow.SetActive(ingredientPanelActive || unitPanelActive);
    }

    private void SelectUnitSuggestion(Unit unit)
    {
        IngredientInputData data = GetComponent<IngredientInputData>();

        if (data == null || data.unitInput == null)
            return;

        data.unitInput.text = unit.Name;


        unitSuggestionsPanel.gameObject.SetActive(false);
        CheckSuggestionRowVisibility();
    }

    public void ScrollThisItemToTop()
    {
        Debug.Log("Scroll demandé pour : " + gameObject.name);

        ScrollToIngredientItem scrollHelper = FindObjectOfType<ScrollToIngredientItem>();

        if (scrollHelper == null)
            return;

        RectTransform itemRect = GetComponent<RectTransform>();

        scrollHelper.ScrollToBottom();
    }

    public void ClearSuggestionState()
    {
        firstSuggestion = null;
        firstUnitSuggestion = null;

        suggestionsPanel.gameObject.SetActive(false);
        unitSuggestionsPanel.gameObject.SetActive(false);
        CheckSuggestionRowVisibility();
    }

    public void ValidateIngredientWithEnter()
    {
        if (firstSuggestion != null)
        {
            SelectSuggestion(firstSuggestion);
        }
    }

    public void ValidateUnitWithEnter()
    {
        if (firstUnitSuggestion != null)
        {
            SelectUnitSuggestion(firstUnitSuggestion);
        }
    }

    public void ValidateIngredientFirstSuggestion()
    {
        if (firstSuggestion != null)
        {
            SelectSuggestion(firstSuggestion);
        }
    }
}