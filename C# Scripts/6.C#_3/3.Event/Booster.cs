using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    public void hpBooster(Character player) //플레이어 캐릭터를 입력하면 hp를 증가시킨다.
    {
        Debug.Log(player.playerName + "의 체력이 강화되었다.");
        player.hp += 10;
    }

    public void defBooster(Character player)
    {
        Debug.Log(player.playerName + "의 방어력이 강화되었다.");
        player.defense += 10;
    }

    public void damageBooster(Character player)
    {
        Debug.Log(player.playerName + "의 공격력이 강화되었다.");
        player.damage += 10;
    }

    private void Awake()
    {
        Character player = FindObjectOfType<Character>(); //시작 시 Character 스크립트를 가진 오브젝트를 찾아서, Character로서 불러온다.

        player.playerBoost += hpBooster; //시작 시 발동할 부스터들을 목록에 추가한다.
        player.playerBoost += defBooster;
        player.playerBoost += damageBooster;
    }
}
