#import "MTRGUnityObjectsManager.h"
#import "MTRGUnityTracer.h"

static const uint32_t MTRGUnityUndefinedObjectId = 0;

typedef void (*MTRGCallbackTypeAdId)(uint32_t);
typedef void (*MTRGCallbackTypeOnNoAdWithReason)(uint32_t, const char *);

#pragma mark - interstitial callbacks

MTRGCallbackTypeAdId _MTRGInterstitialAdCallbackOnLoad = NULL;
MTRGCallbackTypeAdId _MTRGInterstitialAdCallbackOnClick = NULL;
MTRGCallbackTypeAdId _MTRGInterstitialAdCallbackOnClose = NULL;
MTRGCallbackTypeAdId _MTRGInterstitialAdCallbackOnVideoComplete = NULL;
MTRGCallbackTypeAdId _MTRGInterstitialAdCallbackOnDisplay = NULL;
MTRGCallbackTypeOnNoAdWithReason _MTRGInterstitialAdCallbackOnNoAdWithReason = NULL;

#pragma mark - standard ads callbacks

MTRGCallbackTypeAdId _MTRGStandardAdCallbackOnAdLoadCompleted = NULL;
MTRGCallbackTypeAdId _MTRGStandardAdCallbackOnAdClicked = NULL;
MTRGCallbackTypeOnNoAdWithReason _MTRGStandardAdCallbackOnAdLoadFailed = NULL;
MTRGCallbackTypeAdId _MTRGStandardAdCallbackOnAdShown = NULL;

@implementation MTRGUnityObjectsManager
{
    NSMutableDictionary<NSNumber *, id> *_registeredObjects;
    uint32_t _activeObjectId;
}

+ (MTRGUnityObjectsManager *)sharedManager
{
    static MTRGUnityObjectsManager *_instance = nil;
    static dispatch_once_t once;
    dispatch_once(&once,
                  ^
                  {
                      _instance = [[MTRGUnityObjectsManager alloc] init];
                  });
    return _instance;
}

- (instancetype)init
{
    self = [super init];
    if (self)
    {
        _registeredObjects = [NSMutableDictionary new];
        _activeObjectId = 0;
    }
    return self;
}

- (uint32_t)numberToObjectId:(NSNumber *)number
{
    return number ? [number unsignedIntValue] : MTRGUnityUndefinedObjectId;
}

- (uint32_t)registerObject:(__strong id)object
{
    if (!object)
    {
        return MTRGUnityUndefinedObjectId;
    }

    @synchronized (self)
    {
        _activeObjectId++;
        uint32_t objectId = _activeObjectId;
        NSNumber *objectIdNum = @((objectId));
        _registeredObjects[objectIdNum] = object;
        return objectId;
    }
}

- (id)findObjectWithId:(uint32_t)objectId
{
    NSNumber *adIdNum = @((objectId));
    if (adIdNum)
    {
        @synchronized (self)
        {
            return _registeredObjects[adIdNum];
        }
    }
}

- (uint32_t)findObjectIdWithObject:(id)object
{
    @synchronized (self)
    {
        for (NSNumber *objectIdTemp in [_registeredObjects allKeys])
        {
            id obj = _registeredObjects[objectIdTemp];
            if (object == obj)
            {
                return [self numberToObjectId:objectIdTemp];
            }
        }
        return MTRGUnityUndefinedObjectId;
    }
}

- (void)unregisterObject:(uint32_t)objectId
{
    NSNumber *adIdNum = @((objectId));
    if (adIdNum)
    {
        @synchronized (self)
        {
            [_registeredObjects removeObjectForKey:adIdNum];
        }
    }
}

#pragma mark - MTRGInterstitialAdDelegate

- (void)onLoadWithInterstitialAd:(MTRGInterstitialAd *)interstitialAd
{
    uint32_t objectId = [self findObjectIdWithObject:interstitialAd];
    if (MTRGUnityUndefinedObjectId == objectId)
    {
        return;
    }

    MTRGCallbackTypeAdId callback = _MTRGInterstitialAdCallbackOnLoad;
    if (callback)
    {
        mtrg_unity_tracer_d(@"MTRGUnityProxy: onLoadWithInterstitialAd");

        callback(objectId);
    }
}

