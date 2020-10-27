using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager
{
    private int _order = Define.DefaultSetting.UiOrder;
    private Stack<UiPopupBase> _popupStack = new Stack<UiPopupBase>();
    private List<UiSceneBase> _sceneUI = new List<UiSceneBase>();

    private Transform _root = null;
    public Transform Root
	{
        get
		{
            if (_root == null)
			{
                GameObject go = GameObject.Find("@UI");
                if (go == null)
                    go = new GameObject("@UI");

                _root = go.transform;
            }

            return _root;
        }
	}

    int SetCanvas(GameObject obj, bool sort, int customOrder)
	{
        Canvas canvas = obj.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if(sort)
		{
            canvas.sortingOrder = _order;
            return _order++;
		}
        else
		{
            canvas.sortingOrder = customOrder;
            return customOrder;
        }
	}
    public int SetPopupCanvas(GameObject obj) => SetCanvas(obj, true, 0);
    public int SetSceneCanvas(GameObject obj, int order = 0) => SetCanvas(obj, false, order);

    public T ShowSceneUI<T>(string name = null) where T : UiSceneBase
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = GameManager.Resource.Instantiate($"{Define.ResourcesPath.SceneUi}{name}", Root);
        T sceneUI = go.GetOrAddComponent<T>();
        _sceneUI.Add(sceneUI);

        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null) where T : UiPopupBase
	{
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = GameManager.Resource.Instantiate($"{Define.ResourcesPath.PopupUi}{name}", Root);
        T popup = go.GetOrAddComponent<T>();
        _popupStack.Push(popup);

        return popup;
	}

    public bool ClosePopupUI()
	{
        if (_popupStack.Count == 0)
            return false;

        UiPopupBase target = _popupStack.Pop();
        GameManager.Resource.Destroy(target.gameObject);
        target = null;

        _order--;
        return true;
	}

    public void CloseAllPopupUI()
	{
        while (_popupStack.Count > 0)
            ClosePopupUI();
	}

    public bool ClosePopupUI(UiPopupBase target, bool findAllPopup = false)
	{
        if (_popupStack.Count == 0)
            return false;

        if (_popupStack.Peek() == target)
		{
            return ClosePopupUI();
		}
        else
		{
            if (findAllPopup)
			{
                bool closed = false;
                Stack<UiPopupBase> tempStack = new Stack<UiPopupBase>();
                while (_popupStack.Count > 1)
				{
                    tempStack.Push(_popupStack.Pop());

                    UiPopupBase now = _popupStack.Peek();
                    if (now == target)
					{
                        closed = ClosePopupUI();
                        break;
					}
				}
                while (tempStack.Count > 0)
				{
                    UiPopupBase now = tempStack.Pop();
                    if (closed)
                        now.GetComponent<Canvas>().sortingOrder--;

                    _popupStack.Push(now);
                }

                return closed;
            }
            else
			{
                Debug.Log("Top Popup is not Target.");
                return false;
			}
		}
	}
}
