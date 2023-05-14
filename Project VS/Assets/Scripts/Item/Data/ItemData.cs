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
    [BoxGroup("Spec")] public int baseCount;
    [BoxGroup("Spec")] public List<float> values = new List<float>();
    [BoxGroup("Spec")] public List<float> subValues = new List<float>();
    [BoxGroup("Spec")] public List<int> counts = new List<int>();

    public static ItemData CreateDefault(ItemIndex itemIndex) {
        return new ItemData {
            itemIndex = itemIndex
        };
    }

    public float GetValue(int oneBasedLevel) {
        if (values == null || values.Count == 0) return baseValue;

        var index = (oneBasedLevel - 1).Clamp(0, values.Count - 1);
        if (index != (oneBasedLevel - 1)) {
            Debugger.Warning($"[ItemData.GetValue] Cannot find {oneBasedLevel} level data on {itemIndex}. Return clamped value.");
        }
        return values[index];
    }
}
