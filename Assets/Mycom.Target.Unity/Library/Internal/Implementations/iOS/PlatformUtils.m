#import "PlatformUtils.h"

const char *mtrg_unity_makeStringCopy(NSString *sourceString)
{
    if (sourceString)
    {
        const char *tempResult = [sourceString UTF8String];
        if (tempResult)
        {
            char *result = (char *) malloc(strlen(tempResult) + 1);
            strcpy(result, tempResult);
            return result;
        }
    }

    return NULL;
}

UIViewController *mtrg_unity_topViewController()
{
    UIViewController *topViewController = [UIApplication sharedApplication].keyWindow.rootViewController;
    while (topViewController.presentedViewController)
    {
        topViewController = topViewController.presentedViewController;
    }
    return topViewController;
}

void MTRGDebugLog(const char *message)
{
    if (message)
    {
        NSString *messageString = [NSString stringWithUTF8String:message];
        if (messageString)
        {
            NSLog(@"%@", messageString);
        }
    }
}
