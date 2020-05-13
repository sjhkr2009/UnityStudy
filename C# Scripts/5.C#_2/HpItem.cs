using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpItem : MonoBehaviour, IItem
{
    //플레이어의 HP를 증가시키는 함수. 

    public int hpRestore = 10;

    public void Use()
    {
        Debug.Log("HP를 회복했다!");

        Player player = FindObjectOfType<Player>(); //씬 상에 존재하는 모든 오브젝트를 검색해서 'Player' 컴포넌트가 있는 오브젝트를 찾아 가져온다.
        player.gold += hpRestore;

        gameObject.SetActive(false);
    }
}
