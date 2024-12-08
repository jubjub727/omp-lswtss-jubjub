using System.Runtime.InteropServices;
using OMP.LSWTSS.CApi1;

namespace OMP.LSWTSS;

public static partial class V1
{
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NotOwnedStringMarshaler))]
    delegate string NuStringTableGetByNameDelegate([MarshalAs(UnmanagedType.LPUTF8Str)] string stringId, nint zeroPtr);

    static readonly NuStringTableGetByNameDelegate _nuStringTableGetByName = NativeFunc.GetExecute<NuStringTableGetByNameDelegate>(
        NativeFunc.GetPtr(
            GetVariantValue.Execute(steamValue: 0xc51e0, egsValue: 0xc51b0)
        )
    );

    public static string TranslateString(string stringId)
    {
        return _nuStringTableGetByName(stringId, 0);
    }
}