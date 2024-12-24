namespace OMP.LSWTSS;

public partial class GalaxyUnleashed
{
    void UpdateOverlay()
    {
        _overlay.IsActive = _modeState is MenuModeState;
    }
}