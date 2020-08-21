using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager
{
    public event Action<string> InputMaterialSelect;
    public event Action<CocktailMaterials> InputMaterialInfo;
    public event Action<Customers> InputBirdInfo;
    public event Action<GameState> InputStateChange;
    public event Action InputRetryCocktail;
    public event Action InputEscape;

    public void InMaterialSelect(string id) => InputMaterialSelect(id);
    public void InMaterialInfo(CocktailMaterials material) => InputMaterialInfo(material);
    public void InBirdInfo(Customers customer) => InputBirdInfo(customer);
    public void InStateChange(GameState state) => InputStateChange(state);
    public void InRetryCocktail() => InputRetryCocktail();
    public void InEscape() => InputEscape();
    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            InputEscape();
    }
    public void Clear()
    {
        InputMaterialSelect = null;
        InputMaterialInfo = null;
        InputBirdInfo = null;
        InputStateChange = null;
        InputRetryCocktail = null;
        InputEscape = null;
    }
}
