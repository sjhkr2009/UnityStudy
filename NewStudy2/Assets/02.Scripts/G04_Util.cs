using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;

public class G04_Util
{
    // 여러 사용자 정의 함수를 작성해두는 곳.
    // 함수는 static으로 선언하며, 컴포넌트로 쓸 것이 아니니 MonoBehavior는 불필요하다.

    // FindChild는 GameObject를 반환하지 못하므로, GameObject 타입을 찾을 때는 이 함수를 오버로드해놓고 사용한다.
    // 기본적으로 FindChild를 이용하되, 모든 게임오브젝트가 Transform을 갖는다는 점에 착안하여 Transform을 불러와서 GameObject를 반환해준다.
    public static GameObject FindChild(GameObject gameObject, string name = null, bool findOnlyDirectChild = false)
    {
        Transform tr = FindChild<Transform>(gameObject, name, findOnlyDirectChild);
        if (tr != null) return tr.gameObject;
        else return null;
    }

    // 오브젝트의 자식 오브젝트들을 탐색하여, T 타입의 컴포넌트를 불러와서 반환한다.
    // 오브젝트의 이름을 입력하면 이름이 일치하는 오브젝트를, 입력하지 않으면 가장 먼저 찾은 T타입 컴포넌트를 반환한다.
    // findOnlyDirectChild가 false라면 모든 자식 오브젝트를, true라면 직속 자식들만 탐색한다.
    public static T FindChild<T>(GameObject gameObject, string name = null, bool findOnlyDirectChild = false) where T : UnityEngine.Object
    {
        // 탐색 대상이 없으면 바로 null을 반환한다.
        if (gameObject == null) return null;

        //직속 자식 오브젝트만 탐색
        if (findOnlyDirectChild)
        {
            // 자식 오브젝트 개수만큼 실행한다.
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                // transform.GetChild를 이용하여 직속 자식들을 순서대로 불러와서, 이름이 일치하면 T타입 컴포넌트를 불러와서 반환해준다.
                Transform tr = gameObject.transform.GetChild(i);
                if(string.IsNullOrEmpty(name) || tr.name == name)
                {
                    T component = tr.GetComponent<T>();
                    if (component != null) return component;
                }
            }
        }
        //모든 자식 오브젝트 탐색
        else
        {
            // gameObject.GetComponentsInChildren를 이용하여 자식 오브젝트의 T타입 컴포넌트를 모두 불러온다.
            // 불러온 컴포넌트들을 하나씩 탐색하여 이름이 일치하면 반환한다.
            foreach (T component in gameObject.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        // 이름이 일치하는 T타입 컴포넌트를 하나도 찾지 못했다면 null을 반환한다.
        return null;
    }

    // 게임오브젝트에서 T타입 컴포넌트를 불러오되, 없다면 추가한 다음 불러온다.
    public static T AddOrGetComponent<T>(GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (component == null) component = gameObject.AddComponent<T>();

        return component;
    }
}
