using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    //마우스 위치를 향하도록 타워의 y축을 중심으로 회전시키기
    void Update()
    {
        //스크린에 ray를 쏴서 마우스의 위치를 가져온다.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Physics.Raycast(ray, out hit); //마우스에서 ray라는 레이저를 쏴서 맞춘 지점의 정보를 hit에 넣는다. ray가 비어 있으면 false를 반환한다.
        Debug.Log(hit.point); //point: 해당 지점 좌표를 Vector3 값으로 나타낸 것.

        Vector3 dir = hit.point - transform.position; //마우스 위치를 바라보는 벡터값
        dir.Normalize();

        transform.rotation = Quaternion.LookRotation(dir);
    }
}
