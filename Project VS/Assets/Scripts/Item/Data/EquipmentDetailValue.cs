using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class EquipmentDetailValue {
    [SerializeField] private EquipmentValueType type;
    [SerializeField] private List<float> values;
    
    private EquipmentDetailValue(){}

    public static EquipmentDetailValue Create(EquipmentValueType type, int count) {
        var value = new EquipmentDetailValue() {
            type = type
        };
        value.SetValueCount(count);
        return value;
    }

    public void SetValueCount(int count) {
        values ??= new List<float>(count);
        
        while (values.Count < count) {
            values.Add(values.LastOrDefault());
        }

        while (values.Count > count) {
            values.RemoveAt(values.Count - 1);
        }
    }

    public EquipmentValueType Type => type;
    public float GetValue(int index) {
        if (values == null || values.Count == 0) {
            Debugger.Error($"[ItemDetailValue.GetValue] {type} value is empty!");
            return default;
        }

        if (index < 0 || index >= values.Count) {
            Debugger.Warning($"[ItemDetailValue.GetValue] {type} value not contain {index} index!");
            index = index.Clamp(0, values.Count - 1);
        }

        return values[index];
    }

    public void SetValue(int index, float value) {
        if (index < 0 || index >= values.Count) {
            Debugger.Error($"[ItemDetailValue.SetValue] {type} value not contain {index} index!");
            return;
        }

        values[index] = value;
    }
}
