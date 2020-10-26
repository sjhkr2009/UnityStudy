using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager
{
    Stack<UIBase_Popup> popupUI = new Stack<UIBase_Popup>();
    Dictionary<string, UIBase_Scene> sceneUI = new Dictionary<string, UIBase_Scene>();
    Dictionary<string, UIBase_Popup> closedUI = new Dictionary<string, UIBase_Popup>();
    int order = 10;

    public void Init()
	{
        _root = new GameObject("@UI").transform;
        OpenPopupUI<CollectionUI>();
    }

    #region 현재 확인중인 정보

    // 현재 정보를 요청하는 재료의 클래스. 재료 선택 화면에서 ? 버튼을 눌러 정보를 확인할 때 사용한다.
    public CocktailMaterials CurrentMaterialInfo { get; set; }
    public void SetMaterialInfo(CocktailMaterials material) => CurrentMaterialInfo = material;

    // 현재 확인하려는 새의 정보. Bird Collection에서 특정 새의 정보를 확인하고자 할 때 사용한다.
    public Customers CurrentBirdInfo { get; set; }
    public void SetBirdInfo(Customers bird) => CurrentBirdInfo = bird;

    // 현재 표시할 경고창의 타입. 이 값을 참조하여 경고창의 텍스트와 버튼 동작이 세팅된다.
    public Define.WarningType CurrentWarningType { get; set; } = Define.WarningType.QuitApp;

    #endregion

    /// <summary>
    /// UI를 담을 빈 오브젝트 @UI 를 찾아서 반환합니다. 없으면 새로 생성합니다.
    /// </summary>
    private Transform _root;
    public Transform Root
    {
        get
        {
            if (_root == null)
			{
                GameObject gameObject = GameObject.Find("@UI");
                if (gameObject == null)
                    gameObject = new GameObject("@UI");

                _root = gameObject.transform;
            }
                
            return _root;
        }
    }

    /// <summary>
    /// 해당 UI 및 자식 오브젝트들의 캔버스를 적용 또는 불러와서 Sorting Order를 지정합니다.
    /// 만약 입력된 게임오브젝트가 이미 부모로 Canvas를 가지고 있더라도, 부모 오브젝트의 설정값과 무관하게 지정된 Order 값을 따르게 됩니다.
    /// </summary>
    /// <param name="gameObject">이미지 순서를 조정할 오브젝트</param>
    /// <param name="isPopup">씬 기본 UI/팝업 UI 여부. 기본값은 팝업창이며, 팝업창이 아닐 경우 false로 입력하세요. 팝업창은 order가 자동으로 지정되고, 아닐 경우 프리팹이 기본적으로 가진 값을 따릅니다.</param>
    /// <param name="customOrder">씬 기본 UI의 Sorting Order를 코드상으로 직접 지정하고 싶을 때 사용하세요. 팝업창에는 적용할 수 없습니다.</param>
    public void SetCanvasOrder(GameObject gameObject, bool isPopup = true, int customOrder = int.MinValue)
    {
        Canvas canvas = gameObject.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (customOrder != int.MinValue)
        {
            canvas.sortingOrder = customOrder;
            return;
        }

        if (isPopup)
        {
            canvas.sortingOrder = order;
            order++;
        }
    }

    /// <summary>
    /// 팝업창이 아닌 UI를 화면상에 불러옵니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T OpenSceneUI<T>(string name = null) where T : UIBase_Scene
    {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        T component = null;
        if (sceneUI.ContainsKey(name))
		{
            component = sceneUI[name] as T;
            component.gameObject.SetActive(true);
		}
		else
		{
            GameObject gameObject = GameManager.Resource.Instantiate($"UI/Scene/{name}", Root);
            component = gameObject.GetOrAddComponent<T>();
        }

        return component;
    }
    public T CloseSceneUI<T>(string name = null) where T : UIBase_Scene
	{
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        T component = null;
        if (sceneUI.ContainsKey(name))
		{
            component = sceneUI[name] as T;
            if(component.gameObject.activeSelf) 
                component.gameObject.SetActive(false);
        }
		else
		{
            component = Object.FindObjectOfType<T>();
            sceneUI.Add(name, component);
        }

        component.gameObject.SetActive(false);
        return component;
    }

    /// <summary>
    /// 특정 UI 팝업창을 화면상에 엽니다. Resource/Prefab/UI/Popup 폴더 내의 프리팹 형태여야 하며, UIBase_Popup 계열의 컴포넌트가 적용되어 있어야 합니다.
    /// </summary>
    /// <typeparam name="T">UIBase_Popup을 상속받은 컴포넌트명</typeparam>
    /// <param name="name">T 컴포넌트가 프리팹의 이름과 일치하지 않을 경우, 프리팹의 이름을 입력하세요.</param>
    public T OpenPopupUI<T>(string name = null) where T : UIBase_Popup
    {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        T component = null;

        if (closedUI.ContainsKey(name))
        {
            component = closedUI[name] as T;

            if(component.gameObject.activeSelf)
                SetCanvasOrder(component.gameObject);
            else
                component.gameObject.SetActive(true);
        }
        else
        {
            component = Object.FindObjectOfType<T>();
            if(component != null)
			{
                SetCanvasOrder(component.gameObject);
			}
			else
			{
                GameObject gameObject = GameManager.Resource.Instantiate($"UI/Popup/{name}", Root);
                component = gameObject.GetOrAddComponent<T>();
            }
        }

        popupUI.Push(component);
        return component;
    }

    /// <summary>
    /// 가장 나중에 띄운 팝업창을 닫습니다. 팝업창 닫기와 관련된 모든 함수는 공통으로 이 함수를 호출하여 Destroy 처리하게 됩니다.
    /// </summary>
    public void ClosePopupUI()
    {
        if (popupUI.Count == 0) return;

        UIBase_Popup pop = popupUI.Pop();

        if (!AllowDestroy(pop)) return;

        if (pop.hasDestroyMotion)
            pop.OnDestroyMotion();

        GameManager.Resource.Destroy(pop.gameObject, pop.destroyTime);

        order--;
    }

    /// <summary>
    /// 특정 팝업창을 지정하여, 해당 팝업이 가장 위에 있다면 닫습니다. 아니라면 아무 동작도 실행하지 않습니다.
    /// 게임 진행과 무관한 도움말 등이 열려 있을 때 Escape로 닫는 용도로 사용됩니다. 아무 동작도 실행하지 않으면 false, 성공적으로 창을 닫았으면 true를 반환합니다.
    /// </summary>
    public bool TryClosePopupUI<T>(string name = null)
    {
        if (popupUI.Count == 0) return false;
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        UIBase_Popup nowPopup = popupUI.Peek();
        if (nowPopup.name == name + "(Clone)")
        {
            ClosePopupUI();
            return true;
        }
        else Debug.Log("대상 팝업창이 오픈되어있지 않습니다.");

        return false;
    }

    /// <summary>
    /// 특정 팝업창을 지정하여 닫습니다. 지정된 팝업창이 가장 나중에 띄운 팝업창이라면 즉시 닫고, 그렇지 않을 경우 팝업창을 하나씩 탐색하여 지정된 창을 찾습니다.
    /// 매개변수가 없는 ClosePopupUI()에 비해 부하율이 높지만 더 안전합니다. 해당 팝업창이 맨 위에 있을 때만 닫아야 한다면 TryClosePopupUI를 이용하세요.
    /// </summary>
    public void ClosePopupUI<T>(string name = null)
    {
        if (TryClosePopupUI<T>()) return;

        if (popupUI.Count == 0) return;
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        bool isFound = false;
        int targetOrder = int.MaxValue;

        Stack<UIBase_Popup> tempStack = new Stack<UIBase_Popup>();

        while(popupUI.Count > 0)
        {
            UIBase_Popup lastPopup = popupUI.Peek();
            if (lastPopup.name != name + "(Clone)")
                tempStack.Push(popupUI.Pop());
            else
            {
                targetOrder = lastPopup.gameObject.GetComponent<Canvas>().sortingOrder;
                ClosePopupUI();
                isFound = true;
                break;
            }
        }
        while (tempStack.Count > 0)
        {
            UIBase_Popup tempPopup = tempStack.Pop();
            if (isFound)
			{
                Canvas canvas = tempPopup.gameObject.GetComponent<Canvas>();
                if (targetOrder < canvas.sortingOrder)
                    canvas.sortingOrder--;
            }
            
            popupUI.Push(tempPopup);
        }
    }

    /// <summary>
    /// 모든 UI 팝업창을 닫습니다. 기본적으로 씬에 있는 UI는 유지됩니다.
    /// </summary>
    public void CloseAllPopup()
    {
        while (popupUI.Count > 0) 
            ClosePopupUI();
    }

    /// <summary>
    /// 팝업창 닫기를 실행했을 때, 오브젝트 풀링이 필요한 UI라면 파괴하는 대신 비활성화시키고 true를 반환합니다.
    /// DontDestroy 변수가 false일 경우에는 아무 동작도 하지 않고 false를 반환합니다.
    /// </summary>
    bool AllowDestroy(UIBase_Popup popup)
    {
        if (popup.DontDestroy)
        {
            popup.gameObject.SetActive(false);

            if (popup.hasDestroyMotion)
                popup.OnDestroyMotion();

            string name = popup.gameObject.name;
            string key = name.Remove(name.IndexOf('('));

            if (!closedUI.ContainsKey(key))
                closedUI.Add(key, popup);

            GameManager.Resource.Disable(popup.gameObject, popup.destroyTime);
            order--;

            return false;
        }
        return true;
    }

    public Sprite NullImage => GameManager.Resource.Load<Sprite>("Sprites/UI/NullImage");
}
