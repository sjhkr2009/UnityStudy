using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Ork : UnitBase
{
    public Ork()
    {
        unitType = UnitTypeOnFactoryMethod.Ork;
        name = "오크";
        hp = 150;
        exp = 15;

        Debug.Log($"{name} 생성됨");
    }
    
    public override void Attack()
    {
        Debug.Log($"{name} : 공격!");
    }
}
