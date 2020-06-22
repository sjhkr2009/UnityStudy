using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGoblin : BaseMonster //BaseMonster를 상속받아 고블린의 행동을 적은 스크립트. Attack() 함수를 반드시 정의해야 한다.
{
    public override void Attack()
    {
        Debug.Log("고블린의 광역 공격!");
        Debug.Log("공격력: " + damage);
    }
}
