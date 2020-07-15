using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public enum InputButton
//{
//    None,
//    MaterialSelect,
//    StateChange
//}

/// <summary>
/// 씬 전환을 제외한 게임 내 동작은 InputManager를 통해 관리합니다.
/// </summary>
public class InputManager
{
    public event Action<string> InputMaterialSelect;
    public event Action<CocktailMaterials> InputMaterialInfo;
    public event Action<GameState> InputStateChange;
    public event Action InputNextState;
    public event Action InputRetryCocktail;
    public event Action InputEscape;

    public void InMaterialSelect(string id) => InputMaterialSelect(id);
    public void InMaterialInfo(CocktailMaterials material) => InputMaterialInfo(material);
    public void InStateChange(GameState state) => InputStateChange(state);
    public void InNextState() => InputNextState();
    public void InRetryCocktail() => InputRetryCocktail();
    public void InEscape() => InputEscape();
    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            InputEscape();
    }
}
