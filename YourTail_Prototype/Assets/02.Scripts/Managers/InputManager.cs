using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum InputButton
{
    None,
    MaterialSelect,
    StateChange
}

/// <summary>
/// 씬 전환을 제외한 게임 내 동작은 InputManager를 통해 관리합니다.
/// </summary>
public class InputManager : MonoBehaviour
{
    public event Action<int> InputMaterialSelect;
    public event Action<GameState> InputStateChange;
    public event Action InputNextState;

    public void InMaterialSelect(int number)
    {
        InputMaterialSelect(number);
    }
    public void InStateChange(GameState state)
    {
        InputStateChange(state);
    }
    public void InNextState()
    {
        InputNextState();
    }
}
