using TMPro;
using UnityEngine;
using System.Text;

public class OCRTestController : MonoBehaviour
{
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private OCRManager ocrManager;

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

        StringBuilder ingredientsBuilder = new StringBuilder();

        foreach (string ingredientLine in result.ingredients)
        {
            RecipeOCRIngredient parsedIngredient = IngredientParser.Parse(ingredientLine);

            ingredientsBuilder.AppendLine("• " + parsedIngredient.Name);

            if (!string.IsNullOrEmpty(parsedIngredient.Quantity))
            {
                ingredientsBuilder.AppendLine("  Quantité : " + parsedIngredient.Quantity);
            }

            if (!string.IsNullOrEmpty(parsedIngredient.Unit))
            {
                ingredientsBuilder.AppendLine("  Unité : " + parsedIngredient.Unit);
            }

            ingredientsBuilder.AppendLine();
        }

        resultText.text =
            "Titre détecté : " + result.title +
            "\nPortions : " + result.portions +
            "\nPréparation : " + result.preparationTime +
            "\nCuisson : " + result.cookingTime +
            "\nIngrédients détectés : " + result.ingredientCount +
            "\n\nListe des ingrédients :\n" +
            ingredientsBuilder.ToString() +
            "\n\n--- TEXTE OCR ---\n\n" +
            recognizedText;
    }
}