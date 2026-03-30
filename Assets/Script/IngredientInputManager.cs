using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class IngredientInputManager : MonoBehaviour
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

        //Ne fais rien si ce n'est pas le dernier input
        if (currentInput.transform.GetSiblingIndex() != transform.childCount - 1)
            return;

        TMP_InputField newInput = CreateNewInput();

        // Focus sur le nouveau champ
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
}
