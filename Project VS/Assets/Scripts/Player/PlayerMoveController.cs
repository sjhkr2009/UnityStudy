using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController {
    private Rigidbody2D Rigidbody { get; }

    public float Speed { get; private set; }
    public float Acceleration { get; private set; }
    public Vector2 CurrentSpeed { get; private set; }
    public Vector2 DeltaMove { get; private set; }

    public PlayerMoveController(GameObject target, float speed = 3f, float acceleration = 20f) {
        Rigidbody = target.GetOrAddComponent<Rigidbody2D>();
        Speed = speed;
        Acceleration = acceleration;
    }

    public void Move(PlayerStatus playerStatus) {
        var inputVector = playerStatus.inputVector;
        
        // 참고) 속도 제어 방식은 AddForce나 velocity 사용
        var inputX = Mathf.Approximately(inputVector.x, 0f)
            ? (Mathf.Abs(CurrentSpeed.x) < 0.001f ? 0f : Mathf.Lerp(CurrentSpeed.x, 0f, Acceleration * Time.fixedDeltaTime)) 
            : (CurrentSpeed.x + (inputVector.x * Acceleration * Time.fixedDeltaTime)).Clamp(-1f, 1f);
        var inputY = Mathf.Approximately(inputVector.y, 0f)
            ? (Mathf.Abs(CurrentSpeed.y) < 0.001f ? 0f : Mathf.Lerp(CurrentSpeed.y, 0f, Acceleration * Time.fixedDeltaTime)) 
            : (CurrentSpeed.y + (inputVector.y * Acceleration * Time.fixedDeltaTime)).Clamp(-1f, 1f);
        CurrentSpeed = new Vector2(inputX, inputY);
        
        var moveDelta = (Speed * Time.fixedDeltaTime) * CurrentSpeed;
        Rigidbody.MovePosition(Rigidbody.position + moveDelta);
        playerStatus.deltaMove = moveDelta;
    }

    public void SetSpeed(float speed) => Speed = speed;
    public void SetAcceleration(float acceleration) => Acceleration = acceleration;
}
