using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class UIBase : MonoBehaviour
{
    public Dictionary<Type, UnityEngine.Object[]> _objects { get; private set; } = new Dictionary<Type, UnityEngine.Object[]>();
    public bool hasDestroyMotion { get; protected set; } = false;
    public float destroyTime { get; protected set; } = 0f;

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];

        _objects.Add(typeof(T), objects);

        for (int i = 0; i < objects.Length; i++)
        {
            if (names[i] == "Count") continue;

            if (typeof(T) == typeof(GameObject)) objects[i] = gameObject.FindChild(names[i]);
            else objects[i] = gameObject.FindChild<T>(names[i]);

            if (objects[i] == null)
                Debug.Log($"불러오기 실패 : {names[i]}");
        }
    }

    protected T Get<T>(int index) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects)) return (T)objects[index];

        return null;
    }

    protected Button GetButton(int index) { return Get<Button>(index); }
    protected Text GetText(int index) { return Get<Text>(index); }
    protected Image GetImage(int index) { return Get<Image>(index); }

    // 매핑한 하위 오브젝트들에 클릭(및 다른 형태의) 이벤트를 추가해주는 임시 함수. 
    // 우선 개별 스크립트를 만들어 붙이고, 나중에 아이콘 클릭 관련 함수가 많아질 때 이 방식으로 변경할 예정.
    // 게임 특성상 PointerEventData가 불필요한 경우가 많으므로, EventHandler에 매개변수가 없는 클릭 이벤트 등을 추가해주고, Action(매개변수 X)을 변수로 받는 AddEvent 함수를 오버로드하는 것이 쓰기 편할듯.
    public static void AddEvent(GameObject gameObject, Action<PointerEventData> action, Define.EventType eventType = Define.EventType.Click)
    {
        EventHandler eventHandler = gameObject.GetOrAddComponent<EventHandler>();

        switch (eventType)
        {
            case Define.EventType.Click:
                eventHandler.EventOnClick -= action;
                eventHandler.EventOnClick += action;
                break;
        }
    }

    protected void ResetButtons()
    {
        if(!_objects.ContainsKey(typeof(Button)))
		{
            Debug.Log($"{gameObject.name} 에 버튼이 할당되지 않았습니다.");
            return;
		}

        for (int i = 0; i < _objects[typeof(Button)].Length; i++)
        {
            Button button = GetButton(i);
            if(button != null) GetButton(i).onClick.RemoveAllListeners();
        }
    }
    public virtual void OnDestroyMotion()
    {
        DOTween.KillAll();
    }
}
