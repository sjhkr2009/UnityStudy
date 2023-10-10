using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightMoveStrategy : IMoveStrategy {
    public StraightMoveStrategy(Transform transform, float speed) {
        Transform = transform;
        Speed = speed;
    }
    
    public Transform Transform { get; }
    public float Speed { get; set; }

    public void Move() {
        Transform.Translate(0f, 0f, Speed * Time.deltaTime);
    }
}
