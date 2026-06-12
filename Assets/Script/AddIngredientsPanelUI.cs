using System.Collections.Generic;
using UnityEngine;

public class AddIngredientsPanelUI : MonoBehaviour
{
    public IngredientInputData inputLine;
    public IngredientInputManager2 ingredientsContainer;

    

    public void AddIngredientToList()
    {
        if (inputLine.selectedIngredient == null)
        {
            Debug.LogWarning("Aucun ingrédient sélectionné.");
            return;
        }

        

        ingredientsContainer.CreateInputFromData(
            inputLine.selectedIngredient,
            inputLine.quantityInput.text,
            inputLine.unitInput.text
        );

        inputLine.selectedIngredient = null;
        inputLine.nameInput.text = "";
        IngredientItemUI inputUI = inputLine.GetComponent<IngredientItemUI>();
        if (inputUI != null)
        {
            inputUI.ClearSuggestionState();
        }
        inputLine.quantityInput.text = "";
        inputLine.unitInput.text = "";

        inputLine.nameInput.Select();
        inputLine.nameInput.ActivateInputField();

        
    }


    
}