#import <Foundation/Foundation.h>

extern "C"
{
    void StartAppleVisionOCR(const char* imagePath)
    {
        NSLog(@"AppleVisionOCR appelé avec imagePath : %s", imagePath);
    }
}