using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property : MonoBehaviour
{
    // 속성 (property)

    // 객체지향 프로그래밍 - 정보은닉: 필요한 것만 보여주기
    // 변수를 직접 조작하지 않고 속성을 통해 get, set으로 처리한다.

        //속성은 스패너 모양으로 나오며, set을 가지지 않은 경우 임의의 값을 할당할 수 없다.

    private int level = 3;

    public int prop
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
        }
    }


    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Level: " + level);
            Debug.Log("Prop: " + prop);
            prop = 10;
            Debug.Log("Level: " + level);
            Debug.Log("Prop: " + prop);
        }
    }
}
