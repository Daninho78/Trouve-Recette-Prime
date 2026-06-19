using TMPro;
using UnityEngine;

public class OCRTestController : MonoBehaviour
{
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private OCRManager ocrManager;

    public void OnTakePhotoButtonClicked()
    {
        resultText.text = "Demande OCR en cours...";

        ocrManager.RecognizeTextFromImage("test_image_path", OnTextRecognized);
    }

    private void OnTextRecognized(string recognizedText)
    {
        resultText.text = recognizedText;
    }
}