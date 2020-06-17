using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D03_MousePosition : MonoBehaviour
{
    // 유니티 좌표계
    // 1. Local : 특정 오브젝트를 기준(원점)으로 하는 좌표
    // 2. World : 게임 내 세계의 절대적 위치를 나타내는 좌표
    // 3. Screen : 플레이 화면의 좌측 하단을 원점으로 1픽셀당 1씩 증가하는 좌표. 2D 화면을 기준으로 하므로 Z축의 값은 항상 0이다.
    // 4. ViewPort :  스크린 좌표처럼 화면을 기준으로 하지만, 0에서 1 사이의 값을 갖는다. 화면 좌측 하단은 스크린과 같이 원점(0,0)이며, 우측 상단이 (1,1)이 된다.


    /// <summary>
    /// 플레이어 화면으로부터 Ray를 쏴서 해당 지점의 물체를 감지하는 함수.
    /// </summary>
    void TestClickObject()
    {
        //마우스 위치를 Input으로 받아온다. 현재 화면을 비추는 카메라는 Camera.main으로 받아올 수 있다.
        //참고로 Camera.main은 'MainCamera' 태그가 붙은 카메라를 가져온다. 카메라의 태그를 변경하면 불러올 수 없다.
        Vector3 _mousePos = Input.mousePosition;
        Camera cam = Camera.main; 

        //마우스 위치를 월드 좌표계로 변환한다. Z축은 카메라가 비추기 시작하는 지점(near)으로 설정한다.
        Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(_mousePos.x, _mousePos.y, cam.nearClipPlane));

        //레이의 방향은 '카메라'에서 '카메라가 비추는 화면' 방향으로 한다. (A에서 B로 가는 방향은 'B - A')
        Vector3 cameraPos = cam.transform.position;
        Vector3 dir = mousePos - cameraPos;
        dir.Normalize();

        //------------------------------------------------------- 여기까지(카메라에서 마우스 위치로 Ray 발사)는 아래 언급할 ScreenPointToRay로 대체가능

        Debug.DrawRay(cameraPos, dir * 100, Color.white);

        RaycastHit hit;
        if (Physics.Raycast(cameraPos, dir, out hit, 100))
        {
            Debug.Log($"{hit.collider.gameObject.name}이 클릭되었습니다.");
        }
    }

    //다만 위는 레이캐스트 감지 과정을 순차적으로 나타낸 것이고, 실제로는 아래와 같이 ScreenPointToRay를 많이 사용한다.

    void ClickObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.white);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            Debug.Log($"{hit.collider.gameObject.name}이 클릭되었습니다.");
        }
    }


    void ClickObjectByLayermask()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.white);

        // 레이어 마스크를 통해 클릭할 레이어를 선택할 수 있다.
        // Ray가 설정된 레이어 외의 콜라이더를 무시한다.

        // 레이어마스크는 비트 연산을 사용한다. 0~31번 레이어까지 설정 가능한 이유는 int32를 사용하기 때문.
        // 32개의 비트는 각각 0~31 레이어를 의미하며, 해당하는 자리의 비트가 0이면 감지하지 않고, 1이면 감지한다.
        // 따라서 shift 연산을 이용하여 감지할 레이어를 켤 수 있고, 레이어 하나를 켠 다음 비트를 반전시켜 특정 레이어만 끌 수도 있다.

        int mask = 1 << 8; // 1을 켜고 8자리만큼 밀면, 9번째 자리의 비트가 1이 된다. 즉 8번 레이어를 무시한다.
        mask = (1 << 8) | (1 << 9); // 비트연산의 or 연산자 | 를 이용하여 여러 개의 비트를 켜줄 수 있다.

        //LayerMask 클래스를 통해 레이어 이름을 가져올 수 있다. 내부적으로는 int를 저장하는 클래스이므로, int 변수에 대입 가능하다.
        mask = LayerMask.GetMask("Monster") | LayerMask.GetMask("Wall");

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, mask)) //보통 마지막 자리에 레이어마스크를 int 형태로 입력한다.
        {
            Debug.Log($"{hit.collider.gameObject.name}이 클릭되었습니다.");
        }
    }

    [SerializeField] bool isTest = false;
    [SerializeField] bool useLayerMask;
    private void Start()
    {
        A01_Manager.Input.OnMouseEvent -= OnMouseAction;
        A01_Manager.Input.OnMouseEvent += OnMouseAction;
    }

    void OnMouseAction(E02_Define.MouseEvent evt)
    {
        if (evt != E02_Define.MouseEvent.Press) return;

        if (useLayerMask)
        {
            ClickObjectByLayermask();
            return;
        }

        if (isTest) TestClickObject();
        else ClickObject();
    }
}
