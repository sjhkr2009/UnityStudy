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
