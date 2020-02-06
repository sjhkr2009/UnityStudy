using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public enum ExplosionType { Hexagon, Fixed }

public class ItemBomb : ItemBase
{
    public event Action<ItemBomb> EventOnExplosion = (I) => { };
    
    [BoxGroup("Bomb")] public ExplosionType explosionType;
    [BoxGroup("Bomb"), SerializeField] private float durationTime;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (explosionType == ExplosionType.Fixed) Invoke(nameof(TimeOver), durationTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Star") || other.CompareTag("Fever") || other.CompareTag("Planet"))
        {
            EventOnExplosion(this);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}
