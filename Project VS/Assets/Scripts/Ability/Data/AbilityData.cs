using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class AbilityData {
    [SerializeField] public AbilityIndex abilityIndex;
    
    [SerializeField] public string itemName;
    [SerializeField] public Sprite itemIcon;
    [SerializeField] public List<Sprite> iconsByLevel = new List<Sprite>();
    
    [SerializeField] public int maxLevel = Define.DataSetting.ItemDefaultMaxLevel;
    [SerializeField] public List<AbilityDetailValue> detailValues = new List<AbilityDetailValue>();
    [SerializeField] public List<string> descriptions = new List<string>();
    
    public int GetIntValue(AbilityValueType type, int oneBasedLevel) => Mathf.RoundToInt(GetValue(type, oneBasedLevel));
    public float GetValue(AbilityValueType type, int oneBasedLevel = 1) {
        var data = detailValues.FirstOrDefault(v => v.Type == type);
        if (data == null) {
            Debugger.Error($"[ItemData.GetValue] {abilityIndex} | indexedValues is empty!!");
            return default;
        }

        int zeroBasedIndex = oneBasedLevel - 1;
        return data.GetValue(zeroBasedIndex);
    }

    public Sprite GetIcon(int oneBasedLevel) {
        if (iconsByLevel == null || iconsByLevel.Count == 0) return itemIcon;

        var icon = iconsByLevel[(oneBasedLevel - 1).Clamp(0, iconsByLevel.Count - 1)];
        if (icon == null) return itemIcon;

        return icon;
    }

    public string GetDescription(int oneBasedLevel) {
        int zeroBasedLevel = oneBasedLevel - 1;
        
        if (descriptions == null || descriptions.Count == 0) {
            Debugger.Error($"[ItemData.GetDescription] {abilityIndex} | descriptions is empty!!");
            return string.Empty;
        }

        if (zeroBasedLevel < 0 || zeroBasedLevel >= descriptions.Count) {
            Debugger.Warning($"[ItemData.GetDescription] {abilityIndex} | descriptions not have {zeroBasedLevel} data. Return clamped value(0~{descriptions.Count}).");
            zeroBasedLevel = zeroBasedLevel.Clamp(0, descriptions.Count - 1);
        }

        var ret = descriptions[zeroBasedLevel];
        if (string.IsNullOrWhiteSpace(ret)) {
            Debugger.Log($"[ItemData.GetDescription] {abilityIndex} | description is empty. Return first valid description.");
            return descriptions.FirstOrDefault(desc => !string.IsNullOrWhiteSpace(desc)) ?? string.Empty;
        }
        return ret;
    }
}
