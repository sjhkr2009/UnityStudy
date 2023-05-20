using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ItemDataContainer), menuName = "Custom/Create Weapon Data")]
public class ItemDataContainer : ScriptableObject {
    private static ItemDataContainer instance;
    
    [SerializeField] public List<ItemData> dataContainer = new List<ItemData>();

    public ItemData GetData(ItemIndex itemIndex) {
        return dataContainer.FirstOrDefault(d => itemIndex == d.itemIndex);
    }

    private ItemData AddDataInternal(ItemIndex itemIndex) {
        var data = new ItemData() { itemIndex = itemIndex };
        dataContainer.Add(data);
        return data;
    }

    public ItemData AddData(ItemIndex itemIndex) {
        if (dataContainer.Exists(d => d.itemIndex == itemIndex)) {
            Debugger.Warning($"{itemIndex} data already exist!");
            return GetData(itemIndex);
        }
        
        return AddDataInternal(itemIndex);
    }

    public void Validate(bool removeInvalidData) {
        Dictionary<ItemIndex, ItemData> itemDatas = new Dictionary<ItemIndex, ItemData>();
        List<ItemData> invalidData = new List<ItemData>();
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

    public static ItemData GetDataOrDefault(ItemIndex itemIndex) {
        return GetInstance().GetData(itemIndex) ?? new ItemData() { itemIndex = itemIndex };
    }

    private static ItemDataContainer GetInstance() {
        if (instance) return instance;

        instance = Resources.Load<ItemDataContainer>(nameof(ItemDataContainer));
        if (!instance) {
            Debugger.Error("Cannot find 'ItemDataContainer' in Resources...");
            instance = CreateInstance<ItemDataContainer>();
        }

        return instance;
    }
}
