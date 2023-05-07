using System;
using UnityEngine;

[Serializable]
public class PlayerStatus {
    public GameObject GameObject { get; }

    public float Speed { get; set; } = 3f;
    public float Acceleration { get; set; } = 20f;
    public Vector2 InputVector { get; set; } = Vector2.zero;
    public Vector2 DeltaMove { get; set; } = Vector2.zero;
    public bool IsDead { get; set; } = false;

    public PlayerStatus(GameObject playerObject) {
        GameObject = playerObject;
    }

    public PlayerStatus Clone() {
        return new PlayerStatus(GameObject) {
            InputVector = InputVector,
            DeltaMove = DeltaMove,
            IsDead = IsDead
        };
    }
}
