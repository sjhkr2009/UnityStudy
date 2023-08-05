using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DropItemManager {
    public static HashSet<DropItemBehavior> SpawnedDropItems { get; } = new HashSet<DropItemBehavior>();

    public static void SpawnByDropTable(List<DropTable> dropTables, Vector2 position) {
        var dropItemIndices = dropTables.SelectMany(dropTable => dropTable.Get()).ToList();

        foreach (var dropItemIndex in dropItemIndices) {
            var posDiff = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            posDiff *= (dropItemIndices.Count - 1) * 0.1f;

            Spawn(dropItemIndex, position + posDiff);
        }
    }
    
    public static void Spawn(DropItemIndex itemIndex, Vector2 position) {
        if (itemIndex == DropItemIndex.None) return;
        
        var prefabName = itemIndex.GetPrefabName();
        var dropItem = PoolManager.Get<DropItemBehavior>(prefabName);
        if (!dropItem) {
            Debugger.Error($"[DropItemSpawner.Spawn] Fail to Load Prefab: {itemIndex}");
            return;
        }

        dropItem.transform.position = position;
    }

    public static int GetItemExp(this DropItemIndex dropItemIndex) {
        return dropItemIndex switch {
            DropItemIndex.SmallSoul => 10,
            DropItemIndex.MiddleSoul => 100,
            DropItemIndex.BigSoul => 300,
            _ => 0
        };
    }

    private static string GetPrefabName(this DropItemIndex dropItemIndex) {
        return dropItemIndex switch {
            DropItemIndex.SmallSoul => "SmallSoul",
            DropItemIndex.MiddleSoul => "MiddleSoul",
            DropItemIndex.BigSoul => "BigSoul",
            _ => string.Empty
        };
    }
}
