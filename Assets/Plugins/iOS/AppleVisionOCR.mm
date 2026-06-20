#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <Vision/Vision.h>

extern void UnitySendMessage(const char* obj, const char* method, const char* msg);

static void SendTextToUnity(NSString* text)
{
    UnitySendMessage(
        "OCR_Manager",
        "OnOCRTextRecognized",
        [text UTF8String]
    );
}

extern "C"
{
    void StartAppleVisionOCR(const char* imagePath)
    {
        NSLog(@"AppleVisionOCR appelé avec imagePath : %s", imagePath);

        NSString* path = [NSString stringWithUTF8String:imagePath];
        UIImage* image = [UIImage imageWithContentsOfFile:path];

        if (image == nil)
        {
            SendTextToUnity(@"Image introuvable ou illisible.");
            return;
        }

        VNRecognizeTextRequest* request = [[VNRecognizeTextRequest alloc] initWithCompletionHandler:^(VNRequest* request, NSError* error)
        {
            if (error != nil)
            {
                SendTextToUnity([NSString stringWithFormat:@"Erreur OCR : %@", error.localizedDescription]);
                return;
            }

            NSMutableString* resultText = [NSMutableString string];

            for (VNRecognizedTextObservation* observation in request.results)
            {
                VNRecognizedText* bestCandidate = [[observation topCandidates:1] firstObject];

                if (bestCandidate != nil)
                {
                    [resultText appendString:bestCandidate.string];
                    [resultText appendString:@"\n"];
                }
            }

            if (resultText.length == 0)
            {
                SendTextToUnity(@"Aucun texte détecté.");
            }
            else
            {
                SendTextToUnity(resultText);
            }
        }];

        request.recognitionLevel = VNRequestTextRecognitionLevelAccurate;
        request.usesLanguageCorrection = YES;

        VNImageRequestHandler* handler = [[VNImageRequestHandler alloc] initWithCGImage:image.CGImage options:@{}];

        NSError* error = nil;
        [handler performRequests:@[request] error:&error];

        if (error != nil)
        {
            SendTextToUnity([NSString stringWithFormat:@"Erreur Vision : %@", error.localizedDescription]);
        }
    }
}
