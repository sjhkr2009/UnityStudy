using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollower : MonoBehaviour
{
    // 카메라로 오브젝트를 추적하고, 줌 인/줌 아웃을 조정하는 컴포넌트.
    // 카메라가 오브젝트 위치와 동일하게 하면 안 되니까, 카메라에서 일정 거리 떨어진 빈 오브젝트를 만든 후 카메라를 자식(하위) 오브젝트로 넣는다.
    // 이 빈 오브젝트가 추적할 오브젝트의 위치를 따라다니게 한다.

    // 3가지 상태: 라운드 대기 상태, 포탄 발사 준비 상태, 포탄 발사 후 추적하는 상태
    public enum State
    {
        Idle,Ready,Tracking
    }

    // property: 특정 변수를 바꾸면 자동으로 함수가 실행되게 한다. 즉 외부에서는 이 변수를 바꾸기만 해도 특정 동작이 시행된다. 함수로 대체할 수도 있지만, 더 간결한 방식.
    /* 다음과 같이 사용한다.
     * (변수 유형) (변수명) {                          //예시:  int score
     *                                                          {
     *      set                                                     set
     *      {                                                       {
     *      (해당 변수에 'value'를 넣었을 때 실행할 동작)               score = value + 2; //이렇게 하면 외부에서 score 값에 5를 넣으면 자동으로 7이 됨.
     *      }                                                       }
     * }                                                        }
     */
    private State state //enum State의 상태를 담는 state 변수를 만들고,
    {
        set
        {
            switch (value) //state 변수에 value 값을 대입했을 때
            {
                case State.Idle: // value가 State.Idle이면 (즉 누가 state = State.Idle 이라고 하면) 아래를 실행한다
                    zoomSize = idleZoomSize;
                    break;
                case State.Ready:
                    zoomSize = readyZoomSize;
                    break;
                case State.Tracking:
                    zoomSize = trackingZoomSize;
                    break;
            }
        }
    }
    
    //대상을 추적하되, 대상의 움직임을 부드럽게 추적하게 만든다.
    private Transform target; //추적할 대상. 유니티가 아니라 다른 스크립트에서 무엇을 추적할 지 정해줄 예정이니 private로 선언. (맨 아래 SetTarget() 함수를 통해서)
    public float smoothTime = 0.2f; //0.2초에 걸처 추적하게 할 것.

    //부드러운 움직임을 적용하기 위해 필요한 요소
    private Vector3 lastMovingVelocity; // 마지막 순간의 속도를 기록할 변수 (부드러운 카메라 움직임 위해 필요)
    private float lastZoomSpeed; // 마지막 순간의 줌 속도를 기록할 변수 (부드러운 줌인/줌아웃 위해 필요)
    private Vector3 targetPosition; //추적할 대상의 위치값을 넣을 벡터3 변수

    private Camera cam;
    private float zoomSize = 5f;

    private const float idleZoomSize = 14.5f; //const: 해당 변수를 변경할 수 없게 만든다.
    private const float readyZoomSize = 5f;
    private const float trackingZoomSize = 10f;


    
    void Awake()
    {
        cam = GetComponentInChildren<Camera>();//GetComponent는 자신에게 있는 컴포넌트를 검색하지만, InChildren을 붙이면 자식 오브젝트의 컴포넌트까지 검색해 가져온다.
        state = State.Idle; //상태를 Idle로 설정 (이 때 property 때문에 위의 'zoomSize = idleZoomSize;'가 자동으로 실행된다.)
    }

    private void Move() //카메라 이동 함수
    {
        targetPosition = target.transform.position;

        //Vector3.SmoothDamp를 통해 A가 B로 부드럽게 이동하는 경로의 위치값을 구할 수 있다. 괄호에는 (A의 위치, B의 위치, ref 최근 위치값을 기록할 변수, 지연시간) 이 들어간다.
        //ref 뒤의 값은 참고할 최근 값이 자동으로 기록되니, 우리가 값을 입력할 필요 없이 변수만 만들어서 넣어주면 된다.
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref lastMovingVelocity, smoothTime); //'내'가 '추적할 오브젝트'로 이동할 위치값을 구한다.

        transform.position = smoothPosition; //해당 위치로 이동한다.

    }
    
    private void Zoom() //카메라 줌 조절 함수
    {
        //부드러운 줌도 위의 부드러운 이동과 같다. 여기선 벡터값이 아닌 숫자를 구해야 하니 Mathf 함수를 사용한다.
        //orthographicSize는 이 카메라의 사이즈. 유니티에서 이 모드와 Perspective 모드 중 하나를 택할 수 있다. Perspective는 원근감이 있고, Orthographic은 원근감이 없으며 2D에서 자주 쓰인다.
        float smoothZoomSize = Mathf.SmoothDamp(cam.orthographicSize, zoomSize, ref lastZoomSpeed, smoothTime);

        cam.orthographicSize = smoothZoomSize;
    }


    void FixedUpdate() //FixedUpdate(): 프레임에 따라 변하지 않고, 정확한 간격마다 실행되는 함수. 정확한 동작을 요구할 때 쓰인다.
    {
        if(target != null) //추적할 타겟이 있을 경우
        {
            Move(); //이동과 줌 함수를 실행한다. 계산된 SmoothDamp 값에 의해 이동/줌인이 시행되니 FixedUpdate로 실행.
            Zoom();
        }
    }

    public void Reset() //이 스크립트가 생성 또는 재시작할 때(리셋 명령을 받았을 때) 실행되는 함수. 게임매니져 등 외부 컴포넌트가 리셋시킬 수 있게 public으로 선언.
    {
        state = State.Idle; //상태만 바꿔도 자동으로 property에 의해 zoomSize = idleZoomSize; 가 실행된다.
    }

    public void SetTarget(Transform newTarget, State newState) //외부에서 카메라의 타겟과 상태를 조정하기 위한 함수. 외부에서 호출해야 하니 public으로 선언.
    {
        target = newTarget;
        state = newState;
    }
}
