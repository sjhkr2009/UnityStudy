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

        Test(5);

        Physics.Raycast(ray, out hit); //마우스에서 ray라는 레이저를 쏴서 맞춘 지점의 정보를 hit에 넣는다. ray가 비어 있으면 false를 반환한다.
        Vector3 dir = hit.point - transform.position; //마우스 위치를 바라보는 벡터값

        Test(3);

        //1번
        //transform.rotation = Quaternion.AngleAxis(Quaternion.LookRotation(dir).eulerAngles.y, Vector3.up); //세로축을 기준으로 해당 벡터만큼 회전

        //2번
        dir.y = 0; //벡터값의 y는 0으로 고정시키고
        transform.rotation = Quaternion.LookRotation(dir.normalized); //x,z의 좌표(위에서 바라본 마우스의 평면상 위치)에 따라서만 바라보게 한다.
    }


    //디버깅: F9 또는 코드 왼쪽에 마우스 클릭으로 체크할 줄을 선택할 수 있다.
    //F5를 누르고, 유니티의 실행 버튼 클릭 후 Visual Studio로 이동.
    //체크된 줄에서 실행이 멈춰 있다. F10을 눌러 다음 줄 실행, F5를 눌러 다음 선택된 행으로 이동할 수 있다.
    //함수 위에서 F11을 누르면 해당 함수로 이동, 빠져나가려면 Shift + F11
    void Test(int input)
    {
        int n = input;
        int n2 = 3;
        int n3 = n + n2;
        //Debug.Log(n3);
    }

    /*
    Quaternion SetRotate(Vector2 now, Vector2 target)
    {
        Vector2 direction = target - now;
        float rotateLevel = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0, 0, rotateLevel));
    }*/
}
