using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager
{
    private int _order = Define.DefaultSetting.UiOrder;
    private Stack<BaseUIPopup> _popupStack = new Stack<BaseUIPopup>();
    private List<BaseUIScene> _sceneUI = new List<BaseUIScene>();

    private Transform _root = null;
    public Transform Root
	{
        get
		{
            if (_root == null)
			{
                GameObject go = GameObject.Find(Define.DefaultName.UiRoot);
                if (go == null)
                    go = new GameObject(Define.DefaultName.UiRoot);

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

    public T ShowSceneUI<T>(string name = null) where T : BaseUIScene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = GameManager.Resource.Instantiate(Define.ResourcesPath.ToSceneUI(name), Root);
        T sceneUI = go.GetOrAddComponent<T>();
        _sceneUI.Add(sceneUI);

        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null) where T : BaseUIPopup
	{
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = GameManager.Resource.Instantiate(Define.ResourcesPath.ToPopupUI(name), Root);
        T popup = go.GetOrAddComponent<T>();
        _popupStack.Push(popup);

        return popup;
	}

    public T MakeSubItem<T>(Transform parent, string name = null) where T : BaseUI
	{
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = GameManager.Resource.Instantiate(Define.ResourcesPath.ToSubItemUI(name), parent);
        return go.GetOrAddComponent<T>();
	}
    public T MakeWorldSpace<T>(Transform parent, string name = null) where T : BaseUI
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = GameManager.Resource.Instantiate(Define.ResourcesPath.ToWorldSpaceUI(name), parent);

        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        return go.GetOrAddComponent<T>();
    }

    public bool ClosePopupUI()
	{
        if (_popupStack.Count == 0)
            return false;

        BaseUIPopup target = _popupStack.Pop();
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

    public bool ClosePopupUI(BaseUIPopup target, bool findAllPopup = false)
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
                Stack<BaseUIPopup> tempStack = new Stack<BaseUIPopup>();
                while (_popupStack.Count > 1)
				{
                    tempStack.Push(_popupStack.Pop());

                    BaseUIPopup now = _popupStack.Peek();
                    if (now == target)
					{
                        closed = ClosePopupUI();
                        break;
					}
				}
                while (tempStack.Count > 0)
				{
                    BaseUIPopup now = tempStack.Pop();
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

    public void Clear()
	{
        CloseAllPopupUI();
        _sceneUI.Clear();
	}
}
