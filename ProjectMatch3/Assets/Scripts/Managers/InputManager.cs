using System;
using System.Collections.Generic;
using UnityEngine;
using Define;

/// <summary>
/// 유저 입력을 처리하며 GameManager를 통해 매 프레임 실행됩니다.
/// </summary>
public class InputManager
{
    /// <summary>
    /// 입력에 따라 실행되는 이벤트 동작입니다.
    /// </summary>
    public event Action<InputType, Vector2> InputAction = null;

    /// <summary>
    /// 현재 받고 있는 입력의 유형을 저장합니다.
    /// </summary>
    private InputType currentInput = InputType.None;
    /// <summary>
    /// 최근 클릭 입력을 받은 위치를 스크린 좌표로 저장합니다.
    /// </summary>
    public Vector2 LastDownPoint { get; private set; }
    /// <summary>
    /// 최근 클릭 입력을 뗀 지점의 위치를 스크린 좌표로 저장합니다.
    /// </summary>
    public Vector2 LastUpPoint { get; private set; }

    /// <summary>
    /// true로 설정된 경우 입력을 받지 않습니다.
    /// </summary>
    public bool cannotInput = false;

    public void OnUpdate()
    {
        if (cannotInput)
            return;

        if (!Input.anyKey && currentInput == InputType.None)
            return;


        if (Input.GetMouseButton(0))
        {
            switch (currentInput)
            {
                case InputType.None:
                case InputType.Release:
                    currentInput = InputType.Press;
                    LastDownPoint = Input.mousePosition;
                    break;
                case InputType.Press:
                    currentInput = InputType.Drag;
                    break;
                case InputType.Drag:
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (currentInput)
            {
                case InputType.None:
                    break;
                case InputType.Press:
                case InputType.Drag:
                    currentInput = InputType.Release;
                    LastUpPoint = Input.mousePosition;
                    break;
                case InputType.Release:
                    currentInput = InputType.None;
                    break;
                default:
                    break;
            }
        }

        InputAction?.Invoke(currentInput, Input.mousePosition);
    }

    public void Clear()
    {
        InputAction = null;
    }
}
