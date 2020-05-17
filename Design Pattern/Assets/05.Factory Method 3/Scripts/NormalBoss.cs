using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class NormalBoss : BossBase
{
    private void Start()
    {
        type = BossType.Normal;
        hp = 200;
        exp = 60;

        name = "Normal Boss";
    }

    public override void Attack()
    {
        Debug.Log($"{name} : 공격!");
    }
}
