#if UNITY_IOS
using System;
using Mycom.Target.Unity.Internal.Interfaces;

namespace Mycom.Target.Unity.Internal.Implementations.iOS
{
    internal class Dispatcher : IDispatcher
    {
        public void Perform(Action action)
        {
            action();
        }
    }
} 
#endif