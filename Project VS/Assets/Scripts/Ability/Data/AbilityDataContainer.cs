using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(AbilityDataContainer), menuName = "Custom/Create Weapon Data")]
public class AbilityDataContainer : ScriptableObject {
    private static AbilityDataContainer instance;
    
    [SerializeField] public List<AbilityData> dataContainer = new List<AbilityData>();

    public AbilityData GetData(AbilityIndex abilityIndex) {
        return dataContainer.FirstOrDefault(d => abilityIndex == d.abilityIndex);
    }

    private AbilityData AddDataInternal(AbilityIndex abilityIndex) {
        var data = new AbilityData() { abilityIndex = abilityIndex };
        dataContainer.Add(data);
        return data;
    }

    public AbilityData AddData(AbilityIndex abilityIndex) {
        if (dataContainer.Exists(d => d.abilityIndex == abilityIndex)) {
            Debugger.Warning($"{abilityIndex} data already exist!");
            return GetData(abilityIndex);
        }
        
        return AddDataInternal(abilityIndex);
    }

    public void Validate(bool removeInvalidData) {
        Dictionary<AbilityIndex, AbilityData> itemDatas = new Dictionary<AbilityIndex, AbilityData>();
        List<AbilityData> invalidData = new List<AbilityData>();
        int invalidIndex = 0;

        foreach (var itemData in dataContainer) {
            bool isValid = true;
            if (!Enum.IsDefined(typeof(AbilityIndex), itemData.abilityIndex)) {
                Debugger.Warning($"ItemIndex not defined: {itemData.itemName} | This data will be removed.");
                isValid = false;
            }
            if (itemDatas.ContainsKey(itemData.abilityIndex)) {
                Debugger.Warning($"Duplicate data found for {itemData.abilityIndex}!! Removed except the first.");
                isValid = false;
            }

            if (isValid) {
                itemDatas[itemData.abilityIndex] = itemData;
            } else {
                invalidData.Add(itemData);
                invalidIndex++;
            }

        }

        if (removeInvalidData) {
            foreach (var data in invalidData) {
                dataContainer.Remove(data);
            }
        }

        if (invalidIndex > 0) Debugger.Warning($"Find {invalidIndex} invalid data.");
        else Debugger.Log("All data is valid.");
    }

    public static AbilityData LoadData(AbilityIndex abilityIndex) {
        var data = GetInstance().GetData(abilityIndex);
        if (data == null) {
            Debugger.Error($"[AbilityDataContainer.LoadData] {abilityIndex} 무기에 대한 데이터가 없습니다!");
            return new AbilityData() { abilityIndex = abilityIndex };
        }

        return data;
    }

    private static AbilityDataContainer GetInstance() {
        if (instance) return instance;

        instance = Resources.Load<AbilityDataContainer>(nameof(AbilityDataContainer));
        if (!instance) {
            Debugger.Error("Cannot find 'ItemDataContainer' in Resources...");
            instance = CreateInstance<AbilityDataContainer>();
        }

        return instance;
    }
}
