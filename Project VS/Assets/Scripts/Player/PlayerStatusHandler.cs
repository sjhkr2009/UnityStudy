using System;
using UnityEngine;

[Serializable]
public class PlayerStatusHandler {
    public GameObject GameObject { get; }
    
    public Vector2 InputVector { get; set; } = Vector2.zero;
    public Vector2 DeltaMove { get; set; } = Vector2.zero;
    public bool IsDead { get; set; } = false;

    public PlayerStatusHandler(GameObject playerObject) {
        GameObject = playerObject;
    }

    public PlayerStatusHandler Clone() {
        return new PlayerStatusHandler(GameObject) {
            InputVector = InputVector,
            DeltaMove = DeltaMove,
            IsDead = IsDead
        };
    }
}
