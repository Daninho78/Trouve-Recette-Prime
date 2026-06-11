using TMPro;
using UnityEngine;
using System.Collections.Generic;


public class IngredientInputManager2 : MonoBehaviour
{
    public GameObject ingredientItemPrefab;

    public GameObject CreateNewInput()
    {
        GameObject newInput = Instantiate(ingredientItemPrefab, transform);
        var data = newInput.GetComponent<IngredientInputData>();

        data.quantityInput.text = "";
        data.nameInput.text = "";
        
        data.unitInput.text = "";
        return newInput;
    }

    public void OnInputEndEdit(TMP_InputField currentInput)
    {
        if (string.IsNullOrWhiteSpace(currentInput.text))
            return;

        Transform ingredientItem = currentInput.transform.parent.parent;

        if (ingredientItem.GetSiblingIndex() != transform.childCount - 1)
            return;

        // On crée une nouvelle ligne vide en dessous
        CreateNewInput();

        // Mais on garde le focus sur la quantité de la ligne actuelle
        var currentData = ingredientItem.GetComponent<IngredientInputData>();
        currentData.quantityInput.Select();
        currentData.quantityInput.ActivateInputField();
    }

   
     public List<(Ingredient ingredient, int quantity, string unit, string quantityText)> GetAllIngredients()
        {
            List<(Ingredient, int, string, string)> ingredients = new();

            foreach (Transform child in transform)
            {
                var data = child.GetComponent<IngredientInputData>();
                if (data == null) continue;

                Ingredient selectedIngredient = data.selectedIngredient;

                if (selectedIngredient == null)
                continue;

            string selectedUnit = data.unitInput.text.Trim();
            if (string.IsNullOrWhiteSpace(selectedUnit))
                selectedUnit = null;

            if (selectedUnit == "Autre")
                {
                    string quantityText = data.customQuantityInput.text;

                    ingredients.Add((selectedIngredient, 0, null, quantityText));
            }
                else
                {
                    int.TryParse(data.quantityInput.text, out int quantity);

                    ingredients.Add((selectedIngredient, quantity, selectedUnit, null));
            }
            }

            return ingredients;
     }
    

    public void ClearInputs()
    {
        if (transform.childCount == 0)
            return;

        //On garde le premier input
        TMP_InputField firstInput = transform.GetChild(0).GetComponent<TMP_InputField>();

        if (firstInput != null)
        {
            firstInput.text = "";
        }

        //On supprime tous les autres
        for (int i = transform.childCount - 1; i >= 1; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        // On remet le focus sur le premier
        if (firstInput != null)
        {
            firstInput.Select();
            firstInput.ActivateInputField();
        }

    }

    public void OnUnitChanged(TMP_Dropdown dropdown, GameObject customInput)
    {
        string selected = dropdown.options[dropdown.value].text;

        if (selected == "Autre...")
        {
            customInput.SetActive(true);
        }
        else
        {
            customInput.SetActive(false);
        }
    }

    public void EnsureEmptyLineAtEnd()
    {
        if (transform.childCount == 0)
        {
            CreateNewInput();
            return;
        }

        Transform lastItem = transform.GetChild(transform.childCount - 1);
        IngredientInputData data = lastItem.GetComponent<IngredientInputData>();

        if (data != null && !string.IsNullOrWhiteSpace(data.nameInput.text))
        {
            CreateNewInput();
        }
    }

    /*public void OnUnitEndEdit(string value)
    {
        IngredientInputManager2 manager = GetComponentInParent<IngredientInputManager2>();

        if (manager != null)
        {
            //manager.EnsureEmptyLineAtEnd();

            Transform lastItem = manager.transform.GetChild(manager.transform.childCount - 1);
            IngredientInputData lastData = lastItem.GetComponent<IngredientInputData>();

            if (lastData != null && lastData.nameInput != null)
            {
                lastData.nameInput.Select();
                lastData.nameInput.ActivateInputField();
            }
        }
    }*/

    public GameObject CreateInputFromData(Ingredient ingredient, string quantity, string unit)
    {
        GameObject newInput = CreateNewInput();

        IngredientInputData data = newInput.GetComponent<IngredientInputData>();
        IngredientItemUI ui = newInput.GetComponent<IngredientItemUI>();

        if (ui != null)
            ui.suppressSuggestions = true;

        data.selectedIngredient = ingredient;

        data.nameInput.SetTextWithoutNotify(ingredient.Name);
        data.quantityInput.SetTextWithoutNotify(quantity);
        data.unitInput.SetTextWithoutNotify(unit);


        if (ui != null)
            ui.suppressSuggestions = false;
        ui.ClearSuggestionState();

        return newInput;
    }

    public void ClearAllInputs()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

}