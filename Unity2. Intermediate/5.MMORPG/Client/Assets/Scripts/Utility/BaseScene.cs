using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

public abstract class BaseScene : MonoBehaviour
{
    public Scene SceneType { get; protected set; } = Scene.Unknown;

    void Awake() => Init();

    protected virtual void Init()
	{
        Object obj = FindObjectOfType<EventSystem>();
        if (obj == null) {
            var eventSystem = Director.Resource.Instantiate(ResourcesPath.EventSystem);
            eventSystem.name = DefaultName.EventSystem;
        }
    }

    public abstract void Clear();
}
