using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPiece : Entity, IFlyawayCalculateTarget {
    public NormalPiece() : base(EntityIndex.NORMAL_PIECE) { }
    public List<EntityMetaData> GetMetaDatas() {
        return new List<EntityMetaData>() {
            EntityMetaData.Create(Index, "노말피스", "normal_piece.png"),
        };
    }
}

public class MovableBlockerPiece : Entity, ICustomFlyawayCalculateTarget {
    public MovableBlockerPiece() : base(EntityIndex.MOVABLE_BLOCKER) { }
    
    public List<EntityMetaData> GetMetaDatas() {
        return new List<EntityMetaData>() {
            EntityMetaData.Create(Index, 0, "블로커 1", "1.png"),
            EntityMetaData.Create(Index, 1, "블로커 2", "22.png"),
            EntityMetaData.Create(Index, 2, "블로커 3", "333.png"),
        };
    }

    public void ApplyCustomPriority(TilePriorityData tilePriorityData) {
        if (DisplayType == 0) tilePriorityData.priority += 100;
    }
}

public class DummyEntity : Entity {
    public DummyEntity() : base(EntityIndex.NONE) { }
}
