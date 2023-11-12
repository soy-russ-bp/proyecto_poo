using System;
using System.Threading;

namespace ARJE.Utils.Threading
{
    public static class SynchronizationContextExt
    {
        public static void SendInCtxOrCurrent<TState>(this SynchronizationContext? synchronizationContext, TState state, Action<TState> action)
        {
            if (synchronizationContext == null)
            {
                action.Invoke(state);
                return;
            }

            synchronizationContext.Send(state, action);
        }

        public static void Send<TState>(this SynchronizationContext synchronizationContext, TState state, Action<TState> action)
        {
            synchronizationContext.Send(
                state => action.Invoke((TState)state!),
                state);
        }
    }
}
