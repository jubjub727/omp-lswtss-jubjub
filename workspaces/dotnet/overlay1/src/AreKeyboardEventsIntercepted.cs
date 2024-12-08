namespace OMP.LSWTSS;

partial class Overlay1
{
    bool _areKeyboardEventsIntercepted;

    public bool AreKeyboardEventsIntercepted
    {
        get
        {
            return _areKeyboardEventsIntercepted;
        }
        set
        {
            _areKeyboardEventsIntercepted = value;

            UpdateInputHookClient();
        }
    }
}