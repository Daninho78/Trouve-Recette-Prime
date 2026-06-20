#import <Foundation/Foundation.h>
#import <Vision/Vision.h>

extern void UnitySendMessage(const char* obj, const char* method, const char* msg);

extern "C"
{
    void StartAppleVisionOCR(const char* imagePath)
    {
        NSLog(@"AppleVisionOCR appelé avec imagePath : %s", imagePath);
        
        VNRecognizeTextRequest* request = [[VNRecognizeTextRequest alloc] init];

        UnitySendMessage(
            "OCR_Manager",
            "OnOCRTextRecognized",
            "Texte reçu depuis iOS"
        );
    }
}
