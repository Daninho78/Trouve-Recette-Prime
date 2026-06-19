using System;

public interface IOCRService
{
    void RecognizeTextFromImage(string imagePath, Action<string> onTextRecognized);
}