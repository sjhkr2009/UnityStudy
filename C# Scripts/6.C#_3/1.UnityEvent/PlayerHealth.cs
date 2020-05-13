using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; //이벤트를 사용하기 위해 필요하다.

public class PlayerHealth : MonoBehaviour
{
    // 플레이어가 붉은 바닥에 닿으면 사망처리하는 스크립트

    // 플레이어 사망 시 사망 UI(UIManager), 도전과제(AchievementSystem), 재시작 처리(GameManager)를 동시에 발동시켜야 한다.

    //기존 방식: 모든 스크립트를 불러온 다음, Dead()에서 하나씩 발동시킨다.
        /*
    public UIManager uiManager;
    public AchievementSystem achievement;
    public GameManager gameManager;
        */

    //유니티 이벤트
    public UnityEvent onPlayerDead; //유니티 이벤트를 불러와서

    private void OnTriggerEnter(Collider other)
    {
        Dead();
    }

    void Dead()
    {
        //기존 방식: 불러온 각각의 스크립트에서 함수를 발동한다.
            /*
        uiManager.OnPlayerDead();
        achievement.UnlockAchievement("뉴턴의 법칙");
        gameManager.OnPlayerDead();
            */

        //문제점
        //모든 게임 오브젝트를 PlayerHealth가 알고 있어야 하는 구조는 복잡하다.
        //도전과제는 게임 개발 완료 시점에 추가되는 기능인데, 기존 게임 오브젝트들이 이를 알고 있어야 함은 부당하다.
        //플레이어 죽음에 따른 모든 기능(점수, 라이프, 시각효과, 사운드, UI, 씬 처리 등등)이 모두 여기에 연결되어 있어야 한다.
        //점수 시스템, 도전과제 등 게임 외적인 기능까지 플레이어에 연결되어 있어야 하는 것은 논리적이지 않다.

            // -> 이 때 필요한 게 '유니티 이벤트'
        
        //이벤트 기능의 핵심: 각 스크립트의 직접적 연결을 끊고, 이후 기능을 추가할 때도 유니티 내에서 추가하면 되게 (코드를 수정할 필요가 없도록) 한다.

            //Dead() 함수가 발동될 때, 함께 발동될 함수들의 리스트를 만든다.
            //이들은 서로 스크립트를 불러올 필요가 없으며, 서로의 존재를 알 필요가 없다. 단지 Dead()가 발동될 때 리스트의 모든 기능이 같이 발동된다.

        onPlayerDead.Invoke(); //발동시킨다.
                               //이후 플레이어 사망 시 처리할 함수가 늘어나도, 코드는 더 이상 늘어나지 않는다.



        Debug.Log("죽었다");
        Destroy(gameObject);
    }
}
