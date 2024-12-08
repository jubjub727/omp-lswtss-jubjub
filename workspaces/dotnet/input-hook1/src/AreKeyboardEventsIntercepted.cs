using System.Linq;

namespace OMP.LSWTSS;

partial class InputHook1
{
    static bool AreKeyboardEventsIntercepted
    {
        get
        {
            lock (_clients)
            {
                return _clients.Any((client) => client.AreKeyboardEventsIntercepted);
            }
        }
    }
}