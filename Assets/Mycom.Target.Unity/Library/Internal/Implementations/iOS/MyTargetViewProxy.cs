#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using Mycom.Target.Unity.Ads;
using Mycom.Target.Unity.Internal.Interfaces;

namespace Mycom.Target.Unity.Internal.Implementations.iOS
{
    internal sealed class MyTargetViewProxy : IMyTargetViewProxy
    {
        private static readonly Dictionary<UInt32, MyTargetViewProxy> Instanses = new Dictionary<UInt32, MyTargetViewProxy>();

        static MyTargetViewProxy()
        {
            MTRGStandardAdSetCallbackOnAdClicked(OnAdClicked);
            MTRGStandardAdSetCallbackOnAdLoadCompleted(OnAdLoadCompleted);
            MTRGStandardAdSetCallbackOnAdLoadFailed(OnAdLoadFailed);
            MTRGStandardAdSetCallbackOnAdShown(OnAdShown);
        }

        [DllImport("__Internal")]
        private static extern UInt32 MTRGStandardAdCreate(UInt32 slotId, Boolean isRefreshAd, UInt32 adSize);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdDelete(UInt32 adId);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdLoad(UInt32 adId);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdSetCallbackOnAdClicked(Action<UInt32> callback);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdSetCallbackOnAdLoadCompleted(Action<UInt32> callback);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdSetCallbackOnAdLoadFailed(Action<UInt32, String> callback);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdSetCallbackOnAdShown(Action<UInt32> callback);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdSetHeight(UInt32 adId, UInt32 height);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdSetWidth(UInt32 adId, UInt32 width);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdSetX(UInt32 adId, UInt32 x);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdSetY(UInt32 adId, UInt32 y);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdStart(UInt32 adId);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdStop(UInt32 adId);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdSetTrackingLocationEnabled(UInt32 adId, Boolean value);

        [MonoPInvokeCallback(typeof (Action<UInt32>))]
        private static void OnAdClicked(UInt32 adId)
        {
            lock (Instanses)
            {
                MyTargetViewProxy myTargetViewProxy;
                if (Instanses.TryGetValue(adId, out myTargetViewProxy))
                {
                    myTargetViewProxy.OnAdClicked();
                }
            }
        }

        [MonoPInvokeCallback(typeof (Action<UInt32>))]
        private static void OnAdLoadCompleted(UInt32 adId)
        {
            lock (Instanses)
            {
                MyTargetViewProxy myTargetViewProxy;
                if (Instanses.TryGetValue(adId, out myTargetViewProxy))
                {
                    myTargetViewProxy.OnAdLoadCompleted();
                }
            }
        }

        [MonoPInvokeCallback(typeof (Action<UInt32, String>))]
        private static void OnAdLoadFailed(UInt32 adId, String reason)
        {
            lock (Instanses)
            {
                MyTargetViewProxy myTargetViewProxy;
                if (Instanses.TryGetValue(adId, out myTargetViewProxy))
                {
                    myTargetViewProxy.OnAdLoadFailed(reason);
                }
            }
        }

        [MonoPInvokeCallback(typeof (Action<UInt32>))]
        private static void OnAdShown(UInt32 adId)
        {
            lock (Instanses)
            {
                MyTargetViewProxy myTargetViewProxy;
                if (Instanses.TryGetValue(adId, out myTargetViewProxy))
                {
                    myTargetViewProxy.OnAdShown();
                }
            }
        }

        private readonly UInt32 _instanseId;
        private Boolean _isDisposed;

        public MyTargetViewProxy(UInt32 slotId,
                                 MyTargetView.AdSize adSize,
                                 Boolean isRefreshAd)
        {
            _instanseId = MTRGStandardAdCreate(slotId, isRefreshAd, (UInt32) adSize);
            CustomParamsProxy = new CustomParamsProxy(_instanseId);

            lock (Instanses)
            {
                Instanses[_instanseId] = this;
            }
        }

        ~MyTargetViewProxy()
        {
            Dispose();
        }

        private void OnAdClicked()
        {
            var handler = AdClicked;
            if (handler != null)
            {
                handler();
            }
        }

        private void OnAdLoadCompleted()
        {
            var handler = AdLoadCompleted;
            if (handler != null)
            {
                handler();
            }
        }

        private void OnAdLoadFailed(String reason)
        {
            var handler = AdLoadFailed;
            if (handler != null)
            {
                handler(reason);
            }
        }

        private void OnAdShown()
        {
            var handler = AdShown;
            if (handler != null)
            {
                handler();
            }
        }

        public event Action AdClicked;
        public event Action AdLoadCompleted;
        public event Action<String> AdLoadFailed;
        public event Action AdShown;

        public ICustomParamsProxy CustomParamsProxy { get; private set; }

        public void Load()
        {
            MTRGStandardAdLoad(_instanseId);
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            lock (Instanses)
            {
                Instanses.Remove(_instanseId);
            }

            MTRGStandardAdDelete(_instanseId);

            GC.SuppressFinalize(this);
        }
        
        public void SetHeight(Double value)
        {
            MTRGStandardAdSetHeight(_instanseId, (UInt32) value);
        }

        public void SetWidth(Double value)
        {
            MTRGStandardAdSetWidth(_instanseId, (UInt32) value);
        }

        public void SetX(Double value)
        {
            MTRGStandardAdSetX(_instanseId, (UInt32) value);
        }

        public void SetY(Double value)
        {
            MTRGStandardAdSetY(_instanseId, (UInt32) value);
        }

        public void Start()
        {
            MTRGStandardAdStart(_instanseId);
        }

        public void Stop()
        {
            MTRGStandardAdStop(_instanseId);
        }

        public void SetTrackingEnvironmentEnabled(Boolean value){ }

        public void SetTrackingLocationEnabled(Boolean value)
        {
            MTRGStandardAdSetTrackingLocationEnabled(_instanseId, value);
        }
    }
}

#endif