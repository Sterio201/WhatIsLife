#if UNITY_IOS
using System;
using System.Runtime.InteropServices;
using Mycom.Target.Unity.Internal.Interfaces;

namespace Mycom.Target.Unity.Internal.Implementations.iOS
{
    internal sealed class DebugModeProxy : IDebugModeProxy
    {
        private Boolean _isEnabled;

        [DllImport("__Internal")]
        private static extern void MTRGManagerSetLoggingEnabled(Byte isEnabled);

        [DllImport("__Internal")]
        private static extern Byte MTRGManagerGetLoggingEnabled();

        public DebugModeProxy()
        {
            _isEnabled = Convert.ToBoolean(MTRGManagerGetLoggingEnabled());
        }

        public Boolean IsDebugMode
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled == value)
                {
                    return;
                }

                _isEnabled = value;
                MTRGManagerSetLoggingEnabled(Convert.ToByte(value));
            }
        }
    }
}

#endif