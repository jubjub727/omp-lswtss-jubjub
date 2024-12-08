using System.Runtime.InteropServices;

namespace OMP.LSWTSS;

public static partial class V1
{
    static readonly CFuncHook1<CApi1.collectables_.Table.AddEntryInnerMethod.Delegate> cApi1CollectablesTableAddEntryInnerMethodHook = new(
        CApi1.collectables_.Table.AddEntryInnerMethod.Ptr,
        (
            handle,
            param0,
            param1,
            param2
        ) =>
        {
            var characterId = Marshal.PtrToStringUTF8(Marshal.ReadIntPtr(param0));

            foreach (var customCharacterInfo in _customCharactersInfo!)
            {
                if (customCharacterInfo.Id == characterId)
                {
                    var customCharacterGuid = GetCustomCharacterCApi1NuGuid(customCharacterInfo.Id);

                    var customCharacterGuidPtr = Marshal.AllocHGlobal(Marshal.SizeOf<TempNuGuid>());

                    Marshal.StructureToPtr(customCharacterGuid, customCharacterGuidPtr, false);

                    var result = cApi1CollectablesTableAddEntryInnerMethodHook!.Trampoline!(handle, param0, customCharacterGuidPtr, param2);

                    Marshal.FreeHGlobal(customCharacterGuidPtr);

                    return result;
                }
            }

            return cApi1CollectablesTableAddEntryInnerMethodHook!.Trampoline!(handle, param0, param1, param2);
        }
    );
}