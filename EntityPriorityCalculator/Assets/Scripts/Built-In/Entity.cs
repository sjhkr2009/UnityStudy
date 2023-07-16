using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity {
    protected Entity(EntityIndex entityIndex) {
        Index = entityIndex;
    }
    public EntityIndex Index { get; }
    public int DisplayType { get; set; }
}

public static class EntityGetter {
    public static Dictionary<EntityIndex, Entity> Get() {
        return new Dictionary<EntityIndex, Entity>() {
            { EntityIndex.NONE, new DummyEntity() },
            { EntityIndex.NORMAL_PIECE, new NormalPiece() },
            { EntityIndex.MOVABLE_BLOCKER, new MovableBlockerPiece() },
        };
    }
}
