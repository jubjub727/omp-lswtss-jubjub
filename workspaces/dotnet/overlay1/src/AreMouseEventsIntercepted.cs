namespace OMP.LSWTSS;

partial class Overlay1
{
    bool _areMouseEventsIntercepted;

    public bool AreMouseEventsIntercepted
    {
        get
        {
            return _areMouseEventsIntercepted;
        }
        set
        {
            _areMouseEventsIntercepted = value;

            UpdateInputHookClient();
        }
    }
}