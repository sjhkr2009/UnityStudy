using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//카멜명명법
//함수와 클래스명은 대문자로 시작 ex: class Player, void Start
//단어와 단어 사이는 대문자로 구분 ex: MonoBehavior (언더바 등을 쓰지 않음)
//변수 이름은 소문자로 시작 ex: float moveSpeed

public class Player : MonoBehaviour
{
    //접근 지시자 public 또는 private 등을 선언하지 않으면, 자동으로 private이 된다.
    public float speed = 10f;
    public Rigidbody playerRigidbody;

    public GameManager gameManager; //게임 종료 여부를 확인하기 위해 가져온다.


    void Start()
    {
        //GetComponent<>(); : 해당 객체에게 적용되어 있는 다른 컴포넌트들 중 해당 이름의 컴포넌트를 찾아 지정한다.
        //유니티에서 playerRigidbody 칸에 Rigidbody를 드래그 앤 드롭하는 것과 결과는 같지만, Rigidbody를 위에서처럼 public으로 하지 않고, private으로 한다면 GetComponent로 지정해줄 수 있다.
        playerRigidbody = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        // +추가
        //게임이 종료될 경우 움직이지 않게 한다.
        if (gameManager.isGameOver == true)
        {
            return; //함수 종료
        }
        
        
        
        
        //특정 키 입력에 대한 반응 넣기 (WASD로 캐릭터 조작)
        //이 방식은 조작 기능과 키 입력을 직접 연결하는데, 실제 게임은 거의 이렇게 만들지 않음. (키보드 커스터마이징이 불가하며, 조이스틱에 대응 X)
        /*
        if (Input.GetKey(KeyCode.W))
        {
            playerRigidbody.AddForce(0, 0, speed);
        }

        if (Input.GetKey(KeyCode.A)){
            playerRigidbody.AddForce(-speed, 0, 0);
        }

        if (Input.GetKey(KeyCode.S))
        {
            playerRigidbody.AddForce(0, 0, -speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            playerRigidbody.AddForce(speed, 0, 0);
        }
        */



        //캐릭터 조작
        //키 입력 - 조작 명령 - 조작 기능 순으로 이어짐.
        //키 입력에 연결된 명령을 수정해도 기능을 코드상에서 수정할 필요가 없음. 즉, 키 세팅(키보드 커스터마이징)이 가능함.

        //GetAxis는 키 입력을 -1 ~ 1 사이의 값으로 받아온다. 
        //조이스틱에선 스틱을 살짝 밀어서 소수값을 입력하는게 가능하므로, bool 값이 아니라 숫자로 받아온다.

        float inputX = Input.GetAxis("Horizontal");

        //[←→]키 - "Horizontal" 명령 - 캐릭터 조작.
        //Horizontal 명령을 내리는 키를 무엇으로 할지는 유니티 상단 [Edit] - [Project Settings] - [Input] 에서 변경 가능. 디폴트값은 좌우 방향키 또는 A,D키 및 조이스틱.
        //즉, 코드상에선 Horizontal 명령을 받아와서 캐릭터 조작으로 이어지게만 해 주고, 키 지정은 유니티에서 할당한다.

        float inputZ = Input.GetAxis("Vertical");

        //AddForce: 힘을 주어 대상을 움직이게 한다. 즉, 움직임에 관성이 붙는다. 힘을 얼마나 줄 지 정해줘야 한다.
        //playerRigidbody.AddForce(inputX * speed, 0, inputZ * speed); //각 입력에 위에서 정의한 속도상수(speed)를 곱한 만큼 캐릭터가 이동한다.




        //velocity: 대상이 그 속도로 움직이게 한다. 움직일 거리와 방향을 Vector3로 정해줘야 한다.
        float fallSpeed = playerRigidbody.velocity.y;
        //y값의 속도를 특정 값으로 업데이트하면, 중력의 영향을 받다가도 매 프레임마다 그 속도로 초기화된다.
        //자연스럽게 중력의 영향을 받아 떨어지게 하려면, 원래의 y값을 받아와서 계속 업데이트해주면 된다.
        
        Vector3 velocity = new Vector3(inputX * speed, fallSpeed, inputZ * speed); //Horizontal 및 Vertical 명령을 해당 방향으로 조작하는 기능으로 연결하고, 가속도를 곱한다.
        playerRigidbody.velocity = velocity; //rigidbody의 velocity 기능에 해당 Vector3 값을 지정한다.

        

    }
}
