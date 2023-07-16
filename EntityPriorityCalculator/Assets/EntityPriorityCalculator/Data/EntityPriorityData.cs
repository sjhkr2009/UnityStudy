[System.Serializable]
public class EntityPriorityData {
    public EntityIndex targetEntity;
    public int targetDisplayType;
    public int priorityGroup;
    public int priority;
    public bool isBlocker;

    public bool IsTargetData(Entity entity) {
        return entity.Index == targetEntity && entity.DisplayType == targetDisplayType;
    }
}
