using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class DropTable {
    [SerializeField] private bool allowMultipleDrops;
    [SerializeField] private List<DropProbability> dropProbabilities;

    public IEnumerable<DropItemIndex> Get() {
        if (allowMultipleDrops) {
            foreach (var dropItemIndex in GetMultiDropItems()) {
                yield return dropItemIndex;
            }
        } else {
            var index = GetSingleDropItem();
            if (index != DropItemIndex.None) yield return index;
        }
    }

    private DropItemIndex GetSingleDropItem() {
        if (dropProbabilities == null) return DropItemIndex.None;

        var sum = dropProbabilities.Sum(p => p.probability);
        var value = Random.Range(0f, sum.ClampMin(100));
        var cur = 0f;

        foreach (var dropProbability in dropProbabilities) {
             cur += dropProbability.probability;
             if (value < cur) return dropProbability.itemIndex;
        }

        return DropItemIndex.None;
    }

    private IEnumerable<DropItemIndex> GetMultiDropItems() {
        foreach (var dropProbability in dropProbabilities) {
            if (dropProbability.TryGetItem()) yield return dropProbability.itemIndex;
        }
    }
}
