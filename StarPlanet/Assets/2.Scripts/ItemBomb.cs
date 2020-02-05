using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum ExplosionType { Hexagon, Fixed }

public class ItemBomb : MonoBehaviour
{
    public event Action<ItemBomb> EventOnExplosion = (I) => { };
    
    [BoxGroup("Basic")] public ExplosionType explosionType;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Star") || other.CompareTag("Fever"))
        {
            EventOnExplosion(this);
        }
    }
}
