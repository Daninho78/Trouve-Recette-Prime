using System;
using UnityEngine;

public class OCRManager : MonoBehaviour
{
    private IOCRService ocrService;

    public static OCRManager Instance;

    private Action<string> currentCallback;

    private void Awake()
    {
        Instance = this;

#if UNITY_IOS && !UNITY_EDITOR
    ocrService = new AppleVisionOCRService();
    Debug.Log("OCRManager : service Apple Vision s�lectionn�.");
#elif UNITY_ANDROID && !UNITY_EDITOR
    Debug.Log("OCRManager : Android pas encore impl�ment�.");
#else
        ocrService = new AppleVisionOCRService();
        Debug.Log("OCRManager : mode Editor / test.");
#endif
    }

    public void RecognizeTextFromImage(string imagePath, Action<string> onTextRecognized)
    {
        currentCallback = onTextRecognized;

        Debug.Log("Demande OCR re�ue pour : " + imagePath);

        ocrService.RecognizeTextFromImage(imagePath, onTextRecognized);
    }

    public void OnOCRTextRecognized(string recognizedText)
{
    Debug.Log("Texte OCR reçu depuis iOS : " + recognizedText);

    currentCallback?.Invoke(recognizedText);
}
}