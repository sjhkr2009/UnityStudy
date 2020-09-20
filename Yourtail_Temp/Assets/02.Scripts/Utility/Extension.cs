﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (component == null) component = gameObject.AddComponent<T>();

        return component;
    }
    public static T GetOrAddComponent<T>(this Transform transform) where T : Component
    {
        T component = transform.GetComponent<T>();
        if (component == null) component = transform.gameObject.AddComponent<T>();

        return component;
    }

    public static GameObject FindChild(this GameObject gameObject, string name = null)
    {
        Transform tr = gameObject.FindChild<Transform>(name);
        if (tr != null) return tr.gameObject;
        else return null;
    }

    public static T FindChild<T>(this GameObject gameObject, string name = null) where T : UnityEngine.Object
    {
        if (gameObject == null) return null;

        foreach (T component in gameObject.GetComponentsInChildren<T>())
        {
            if (string.IsNullOrEmpty(name) || component.name == name)
                return component;
        }

        return null;
    }

    public static void OpenWindow(this List<GameObject> windowList, int index)
    {
        for (int i = 0; i < windowList.Count; i++)
        {
            if (i != index) windowList[i].SetActive(false);
            else windowList[i].SetActive(true);
        }
    }

    public static void SetNativeSize(this Image self, float size)
    {
        self.SetNativeSize();
        self.transform.localScale *= size;
    }

    /// <summary>
    /// 여러 개의 값을 받아서 임의의 인덱스를 반환합니다. 각 값에는 가중치를 부여할 수 있습니다.
    /// 4개의 값을 10, 10, 10, 50 으로 입력하면 인덱스 3이 반환될 확률이 다른 값보다 5배 높습니다.
    /// </summary>
    /// <param name="rateList">각 인덱스에 할당될 가중치를 입력하세요. 합계는 어떻게 되든 상관 없으나 모든 값은 양수여야 합니다.</param>
    /// <returns></returns>
    public static int RandomInWeighted(this float[] rateList)
    {
        if (rateList == null || rateList.Length <= 1)
            return 0;

        float total = rateList[0];
        for (int i = 1; i < rateList.Length; i++)
        {
            total += rateList[i];
            rateList[i] += rateList[1 - 1];
        }
        float rate = Random.Range(0f, total);
        int target = rateList.Length - 1;

        for (int i = 0; i < rateList.Length; i++)
        {
            if (rate < rateList[i])
            {
                target = i;
                break;
            }
        }

        return target;
    }

}
