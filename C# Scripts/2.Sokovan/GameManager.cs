using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //씬 관리하는 기능을 가지고 있음

public class GameManager : MonoBehaviour
{
    //1. 모든 ItemBox가 충돌했는지 매 순간 확인하여, 충돌했으면 승리를 선언하게 한다.
        //이 때, 이 스크립트는 모든 ItemBox를 가져와야 하고, 각각의 ItemBox는 자신이 충돌했는지의 여부를 알고 있어야 한다.
    
    //2. 게임 승리 시 승리 UI를 출력한다.

    //3. 게임 재시작 기능을 추가한다.

    public ItemBox[] itemBoxes; //다른 오브젝트를 불러온다. 여러 개이므로 배열 형식[] 으로 선언한다.
                                //유니티 내에서 해당 스크립트 적용 후 size 란에 개수를 입력하여 몇 개인지 정할 수 있다(이 씬에서는 3개). 그 후 각각의 칸에 ItemBox들을 드래그 앤 드롭으로 넣어준다.

    public GameObject winUI; //승리 시 출력할 UI

    public bool isGameOver; //게임 종료 여부 확인
    
    void Start()
    {
        isGameOver = false; //시작 시 게임 종료 여부는 false로 지정.
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //GetKey와 달리 키를 누르는 순간에만 1번 활성화된다.
        {
            SceneManager.LoadScene("Main"); //해당 씬을 시작한다. 현재 씬을 재시작할 것이므로 현재 씬의 이름을 적어준다.
                                            //BuildSetting(File - Build Setting, 또는 Ctrl+Shift+B)의 씬 번호를 넣어도 된다. 이 경우 SceneManager.LoadScene(0) 이 된다.
        }

        if (isGameOver == true) //게임 승리 이벤트가 계속 뜨지 않도록.
        {
            return; //만일 게임오버가 되었다면, 이대로 Update 함수를 종료한다. (이하의 코드는 실행되지 않는다)
        }


        int count = 0; //매 순간마다 골인지점에 도달한 오브젝트 개수를 0부터 세야 한다.

        for (int i = 0; i < 3; i++) //각각의 itembox가 골인지점에 도달했는지 순차적으로 확인
        {
            if (itemBoxes[i].isOveraped == true) //해당 오브젝트가 가진 변수 중, 충돌 여부를 확인하는 isOveraped를 체크한다. 만약 충돌했다면 카운트한다.
            {
                count++;
            }
        }

        if (count >= 3)
        {
            Debug.Log("승리!");
            isGameOver = true; //3개 모두 충돌했다면 게임을 종료한다.
            winUI.SetActive(true); //승리 UI를 활성화한다.
        }
    }
}
