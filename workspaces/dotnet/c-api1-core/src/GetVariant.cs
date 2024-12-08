using System.Linq;
using OMP.LSWTSS.CApi1;

public static class GetVariant
{
    static Variant? _cachedVariant;

    public static Variant Execute()
    {
        if (_cachedVariant == null)
        {
            if (System.Environment.GetEnvironmentVariable("SteamAppId") != null)
            {
                _cachedVariant = Variant.Steam;
            }
            else if (System.Environment.GetCommandLineArgs().Any(x => x.Contains("-epicapp")))
            {
                _cachedVariant = Variant.EGS;
            }
            else
            {
                throw new System.InvalidOperationException();
            }
        }

        return _cachedVariant.Value;
    }
}