using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;

public class PlayerController : MonoBehaviour {
    [ShowInInspector, ReadOnly] private PlayerStatus _playerStatus; // for debug

    public PlayerStatus Status => _playerStatus;
    
    private PlayerMoveController moveController;
    private PlayerView viewController;

    private void Awake() {
        GameManager.Player = this;
        
        var go = gameObject;
        _playerStatus = new PlayerStatus(go);
        moveController = new PlayerMoveController(Status);
        viewController = new PlayerView(Status);
    }

    private void FixedUpdate() {
        _playerStatus.DeltaMove = Vector2.zero;
        moveController?.Move();
    }

    private void LateUpdate() {
        viewController?.Render();
        viewController?.UpdateAnimator();
    }

    /** PlayerInput 컴포넌트에 의해 매 프레임 자동으로 호출됩니다. */
    [Preserve]
    void OnMove(InputValue inputValue) {
        // 세팅에 의해 normalized Vector2 값이 들어온다.
        _playerStatus.InputVector = inputValue.Get<Vector2>();
    }
}
