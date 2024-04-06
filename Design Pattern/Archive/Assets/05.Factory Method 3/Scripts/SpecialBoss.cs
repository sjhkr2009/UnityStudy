using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SpecialBoss : BossBase
{
    private void Start()
    {
        type = BossType.Special;
        hp = 150;
        exp = 120;

        name = "Special Boss";
    }

    public override void Attack()
    {
        Debug.Log($"{name} : 공격!");
    }
}