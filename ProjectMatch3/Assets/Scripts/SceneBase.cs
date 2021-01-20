using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

/// <summary>
/// 씬에 반드시 포함되는 요소로, 이를 상속받은 클래스는 해당 유형의 씬에서 공통으로 필요한 초기 작업을 실행합니다.
/// </summary>
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
