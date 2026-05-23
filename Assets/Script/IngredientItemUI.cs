using TMPro;
using UnityEngine;

public class IngredientItemUI : MonoBehaviour
{
    public TMP_Dropdown unitDropdown;
    public GameObject customQuantityInput;

    public GameObject suggestionButtonPrefab;
    public Transform suggestionsPanel;

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
        }
    }
}