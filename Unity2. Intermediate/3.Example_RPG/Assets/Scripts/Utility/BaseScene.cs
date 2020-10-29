using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    void Awake() => Init();

    protected virtual void Init()
	{
        Object obj = GameObject.FindObjectOfType<EventSystem>();
        if (obj == null)
            GameManager.Resource.Instantiate(Define.ResourcesPath.EventSystem).name = Define.DefaultName.EventSystem;
	}

    public abstract void Clear();
}
