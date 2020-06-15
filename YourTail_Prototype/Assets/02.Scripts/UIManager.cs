using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text currentBase;
    [SerializeField] Text currentSub;
    [SerializeField, ReadOnly] bool isBaseNone = true;
    [SerializeField, ReadOnly] bool isSubNone = true;


    private void Awake()
    {
        CurrentUIReset();
    }
    private void Start()
    {
        GameManager.Instance.OnSetBaseMaterial -= SetCurrentUI;
        GameManager.Instance.OnSetSubMaterial -= SetCurrentUI;
        GameManager.Instance.OnGameStateChange -= OnGameStateChange;

        GameManager.Instance.OnGameStateChange += OnGameStateChange;
        GameManager.Instance.OnSetBaseMaterial += SetCurrentUI;
        GameManager.Instance.OnSetSubMaterial += SetCurrentUI;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnSetBaseMaterial -= SetCurrentUI;
        GameManager.Instance.OnSetSubMaterial -= SetCurrentUI;
        GameManager.Instance.OnGameStateChange -= OnGameStateChange;
    }
    void CurrentUIReset()
    {
        currentBase.text = "현재 베이스 재료 : ";
        isBaseNone = true;
        currentSub.text = "현재 부재료 : ";
        isSubNone = true;
    }
    void SetCurrentUI(BaseMaterials material)
    {
        if (isBaseNone) isBaseNone = false;
        else currentBase.text += ", ";
        currentBase.text += material.Name;
    }
    void SetCurrentUI(SubMaterials material)
    {
        if (isSubNone) isSubNone = false;
        else currentSub.text += ", ";
        currentSub.text += material.Name;
    }
    void OnGameStateChange(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.None:
                CurrentUIReset();
                break;
            case GameState.Order:
                break;
            case GameState.SelectBase:
                break;
            case GameState.SelectSub:
                break;
            case GameState.Combine:
                break;
            case GameState.SetCocktail:
                break;
        }
    }
}
