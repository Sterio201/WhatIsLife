#import "MTRGUnityTracer.h"
#import <MyTargetSDK/MTRGInterstitialAd.h>

void MTRGManagerSetLoggingEnabled(bool isEnabled)
{
    BOOL isEnabledVal = isEnabled == true;
    [MTRGUnityTracer setEnabled:isEnabledVal];
    mtrg_unity_tracer_d(@"MTRGManagerSetLoggingEnabled = %@", isEnabledVal ? @"true" : @"false");
    [MTRGInterstitialAd setDebugMode:isEnabledVal];
}

bool MTRGManagerGetLoggingEnabled()
{
    mtrg_unity_tracer_d(@"MTRGManagerGetLoggingEnabled");
    return [MTRGInterstitialAd isDebugMode];
}