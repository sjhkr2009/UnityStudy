using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticTest : MonoBehaviour
{
    //Dog 스크립트의 static 변수를, Dog 함수를 불러오지 않고 바로 사용하기.

    //정적 변수가 아닌 닉네임을 출력하려면 아래와 같이 Dog 함수를 불러와서 해당 스크립트를 유니티에서 Dog를 드로그 앤 드롭하거나, 14번째 줄과 같이 GetComponent로 불러와야 한다.
    public Dog dog;

    void Start()
    {
        //다른 함수에서 정적 변수가 아닌 변수를 가져와 출력할 때
        dog = GetComponent<Dog>();
        string name = dog.nickName;
        Debug.Log("Test Name: " + name);
        //이 때, 불러오는 과정 없이 그냥 Dog.nickName 이라고 적으면 에러가 뜬다.


        //정적 변수를 가져올 때
        int cnt = Dog.count;
        Debug.Log("Test Count: " + cnt);
        //불러오는 과정 없이 바로 '함수명.변수명' 으로 사용할 수 있다.

        //정적 함수도 마찬가지.
        Dog.ShowAnimalType();
        
    }

    
    void Update()
    {
        
    }
}
