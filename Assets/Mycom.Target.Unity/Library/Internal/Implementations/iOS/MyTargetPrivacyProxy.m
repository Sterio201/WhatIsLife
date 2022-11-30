#import <MyTargetSDK/MTRGPrivacy.h>

bool MTRGGetUserConsent()
{
    return (bool)[MTRGPrivacy userConsent];
}

void MTRGSetUserConsent(bool userConsent)
{
    BOOL value = userConsent == true ? YES : NO;
    [MTRGPrivacy setUserConsent:value];
}

bool MTRGGetUserAgeRestricted()
{
    return (bool)[MTRGPrivacy userAgeRestricted];
}

void MTRGSetUserAgeRestricted(bool userAgeRestricted)
{
    BOOL value = userAgeRestricted == true ? YES : NO;
    [MTRGPrivacy setUserAgeRestricted:value];
}
