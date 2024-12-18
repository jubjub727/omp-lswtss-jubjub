using System.Runtime.InteropServices;
using OMP.LSWTSS.CApi1;

namespace OMP.LSWTSS;

public static partial class V1
{
    static readonly unsafe CFuncHook1<Collectables.Table.AddEntry2Method.NativeDelegate> _cApi1CollectablesTableAddEntry2MethodHook = new(
        Collectables.Table.AddEntry2Method.Info.NativePtr,
        (
            nativeDataRawPtr,
            arg0,
            arg1,
            arg2
        ) =>
        {
            var arg0AsString = Marshal.PtrToStringUTF8(Marshal.ReadIntPtr(arg0));

            foreach (var customCharacterInfo in _customCharactersInfo!)
            {
                if (customCharacterInfo.Id == arg0AsString)
                {
                    var customCharacterCApi1Guid = GetCustomCharacterCApi1Guid(customCharacterInfo.Id);

                    NuGuid.NativeHandle customCharacterCApi1GuidNativeHandle = &customCharacterCApi1Guid; 

                    var result = _cApi1CollectablesTableAddEntry2MethodHook!.Trampoline!(
                        nativeDataRawPtr,
                        arg0,
                        customCharacterCApi1GuidNativeHandle,
                        arg2
                    );

                    return result;
                }
            }

            return _cApi1CollectablesTableAddEntry2MethodHook!.Trampoline!(nativeDataRawPtr, arg0, arg1, arg2);
        }
    );
}