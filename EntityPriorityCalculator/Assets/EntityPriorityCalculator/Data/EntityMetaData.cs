using System;

public class EntityMetaData {
    public EntityIndex entityIndex;
    public int displayType;
    public string entityName;
    public string spriteName;

    private EntityMetaData() { }
    public static EntityMetaData Create(EntityIndex entityIndex, int displayType, string entityName, string spriteName) {
        return new EntityMetaData() {
            entityIndex = entityIndex,
            displayType = displayType,
            entityName = entityName,
            spriteName = spriteName
        };
    }

    public static EntityMetaData Create(EntityIndex entityIndex, string entityName, string spriteName)
        => Create(entityIndex, 0, entityName, spriteName);
}
