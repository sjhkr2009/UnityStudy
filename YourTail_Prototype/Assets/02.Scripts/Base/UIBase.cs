using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];

        _objects.Add(typeof(T), objects);

        for (int i = 0; i < objects.Length; i++)
        {
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
}
