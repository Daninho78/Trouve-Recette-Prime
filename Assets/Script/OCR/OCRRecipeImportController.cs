using TMPro;
using UnityEngine;
using System.Text;

public class OCRRecipeImportController : MonoBehaviour
{
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private OCRManager ocrManager;

    [SerializeField] private TMP_InputField titleInput;
    [SerializeField] private TMP_InputField portionsInput;
    [SerializeField] private TMP_InputField preparationInput;
    [SerializeField] private TMP_InputField cookingInput;
    [SerializeField] private Transform ingredientsContainer;
    [SerializeField] private IngredientItemUI ingredientItemPrefab;
    [SerializeField] private IngredientSuggestionManager ingredientSuggestionManager;

    public void OnTakePhotoButtonClicked()
    {
        resultText.text = "Demande OCR en cours...";

        NativeGallery.GetImageFromGallery((path) =>
{
    if (path == null)
    {
        resultText.text = "Aucune image sélectionnée.";
        return;
    }

    resultText.text = "Image sélectionnée. OCR en cours...";
    ocrManager.RecognizeTextFromImage(path, OnTextRecognized);

}, "Choisir une image pour l'OCR", "image/*");
    }

    private void OnTextRecognized(string recognizedText)
    {
        RecipeOCRResult result = RecipeOCRParser.Parse(recognizedText);
        titleInput.text = result.title;
        portionsInput.text = result.portions;
        preparationInput.text = result.preparationTime;
        cookingInput.text = result.cookingTime;

        foreach (Transform child in ingredientsContainer)
        {
            Destroy(child.gameObject);
        }

        StringBuilder cacheDebug = new StringBuilder();

        foreach (string ingredientLine in result.ingredients)
        {
            RecipeOCRIngredient parsedIngredient = IngredientParser.Parse(ingredientLine);

            GameObject item = Instantiate(ingredientItemPrefab.gameObject, ingredientsContainer);

            IngredientInputData data = item.GetComponent<IngredientInputData>();
            IngredientItemUI itemUI = item.GetComponent<IngredientItemUI>();

            if (data != null)
            {
                data.nameInput.text = parsedIngredient.Name;
                data.quantityInput.text = parsedIngredient.Quantity;
                data.unitInput.text = parsedIngredient.Unit;

                Ingredient ingredient = ingredientSuggestionManager.FindExactIngredient(parsedIngredient.Name);

                Debug.Log($"OCR : {parsedIngredient.Name} -> {(ingredient != null ? "Trouvé" : "Introuvable")}");
                cacheDebug.AppendLine(
                    $"{parsedIngredient.Name} -> {(ingredient != null ? "Trouvé" : "Introuvable")}"
                    );

                itemUI.ClearSuggestionState();
            }

        }

        resultText.text =
     "--- TEST CACHE ---\n\n" +
     cacheDebug.ToString();
    }
}