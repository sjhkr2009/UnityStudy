using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C02_ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        //오브젝트 풀링 수정사항 - 풀링된 오브젝트 로딩 없이 불러오기
        if(typeof(T) == typeof(GameObject))
        {
            // 폴더명을 잘라내서 오브젝트의 이름을 얻는다. LastIndexOf 는 해당 문자를 찾지 못하면 -1을 반환한다.
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);

            // 해당 이름의 오브젝트가 풀링되어 있으면 그 오브젝트를 반환한다.
            GameObject origin = A01_Manager.Pool.GetOrigin(name);
            if (origin != null)
                return origin as T;
        }

        
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if(original == null)
        {
            Debug.Log("오브젝트 정보를 불러오는 데 실패했습니다.");
            return null;
        }

        // 풀링 대상 오브젝트일 경우, 씬에 직접 생성하는 대신 풀 매니저를 이용하여 생성한다.
        // 씬에 생성되어 있으면 활성화 처리, 아니라면 풀 매니저가 생성 후 저장해둘 것이다.
        if (original.GetComponent<J02_Poolable>() != null)
            return A01_Manager.Pool.Pop(original, parent).gameObject;


        return Object.Instantiate(original, parent);
    }

    public void Destroy(GameObject gameObject, float delay = 0f)
    {
        if (gameObject == null) return;

        //만약 오브젝트 풀링 대상이면, Destroy 대신 비활성화

        J02_Poolable poolable = gameObject.GetComponent<J02_Poolable>();
        if (poolable != null)
        {
            A01_Manager.Pool.Push(poolable);
            return;
        }

        Object.Destroy(gameObject, delay);
    }
}
