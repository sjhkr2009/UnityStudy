using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;

public class PlayerController : MonoBehaviour {
    [SerializeField] private ItemController itemController;
    [ShowInInspector, ReadOnly] private PlayerStatus _playerStatus; // for debug

    public PlayerStatus Status => _playerStatus;
    protected bool isPaused = false;
    
    private PlayerMoveController moveController;
    private PlayerView viewController;

    private void Awake() {
        GameManager.Player = this;
        GameManager.OnPauseGame += OnPauseGame;
        GameManager.OnResumeGame += OnResumeGame;
        
        if (!itemController) itemController = GetComponentInChildren<ItemController>();
        
        var go = gameObject;
        _playerStatus = new PlayerStatus(go, itemController);
        moveController = new PlayerMoveController(Status);
        viewController = new PlayerView(Status);
    }

    private void FixedUpdate() {
        if (isPaused) return;
        
        _playerStatus.DeltaMove = Vector2.zero;
        moveController?.Move();
    }

    private void LateUpdate() {
        if (isPaused) return;
        
        viewController?.Render();
        viewController?.UpdateAnimator();
    }

    protected virtual void OnPauseGame() {
        isPaused = true;
    }
    
    protected virtual void OnResumeGame() {
        isPaused = false;
    }

    /** PlayerInput 컴포넌트에 의해 매 프레임 자동으로 호출됩니다. */
    [Preserve]
    void OnMove(InputValue inputValue) {
        // 세팅에 의해 normalized Vector2 값이 들어온다.
        _playerStatus.InputVector = inputValue.Get<Vector2>();
    }

    private void OnDestroy() {
        GameManager.OnPauseGame -= OnPauseGame;
        GameManager.OnResumeGame -= OnResumeGame;
    }
}
