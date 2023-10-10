using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitMoveStrategy : IMoveStrategy {
    public OrbitMoveStrategy(Transform transform, Transform center, float angleSpeed, bool isClockwise) {
        Transform = transform;
        Center = center;
        Speed = angleSpeed;
        IsClockwise = isClockwise;
    }
    
    public Transform Transform { get; }
    public Transform Center { get; }
    public float Speed { get; set; }
    public bool IsClockwise { get; set; }
    
    public void Move() {
        Vector3 vectorDiff = Transform.position - Center.position;
        vectorDiff.y = 0f;
        
        var radius = vectorDiff.magnitude;
        float deltaAngle = (Speed / radius) * Time.deltaTime * (IsClockwise ? -1 : 1);
        Transform.RotateAround(Center.position, Vector3.up, deltaAngle);
        
        // 회전방향을 반영한 Center에 수직인 방향 벡터를 Transform의 z축으로 바라보게 한다.
        Transform.rotation = Quaternion.LookRotation(vectorDiff) * Quaternion.Euler(new Vector3(0, IsClockwise ? -90 : 90, 0));
    }
}
