using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloFunction : MonoBehaviour
{
    
    void Start()
    {
        float sizeOfCircle = 30;
        Debug.Log("원의 사이즈: " + sizeOfCircle);

        float radius = GetRadius(sizeOfCircle);
        Debug.Log("원의 반지름: " + radius);
    }

    //원의 넓이를 주면 반지름의 길이를 반환하는 함수
    float GetRadius(float size)
    {
        float pi = 3.14f;
        float tmp = size / pi;

        float radius = Mathf.Sqrt(tmp); //제곱근 구하기

        return radius;
    }
}
