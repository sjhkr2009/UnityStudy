using System;
using UnityEngine;

[Serializable]
public class PlayerStatus {
    public GameObject GameObject { get; }
    
    public Vector2 inputVector = Vector2.zero;
    public Vector2 deltaMove = Vector2.zero;
    public bool isDead = false;

    public PlayerStatus(GameObject playerObject) {
        GameObject = playerObject;
    }
}
