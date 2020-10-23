using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class G05_UIBase : MonoBehaviour
{
    // UI의 기본적인 동작을 베이스 코드로 저장하고, UI를 매핑하거나 컨트롤하는 스크립트에 상속받아 쓴다.

    // UI의 종류(type)와 그 종류에 해당하는 컴포넌트들을 저장할 딕셔너리
    Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    /// <summary>
    /// 딕셔너리에 산하 오브젝트들을 매핑한다.
    /// </summary>
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        // Enum 내의 값들을 string 형태로 받아온다. C++에서는 지원하지 않는다.
        string[] names = Enum.GetNames(type);

        // 저장할 오브젝트 배열을 enum에서 추출한 이름의 개수만큼 만든다.
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];

        // 딕셔너리에 (타입 이름, 오브젝트 배열) 을 저장한다.
        _objects.Add(typeof(T), objects);

        // enum에서 추출한 오브젝트명을 통해, 이 캔버스의 자식 중에서 T타입의 오브젝트를 탐색해 불러와, 딕셔너리에 저장한다.
        for (int i = 0; i < objects.Length; i++)
        {
            // FindChild로 GameObject 자체는 받아올 수 없으니, GameObject를 가져올 수 있는 함수는 따로 선언한다.
            if (typeof(T) == typeof(GameObject)) objects[i] = G04_Util.FindChild(gameObject, names[i]);
            else objects[i] = G04_Util.FindChild<T>(gameObject, names[i]);

            if (objects[i] == null)
                Debug.Log($"불러오기 실패 : {names[i]}");
        }
    }

    /// <summary>
    /// 매핑한 오브젝트를 딕셔너리에서 불러온다. enum값에 저장해둔 오브젝트명을 활용한다.
    /// </summary>
    protected T Get<T>(int index) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects)) return (T)objects[index];

        return null;
    }

    // 자주 쓰는 타입의 컴포넌트는 불러오기 편하게 따로 만들어둔다.
    protected Button GetButton(int index) { return Get<Button>(index); }
    protected Text GetText(int index) { return Get<Text>(index); }
    protected Image GetImage(int index) { return Get<Image>(index); }


    // 이벤트를 드래그 앤 드롭 없이 간편하게 이벤트 핸들러에 추가해줄 수 있는 함수
    // 어떤 이벤트에 추가할지는 enum값을 저장해두는 스크립트(여기서는 E02_Define)에 적어두고 이 값을 활용한다.
    public static void AddEvent(GameObject gameObject, Action<PointerEventData> action, E02_Define.EventType eventType = E02_Define.EventType.Click)
    {
        G06_EventHandler eventHandler = G04_Util.AddOrGetComponent<G06_EventHandler>(gameObject);

        switch (eventType)
        {
            case E02_Define.EventType.Click:
                eventHandler.EventOnClick -= action;
                eventHandler.EventOnClick += action;
                break;
            case E02_Define.EventType.Drag:
                eventHandler.EventOnDrag -= action;
                eventHandler.EventOnDrag += action;
                break;
        }
    }
}
