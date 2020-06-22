using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTest : MonoBehaviour
{
    // PointManager 스크립트에서 점수를 받아와서 오브젝트의 점수(포인트)를 조절하는 스크립트.

    public PointManager pm;

    void Start()
    {
        //pm.point = 99999999; //직접 변수를 수정하면 어떤 값이든 들어갈 위험이 있다.

        //함수를 사용하여 가져올 경우 (기존 방식)

            /*
        pm.SetPoint(99999999); //함수를 통해 변수를 설정하고 가져온다.
        int myPoint = pm.GetPoint();
        Debug.Log("현재 포인트: " + myPoint);

        pm.SetPoint(-9999);
        myPoint = pm.GetPoint();
        Debug.Log("현재 포인트: " + myPoint);
            */
        
        
        //프로퍼티를 사용할 경우

        pm.point = -1000;

        Debug.Log(pm.point);
        Debug.Log(pm.count);
    }
    
    void Update()
    {
        
    }
}
