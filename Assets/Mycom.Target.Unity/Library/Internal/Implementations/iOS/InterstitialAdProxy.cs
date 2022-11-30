#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using Mycom.Target.Unity.Internal.Interfaces;

namespace Mycom.Target.Unity.Internal.Implementations.iOS
{
    internal sealed class InterstitialAdProxy : IInterstitialAdProxy
    {
        private static readonly Dictionary<UInt32, InterstitialAdProxy> Instanses = new Dictionary<UInt32, InterstitialAdProxy>();

        static InterstitialAdProxy()
        {
            MTRGInterstitialAdSetCallbackOnClick(OnAdClicked);
            MTRGInterstitialAdSetCallbackOnClose(OnAdDismissed);
            MTRGInterstitialAdSetCallbackOnDisplay(OnAdDisplayed);
            MTRGInterstitialAdSetCallbackOnLoad(OnAdLoadCompleted);
            MTRGInterstitialAdSetCallbackOnNoAdWithReason(OnAdLoadFailed);
            MTRGInterstitialAdSetCallbackOnVideoComplete(OnAdVideoCompleted);
        }

        [DllImport("__Internal")]
        private static extern void MTRGInterstitialAdClose(UInt32 adId);

        [DllImport("__Internal")]
        private static extern UInt32 MTRGInterstitialAdCreate(UInt32 slotId);

        [DllImport("__Internal")]
        private static extern void MTRGInterstitialAdDelete(UInt32 adId);

        [DllImport("__Internal")]
        private static extern void MTRGInterstitialAdLoad(UInt32 adId);

        [DllImport("__Internal")]
        private static extern void MTRGInterstitialAdSetCallbackOnClick(Action<UInt32> callback);

        [DllImport("__Internal")]
        private static extern void MTRGInterstitialAdSetCallbackOnClose(Action<UInt32> callback);

        [DllImport("__Internal")]
        private static extern void MTRGInterstitialAdSetCallbackOnDisplay(Action<UInt32> callback);

        [DllImport("__Internal")]
        private static extern void MTRGInterstitialAdSetCallbackOnLoad(Action<UInt32> callback);

        [DllImport("__Internal")]
        private static extern void MTRGInterstitialAdSetCallbackOnNoAdWithReason(Action<UInt32, String> callback);

        [DllImport("__Internal")]
        private static extern void MTRGInterstitialAdSetCallbackOnVideoComplete(Action<UInt32> callback);

        [DllImport("__Internal")]
        private static extern void MTRGInterstitialAdShow(UInt32 adId);

        [DllImport("__Internal")]
        private static extern void MTRGInterstitialAdShowModal(UInt32 adId);

        [DllImport("__Internal")]
        private static extern void MTRGInterstitialAdSetTrackingLocationEnabled(UInt32 adId, Boolean value);

        [MonoPInvokeCallback(typeof (Action<UInt32>))]
        private static void OnAdClicked(UInt32 adId)
        {
            lock (Instanses)
            {
                InterstitialAdProxy interstitialAdProxy;
                if (Instanses.TryGetValue(adId, out interstitialAdProxy))
                {
                    interstitialAdProxy.OnAdClicked();
                }
            }
        }

        [MonoPInvokeCallback(typeof (Action<UInt32>))]
        private static void OnAdDismissed(UInt32 adId)
        {
            lock (Instanses)
            {
                InterstitialAdProxy interstitialAdProxy;
                if (Instanses.TryGetValue(adId, out interstitialAdProxy))
                {
                    interstitialAdProxy.OnAdDismissed();
                }
            }
        }

        [MonoPInvokeCallback(typeof (Action<UInt32>))]
        private static void OnAdDisplayed(UInt32 adId)
        {
            lock (Instanses)
            {
                InterstitialAdProxy interstitialAdProxy;
                if (Instanses.TryGetValue(adId, out interstitialAdProxy))
                {
                    interstitialAdProxy.OnAdDisplayed();
                }
            }
        }

        [MonoPInvokeCallback(typeof (Action<UInt32>))]
        private static void OnAdLoadCompleted(UInt32 adId)
        {
            lock (Instanses)
            {
                InterstitialAdProxy interstitialAdProxy;
                if (Instanses.TryGetValue(adId, out interstitialAdProxy))
                {
                    interstitialAdProxy.OnAdLoadCompleted();
                }
            }
        }

        [MonoPInvokeCallback(typeof (Action<UInt32, String>))]
        private static void OnAdLoadFailed(UInt32 adId, String reason)
        {
            lock (Instanses)
            {
                InterstitialAdProxy interstitialAdProxy;
                if (Instanses.TryGetValue(adId, out interstitialAdProxy))
                {
                    interstitialAdProxy.OnAdLoadFailed(reason);
                }
            }
        }

        [MonoPInvokeCallback(typeof (Action<UInt32>))]
        private static void OnAdVideoCompleted(UInt32 adId)
        {
            lock (Instanses)
            {
                InterstitialAdProxy interstitialAdProxy;
                if (Instanses.TryGetValue(adId, out interstitialAdProxy))
                {
                    interstitialAdProxy.OnAdVideoCompleted();
                }
            }
        }

        private readonly UInt32 _instanseId;

        private Boolean _isDisposed;

        public InterstitialAdProxy(UInt32 slotId)
        {
            _instanseId = MTRGInterstitialAdCreate(slotId);
            CustomParamsProxy = new CustomParamsProxy(_instanseId);

            lock (Instanses)
            {
                Instanses[_instanseId] = this;
            }
        }

        ~InterstitialAdProxy()
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

        private void OnAdDismissed()
        {
            var handler = AdDismissed;
            if (handler != null)
            {
                handler();
            }
        }

        private void OnAdDisplayed()
        {
            var handler = AdDisplayed;
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

        private void OnAdVideoCompleted()
        {
            var handler = AdVideoCompleted;
            if (handler != null)
            {
                handler();
            }
        }

        public event Action AdClicked;

        public event Action AdLoadCompleted;

        public event Action<String> AdLoadFailed;

        public void Load()
        {
            MTRGInterstitialAdLoad(_instanseId);
        }

        public ICustomParamsProxy CustomParamsProxy { get; private set; }

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

            MTRGInterstitialAdDelete(_instanseId);

            GC.SuppressFinalize(this);
        }

        public event Action AdDismissed;

        public event Action AdDisplayed;

        public event Action AdVideoCompleted;

        public void Dismiss()
        {
            MTRGInterstitialAdClose(_instanseId);
        }

        public void Show()
        {
            MTRGInterstitialAdShow(_instanseId);
        }

        public void ShowDialog()
        {
            MTRGInterstitialAdShowModal(_instanseId);
        }

        public void SetTrackingEnvironmentEnabled(Boolean value){}

        public void SetTrackingLocationEnabled(Boolean value)
        {
            MTRGInterstitialAdSetTrackingLocationEnabled(_instanseId, value);
        }
    }
}

#endif