using System;

namespace OMP.LSWTSS;

partial class InputHook1
{
    public sealed class Client : IDisposable
    {
        public delegate bool HandleNativeMessageDelegate(NativeMessage nativeMessage);

        public readonly int Order;

        public readonly HandleNativeMessageDelegate HandleNativeMessage;

        public nint? CursorOverrideImageHandle;

        public bool IsCursorVisible;

        public bool AreKeyboardEventsIntercepted;

        public bool AreMouseEventsIntercepted;

        bool _isDisposed;

        public Client(int order, HandleNativeMessageDelegate handleNativeMessage)
        {
            Order = order;
            HandleNativeMessage = handleNativeMessage;

            lock (_clients)
            {
                _clients.Add(this);
            }
            SortClients();
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                lock (_clients)
                {
                    _clients.Remove(this);
                }
                SortClients();

                _isDisposed = true;
            }
        }
    }
}