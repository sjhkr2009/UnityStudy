using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(EquipmentDataContainer), menuName = "Custom/Create Weapon Data")]
public class EquipmentDataContainer : ScriptableObject {
    private static EquipmentDataContainer instance;
    
    [SerializeField] public List<EquipmentData> dataContainer = new List<EquipmentData>();

    public EquipmentData GetData(ItemIndex itemIndex) {
        return dataContainer.FirstOrDefault(d => itemIndex == d.itemIndex);
    }

    private EquipmentData AddDataInternal(ItemIndex itemIndex) {
        var data = new EquipmentData() { itemIndex = itemIndex };
        dataContainer.Add(data);
        return data;
    }

    public EquipmentData AddData(ItemIndex itemIndex) {
        if (dataContainer.Exists(d => d.itemIndex == itemIndex)) {
            Debugger.Warning($"{itemIndex} data already exist!");
            return GetData(itemIndex);
        }
        
        return AddDataInternal(itemIndex);
    }

    public void Validate(bool removeInvalidData) {
        Dictionary<ItemIndex, EquipmentData> itemDatas = new Dictionary<ItemIndex, EquipmentData>();
        List<EquipmentData> invalidData = new List<EquipmentData>();
        int invalidIndex = 0;

        foreach (var itemData in dataContainer) {
            bool isValid = true;
            if (!Enum.IsDefined(typeof(ItemIndex), itemData.itemIndex)) {
                Debugger.Warning($"ItemIndex not defined: {itemData.itemName} | This data will be removed.");
                isValid = false;
            }
            if (itemDatas.ContainsKey(itemData.itemIndex)) {
                Debugger.Warning($"Duplicate data found for {itemData.itemIndex}!! Removed except the first.");
                isValid = false;
            }

            if (isValid) {
                itemDatas[itemData.itemIndex] = itemData;
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

    public static EquipmentData GetDataOrDefault(ItemIndex itemIndex) {
        return GetInstance().GetData(itemIndex) ?? new EquipmentData() { itemIndex = itemIndex };
    }

    private static EquipmentDataContainer GetInstance() {
        if (instance) return instance;

        instance = Resources.Load<EquipmentDataContainer>(nameof(EquipmentDataContainer));
        if (!instance) {
            Debugger.Error("Cannot find 'ItemDataContainer' in Resources...");
            instance = CreateInstance<EquipmentDataContainer>();
        }

        return instance;
    }
}
