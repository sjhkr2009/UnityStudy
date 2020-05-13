using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterRotator : MonoBehaviour
{
    // 포신을 쏘는 막대기의 회전을 제어한다.

    enum RotateState //enum은 오브젝트의 상태를 모아놓을 때 쓴다. 컴퓨터는 여기 들어간 단어들을 각각 0,1,2,3,... 의 숫자로 인식한다.
    {
        Idle,Vertical, Horizontal,Ready
    }
    //여기서, isIdle, isVertical, isHorizontal, isReady 라는 bool 변수를 모두 만들어놓고, 오브젝트 상태가 변할 때마다 하나는 true, 나머진 false로 바뀌게 해줄 수도 있다.
    //하지만 상태가 바뀔 때마다 모든 변수의 true/false를 모두 바꾸는 것은 리소스의 낭비이므로, 오브젝트의 상태를 여기에 정리해 두는 역할로 enum을 쓴다.
    //상태에 따라 달리 반응하는 switch 문과 함께 많이 쓰인다.

    RotateState state = RotateState.Idle; //enum을 state 변수로 불러온다. (여기선 처음 호출하는 상태이니 idle로 불러오기로 한다)

    public float verticalRotateSpeed = 360f; //회전 속도를 정해준다. 여기서는 초당 360도 회전시키기로 한다.
    public float horizontalRotateSpeed = 360f;

    public BallShooter ballShooter; //막대기의 세팅이 끝난 후에 발사 스크립트를 켜기 위해 불러온다.

    void Start()
    {
        
    }

    
    void Update()
    {
        //막대기는 유저의 조작에 따라 대기상태에서 수평 회전 - 수직 회전 - 준비(게이지 모으기) - 발사 순으로 조작되게 한다.

        /*
        if (state == RotateState.Idle)
        {
            if (Input.GetButtonDown("Fire1")) //대기상태일 때 키 입력 시
            {
                state = RotateState.Horizontal; //수평회전 상태로 변경
            }
        }
        else if (state == RotateState.Horizontal)
        {
            if (Input.GetButton("Fire1")) //수평회전 상태에서 키 입력이 계속되는 동안
            {
                transform.Rotate(new Vector3(0, horizontalRotateSpeed * Time.deltaTime, 0)); //Y를 축으로 초당 360만큼 회전
            }
            else if (Input.GetButtonUp("Fire1")) //키 입력을 떼면
            {
                state = RotateState.Vertical; //수직회전 상태로 변경
            }
        }
        else if (state == RotateState.Vertical)
        {
            if (Input.GetButton("Fire1")) //수직회전 상태에서 키 입력 시
            {
                transform.Rotate(new Vector3(-verticalRotateSpeed * Time.deltaTime, 0, 0)); //X를 축으로 초당 360만큼 회전
            }
            else if (Input.GetButtonUp("Fire1")) //키 입력을 떼면
            {
                state = RotateState.Ready; //준비상태로 변경
            }
        }
        */

        //위를 switch문으로 바꿔보면 아래와 같다.

        switch (state)
        {
            case RotateState.Idle:

                if (Input.GetButtonDown("Fire1")) //대기상태일 때 키 입력 시
                {
                    state = RotateState.Horizontal; //수평회전 상태로 변경
                }

            break;
            case RotateState.Horizontal:

                if (Input.GetButton("Fire1")) //수평회전 상태에서 키 입력이 계속되는 동안
                {
                    transform.Rotate(new Vector3(0, horizontalRotateSpeed * Time.deltaTime, 0)); //Y를 축으로 초당 360만큼 회전
                }
                else if (Input.GetButtonUp("Fire1")) //키 입력을 떼면
                {
                    state = RotateState.Vertical; //수직회전 상태로 변경
                }

            break;
            case RotateState.Vertical:

                if (Input.GetButton("Fire1")) //수직회전 상태에서 키 입력 시
                {
                    transform.Rotate(new Vector3(-verticalRotateSpeed * Time.deltaTime, 0, 0)); //X를 축으로 초당 360만큼 회전
                }
                else if (Input.GetButtonUp("Fire1")) //키 입력을 떼면
                {
                    state = RotateState.Ready; //준비상태로 변경
                    ballShooter.enabled = true; //ballShooter 스크립트를 활성화한다. (유니티 상에서는 꺼 두고, 여기서 활성화시킨다)
                }

            break;
            case RotateState.Ready:
            break;
            
        }
    }

    private void OnEnable() //이 함수가 시작될 때마다
    {
        transform.rotation = Quaternion.identity; //회전값을 초기화하며 (.identity는 (0,0,0)에 해당하는 회전값)
        state = RotateState.Idle; //현재상태를 아이들 상태로 바꾸고
        ballShooter.enabled = false; //ballShooter는 비활성화한다.
    }
}
