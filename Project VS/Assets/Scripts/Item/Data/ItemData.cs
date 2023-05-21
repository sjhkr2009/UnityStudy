using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class ItemData {
    [SerializeField] public ItemType itemType = ItemType.Unknown;
    [SerializeField] public ItemIndex itemIndex;
    
    [SerializeField] public string itemName;
    [SerializeField] public Sprite itemIcon;
    
    [SerializeField] public int maxLevel = Define.DataSetting.ItemMaxLevel;
    [SerializeField] public List<ItemIndexedValue> indexedValues = new List<ItemIndexedValue>();
    [SerializeField] public List<string> descriptions = new List<string>();

    public ItemIndexedValue GetValue(int oneBasedLevel) {
        int zeroBasedLevel = oneBasedLevel - 1;

        if (indexedValues == null || indexedValues.Count == 0) {
            Debugger.Error($"[ItemData.GetValue] {itemIndex} | indexedValues is empty!!");
            return new ItemIndexedValue();
        }

        if (zeroBasedLevel < 0 || zeroBasedLevel >= indexedValues.Count) {
            Debugger.Warning($"[ItemData.GetValue] {itemIndex} | indexedValues not have {zeroBasedLevel} data. Return clamped value(0~{indexedValues.Count}).");
            zeroBasedLevel = zeroBasedLevel.Clamp(0, indexedValues.Count - 1);
        }

        return indexedValues[zeroBasedLevel];
    }

    public string GetDescription(int oneBasedLevel) {
        int zeroBasedLevel = oneBasedLevel - 1;
        
        if (descriptions == null || descriptions.Count == 0) {
            Debugger.Error($"[ItemData.GetDescription] {itemIndex} | descriptions is empty!!");
            return string.Empty;
        }

        if (zeroBasedLevel < 0 || zeroBasedLevel >= descriptions.Count) {
            Debugger.Warning($"[ItemData.GetDescription] {itemIndex} | descriptions not have {zeroBasedLevel} data. Return clamped value(0~{descriptions.Count}).");
            zeroBasedLevel = zeroBasedLevel.Clamp(0, descriptions.Count - 1);
        }

        var ret = descriptions[zeroBasedLevel];
        if (string.IsNullOrWhiteSpace(ret)) {
            Debugger.Log($"[ItemData.GetDescription] {itemIndex} | description is empty. Return first valid description.");
            return descriptions.FirstOrDefault(desc => !string.IsNullOrWhiteSpace(desc)) ?? string.Empty;
        }
        return ret;
    }
}
