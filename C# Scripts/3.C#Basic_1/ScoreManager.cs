using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //리스트: 배열의 확장으로서, 여러 개의 게임 오브젝트를 한번에 다루게 해 준다는 점은 동일하다.
    //        그러나 배열과 달리 오브젝트가 들어갈 공간의 개수를 실시간으로 변경하거나, 특정 오브젝트를 추가/삭제하는 등의 편의 기능이 많다.


    //여기서는 10개의 학생들에게 차례로 임의의 점수를 부여하는 코드를 작성해 본다.
    public int[] scores = new int[10];  //배열을 이용한 방식
    public List<int> scores2 = new List<int>(); //리스트를 이용한 방식
    int index = 0;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        //배열을 이용한 방식

        if (Input.GetMouseButtonDown(0)) //0이 들어가면 마우스 왼쪽을 누르는 순간에 값이 True가 된다(= 실행된다)
        {
            scores[index] = Random.Range(0, 100); //0~100 사이의 점수를 scores의 값에 차례로 넣어준다.
            index++; //점수를 입력 후 다음 순번으로 넘어감.
        }
        //이 때, 학생 수가 1명 늘어나 11명이 되면 에러가 난다.
        //배열에서 오브젝트가 들어갈 공간의 개수를 증가시키는 방법은 아예 기존 배열을 삭제하고 새로 만드는 것뿐. (예를 들어, scores = new int[11]; 이런 식으로)
        //또한 학생 수가 줄어서 특정 값을 빼는 것도 불가능하다.



        //리스트를 이용한 방식
        if (Input.GetMouseButtonDown(1)) //1이 들어가면 마우스 오른쪽을 누르는 순간 실행된다.
        {
            int randomNumber = Random.Range(0, 100); //임의의 점수를 생성하고
            scores2.Add(randomNumber); //리스트에 추가한다.
        }
        //처음부터 학생 수를 정할 필요 없이, 값을 추가할 때마다 증가한다.
        //또한 값을 삭제할수도 있다.
        if (Input.GetKeyDown(KeyCode.D))
        {
            scores2.RemoveAt(2); //2번 값(실질적으로는 세번째 값)이 리스트에서 삭제된다.
            //참고로 특정 순번이 아닌 특정 값을 뺄 때는 scores2.Remove(값) 을 사용하면 된다.
        }
    }
}
