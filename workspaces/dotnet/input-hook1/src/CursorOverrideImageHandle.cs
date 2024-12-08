using System.Linq;

namespace OMP.LSWTSS;

partial class InputHook1
{
    static nint? CursorOverrideImageHandle
    {
        get
        {
            lock (_clients)
            {
                return _clients
                    .FirstOrDefault((client) => client.CursorOverrideImageHandle.HasValue)?
                    .CursorOverrideImageHandle;
            }
        }
    }
}