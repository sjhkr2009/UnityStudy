using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "FlyawayPriorityData", menuName = "Create FlyawayPriorityData")]
public class FlyawayPriorityData : ScriptableObject {
    [SerializeField] public List<EntityPriorityData> entityPriorities;

    public TilePriorityData CalculateTilePriority(Tile tile) {
        TilePriorityData tilePriorityData = new TilePriorityData(tile);
        foreach (var entity in tile.Entities) {
            var data = entityPriorities.FirstOrDefault(ep => ep.IsTargetData(entity));
            ApplyOnTilePriority(tilePriorityData, data);

            if (entity is ICustomFlyawayCalculateTarget customCalculateTarget) {
                customCalculateTarget.ApplyCustomPriority(tilePriorityData);
            }

            if (tilePriorityData.isBlocked) break;
        }
        
        return tilePriorityData;
    }

    private void ApplyOnTilePriority(TilePriorityData tilePriorityData, EntityPriorityData entityPriorityData) {
        if (entityPriorityData == null) return;
        
        tilePriorityData.priority += entityPriorityData.priority;
        tilePriorityData.group = Mathf.Max(entityPriorityData.priorityGroup, tilePriorityData.group);
        if (entityPriorityData.isBlocker) tilePriorityData.isBlocked = true;
    }

    public void ResetByMetaData(List<EntityMetaData> metaDatas) {
        entityPriorities = metaDatas.Select(m => new EntityPriorityData() {
            targetEntity = m.entityIndex,
            targetDisplayType = m.displayType
        }).ToList();
    }
}
