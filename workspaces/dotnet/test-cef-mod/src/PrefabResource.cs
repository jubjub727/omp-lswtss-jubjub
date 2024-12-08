using System;
using System.Runtime.InteropServices;
using OMP.LSWTSS.CApi1;

namespace OMP.LSWTSS;

public partial class TestCefMod
{
    class PrefabResource : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public string Path { get; private set; }

        nttSceneGraphResourceHandle.Handle _handle;

        public PrefabResource(string path)
        {
            IsDisposed = false;

            Path = path;

            _handle = (nttSceneGraphResourceHandle.Handle)Marshal.AllocHGlobal(nttSceneGraphResourceHandle.StructSize);

            for (int i = 0; i < nttSceneGraphResourceHandle.StructSize; i++)
            {
                Marshal.WriteByte(_handle, i, 0);
            }

            _handle.Constructor(2);

            _handle.set_ResourcePath(Path);
            _handle.AsyncLoad();
        }

        void ThrowIfDisposed()
        {
            if (IsDisposed)
            {
                throw new InvalidOperationException();
            }
        }

        public bool FetchIsLoaded()
        {
            ThrowIfDisposed();

            return _handle.IsLoadedAndFixedUp();
        }

        public apiEntity.Handle FetchPrefabHandle()
        {
            ThrowIfDisposed();

            if (!FetchIsLoaded())
            {
                throw new InvalidOperationException();
            }

            return _handle.Get().GetRoot();
        }

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            // TODO:

            IsDisposed = true;
        }
    }
}