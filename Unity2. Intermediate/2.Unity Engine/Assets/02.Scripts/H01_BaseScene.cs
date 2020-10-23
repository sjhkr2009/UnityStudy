using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class H01_BaseScene : MonoBehaviour
{
    public E02_Define.Scene SceneType { get; protected set; } = E02_Define.Scene.Unknown;

    void Awake() => Init();

    protected virtual void Init()
    {
        Object obj = FindObjectOfType(typeof(EventSystem));
        if(obj == null)
            A01_Manager.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
    }

    public abstract void Clear();
}
