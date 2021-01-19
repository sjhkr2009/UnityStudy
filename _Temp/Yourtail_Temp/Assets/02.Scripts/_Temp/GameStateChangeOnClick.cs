using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameStateChangeOnClick : MonoBehaviour, IPointerClickHandler
{
    public GameState targetState;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.GameState = targetState;
    }
}
