#if UNITY_ANDROID
using System;
using System.Diagnostics.CodeAnalysis;
using Mycom.Target.Unity.Ads;
using Mycom.Target.Unity.Internal.Interfaces;
using UnityEngine;

namespace Mycom.Target.Unity.Internal.Implementations.Android
{
    internal sealed class MyTargetViewProxy : IMyTargetViewProxy
    {
        const String ClassName = "com.my.target.ads.MyTargetView";
        const String ClassNameFrameLayouParams = "android.widget.FrameLayout$LayoutParams";
        const String MethodGetCustomParams = "getCustomParams";
        const String MethodInit = "init";
        const String MethodLoad = "load";
        const String MethodNameRequestLayout = "requestLayout";
        const String MethodSetListener = "setListener";

        const String MethoSetLayoutParams = "setLayoutParams";
        const String MethodSetTrackingEnvironmentEnabled = "setTrackingEnvironmentEnabled";
        const String MethodSetTrackingLocationEnabled = "setTrackingLocationEnabled";

        readonly AndroidJavaObject _layoutParams;
        readonly AndroidJavaObject _myTargetViewObject;

        Boolean _isDisposed;
        Boolean _isShown;

        public MyTargetViewProxy(UInt32 slotId,
                                 MyTargetView.AdSize adSize,
                                 Boolean isRefreshAd)
        {
            var currentActivity = PlatformHelper.GetCurrentActivity();

            _myTargetViewObject = new AndroidJavaObject(ClassName, currentActivity);

            _layoutParams = new AndroidJavaObject(ClassNameFrameLayouParams, 0, 0);

            _myTargetViewObject.Call(MethodSetListener, new MyTargetViewListener(this));

            _myTargetViewObject.Call(MethodInit,
                                     (Int32)slotId,
                                     (Int32)adSize,
                                     isRefreshAd);

            _myTargetViewObject.Call(MethoSetLayoutParams, _layoutParams);

            CustomParamsProxy = new CustomParamsProxy(_myTargetViewObject.Call<AndroidJavaObject>(MethodGetCustomParams));
        }

        ~MyTargetViewProxy()
        {
            Dispose();
        }

        void OnAdClicked()
        {
            var handler = AdClicked;
            if (handler != null)
            {
                handler();
            }
        }

        void OnAdLoadCompleted()
        {
            var handler = AdLoadCompleted;
            if (handler != null)
            {
                handler();
            }
        }

        void OnAdLoadFailed(String obj)
        {
            var handler = AdLoadFailed;
            if (handler != null)
            {
                handler(obj);
            }
        }

        void OnAdShown()
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

        public void Load()
        {
            if (_isDisposed)
            {
                return;
            }
            _myTargetViewObject.Call(MethodLoad);
        }

        public ICustomParamsProxy CustomParamsProxy { get; private set; }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            PlatformHelper.RunInUiThread(() =>
                                         {
                                             _myTargetViewObject.Call<AndroidJavaObject>("getParent")
                                                                .Call("removeView", _myTargetViewObject);

                                             _myTargetViewObject.Call("destroy");

                                             _myTargetViewObject.Dispose();

                                             CustomParamsProxy.Dispose();
                                         });

            GC.SuppressFinalize(this);
        }

        public void SetHeight(Double value)
        {
            if (_isDisposed)
            {
                return;
            }

            PlatformHelper.RunInUiThread(() =>
                                         {
                                             _layoutParams.Set("height", PlatformHelper.GetInDp(value));
                                             _myTargetViewObject.Call(MethodNameRequestLayout);
                                         });
        }

        public void SetWidth(Double value)
        {
            if (_isDisposed)
            {
                return;
            }

            PlatformHelper.RunInUiThread(() =>
                                         {
                                             _layoutParams.Set("width", PlatformHelper.GetInDp(value));
                                             _myTargetViewObject.Call(MethodNameRequestLayout);
                                         });
        }

        public void SetX(Double value)
        {
            if (_isDisposed)
            {
                return;
            }

            PlatformHelper.RunInUiThread(() =>
                                         {
                                             _layoutParams.Set("leftMargin", PlatformHelper.GetInDp(value));
                                             _myTargetViewObject.Call(MethodNameRequestLayout);
                                         });
        }

        public void SetY(Double value)
        {
            if (_isDisposed)
            {
                return;
            }

            PlatformHelper.RunInUiThread(() =>
                                         {
                                             _layoutParams.Set("topMargin", PlatformHelper.GetInDp(value));
                                             _myTargetViewObject.Call(MethodNameRequestLayout);
                                         });
        }

        public void Start()
        {
            if (_isDisposed)
            {
                return;
            }

            if (_isShown)
            {
                return;
            }

            _isShown = true;

            PlatformHelper.RunInUiThread(() =>
                                         {
                                             var activity = PlatformHelper.GetCurrentActivity();

                                             var r = new AndroidJavaClass("android.R$id");

                                             var contentId = r.GetStatic<Int32>("content");

                                             activity.Call<AndroidJavaObject>("findViewById", contentId)
                                                     .Call("addView", _myTargetViewObject);
                                         });
        }

        public void Stop()
        {
            if (_isDisposed)
            {
                return;
            }

            if (!_isShown)
            {
                return;
            }

            _isShown = false;

            PlatformHelper.RunInUiThread(() =>
                                         {
                                             _myTargetViewObject.Call<AndroidJavaObject>("getParent")
                                                                .Call("removeView", _myTargetViewObject);
                                         });
        }

        public void SetTrackingEnvironmentEnabled(Boolean value)
        {
            if (_isDisposed)
            {
                return;
            }

            _myTargetViewObject.Call(MethodSetTrackingEnvironmentEnabled, value);
        }

        public void SetTrackingLocationEnabled(Boolean value)
        {
            if (_isDisposed)
            {
                return;
            }

            _myTargetViewObject.Call(MethodSetTrackingLocationEnabled, value);
        }

        sealed class MyTargetViewListener : AndroidJavaProxy
        {
            const String ListenerClassName = "com.my.target.ads.MyTargetView$MyTargetViewListener";

            readonly MyTargetViewProxy _myTargetViewProxy;

            public MyTargetViewListener(MyTargetViewProxy myTargetViewProxy) : base(ListenerClassName)
            {
                _myTargetViewProxy = myTargetViewProxy;
            }

            public void onClick(AndroidJavaObject o)
            {
                _myTargetViewProxy.OnAdClicked();
            }

            public void onLoad(AndroidJavaObject o)
            {
                _myTargetViewProxy.OnAdLoadCompleted();
            }

            public void onNoAd(String error, AndroidJavaObject o)
            {
                _myTargetViewProxy.OnAdLoadFailed(error);
            }

            public void onShow(AndroidJavaObject o)
            {
                _myTargetViewProxy.OnAdShown();
            }
        }
    }
}

#endif