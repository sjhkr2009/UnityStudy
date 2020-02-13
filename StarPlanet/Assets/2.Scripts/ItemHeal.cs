using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHeal : ItemBase
{
    public event Action<ItemHeal, int> EventOnHealingStar = (I,h) => { };
    public event Action<ItemHeal, int> EventOnHealingPlanet = (I,h) => { };

    [BoxGroup("Heal")] public int healValue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fever"))
        {
            EventOnHealingStar(this, healValue);
            EventOnHealingPlanet(this, healValue);
        }
        else if (other.CompareTag("Star"))
        {
            EventOnHealingStar(this, healValue);
        }
        else if (other.CompareTag("Planet"))
        {
            EventOnHealingPlanet(this, healValue * 2);
        }
    }

}
