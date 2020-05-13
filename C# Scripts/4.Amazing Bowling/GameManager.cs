using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //텍스트 및 패널을 조작해야 하므로 UI를 가져온다
using UnityEngine.Events; //이벤트를 추가하기 위해 불러온다.

public class GameManager : MonoBehaviour
{
    //UnityEvent: 이벤트가 발동되면 등록된 모든 함수가 실행된다.
    //등록하는 법은, 유니티에서 + 버튼으로 칸을 추가한 후 오브젝트를 드래그 앤 드롭으로 넣고, 컴포넌트를 고른 다음, 그 컴포넌트가 가진 함수 리스트에서 실행할 함수를 고르면 된다.
    //발동하려면 이벤트명.Invoked(); 를 하면 된다.
    public UnityEvent onReset;
    
    
    
    // 게임은 3단계로 구성된다: 대기(로딩중) - 플레이 - 종료(포탄이 터진 후)
    // 대기: 카메라, 포신, 점수를 리셋하고 대기 UI 출력.
    // 플레이: 무한루프를 돌리다 포신이 폭발하면 엔딩으로 탈출
    // 종료: 모든 조작 비활성화, 엔딩 UI 출력, 게임 리셋.

    //게임 매니저에 쉽게 접근 가능하도록 싱글톤으로 만든다.
    public static GameManager instance; //이 함수 유형을 넣을 수 있는 정적 변수 생성
    private void Awake()
    {
        instance = this; //게임 시작 시 자기 자신을 정적 변수에 대입
    }


    public GameObject readyPannel; //로딩중 활성화할 패널
    public Text scoreText; //표기할 점수
    public Text bestScoreText; //표기할 최고점수
    public Text messageText; //패널에 띄워줄 메시지

    public bool isPlaying = false; //게임이 플레이 중인지 체크해줄 변수. 플레이 중이면 true, 아니면 false.
    private int score = 0; //점수

    //현재 게임 상태(대기, 플레이, 종료 등)를 받아와야 그에 맞게 패널을 끄거나 켤 수 있다.
    public ShooterRotator shooterRotator;

    //게임 상태에 따라 캠도 조작해야 함.
    public CamFollower cam;


    public void AddScore(int newScore) //점수를 추가하는 함수. Prop 스크립트에서 자신이 파괴될 때 실행해줘야 하므로 public으로 선언.
    {
        score += newScore;
        UpdateBestScore(); //최고점수 갱신 (최고점이 아니면 if문이 동작하지 않음)
        UpdateUI(); //점수를 표기하는 UI를 갱신
    }


    //Player Preference: Key-Value 세트를 파일로 저장한다. 이후 키를 통해 Value를 불러오거나 수정할 수 있다. 저장 시 Key가 없다면 해당 이름의 키를, Value가 없다면 0을 넣어서 저장한다.
    //이를 통해 최고점수를 파일 형태로 저장하여, 게임이 종료된 후에도 기록에 남도록 한다.
    void UpdateBestScore()
    {
        if (GetBestScore() < score) //최고점수를 가져왔는데 현재 점수보다 낮다면, (즉 현재 점수가 최고점수보다 높다면)
        {
            PlayerPrefs.SetInt("BestScore", score); //플레이어 프레퍼런스에 Key-Value 값을 저장하거나 갱신한다. 괄호에는 ("키 이름", 값) 을 넣는다.

        }
    }

    int GetBestScore() //최고점수를 가져오는 함수
    {
        int bestScore = PlayerPrefs.GetInt("BestScore"); //플레이어 프레퍼런스에 기록된 값을 키를 통해 불러온다. 아무것도 저장되어 있지 않다면 0을 가져온다.
        return bestScore; //가져온 점수를 반환한다.
    }

    void UpdateUI() //불러온 텍스트 컴포넌트의 텍스트를 바꾼다.
    {
        scoreText.text = "Score: " + score;
        bestScoreText.text = "Best Score: " + GetBestScore();
    }

    public void BallDestroy() //공이 폭발하면 발동시켜 라운드를 종료상태로 보내는 함수. Ball 오브젝트에서 발동시킬 것이므로 public으로 선언. 
    {
        UpdateUI(); //마지막으로 UI를 현재점수로 갱신하고
        isPlaying = false; //플레이 중이 아님을 알린다.
    }

    public void Reset() //라운드가 바뀔 때마다 실행시킬 함수
    {
        score = 0; //점수 초기화
        UpdateUI(); //UI에 초기화된 점수 반영
        StartCoroutine("RoundRoutine"); //라운드 관리 함수 리셋마다 실행.
    }

    IEnumerator RoundRoutine() //라운드 상태에 따라 변화를 준다. 게임 시작 시마다 실행해주면 라운드 관리를 담당하게 된다.
    {
        //페이즈 1: 대기 상태

        onReset.Invoke();

        readyPannel.SetActive(true); //대기 상태 패널 출력
        cam.SetTarget(shooterRotator.transform, CamFollower.State.Idle); //cam 스크립트의 카메라 위치 조작 함수를 통해, 카메라 타겟을 막대에 맞추고 idle 상태로 만든다.
        shooterRotator.enabled = false; //대기상태에서 조작하지 못 하게 이 함수는 꺼준다.
        isPlaying = false; //플레이 중이 아님

        messageText.text = "Loading..."; //화면에 표시할 글자 입력

        yield return new WaitForSeconds(3f); //이 상태로 3초 대기

        //페이즈 2: 플레이 중

        isPlaying = true;
        readyPannel.SetActive(false); //대기상태 패널을 끈다.
        shooterRotator.enabled = true; //조작 가능하도록 조작 스크립트 활성화. 이 때 ShooterRotator 함수의 OnEabled() 함수가 자동 실행된다.

        cam.SetTarget(shooterRotator.transform, CamFollower.State.Ready); //카메라 타겟은 그대로이되 상태를 준비상태로 변경한다. (준비상태에서 발동하는 줌인이 동작함)

        while (isPlaying == true) //플레이 중인 한
        {
            yield return null; //무한루프
        }

        //페이즈 3: 종료
        
        shooterRotator.enabled = false; //조작 불가하게 변경
        
        yield return new WaitForSeconds(3f); //3초의 대기시간 후
        Reset(); //리셋

    }

    void Start()
    {
        UpdateUI();
        StartCoroutine("RoundRoutine"); //라운드 관리 코루틴 실행.
    }

    
    void Update()
    {
        
    }
}
