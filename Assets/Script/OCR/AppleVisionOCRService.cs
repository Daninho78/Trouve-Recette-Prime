using System;
using UnityEngine;
using System.Runtime.InteropServices;

public class AppleVisionOCRService : IOCRService

{
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void StartAppleVisionOCR(string imagePath);
#endif

    public void RecognizeTextFromImage(string imagePath, Action<string> onTextRecognized)
    {
        Debug.Log("Apple Vision OCR demandé pour : " + imagePath);

#if UNITY_IOS && !UNITY_EDITOR
        StartAppleVisionOCR(imagePath);
        onTextRecognized?.Invoke("Appel Apple Vision envoyé à iOS.");
#else
        onTextRecognized?.Invoke("Mode Unity Editor : Apple Vision fonctionne seulement sur iPhone.");
#endif
    }
}