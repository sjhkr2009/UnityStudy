using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : MonoBehaviour
{
    //싱글톤: 프로그래밍 방식(방향)의 하나. 게임 내에 단 하나만 유일하게 존재하며 언제나 접근 가능한 오브젝트를 만들 때 사용.
    //예를 들어 몬스터는 여러 개일 수 있지만, 이를 관리하는 게임 매넞, 몬스터 매니저 등은 하나만 있고 어디서나 접근 가능하게 만들 수 있다.


    //닌자들 중 왕이 하나 있다고 가정한다.
    public string ninjaName;
    public bool isKing; //이것이 체크된 게 왕.

    public static Ninja ninjaKing; //static 변수는 게임 내에 유일하며 메모리상에 하나만 존재한다.
    //Ninja 클래스에 해당하는 모든 오브젝트를 확인해서 왕을 찾는 작업은 번거롭다.
    //따라서 static 변수에 해당 오브젝트(닌자 왕)를 넣음으로써 바로 왕에게 접근할 수 있게 한다.

    void Start()
    {
        if (isKing) //자기 자신의 isKing 값이 true면 실행
        {
            ninjaKing = this; //이 오브젝트를 ninjaKing 변수에 대입한다.
        }
    }

    void Update()
    {
        Debug.Log("My Name: " + ninjaName);
        Debug.Log("Ninja King is " + ninjaKing);
    }

}
