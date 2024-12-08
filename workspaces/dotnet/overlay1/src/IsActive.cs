namespace OMP.LSWTSS;

partial class Overlay1
{
    public bool IsActive
    {
        get
        {
            return _activeInstance == this;
        }
        set
        {
            if (value)
            {
                _activeInstance = this;
            }
            else if (_activeInstance == this)
            {
                _activeInstance = null;
            }

            UpdateInputHookClient();
        }
    }
}