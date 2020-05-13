using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    //static: 모든 오브젝트가 공유하는 단 하나의 변수. 개별 오브젝트에 묶여있지 않은 것.
    //따라서 다른 함수에서 바로 사용할 수 있다. StaticTest 스크립트 참고.

    public static int count = 0; //모든 오브젝트에서 공유하는 변수

    public string nickName; //각 오브젝트마다 다르게 가지는 변수
    public float weight;

    void Awake()
    {
        count++; //시작할 때 우선 모든 동물들이 가진 함수마다 마리 수를 하나씩 세어준다.
    }

    void Start()
    {
        Bark(); //Awake()함수가 실행된 후, 개들의 수와 닉네임을 출력 (확인용)
    }

    public void Bark()
    {
        Debug.Log("모든 개들의 수: " + count);
        Debug.Log(nickName + ": Bark!");
    }

    
    //static은 함수에도 사용할 수 있다

    public static void ShowAnimalType()
    {
        Debug.Log("이것은 개입니다.");
    }
}
