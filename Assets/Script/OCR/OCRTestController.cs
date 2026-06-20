using TMPro;
using UnityEngine;

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
        resultText.text = recognizedText;
    }
}