- (void)onNoAdWithReason:(NSString *)reason interstitialAd:(MTRGInterstitialAd *)interstitialAd
{
    uint32_t objectId = [self findObjectIdWithObject:interstitialAd];
    if (MTRGUnityUndefinedObjectId == objectId)
    {
        return;
    }

    MTRGCallbackTypeOnNoAdWithReason callback = _MTRGInterstitialAdCallbackOnNoAdWithReason;
    if (callback)
    {
        mtrg_unity_tracer_d(@"MTRGUnityProxy: onNoAdWithReason");

        NSString *reasonStr = reason ? reason : @"No Ad";
        callback(objectId, reasonStr.UTF8String);
    }
}

- (void)onClickWithInterstitialAd:(MTRGInterstitialAd *)interstitialAd
{
    uint32_t objectId = [self findObjectIdWithObject:interstitialAd];
    if (MTRGUnityUndefinedObjectId == objectId)
    {
        return;
    }

    MTRGCallbackTypeAdId callback = _MTRGInterstitialAdCallbackOnClick;
    if (callback)
    {
        mtrg_unity_tracer_d(@"MTRGUnityProxy: onClickWithInterstitialAd");

        callback(objectId);
    }
}

- (void)onCloseWithInterstitialAd:(MTRGInterstitialAd *)interstitialAd
{
    uint32_t objectId = [self findObjectIdWithObject:interstitialAd];
    if (MTRGUnityUndefinedObjectId == objectId)
    {
        return;
    }

    MTRGCallbackTypeAdId callback = _MTRGInterstitialAdCallbackOnClose;
    if (callback)
    {
        mtrg_unity_tracer_d(@"MTRGUnityProxy: onCloseWithInterstitialAd");

        callback(objectId);
    }
}

- (void)onVideoCompleteWithInterstitialAd:(MTRGInterstitialAd *)interstitialAd
{
    uint32_t objectId = [self findObjectIdWithObject:interstitialAd];
    if (MTRGUnityUndefinedObjectId == objectId)
    {
        return;
    }

    MTRGCallbackTypeAdId callback = _MTRGInterstitialAdCallbackOnVideoComplete;
    if (callback)
    {
        mtrg_unity_tracer_d(@"MTRGUnityProxy: onVideoCompleteWithInterstitialAd");

        callback(objectId);
    }
}

- (void)onDisplayWithInterstitialAd:(MTRGInterstitialAd *)interstitialAd
{
    uint32_t objectId = [self findObjectIdWithObject:interstitialAd];
    if (MTRGUnityUndefinedObjectId == objectId)
    {
        return;
    }

    MTRGCallbackTypeAdId callback = _MTRGInterstitialAdCallbackOnDisplay;
    if (callback)
    {
        mtrg_unity_tracer_d(@"MTRGUnityProxy: onDisplayWithInterstitialAd");

        callback(objectId);
    }
}

#pragma mark -- MTRGAdViewDelegate

- (void)onLoadWithAdView:(MTRGAdView *)adView
{
    uint32_t objectId = [self findObjectIdWithObject:adView];
    if (MTRGUnityUndefinedObjectId == objectId)
    {
        return;
    }

    MTRGCallbackTypeAdId callback = _MTRGStandardAdCallbackOnAdLoadCompleted;
    if (callback)
    {
        mtrg_unity_tracer_d(@"MTRGUnityProxy: onLoadWithAdView");

        callback(objectId);
    }
}

- (void)onNoAdWithReason:(NSString *)reason adView:(MTRGAdView *)adView
{
    uint32_t objectId = [self findObjectIdWithObject:adView];
    if (MTRGUnityUndefinedObjectId == objectId)
    {
        return;
    }

    MTRGCallbackTypeOnNoAdWithReason callback = _MTRGStandardAdCallbackOnAdLoadFailed;
    if (callback)
    {
        mtrg_unity_tracer_d(@"MTRGUnityProxy: onNoAdWithReason");

        NSString *reasonStr = reason ? reason : @"No Ad";
        callback(objectId, reasonStr.UTF8String);
    }
}

- (void)onAdClickWithAdView:(MTRGAdView *)adView
{
    uint32_t objectId = [self findObjectIdWithObject:adView];
    if (MTRGUnityUndefinedObjectId == objectId)
    {
        return;
    }

    MTRGCallbackTypeAdId callback = _MTRGStandardAdCallbackOnAdClicked;
    if (callback)
    {
        mtrg_unity_tracer_d(@"MTRGUnityProxy: onClickWithInterstitialAd");

        callback(objectId);
    }
}

- (void)onAdShowWithAdView:(MTRGAdView *)adView
{
    uint32_t objectId = [self findObjectIdWithObject:adView];
    if (MTRGUnityUndefinedObjectId == objectId)
    {
        return;
    }

    MTRGCallbackTypeAdId callback = _MTRGStandardAdCallbackOnAdShown;
    if (callback)
    {
        mtrg_unity_tracer_d(@"MTRGUnityProxy: onAdShowWithAdView");

        callback(objectId);
    }
}

