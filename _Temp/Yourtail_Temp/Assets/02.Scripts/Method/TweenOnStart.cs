using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenOnStart : TweenBase
{
    [SerializeField] bool onEnable = false;
    void Start()
    {
        if (!onEnable) DoChange();
    }
    private void OnEnable()
    {
        if (onEnable)
        {
            DoChange();
        }
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        DoOrigin();
    }
}
