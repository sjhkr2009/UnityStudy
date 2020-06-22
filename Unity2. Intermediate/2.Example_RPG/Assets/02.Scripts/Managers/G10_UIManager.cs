using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G10_UIManager
{
    // UI를 생성하고 관리하는 매니저

    Stack<G08_UIPopup> popupUI = new Stack<G08_UIPopup>();
    List<G09_UIScene> uiScene = new List<G09_UIScene>();
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

    // 캔버스가 생성될 때 sortingOrder를 설정한다. 팝업창일 경우 생성되는 순서에 따라 자동으로 정해지고, 아닐 경우 사용자가 지정해줄 수 있다.
    public void SetCanvasOrder(GameObject gameObject, bool isPopup = true, int customOrder = int.MinValue)
    {
        Canvas canvas = G04_Util.AddOrGetComponent<Canvas>(gameObject);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true; // 캔버스 내에 캔버스가 있을 경우, 이를 true로 설정하면 부모 오브젝트의 order에 무관하게 자신의 sortingOrder에 따라서 배치된다.

        if (isPopup)
        {
            canvas.sortingOrder = order;
            order++;
        }
        else if(customOrder != int.MinValue) //별도로 order값을 입력해줄 경우 해당 값으로 설정한다.
        {
            canvas.sortingOrder = customOrder;
        }
    }

    // 팝업이 아닌 기본적으로 씬에 배치된 UI를 생성한다. 혹시 필요할지 모르니 uiScene 리스트에 저장해둔다.
    public T OpenSceneUI<T>(string name = null) where T : G09_UIScene
    {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        GameObject gameObject = A01_Manager.Resource.Instantiate($"UI/Scene/{name}", Root);
        T component = G04_Util.AddOrGetComponent<T>(gameObject);
        uiScene.Add(component);

        return component;
    }

    // 팝업창을 생성하여 화면에 띄운다. 띄운 팝업창은 popupUI 스택에 저장한다.
    // order는 SetCanvasOrder를 통해 증가시켜준다. 다른 곳에서 UI를 활성화할수도 있기 때문에, UI가 활성화될 때 호출되는 SetCanvasOrder에서 제어하는 것이 좋다.
    public T OpenPopupUI<T>(string name = null) where T : G08_UIPopup
    {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        GameObject gameObject = A01_Manager.Resource.Instantiate($"UI/Popup/{name}", Root);
        T component = G04_Util.AddOrGetComponent<T>(gameObject);

        popupUI.Push(component);
        return component;
    }

    // 가장 나중에 띄운 팝업창을 닫는다.
    public void ClosePopupUI()
    {
        if (popupUI.Count == 0) return;

        GameObject gameObject = popupUI.Pop().gameObject;
        A01_Manager.Resource.Destroy(gameObject);

        order--;
    }
    // 닫을 팝업창을 직접 입력할 수 있게 오버로딩한다. 올바른 창이 아니면 return 된다.
    public void ClosePopupUI(G08_UIPopup popup)
    {
        if (popupUI.Count == 0) return;

        G08_UIPopup peek = popupUI.Peek();
        if (peek != popup)
        {
            Debug.Log("최근에 열린 팝업창이 먼저 비활성화되어야 합니다.");
            return;
        }
        ClosePopupUI();
    }

    // 모든 팝업창을 닫는다.
    public void CloseAllPopup()
    {
        while (popupUI.Count > 0) ClosePopupUI();
    }
}
