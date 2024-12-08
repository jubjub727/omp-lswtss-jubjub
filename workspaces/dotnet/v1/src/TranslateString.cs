using System.Runtime.InteropServices;

namespace OMP.LSWTSS;

public static partial class V1
{
    public static string TranslateString(string stringId)
    {
        return CApi1.NuStringTable.GetByNameGlobalFunc.Execute(stringId, 0);
    }
}