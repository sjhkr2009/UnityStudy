using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterOrk : BaseMonster //BaseMonster를 상속받아 오크의 행동을 적은 스크립트. Attack() 함수를 반드시 정의해야 한다.
{
    public override void Attack()
    {
        Debug.Log("오크의 공격!");
        Debug.Log("공격력: " + damage * 2);
    }
}
