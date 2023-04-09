using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;

public class PlayerController : MonoBehaviour {
    [ShowInInspector, ReadOnly] private PlayerStatus playerStatus;

    public PlayerStatus GetStatus => playerStatus;
    
    private PlayerMoveController moveController;
    private PlayerView viewController;

    private void Awake() {
        GlobalCachedData.Player = this;
        
        var go = gameObject;
        playerStatus = new PlayerStatus(go);
        moveController = new PlayerMoveController(go);
        viewController = new PlayerView(go);
    }

    private void FixedUpdate() {
        playerStatus.DeltaMove = Vector2.zero;
        moveController?.Move(playerStatus);
    }

    private void LateUpdate() {
        viewController?.Render(playerStatus);
        viewController?.UpdateAnimator(playerStatus);
    }

    /** PlayerInput 컴포넌트에 의해 매 프레임 자동으로 호출됩니다. */
    [Preserve]
    void OnMove(InputValue inputValue) {
        // 세팅에 의해 normalized Vector2 값이 들어온다.
        playerStatus.InputVector = inputValue.Get<Vector2>();
    }
}
