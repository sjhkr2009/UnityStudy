using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    void Update()
    {
        //transform.forward는 오브젝트의 로컬 좌표축 기준 z 방향
        //Vector3.forward는 글로벌 좌표축 기준 z 방향

        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World); //Space.World는 월드 좌표계 기준 해당 벡터로 이동. 안 붙이거나 Space.Self일 경우 지역 좌표계 기준.
        //구한 벡터가 지역 좌표계 기준이니, 글로벌 좌표계에서 해당 방향으로 움직인다.

        //transform.Translate(Vector3.forward * speed * Time.deltaTime); //글로벌 좌표계 기준 앞 방향을 구한 후 지역 좌표계 기준으로 이동하도록, 이렇게 표현할수도 있다.

        if(transform.position.z > 14f)
        {
            //오브젝트 풀링 방식
            gameObject.SetActive(false);
        }
    }
}