- (void)onShowModalWithAdView:(MTRGAdView *)adView
{
    // empty
}

- (void)onDismissModalWithAdView:(MTRGAdView *)adView
{
    // empty
}

- (void)onLeaveApplicationWithAdView:(MTRGAdView *)adView
{
    // empty
}

@end

#pragma mark -- standard ads callbacks setters

void MTRGStandardAdSetCallbackOnAdLoadCompleted(MTRGCallbackTypeAdId callback)
{
    [[NSOperationQueue mainQueue] addOperationWithBlock:^
    {
        mtrg_unity_tracer_d(@"MTRGStandardAdSetCallbackOnAdLoadCompleted");

        _MTRGStandardAdCallbackOnAdLoadCompleted = callback;
    }];
}

void MTRGStandardAdSetCallbackOnAdLoadFailed(MTRGCallbackTypeOnNoAdWithReason callback)
{
    [[NSOperationQueue mainQueue] addOperationWithBlock:^
    {
        mtrg_unity_tracer_d(@"MTRGStandardAdSetCallbackOnAdLoadFailed");

        _MTRGStandardAdCallbackOnAdLoadFailed = callback;
    }];
}

void MTRGStandardAdSetCallbackOnAdClicked(MTRGCallbackTypeAdId callback)
{
    [[NSOperationQueue mainQueue] addOperationWithBlock:^
    {
        mtrg_unity_tracer_d(@"MTRGStandardAdSetCallbackOnAdClicked");

        _MTRGStandardAdCallbackOnAdClicked = callback;
    }];
}

void MTRGStandardAdSetCallbackOnAdShown(MTRGCallbackTypeAdId callback)
{
    [[NSOperationQueue mainQueue] addOperationWithBlock:^
    {
        mtrg_unity_tracer_d(@"MTRGStandardAdSetCallbackOnAdShown");

        _MTRGStandardAdCallbackOnAdShown = callback;
    }];
}

#pragma mark - interstitial callbacks setters

void MTRGInterstitialAdSetCallbackOnLoad(MTRGCallbackTypeAdId callback)
{
    [[NSOperationQueue mainQueue] addOperationWithBlock:^
    {
        mtrg_unity_tracer_d(@"MTRGInterstitialAdSetCallbackOnLoad");

        _MTRGInterstitialAdCallbackOnLoad = callback;
    }];
}

void MTRGInterstitialAdSetCallbackOnNoAdWithReason(MTRGCallbackTypeOnNoAdWithReason callback)
{
    [[NSOperationQueue mainQueue] addOperationWithBlock:^
    {
        mtrg_unity_tracer_d(@"MTRGInterstitialAdSetCallbackOnNoAdWithReason");

        _MTRGInterstitialAdCallbackOnNoAdWithReason = callback;
    }];
}

void MTRGInterstitialAdSetCallbackOnClick(MTRGCallbackTypeAdId callback)
{
    [[NSOperationQueue mainQueue] addOperationWithBlock:^
    {
        mtrg_unity_tracer_d(@"MTRGInterstitialAdSetCallbackOnClick");

        _MTRGInterstitialAdCallbackOnClick = callback;
    }];
}

void MTRGInterstitialAdSetCallbackOnClose(MTRGCallbackTypeAdId callback)
{
    [[NSOperationQueue mainQueue] addOperationWithBlock:^
    {
        mtrg_unity_tracer_d(@"MTRGInterstitialAdSetCallbackOnClose");

        _MTRGInterstitialAdCallbackOnClose = callback;
    }];
}

void MTRGInterstitialAdSetCallbackOnVideoComplete(MTRGCallbackTypeAdId callback)
{
    [[NSOperationQueue mainQueue] addOperationWithBlock:^
    {
        mtrg_unity_tracer_d(@"MTRGInterstitialAdSetCallbackOnVideoComplete");

        _MTRGInterstitialAdCallbackOnVideoComplete = callback;
    }];
}

void MTRGInterstitialAdSetCallbackOnDisplay(MTRGCallbackTypeAdId callback)
{
    [[NSOperationQueue mainQueue] addOperationWithBlock:^
    {
        mtrg_unity_tracer_d(@"MTRGInterstitialAdSetCallbackOnDisplay");

        _MTRGInterstitialAdCallbackOnDisplay = callback;
    }];
}