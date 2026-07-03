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

foreach (string ingredient in result.ingredients)
{
    ingredientsBuilder.AppendLine("- " + ingredient);
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