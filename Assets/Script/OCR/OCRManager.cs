using System;
using UnityEngine;

public class OCRManager : MonoBehaviour
{
    private IOCRService ocrService;

    private void Awake()
    {
#if UNITY_IOS && !UNITY_EDITOR
    ocrService = new AppleVisionOCRService();
    Debug.Log("OCRManager : service Apple Vision sélectionné.");
#elif UNITY_ANDROID && !UNITY_EDITOR
    Debug.Log("OCRManager : Android pas encore implémenté.");
#else
        ocrService = new AppleVisionOCRService();
        Debug.Log("OCRManager : mode Editor / test.");
#endif
    }

    public void RecognizeTextFromImage(string imagePath, Action<string> onTextRecognized)
    {
        Debug.Log("Demande OCR reçue pour : " + imagePath);

        ocrService.RecognizeTextFromImage(imagePath, onTextRecognized);
    }
}