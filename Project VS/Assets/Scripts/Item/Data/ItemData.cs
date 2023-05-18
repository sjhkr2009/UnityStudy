using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class ItemData {
    [BoxGroup("Main")] public ItemType itemType = ItemType.Unknown;
    [BoxGroup("Main")] public ItemIndex itemIndex;
    
    [BoxGroup("View")] public string itemName;
    [BoxGroup("View")] public string itemDesc;
    [BoxGroup("View")] public Sprite itemIcon;
    
    [BoxGroup("Spec")] public float baseValue;
    [BoxGroup("Spec")] public float baseSubValue;
    [BoxGroup("Spec")] public float baseRange;
    [BoxGroup("Spec")] public int baseCount;
    [BoxGroup("Spec")] public List<float> values = new List<float>();
    [BoxGroup("Spec")] public List<float> subValues = new List<float>();
    [BoxGroup("Spec")] public List<float> ranges = new List<float>();
    [BoxGroup("Spec")] public List<int> counts = new List<int>();

    public static ItemData CreateDefault(ItemIndex itemIndex) {
        return new ItemData {
            itemIndex = itemIndex
        };
    }

    public float GetMainValue(int oneBasedLevel) {
        if (values == null || values.Count == 0) return baseValue;

        var index = (oneBasedLevel - 1).Clamp(0, values.Count - 1);
        if (index != (oneBasedLevel - 1)) {
            Debugger.Warning($"[ItemData.GetValue] Cannot find {oneBasedLevel} level data on {itemIndex}. Return clamped value.");
        }
        return values[index];
    }
    
    public float GetRange(int oneBasedLevel) {
        if (ranges == null || ranges.Count == 0) return baseRange;

        var index = (oneBasedLevel - 1).Clamp(0, ranges.Count - 1);
        if (index != (oneBasedLevel - 1)) {
            Debugger.Warning($"[ItemData.GetRange] Cannot find {oneBasedLevel} level data on {itemIndex}. Return clamped value.");
        }
        return ranges[index];
    }
    
    public float GetSubValue(int oneBasedLevel) {
        if (subValues == null || subValues.Count == 0) return baseSubValue;

        var index = (oneBasedLevel - 1).Clamp(0, subValues.Count - 1);
        if (index != (oneBasedLevel - 1)) {
            Debugger.Warning($"[ItemData.GetSubValue] Cannot find {oneBasedLevel} level data on {itemIndex}. Return clamped value.");
        }
        return subValues[index];
    }
    
    public int GetCount(int oneBasedLevel) {
        if (counts == null || counts.Count == 0) return baseCount;

        var index = (oneBasedLevel - 1).Clamp(0, counts.Count - 1);
        if (index != (oneBasedLevel - 1)) {
            Debugger.Warning($"[ItemData.GetCount] Cannot find {oneBasedLevel} level data on {itemIndex}. Return clamped value.");
        }
        return counts[index];
    }
}
