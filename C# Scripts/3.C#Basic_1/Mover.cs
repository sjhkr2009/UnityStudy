using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    
    //물체 이동시키기 1
    
    void Start()
    {
        //지정된 좌표로 이동
        Vector3 targetPosition = new Vector3(1, 0, 0); //이동시킬 좌표를 지정
        transform.position = targetPosition; //이 오브젝트의 위치를 해당 위치로 변경


        //지정된 수치만큼 이동
        Vector3 move = new Vector3(-1, 1, -1); //어디로 얼마나 이동시킬지 지정
        //transform.position += move; //원래 위치에 해당 수치만큼 더해도 되지만
        transform.Translate(move); //Translate를 사용해도 된다. (괄호 안은 이동시킬 벡터 값 입력)
    }

    
    void Update()
    {
        
    }
    
}
