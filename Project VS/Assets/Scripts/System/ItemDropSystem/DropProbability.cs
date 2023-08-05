using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class DropProbability {
    [HorizontalGroup] public DropItemIndex itemIndex;
    [HorizontalGroup, Range(0, 100)] public float probability;

    public bool TryGetItem() {
        return (Random.Range(0f, 100f) < probability.Clamp(0f, 100f));
    }
}
