using System;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class InputManager
{
    public event Action<InputType, Vector2> InputAction = null;

    private InputType currentInput = InputType.None;
    public Vector2 LastDownPoint { get; private set; }
    public Vector2 LastUpPoint { get; private set; }

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
