using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class IngredientInputManager2 : MonoBehaviour
{
    public TMP_InputField inputPrefab;

    public TMP_InputField CreateNewInput()
    {
        TMP_InputField newInput = Instantiate(inputPrefab, transform);
        newInput.text = "";
        return newInput;
    }

    public void OnInputEndEdit(TMP_InputField currentInput)
    {
        if (string.IsNullOrWhiteSpace(currentInput.text))
            return;

        if (currentInput.transform.GetSiblingIndex() != transform.childCount - 1)
            return;

        TMP_InputField newInput = CreateNewInput();
        newInput.Select();
        newInput.ActivateInputField();
    }

    public List<string> GetAllIngredients()
    {
        List<string> ingredients = new List<string>();

        foreach (Transform child in transform)
        {
            TMP_InputField input = child.GetComponent<TMP_InputField>();

            if (input != null && !string.IsNullOrWhiteSpace(input.text))
            {
                ingredients.Add(input.text);
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
}