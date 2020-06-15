using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C02_ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject gameObject = Load<GameObject>($"Prefabs/{path}");
        if(gameObject == null)
        {
            Debug.Log("오브젝트 정보를 불러오는 데 실패했습니다.");
            return null;
        }

        return Object.Instantiate(gameObject, parent);
    }

    public void Destroy(GameObject gameObject, float delay)
    {
        Object.Destroy(gameObject, delay);
    }
}
