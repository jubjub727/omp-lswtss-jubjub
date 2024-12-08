using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace OMP.LSWTSS;

public static partial class V1
{
    static readonly List<CApi1.collectables.Table.Handle> cApi1CollectablesTableHandles = [];

    static bool wasCApi1CollectablesCharactersTableProcessed = false;

    [StructLayout(LayoutKind.Sequential)]
    struct TempNuGuid
    {
        public byte V1;
        public byte V2;
        public byte V3;
        public byte V4;
        public byte V5;
        public byte V6;
        public byte V7;
        public byte V8;
        public byte V9;
        public byte V10;
        public byte V11;
        public byte V12;
        public byte V13;
        public byte V14;
        public byte V15;
        public byte V16;
    }


    [StructLayout(LayoutKind.Sequential)]
    unsafe struct TempNuVector<T>
    {
        public T* UnknownField1;
        public T* UnknownField2;
        public T* UnknownField3;

        public readonly T* GetHandleAtIndex(int index)
        {
            return (T*)((nint)UnknownField1 + (index * Marshal.SizeOf<T>()));
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct TempCollectablesTableValue
    {
        public nint Value;
        public nint UnknownField2;
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct TempCollectablesEntryValues
    {
        public TempNuVector<TempCollectablesTableValue> Values;
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct TempCollectablesEnumVal
    {
        public nint Vtable;
        public nint UnknownField2;
        public nint UnknownField3;
        public int Value;
        public int UnknownField5;
        public nint UnknownField6;
        public nint UnknownField7;
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct TempNttResourcesNttResourceHandle
    {
        public nint Vtable;
        public nint UnknownField2;
        public nint UnknownField3;
        public nint ResourcePath;
        public byte UnknownField5;
        public byte UnknownField6;
        public byte ResourcePathLength;
        public byte UnknownField8;
        public int UnknownField9;
        public nint UnknownField10;
        public nint UnknownField11Ptr;
        public nint UnknownField12;
        public nint UnknownField13;
        public nint UnknownField14;
        public nint UnknownField15Ptr;
        public nint UnknownField16Ptr;
        public nint UnknownField17Ptr;
        public nint UnknownField18;
        public nint UnknownField19;

        public unsafe static void SetResourcePath(TempNttResourcesNttResourceHandle* handle, string resourcePath)
        {
            handle->ResourcePath = Marshal.StringToCoTaskMemUTF8(resourcePath);
            handle->ResourcePathLength = (byte)(resourcePath.Length + 1);
            handle->UnknownField5 = 0;
            handle->UnknownField6 = 2;
        }

        public static string? GetResourcePath(TempNttResourcesNttResourceHandle* handle)
        {
            if (handle->ResourcePath == nint.Zero)
            {
                return null;
            }

            return Marshal.PtrToStringUTF8(handle->ResourcePath);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct TempCollectablesGraphVal
    {
        public nint Vtable;
        public nint UnknownField2;
        public nint UnknownField3;
        public TempNttResourcesNttResourceHandle Value;
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct TempCollectablesStringVal
    {
        public nint Vtable;
        public nint UnknownField2;
        public nint UnknownField3;
        public nint Value;
        public ushort ValueLength;
        public ushort ValueLength2;

        public static void SetValueAsStr(TempCollectablesStringVal* handle, string value)
        {
            handle->Value = Marshal.StringToCoTaskMemUTF8(value);
            handle->ValueLength = (ushort)(value.Length + 1);
            handle->ValueLength2 = (ushort)(value.Length + 1);
        }

        public static string? GetValueAsStr(TempCollectablesStringVal* handle)
        {
            if (handle->Value == nint.Zero)
            {
                return null;
            }

            return Marshal.PtrToStringUTF8(handle->Value);
        }
    }

    [StructLayout(LayoutKind.Sequential)]

    unsafe struct TempCollectablesBoolVal
    {
        public nint Vtable;
        public nint UnknownField2;
        public nint UnknownField3;
        public byte Value;
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct TempCollectablesIntVal
    {
        public nint Vtable;
        public nint UnknownField2;
        public nint UnknownField3;
        public int Value;
    }

    static TempNuGuid GetCustomCharacterCApi1NuGuid(string customCharacterId)
    {
        // UUID v5 with OID namespace
        byte[] sourceArray = SHA1.HashData(
            new byte[] {
                0x6b, 0xa7, 0xb8, 0x12, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8,
            }
            .Concat(
                Encoding.UTF8.GetBytes(customCharacterId)
            )
            .ToArray()
        );

        byte[] array = new byte[16];
        Array.Copy(sourceArray, array, 16);
        array[6] &= 15;
        array[6] |= 80;
        array[8] &= 63;
        array[8] |= 128;

        return new TempNuGuid
        {
            V1 = array[0],
            V2 = array[1],
            V3 = array[2],
            V4 = array[3],
            V5 = array[4],
            V6 = array[5],
            V7 = array[6],
            V8 = array[7],
            V9 = array[8],
            V10 = array[9],
            V11 = array[10],
            V12 = array[11],
            V13 = array[12],
            V14 = array[13],
            V15 = array[14],
            V16 = array[15],
        };
    }

    static readonly CFuncHook1<CApi1.collectables_.Table.ConstructorMethod.Delegate> cApi1CollectablesTableConstructorMethodHook = new(
        CApi1.collectables_.Table.ConstructorMethod.Ptr,
        (
            handle,
            param0,
            param1,
            param2
        ) =>
        {
            unsafe
            {
                if (!wasCApi1CollectablesCharactersTableProcessed)
                {
                    foreach (var cApi1CollectablesTableHandle in cApi1CollectablesTableHandles)
                    {
                        var cApi1CollectablesTableName = CApi1.collectables.Table.GetNameMethod.Execute(cApi1CollectablesTableHandle);

                        if (cApi1CollectablesTableName == "Characters")
                        {
                            var emptyGuid = new TempNuGuid
                            {
                                V1 = 0,
                                V2 = 0,
                                V3 = 0,
                                V4 = 0,
                                V5 = 0,
                                V6 = 0,
                                V7 = 0,
                                V8 = 0,
                                V9 = 0,
                                V10 = 0,
                                V11 = 0,
                                V12 = 0,
                                V13 = 0,
                                V14 = 0,
                                V15 = 0,
                                V16 = 0,
                            };

                            var emptyGuidPtr = Marshal.AllocHGlobal(Marshal.SizeOf<TempNuGuid>());

                            Marshal.StructureToPtr(emptyGuid, emptyGuidPtr, false);

                            foreach (var customCharacterInfo in _customCharactersInfo!)
                            {
                                var cApi1CollectablesTableNewEntryIndex = cApi1CollectablesTableHandle.GetNumEntries();

                                cApi1CollectablesTableHandle.AddEntry(
                                    customCharacterInfo.Id,
                                    (CApi1.NuGuid.Handle)emptyGuidPtr
                                );

                                var cApi1CollectablesTableValuesHandle = (TempNuVector<TempCollectablesEntryValues>*)(cApi1CollectablesTableHandle + 0x140);

                                var cApi1CollectablesTableNewEntryValuesHandle =
                                    cApi1CollectablesTableValuesHandle->GetHandleAtIndex(cApi1CollectablesTableNewEntryIndex);

                                var cApi1CollectablesTableNewEntryCollectStateValHandle = (TempCollectablesEnumVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(0)->Value;
                                var cApi1CollectablesTableNewEntryGraphValHandle = (TempCollectablesGraphVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(1)->Value;
                                var cApi1CollectablesTableNewEntryCharacterClassValHandle = (TempCollectablesEnumVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(2)->Value;
                                var cApi1CollectablesTableNewEntryIconFileNameValHandle = (TempCollectablesStringVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(3)->Value;
                                var cApi1CollectablesTableNewEntryDisplayStringIdValHandle = (TempCollectablesStringVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(4)->Value;
                                var cApi1CollectablesTableNewEntryDescriptionStringIdValHandle = (TempCollectablesStringVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(5)->Value;
                                var cApi1CollectablesTableNewEntryIsNewValHandle = (TempCollectablesBoolVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(6)->Value;
                                var cApi1CollectablesTableNewEntryBaseCharacterValHandle = (TempCollectablesStringVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(7)->Value;
                                var cApi1CollectablesTableNewEntryCurrentlySelectedOutfitValHandle = (TempCollectablesBoolVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(8)->Value;
                                var cApi1CollectablesTableNewEntryPriceValHandle = (TempCollectablesIntVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(9)->Value;
                                var cApi1CollectablesTableNewEntryCharacterBaseNameIdValHandle = (TempCollectablesStringVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(10)->Value;
                                var cApi1CollectablesTableNewEntryIsPlayableValHandle = (TempCollectablesBoolVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(11)->Value;
                                var cApi1CollectablesTableNewEntryAddToPercentageValHandle = (TempCollectablesBoolVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(12)->Value;
                                var cApi1CollectablesTableNewEntryIsRideableValHandle = (TempCollectablesBoolVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(15)->Value;
                                var cApi1CollectablesTableNewEntryHasBeenRiddenByP1ValHandle = (TempCollectablesBoolVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(16)->Value;
                                var cApi1CollectablesTableNewEntryHasBeenRiddenByP2ValHandle = (TempCollectablesBoolVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(17)->Value;
                                var cApi1CollectablesTableNewEntryCheatCodeValHandle = (TempCollectablesStringVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(19)->Value;
                                var cApi1CollectablesTableNewEntryChronologyValHandle = (TempCollectablesIntVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(20)->Value;
                                var cApi1CollectablesTableNewEntryShowOnCollectValHandle = (TempCollectablesBoolVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(23)->Value;
                                var cApi1CollectablesTableNewEntryIsPopCharacterValHandle = (TempCollectablesBoolVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(25)->Value;
                                var cApi1CollectablesTableNewEntryIsQuestCharacterValHandle = (TempCollectablesBoolVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(26)->Value;
                                var cApi1CollectablesTableNewEntryIsCustomOrCostumeCharacterValHandle = (TempCollectablesBoolVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(27)->Value;
                                var cApi1CollectablesTableNewEntryIsProtocolDroidSegmentValHandle = (TempCollectablesBoolVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(28)->Value;
                                var cApi1CollectablesTableNewEntryIsBossCharacterValHandle = (TempCollectablesBoolVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(29)->Value;
                                var cApi1CollectablesTableNewEntryCheapGraphValHandle = (TempCollectablesGraphVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(30)->Value;
                                var cApi1CollectablesTableNewEntryImmuneToHazardousGasValHandle = (TempCollectablesBoolVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(31)->Value;
                                var cApi1CollectablesTableNewEntryShowOnPartyBarValHandle = (TempCollectablesBoolVal*)cApi1CollectablesTableNewEntryValuesHandle->Values.GetHandleAtIndex(32)->Value;

                                cApi1CollectablesTableNewEntryCollectStateValHandle->Value = 2;
                                TempNttResourcesNttResourceHandle.SetResourcePath(&cApi1CollectablesTableNewEntryGraphValHandle->Value, customCharacterInfo.PrefabResourcePath);
                                cApi1CollectablesTableNewEntryCharacterClassValHandle->Value = customCharacterInfo.Class switch
                                {
                                    CustomCharacterClass.Jedi => 0,
                                    CustomCharacterClass.Sith => 1,
                                    CustomCharacterClass.RebelResistance => 2,
                                    CustomCharacterClass.BountyHunter => 3,
                                    CustomCharacterClass.AstromechDroid => 4,
                                    CustomCharacterClass.ProtocolDroid => 5,
                                    CustomCharacterClass.Scoundrel => 6,
                                    CustomCharacterClass.GalacticEmpire => 7,
                                    CustomCharacterClass.Scavenger => 8,
                                    CustomCharacterClass.Civilian => 9,
                                    _ => throw new InvalidOperationException(),
                                };
                                TempCollectablesStringVal.SetValueAsStr(cApi1CollectablesTableNewEntryIconFileNameValHandle, "Silhouette_White.png");
                                TempCollectablesStringVal.SetValueAsStr(cApi1CollectablesTableNewEntryDisplayStringIdValHandle, customCharacterInfo.NameStringId);
                                TempCollectablesStringVal.SetValueAsStr(cApi1CollectablesTableNewEntryDescriptionStringIdValHandle, customCharacterInfo.DescriptionStringId);
                                cApi1CollectablesTableNewEntryIsNewValHandle->Value = 0;
                                TempCollectablesStringVal.SetValueAsStr(cApi1CollectablesTableNewEntryBaseCharacterValHandle, customCharacterInfo.Id);
                                cApi1CollectablesTableNewEntryCurrentlySelectedOutfitValHandle->Value = 1;
                                cApi1CollectablesTableNewEntryPriceValHandle->Value = 0;
                                TempCollectablesStringVal.SetValueAsStr(cApi1CollectablesTableNewEntryCharacterBaseNameIdValHandle, customCharacterInfo.NameStringId);
                                cApi1CollectablesTableNewEntryIsPlayableValHandle->Value = 1;
                                cApi1CollectablesTableNewEntryAddToPercentageValHandle->Value = 0;
                                cApi1CollectablesTableNewEntryIsRideableValHandle->Value = 0;
                                cApi1CollectablesTableNewEntryHasBeenRiddenByP1ValHandle->Value = 0;
                                cApi1CollectablesTableNewEntryHasBeenRiddenByP2ValHandle->Value = 0;
                                TempCollectablesStringVal.SetValueAsStr(cApi1CollectablesTableNewEntryCheatCodeValHandle, "");
                                cApi1CollectablesTableNewEntryChronologyValHandle->Value = 0;
                                cApi1CollectablesTableNewEntryShowOnCollectValHandle->Value = 0;
                                cApi1CollectablesTableNewEntryIsPopCharacterValHandle->Value = 0;
                                cApi1CollectablesTableNewEntryIsQuestCharacterValHandle->Value = 0;
                                cApi1CollectablesTableNewEntryIsCustomOrCostumeCharacterValHandle->Value = 0;
                                cApi1CollectablesTableNewEntryIsProtocolDroidSegmentValHandle->Value = 0;
                                cApi1CollectablesTableNewEntryIsBossCharacterValHandle->Value = 0;
                                TempNttResourcesNttResourceHandle.SetResourcePath(&cApi1CollectablesTableNewEntryCheapGraphValHandle->Value, customCharacterInfo.PreviewPrefabResourcePath);
                                cApi1CollectablesTableNewEntryImmuneToHazardousGasValHandle->Value = 0;
                                cApi1CollectablesTableNewEntryShowOnPartyBarValHandle->Value = 0;
                            }

                            wasCApi1CollectablesCharactersTableProcessed = true;
                        }
                    }
                }

                cApi1CollectablesTableHandles.Add((CApi1.collectables.Table.Handle)(nint)handle);

                return cApi1CollectablesTableConstructorMethodHook!.Trampoline!(handle, param0, param1, param2);
            }
        }
    );
}