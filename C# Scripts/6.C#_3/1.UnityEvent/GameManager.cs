using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //씬을 이동하거나 재시작할 때 필요하다.

public class GameManager : MonoBehaviour
{
    // 캐릭터 사망 시 5초 후 재시작하는 스크립트

    public void OnPlayerDead() //플레이어 사망 시 호출할 함수이므로 public으로 선언.
    {
        Invoke("Restart", 5f); //Invoke("함수명", 지연시간): 해당 함수를 지연시간 후에 발동시킨다. 여기서는 5초 후 발동.
        //함수명.Invoke()로 쓸 수도 있다. 지연시간은 선택사항으로, 적지 않으면 즉시 발동한다.
    }

    void ReStart()
    {
        SceneManager.LoadScene(0); //씬을 재시작한다. (현재 씬인 0번째 씬을 불러온다.)
    }


}
