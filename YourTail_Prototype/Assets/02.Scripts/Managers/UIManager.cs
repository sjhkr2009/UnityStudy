using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class UIManager : MonoBehaviour
{
    Stack<UIBase_Popup> popupUI = new Stack<UIBase_Popup>();
    List<UIBase_Scene> sceneUI = new List<UIBase_Scene>();
    int order = 10;

    // UI를 담을 빈 오브젝트 @UI 를 찾아서 반환한다. 없으면 만들어서 반환한다.
    public Transform Root
    {
        get
        {
            GameObject gameObject = GameObject.Find("@UI");
            if (gameObject == null) gameObject = new GameObject("@UI");
            return gameObject.transform;
        }
    }

    public void SetCanvasOrder(GameObject gameObject, bool isPopup = true, int customOrder = int.MinValue)
    {
        Canvas canvas = gameObject.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (isPopup)
        {
            canvas.sortingOrder = order;
            order++;
        }
        else if (customOrder != int.MinValue)
        {
            canvas.sortingOrder = customOrder;
        }
    }

    public T OpenSceneUI<T>(string name = null) where T : UIBase_Scene
    {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        GameObject gameObject = GameManager.Resource.Instantiate($"UI/Scene/{name}", Root);
        T component = gameObject.GetOrAddComponent<T>();
        sceneUI.Add(component);

        return component;
    }

    public T OpenPopupUI<T>(string name = null) where T : UIBase_Popup
    {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        GameObject gameObject = GameManager.Resource.Instantiate($"UI/Popup/{name}", Root);
        T component = gameObject.GetOrAddComponent<T>();

        popupUI.Push(component);
        return component;
    }

    // 가장 나중에 띄운 팝업창을 닫는다.
    public void ClosePopupUI()
    {
        if (popupUI.Count == 0) return;

        GameObject gameObject = popupUI.Pop().gameObject;
        GameManager.Resource.Destroy(gameObject);

        order--;
    }
    
    public void ClosePopupUI(UIBase_Popup popup)
    {
        if (popupUI.Count == 0) return;

        UIBase_Popup peek = popupUI.Peek();
        if (peek != popup)
        {
            Debug.Log("최근에 열린 팝업창이 먼저 비활성화되어야 합니다. Sorting Order를 확인해주세요.");
            return;
        }
        ClosePopupUI();
    }

    public void CloseAllPopup()
    {
        while (popupUI.Count > 0) ClosePopupUI();
    }
}
