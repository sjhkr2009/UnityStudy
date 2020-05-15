using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Slime : UnitBase
{
    public Slime()
    {
        unitType = UnitTypeOnFactoryMethod.Slime;
        name = "슬라임";
        hp = 45;
        exp = 4;

        Debug.Log($"{name} 생성됨");
    }

    public override void Attack()
    {
        Debug.Log($"{name} : 공격!");
    }
}
