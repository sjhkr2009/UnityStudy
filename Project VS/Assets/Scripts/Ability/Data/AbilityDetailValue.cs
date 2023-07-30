using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class AbilityDetailValue {
    [SerializeField] private AbilityValueType type;
    [SerializeField] private List<float> values;
    
    private AbilityDetailValue(){}

    public static AbilityDetailValue Create(AbilityValueType type, int count) {
        var value = new AbilityDetailValue() {
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

    public AbilityValueType Type => type;
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
