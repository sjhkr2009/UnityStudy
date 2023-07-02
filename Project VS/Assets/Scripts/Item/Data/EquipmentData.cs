using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class EquipmentData {
    [SerializeField] public EquipmentType equipmentType = EquipmentType.Unknown;
    [SerializeField] public ItemIndex itemIndex;
    
    [SerializeField] public string itemName;
    [SerializeField] public Sprite itemIcon;
    
    [SerializeField] public int maxLevel = Define.DataSetting.ItemMaxLevel;
    [SerializeField] public List<EquipmentDetailValue> detailValues = new List<EquipmentDetailValue>();
    [SerializeField] public List<string> descriptions = new List<string>();
    
    public int GetIntValue(EquipmentValueType type, int oneBasedLevel) => Mathf.RoundToInt(GetValue(type, oneBasedLevel));
    public float GetValue(EquipmentValueType type, int oneBasedLevel) {
        var data = detailValues.FirstOrDefault(v => v.Type == type);
        if (data == null) {
            Debugger.Error($"[ItemData.GetValue] {itemIndex} | indexedValues is empty!!");
            return default;
        }

        int zeroBasedIndex = oneBasedLevel - 1;
        return data.GetValue(zeroBasedIndex);
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
