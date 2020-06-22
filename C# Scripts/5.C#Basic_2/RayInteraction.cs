using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayInteraction : MonoBehaviour
{
    //특정 키를 누르면 카메라로부터 광선을 발사해 물체를 감지하는 컴포넌트
    private Camera cam; //카메라 위치를 가져올 변수
    public float distance = 100f; //광선 길이를 나타낼 변수
    public LayerMask target; //감지하고 싶은 대상의 레이어를 넣을 변수

    private Transform moveTarget; //충돌한 오브젝트를 이 변수에 넣은 다음, 이 변수를 통해 이동시킬 예정.
    private float targetDistance; //충돌 당시 나와 대상의 거리를 이 변수에 넣은 다음, 그 거리를 유지하며 시야에 따라 이동할 예정.

    
    void Start()
    {
        cam = Camera.main; //현재 활성화된 메인 카메라
    }
    
    void Update()
    {
        //광선 시작 지점
        Vector3 rayStart = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f,0f)); //.ViewportToWorldPoint: 카메라 위치한 곳의 특정 지점을 글로벌 좌표에서의 Vector3 값으로 반환한다.
        //괄호 안에는 카메라의 어느 지점인지 표기한다. x,y의 값이 0.5f, 0.5f일 경우 카메라의 정중앙이며, z값은 깊이를 의미하는데 일반적으로 0으로 둔다.

        //광선 방향
        Vector3 rayDirection = cam.transform.forward; //카메라 위치의 앞쪽

        //광선을 확인할 수 있게 씬 위에 그려준다. (실제 게임엔 반영 X)
        Debug.DrawRay(rayStart, rayDirection * 100f, Color.green); //괄호 안에는 (시작 지점, 방향*길이, 색상)

        if (Input.GetMouseButtonDown(0)) //마우스 왼쪽 버튼을 클릭하는 순간
        {
            RaycastHit hit; //광선에 충돌한 대상을 저장할 수 있는 변수

            if (Physics.Raycast(rayStart, rayDirection, out hit, distance, target)) //광선을 생성하며 물체를 감지했을 경우
                               //괄호 안에는 시작 지점(Vector3 값), 방향(Vector3 값), 출력(RaycastHit), 길이(float), 감지할 레이어(LayerMask)가 들어간다. (시작점과 방향 외에는 선택사항)
                               //out: 입력을 통해 생긴 값이 out 뒤에 오는 변수를 통해 빠져나온다. 여기서는 Raycast를 통해 감지된 대상을 hit 변수에 저장한다.
            {
                Debug.Log(hit.collider.transform.name); //.collider: RaycastHit 변수 뒤에 붙일 경우, 광선에 충돌한 대상을 가리킨다.
                //참고로 .point는 충돌 지점 좌표, .normal은 충돌한 대상이 보고 있는 방향, .distance는 충돌 지점까지의 거리 등등으로 활용 가능.

                Debug.Log("뭔가 광선에 걸렸다"); //감지하면 로그로 확인할 수 있게 한다.
            }
        }


        //광선 자체를 변수로 만들수도 있다.
        Ray laser = new Ray(rayStart, rayDirection); //광선을 생성 (시작점과 방향만 입력할 경우, 길이는 무제한.

        if (Input.GetMouseButtonDown(1)) //마우스 오른쪽 버튼을 클릭하는 순간
        {
            Debug.DrawRay(laser.origin, laser.direction * 100f, Color.red); //.origin은 광선의 시작 지점을 의미.

            RaycastHit hit2;

            if(Physics.Raycast(laser, out hit2, distance, target)) //이 경우 시작점과 방향을 광선 변수로 대체
            {
                GameObject hitObject = hit2.collider.gameObject; //닿은 오브젝트를 저장
                hitObject.GetComponent<Renderer>().material.color = Color.red; //닿은 오브젝트에 렌더러를 불러와서, 색상을 바꿔준다.

                moveTarget = hitObject.transform; //충돌한 오브젝트의 트랜스폼을 변수에 넣어준다.
                targetDistance = hit2.distance; //충돌 대상과의 거리를 변수에 넣어준다.
            }
        }

        if (Input.GetMouseButtonUp(1)) //마우스 버튼을 뗄 때
        {
            if(moveTarget != null) //옮길 대상이 있다면
            {
                moveTarget.GetComponent<Renderer>().material.color = Color.white; //대상의 색상을 흰색으로 바꿔주고
                moveTarget = null; //옮길 대상은 없어진다.
            }
        }

        if (moveTarget != null) //옮길 대상이 있다면
        {
            moveTarget.position = laser.origin + laser.direction * targetDistance; //대상 위치를 광선 시작 지점에서 광선 방향*거리를 더한 좌표로 유지시킨다.
            //즉 시야를 바꿔서 광선을 옮겨도(=광선의 방향이 바뀌어도) 일정 거리를 유지한 채 물체가 따라온다.
        }
    }
}
