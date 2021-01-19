using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EscapeOnClick : MonoBehaviour, IPointerClickHandler //임시삭제로 인해 MonoBehavior 제거
{
    public bool isUI = true;
    public List<GameState> interactableOn = new List<GameState>();
    
    void OnEnable()
	{
        Debug.Log($"삭제된 [EscapeOnClick] 컴포넌트를 {gameObject.name} 에서 발견했습니다. 이 메시지를 발견할 경우 개발자에게 얘기해주세요.");
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //if (interactableOn.Count > 0 && !interactableOn.Contains(GameManager.Instance.GameState))
        //    return;

        //if (isUI) GameManager.Input.InEscape();
        Debug.Log($"삭제된 [EscapeOnClick] 컴포넌트에 {gameObject.name}가 접근을 시도헀습니다. 이 메시지를 발견할 경우 개발자에게 얘기해주세요.");
    }

    void OnMouseUpAsButton()
    {
        //if (interactableOn.Count > 0 && !interactableOn.Contains(GameManager.Instance.GameState)) return;

        //if (GameManager.Instance.ignoreOnMouse) return;

        //if (!isUI) GameManager.Input.InEscape();
        Debug.Log($"삭제된 [EscapeOnClick] 컴포넌트에 {gameObject.name}가 접근을 시도헀습니다. 이 메시지를 발견할 경우 개발자에게 얘기해주세요.");
    }
}
