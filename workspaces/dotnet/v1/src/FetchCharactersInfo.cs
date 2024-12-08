using System;
using System.Collections.Generic;
using OMP.LSWTSS.CApi1;

namespace OMP.LSWTSS;

public static partial class V1
{
    public static CharacterInfo[]? FetchCharactersInfo()
    {
        if (wasCApi1CollectablesCharactersTableProcessed == false)
        {
            return null;
        }

        var charactersInfo = new List<CharacterInfo>();

        foreach (var collectablesTableHandle in cApi1CollectablesTableHandles)
        {
            var collectablesTableName = collectables.Table.GetNameMethod.Execute(collectablesTableHandle);

            if (collectablesTableName == "Characters")
            {
                var collectablesTableNumEntries = collectablesTableHandle.GetNumEntries();

                unsafe
                {
                    var collectablesTableValuesHandle = (TempNuVector<TempCollectablesEntryValues>*)(collectablesTableHandle + 0x140);

                    for (int collectablesTableEntryIndex = 0; collectablesTableEntryIndex < collectablesTableNumEntries; collectablesTableEntryIndex++)
                    {
                        var collectablesTableEntryValuesHandle =
                            collectablesTableValuesHandle->GetHandleAtIndex(collectablesTableEntryIndex);

                        var collectablesTableEntryGraphValHandle = (TempCollectablesGraphVal*)collectablesTableEntryValuesHandle->Values.GetHandleAtIndex(1)->Value;
                        var collectablesTableEntryCharacterClassValHandle = (TempCollectablesEnumVal*)collectablesTableEntryValuesHandle->Values.GetHandleAtIndex(2)->Value;
                        var collectablesTableEntryDisplayStringIdValHandle = (TempCollectablesStringVal*)collectablesTableEntryValuesHandle->Values.GetHandleAtIndex(4)->Value;
                        var collectablesTableEntryDescriptionStringIdValHandle = (TempCollectablesStringVal*)collectablesTableEntryValuesHandle->Values.GetHandleAtIndex(5)->Value;

                        charactersInfo.Add(
                            new CharacterInfo
                            {
                                NameStringId = TempCollectablesStringVal.GetValueAsStr(collectablesTableEntryDisplayStringIdValHandle) ?? throw new InvalidOperationException(),
                                DescriptionStringId = TempCollectablesStringVal.GetValueAsStr(collectablesTableEntryDescriptionStringIdValHandle) ?? throw new InvalidOperationException(),
                                Class = collectablesTableEntryCharacterClassValHandle->Value switch {
                                    0 => CharacterClass.Jedi,
                                    1 => CharacterClass.Sith,
                                    2 => CharacterClass.RebelResistance,
                                    3 => CharacterClass.BountyHunter,
                                    4 => CharacterClass.AstromechDroid,
                                    5 => CharacterClass.ProtocolDroid,
                                    6 => CharacterClass.Scoundrel,
                                    7 => CharacterClass.GalacticEmpire,
                                    8 => CharacterClass.Scavenger,
                                    9 => CharacterClass.Civilian,
                                    _ => throw new InvalidOperationException()
                                },
                                PrefabResourcePath = TempNttResourcesNttResourceHandle.GetResourcePath(&collectablesTableEntryGraphValHandle->Value) ?? throw new InvalidOperationException()
                            }
                        );
                    }
                }

                break;
            }
        }

        return charactersInfo.ToArray();
    }
}