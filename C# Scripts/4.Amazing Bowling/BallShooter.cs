using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI인 Slider를 가져와 사용할 것이다.

public class BallShooter : MonoBehaviour
{
    //공을 날려보낼 스크립트

        //조정할 대상: 발사체(공), 발사 위치, 발사할 힘(UI)
    public Rigidbody ball; //공에 있는 Rigidbody 컴포넌트를 불러온다.
    public Transform firePos; //공이 생성될 위치. 빈 오브젝트를 만들어 위치를 조정해준 후 여기에 드래그 앤 드롭으로 넣어준다.
    public Slider powerSlider; //공을 쏘아올릴 에너지를 모을 때 충전량을 보여줘야 하므로, 슬라이더를 가져온다.

        //카메라 타겟 변경 위해 카메라 불러오기
    public CamFollower cam;

        //효과음을 재생할 오디오와 음원 파일
    public AudioSource shootingAudio; //공을 쏠 때 출력될 오디오 컴포넌트를 불러온다.
    public AudioClip fireClip; //공을 발사할 때 오디오 컴포넌트로 재생될 음원 파일.
    public AudioClip chargingClip; //차지 중에 오디오 컴포넌트로 재생될 음원 파일.


        //발사할 힘을 조절한다
    public float minForce = 15f; //최소 힘
    public float maxForce = 30f; //최대 힘
    public float currentForce; //현재 힘
    public float chargeSpeed; //매 초 충전되는 힘
    public float chargingTime = 3f; //충전시간
    private bool fired; //발사 여부


    private void OnEnable() //해당 씬 시작, 혹은 해당 스크립트나 오브젝트가 켜질(활성화될) 때 호출되는 함수. Start()와 유사하지만 1회가 아니라 활성화될 때마다 계속 시행된다는 차이가 있다.
    {
        //재시작할 때마다 초기화할 요소들을 넣는다. 여기서는 충전된 힘과 그걸 보여주는 UI를 최소값으로 초기화하고, 발사 여부를 false로 되돌린다.
        currentForce = minForce;
        powerSlider.value = minForce;
        fired = false;
    }

    void Start()
    {
        //게임 최초 시작 시 한 번만 설정하면 되는 요소.
        chargeSpeed = (maxForce - minForce) / chargingTime; //힘 충전 속도 = (최대 충전량 - 최소 충전량) / 최대 충전시간
    }

    
    //이제 발사 시스템을 만들어보자.
    //발사는 총 3가지 상태로 이루어진다: 충전 시작, 충전 중, 충전 끝(발사) + 최대 충전 시 강제 발사까지 4개
    void Update()
    {
        // 이미 발사되었다면 발사 시스템들을 실행하지 않는다.
        if (fired)  //fired == true 와 같은 의미.
        {
            return;
        }
        
        // 0. 강제 발사
        if (currentForce >= maxForce) //힘이 최대치에 도달했는데 발사되지 않은 경우. 
        {
            currentForce = maxForce; //최대치의 힘으로 설정 후
            Fire(); //발사!
        }
        // 1. 충전 시작
        else if (Input.GetButtonDown("Fire1")) //키를 눌렀을 때
        {
            currentForce = minForce; //최소 힘으로 리셋하고
            shootingAudio.clip = chargingClip; //충전 효과음을 오디오에 넣은 후
            shootingAudio.Play(); //재생한다.
        }
        // 2. 충전 중
        else if (Input.GetButton("Fire1")) //키를 누르는 중일 때
        {
            currentForce += chargeSpeed * Time.deltaTime; //힘이 점점 올라가게 하고
            powerSlider.value = currentForce; //그에 따라 UI도 조정해준다.
        }
        // 3. 발사 처리
        else if (Input.GetButtonUp("Fire1")) //키를 뗐을 때
        {
            Fire(); //발사 처리 함수 실행
        }
    }

    void Fire() //발사 처리 함수
    {
        fired = true;
        Rigidbody ballInstance = Instantiate(ball, firePos.position, firePos.rotation); //공을 발사위치에 생성한다
        ballInstance.velocity = currentForce * firePos.forward; //공의 속도는 (힘 * 방향) 으로 설정한다. ".forward"는 오브젝트의 앞쪽(Z축) 방향의 Vector3 값을 가져오는 기능.
        //이러면 공은 해당 속도를 가지므로, 별도의 조치(폭발)를 취하기 전까지 계속 날아간다.

        shootingAudio.clip = fireClip; //발사 효과음을 오디오에 넣고
        shootingAudio.Play(); //재생

        //충전된 힘과 UI 초기화.
        currentForce = minForce;
        powerSlider.value = currentForce;

        //카메라의 초점을 생성한 공으로 바꾼다.
        cam.SetTarget(ballInstance.transform, CamFollower.State.Tracking); //상태는 트랙킹으로 바꾸어 그에 맞는 줌을 갖게 한다.
    }
}
