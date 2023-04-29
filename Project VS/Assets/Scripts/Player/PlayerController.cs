using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;

public class PlayerController : MonoBehaviour {
    [ShowInInspector, ReadOnly] private PlayerStatus _playerStatusHandler;

    public PlayerStatus GetStatusHandler => _playerStatusHandler;
    
    private PlayerMoveController moveController;
    private PlayerView viewController;

    private void Awake() {
        GlobalData.Player = this;
        
        var go = gameObject;
        _playerStatusHandler = new PlayerStatus(go);
        moveController = new PlayerMoveController(go);
        viewController = new PlayerView(go);
    }

    private void FixedUpdate() {
        _playerStatusHandler.DeltaMove = Vector2.zero;
        moveController?.Move(_playerStatusHandler);
    }

    private void LateUpdate() {
        viewController?.Render(_playerStatusHandler);
        viewController?.UpdateAnimator(_playerStatusHandler);
    }

    /** PlayerInput 컴포넌트에 의해 매 프레임 자동으로 호출됩니다. */
    [Preserve]
    void OnMove(InputValue inputValue) {
        // 세팅에 의해 normalized Vector2 값이 들어온다.
        _playerStatusHandler.InputVector = inputValue.Get<Vector2>();
    }
}
