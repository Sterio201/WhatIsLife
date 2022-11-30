#if UNITY_IOS

using System;
using System.Runtime.InteropServices;

namespace Mycom.Target.Unity.Internal
{
    internal static partial class MyTargetPrivacyProxy
    {
        [DllImport("__Internal")]
        private static extern Boolean MTRGGetUserConsent();

        [DllImport("__Internal")]
        private static extern void MTRGSetUserConsent(Boolean userConsent);

        [DllImport("__Internal")]
        private static extern Boolean MTRGGetUserAgeRestricted();

        [DllImport("__Internal")]
        private static extern void MTRGSetUserAgeRestricted(Boolean userAgeRestricted);

        static partial void GetUserConsent(ref Boolean isUserConsent)
        {
            isUserConsent = MTRGGetUserConsent();
        }

        static partial void SetUserConsent(Boolean userConsent)
        {
            MTRGSetUserConsent(userConsent);
        }

        static partial void GetUserAgeRestricted(ref Boolean isUserAgeRestricted)
        {
            isUserAgeRestricted = MTRGGetUserAgeRestricted();
        }

        static partial void SetUserAgeRestricted(Boolean userAgeRestricted)
        {
            MTRGSetUserAgeRestricted(userAgeRestricted);
        }
    }
}

#endif