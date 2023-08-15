using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController {
    private Rigidbody2D Rigidbody { get; }
    private PlayerStatus Status { get; }
    private Joystick PlayerJoystick { get; }
    
    public Vector2 CurrentSpeed { get; private set; }
    public Vector2 DeltaMove { get; private set; }

    public PlayerMoveController(PlayerController.ComponentHolder componentHolder, PlayerStatus status) {
        Rigidbody = componentHolder.rigidbody;
        Status = status;
        PlayerJoystick = componentHolder.joystick;
    }

    public void Move() {
        if (Status.IsDead) return;
        
        var CurrentSpeed = Status.InputVector.normalized;

        var speed = Status.Speed;
        /*var acceleration = Status.Acceleration;
        
        // 참고) 속도 제어 방식은 AddForce나 velocity 사용
        var inputX = Mathf.Approximately(inputVector.x, 0f)
            ? (Mathf.Abs(CurrentSpeed.x) < 0.001f ? 0f : Mathf.Lerp(CurrentSpeed.x, 0f, acceleration * Time.fixedDeltaTime)) 
            : (CurrentSpeed.x + (inputVector.x * acceleration * Time.fixedDeltaTime)).Clamp(-1f, 1f);
        var inputY = Mathf.Approximately(inputVector.y, 0f)
            ? (Mathf.Abs(CurrentSpeed.y) < 0.001f ? 0f : Mathf.Lerp(CurrentSpeed.y, 0f, acceleration * Time.fixedDeltaTime)) 
            : (CurrentSpeed.y + (inputVector.y * acceleration * Time.fixedDeltaTime)).Clamp(-1f, 1f);
        CurrentSpeed = new Vector2(inputX, inputY).normalized;*/
        
        var moveDelta = (speed * Time.fixedDeltaTime) * CurrentSpeed;
        Rigidbody.MovePosition(Rigidbody.position + moveDelta);
        Status.DeltaMove = moveDelta;

        if (!Mathf.Approximately(CurrentSpeed.magnitude, 0f)) {
            Status.ShowDirection = CurrentSpeed.normalized;
        }
    }
}
