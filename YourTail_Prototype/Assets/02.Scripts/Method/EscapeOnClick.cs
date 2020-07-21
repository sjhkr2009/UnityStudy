using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EscapeOnClick : MonoBehaviour, IPointerClickHandler
{
    public bool isUI = true;
    public List<GameState> interactableOn = new List<GameState>();
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (interactableOn.Count > 0 && !interactableOn.Contains(GameManager.Instance.GameState))
            return;
        
        if (isUI) GameManager.Input.InEscape();
    }

    void OnMouseUpAsButton()
    {
        if (interactableOn.Count > 0 && !interactableOn.Contains(GameManager.Instance.GameState))
            return;

        if (!isUI) GameManager.Input.InEscape();
    }
}
