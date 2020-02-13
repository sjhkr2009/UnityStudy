using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FeverManager : MonoBehaviour
{
    public event Action EventOnFeverTime = () => { };
    public event Action EventExitFeverTime = () => { };
    [SerializeField] float feverDuration = 8f;
    [ReadOnly] public int currentFeverCount = 0;
    [HideInInspector] public int maxFeverCount = 100;

    [ReadOnly] public bool isFeverTime= false;
    Tween tween;

    private void Awake()
    {
        currentFeverCount = 0;
        maxFeverCount = 100;
        isFeverTime = false;
        tween = null;
    }

    public void GetFeverCount(int value)
    {
        if (isFeverTime) return;
        currentFeverCount += value;
        if (currentFeverCount >= maxFeverCount) FeverTime();
    }

    void FeverTime()
    {
        isFeverTime = true;
        EventOnFeverTime();
        Tween tween = DOVirtual.DelayedCall(feverDuration, ExitFeverTime, false);
    }
    void ExitFeverTime()
    {
        EventExitFeverTime();
        isFeverTime = false;
        currentFeverCount = 0;
    }

    private void OnDestroy()
    {
        if(isFeverTime) tween.Kill();
    }
}
