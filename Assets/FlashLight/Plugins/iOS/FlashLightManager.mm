//
//  FlashLightManager.mm
//  FlashLightManagerPlugin
//
//  Created by Mayank Gupta on 19/07/17.
//  Copyright (c) 2017 Mayank Gupta. All rights reserved.
//

#import "FlashLightManager.h"

@interface FlashLightManager() {
    UnityAppController *objectUnityAppController;
    NSString *msgReceivingGameObjectNameGlobal;
    NSString *msgReceivingMethodtNameGlobal;
}
 
@end

@implementation FlashLightManager
#pragma mark Unity bridge
    
+ (FlashLightManager *)pluginSharedInstance {
    static FlashLightManager *sharedInstance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedInstance = [[FlashLightManager alloc] init];
    });
    return sharedInstance;
}



#pragma mark Ios Methods

- (void) switchOnFlashLight
{
    AVCaptureDevice *flashLight = [AVCaptureDevice defaultDeviceWithMediaType:AVMediaTypeVideo];
    if ([flashLight isTorchAvailable] && [flashLight isTorchModeSupported:AVCaptureTorchModeOn])
    {
        BOOL success = [flashLight lockForConfiguration:nil];
        if (success)
        {
            [flashLight setTorchMode:AVCaptureTorchModeOn];
            [self sendMessageToUnity:@"On"];
            [flashLight unlockForConfiguration];
        }
    }
}

- (void) switchOffFlashLight
{
    AVCaptureDevice *flashLight = [AVCaptureDevice defaultDeviceWithMediaType:AVMediaTypeVideo];
    if ([flashLight isTorchAvailable] && [flashLight isTorchModeSupported:AVCaptureTorchModeOn])
    {
        BOOL success = [flashLight lockForConfiguration:nil];
        if (success)
        {
            [flashLight setTorchMode:AVCaptureTorchModeOff];
            [self sendMessageToUnity:@"Off"];
            [flashLight unlockForConfiguration];
        }
    }
}

- (void) toggleFlashlight
{
    AVCaptureDevice *flashLight = [AVCaptureDevice defaultDeviceWithMediaType:AVMediaTypeVideo];
    if ([flashLight isTorchAvailable] && [flashLight isTorchModeSupported:AVCaptureTorchModeOn])
    {
        BOOL success = [flashLight lockForConfiguration:nil];
        if (success)
        {
            if ([flashLight isTorchActive]) {
                [flashLight setTorchMode:AVCaptureTorchModeOff];
                [self sendMessageToUnity:@"Off"];
            } else {
                [flashLight setTorchMode:AVCaptureTorchModeOn];
                [self sendMessageToUnity:@"On"];
            }
            [flashLight unlockForConfiguration];
        }
    }
}

- (void) checkFlashlightStatus
{
    AVCaptureDevice *flashLight = [AVCaptureDevice defaultDeviceWithMediaType:AVMediaTypeVideo];
    if ([flashLight isTorchAvailable] && [flashLight isTorchModeSupported:AVCaptureTorchModeOn])
    {
        BOOL success = [flashLight lockForConfiguration:nil];
        if (success)
        {
            if ([flashLight isTorchActive]) {
                [self sendMessageToUnity:@"On"];
            } else {
                [self sendMessageToUnity:@"Off"];
            }
            [flashLight unlockForConfiguration];
        }
    }
}

//-------------------------------------------------------------------------------------------------

//-------------------------------------------------------------------------------------------------

-(void) sendMessageToUnity : (NSString *) msg {
    const char *msgImageSaved = [msg cStringUsingEncoding:NSASCIIStringEncoding];
    const char *gameObjectName = [msgReceivingGameObjectNameGlobal cStringUsingEncoding:NSASCIIStringEncoding];
    const char *methodName = [msgReceivingMethodtNameGlobal cStringUsingEncoding:NSASCIIStringEncoding];
    UnitySendMessage(gameObjectName,methodName, msgImageSaved);
}

- (NSString *)encodeNSDataToString:(NSData *)theData {
    return [theData base64EncodedStringWithOptions:NSDataBase64Encoding64CharacterLineLength];
}

- (NSData *)decodeNsStringToNsData:(NSString *)theData {
    NSLog(@"===image decodeNSDataToString==%@==", theData);
    NSData *nsdataFromBase64String = [[NSData alloc] initWithBase64EncodedString:theData options:NSDataBase64DecodingIgnoreUnknownCharacters];
    return nsdataFromBase64String;
}


-(void) setCallBackMethod: (NSString *)gameObject
                         : (NSString *)methodName {
    msgReceivingGameObjectNameGlobal = gameObject;
    msgReceivingMethodtNameGlobal = methodName;
}
@end

// Helper method used to convert NSStrings into C-style strings.
NSString *CreateStr(const char* string) {
    if (string) {
        return [NSString stringWithUTF8String:string];
    } else {
        return [NSString stringWithUTF8String:""];
    }
}


// Unity can only talk directly to C code so use these method calls as wrappers
// into the actual plugin logic.
extern "C" {
    void _switchOnFlashLight(){
        FlashLightManager *obj = [FlashLightManager pluginSharedInstance];
        [obj switchOnFlashLight];
    }

    void _switchOffFlashLight(){
        FlashLightManager *obj = [FlashLightManager pluginSharedInstance];
        [obj switchOffFlashLight];
    }

    void _toggleFlashLight(){
        FlashLightManager *obj = [FlashLightManager pluginSharedInstance];
        [obj toggleFlashlight];
    }

    void _checkFlashLightStatus(){
        FlashLightManager *obj = [FlashLightManager pluginSharedInstance];
        [obj checkFlashlightStatus];
    }

    void _setCallBackMethod(const char *msgReceivingGameObjectName, const char *msgReceivingMethodName) {
        FlashLightManager *obj = [FlashLightManager pluginSharedInstance];
        [obj setCallBackMethod:CreateStr(msgReceivingGameObjectName)
                              :CreateStr(msgReceivingMethodName)];
    }
}
