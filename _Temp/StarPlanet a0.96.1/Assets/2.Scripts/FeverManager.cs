using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FeverManager : MonoBehaviour
{
    public event Action<float> EventOnGetFeverGauge = f => { };
    public event Action EventOnFeverTime = () => { };
    public event Action EventExitFeverTime = () => { };

    public float feverDuration = 8f;
    [ReadOnly] public int currentFeverCount = 0;
    [HideInInspector] public int maxFeverCount = 100;
    PoolManager poolManager;

    [ReadOnly] public bool isFeverTime= false;
    Tween tween;

    private void Awake()
    {
        currentFeverCount = 0;
        maxFeverCount = 100;
        isFeverTime = false;
        tween = null;
    }

    private void Start()
    {
        poolManager = GameManager.Instance.PoolManager;
    }

    public void GetFeverCount(int value)
    {
        if (isFeverTime) return;
        currentFeverCount += value;
        EventOnGetFeverGauge((float)currentFeverCount / (float)maxFeverCount);

        if (currentFeverCount >= maxFeverCount) FeverTime();
    }

    /// <summary>
    /// 적이 파괴되었을 때 피버 파티클을 생성합니다. 생성된 파티클은 자동으로 피버 게이지 바로 날아가며, 날아간 후 피버 카운트를 1 증가시킵니다. 
    /// 피버 타임인 경우 파티클을 새로 생성하지 않습니다. 이미 생성된 파티클이 파괴되지는 않으나, 해당 파티클은 피버 카운트를 증가시키지 않습니다.
    /// </summary>
    /// <param name="enemy">파괴된 적의 Transform. 근처 임의의 지점에 파티클을 생성합니다.</param>
    /// <param name="count">생성할 파티클 개수. 파티클 당 게이지가 1 증가하므로 이 숫자만큼 피버 게이지가 증가합니다.</param>
    public void CallParticle(Transform enemy, int count)
    {
        if (isFeverTime) return;
        for (int i = 0; i < count; i++)
        {
            poolManager.Spawn(ObjectPool.ParticleFever, enemy.position + new Vector3(UnityEngine.Random.Range(-0.2f, 0.2f), 0, UnityEngine.Random.Range(-0.2f, 0.2f)), Quaternion.identity);
        }
    }

    public void GetFeverCountPerSecond() { GetFeverCount(1); }

    void FeverTime()
    {
        isFeverTime = true;
        EventOnFeverTime();
        tween = DOVirtual.DelayedCall(feverDuration, ExitFeverTime, false);
    }
    void ExitFeverTime()
    {
        if (isFeverTime) tween.Kill();
        EventExitFeverTime();
        isFeverTime = false;
        currentFeverCount = 0;
    }

    private void OnDestroy()
    {
        if(isFeverTime) tween.Kill();
    }
}
