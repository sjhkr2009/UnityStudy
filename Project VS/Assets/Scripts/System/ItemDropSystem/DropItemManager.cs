using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DropItemManager {
    public static HashSet<DropItemBehavior> SpawnedDropItems { get; } = new HashSet<DropItemBehavior>();

    public static void SpawnByDropTable(IEnumerable<DropTable> dropTables, Vector2 position) {
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
            DropItemIndex.Magnetic => "Magnetic",
            _ => string.Empty
        };
    }

    public static void GainAllByMagnetic() {
        SpawnedDropItems.ForEach(item => {
            if (item.CanGainByMagnetic()) item.AnimateGain();
        });
    }

    public static void GainItem(DropItemIndex itemIndex) {
        if (itemIndex == DropItemIndex.Magnetic) {
            GainAllByMagnetic();
        } else {
            GameManager.Controller.GainExp(itemIndex.GetItemExp());
        }
        
        GameBroadcaster.CallGainDropItem(itemIndex);
    }

    public static void Clear() {
        var clearList = new List<GameObject>();
        
        // Abandon 과정에서 HashSet에서 요소가 제거되므로, 다른 리스트에 복사해두고 릴리즈한다
        SpawnedDropItems.ForEach(item => {
            if (item) clearList.Add(item.gameObject);
        });
        clearList.ForEach(PoolManager.Abandon);
        
        SpawnedDropItems.Clear();
    }
}
