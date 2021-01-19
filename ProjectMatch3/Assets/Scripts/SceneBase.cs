using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public abstract class SceneBase : MonoBehaviour
{
    public SceneType sceneType { get; protected set; } = SceneType.Unknown;

    void Awake()
	{
        Init();
    }

    protected abstract void Init();

    public abstract void Clear();
}
