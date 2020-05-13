using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    float xMove;
    float yMove;
    float isGrounded = 1;
    public static float xSpeed = 3;
    public static float eventSpeed = 2;
    public static float jump = 6.6f;
    public static float enemySpeed = 0.7f;
    public int stage = 1;
    public static int life = 3;
    public static float score = 0;
    int count = 0;
    bool nohit = false;
    bool noscore = false;
    public static int eventtime = 0;
    public static int erdascore = 100;
    public static int minierdascore = 50;
    public static int skillcount = 1;
    public GameObject Erda;
    public GameObject Player;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Transform tr;
    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //키보드 조작
        xMove = Input.GetAxis("Horizontal");
        transform.Translate(xMove * xSpeed * Time.deltaTime, 0, 0);

        //이동 방향에 따라 바라보는 방향을 변경.
        //이유는 모르겠지만 스테이지 1(노말)에서 스케일이 1로 적용되어 있어서 if문으로 스테이지별 크기를 상이하게 설정했습니다. 원래 둘 다 0.35 였는데...
        //그래서 기왕 스테이지별 if문 쓰게 된 김에 스테이지 1에서만 걷기 모션 적용. (2스테이지는 날아다니므로)
        if (stage == 1)
        {
            if (xMove > 0)
            {
                tr.localScale = new Vector3(-1, 1, 0);
                //키보드에서 손을 떼도 1초쯤 있다가 걷기를 멈추는 것이 어색하여, 키 입력이 일정 이상 될 때만 걷기 애니메이션이 나오도록 설정
                //키보드 조작을 GetAxisRaw로 사용하는 게 애니메이션 측면에선 더 자연스럽지만, 움직임에 생동감이 없어 포기했습니다.
                if (xMove > 0.4f)
                {
                    anim.SetBool("ismoving", true);
                } else
                {
                    anim.SetBool("ismoving", false);
                }
            }
            else if (xMove < 0)
            {
                tr.localScale = new Vector3(1, 1, 0);
                if (xMove < -0.4f)
                {
                    anim.SetBool("ismoving", true);
                }
                else
                {
                    anim.SetBool("ismoving", false);
                }
            }
        } else if (stage == 2)
        {
            if (xMove > 0)
            {
                tr.localScale = new Vector3(-0.35f, 0.35f, 0);
            }
            else if (xMove < 0)
            {
                tr.localScale = new Vector3(0.35f, 0.35f, 0);
            }
        }

        //이벤트 발생 시 조작방법이 변경되며, 이벤트 종료 시마다 일부 변수가 조정된다. (자세한 설명은 아래에서)
        if (stage == 2)
        {
            StartCoroutine("EventMove");
        }
        
        //점프
        if (isGrounded > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
                isGrounded = isGrounded - 1;
                StartCoroutine("Jumpanim");
            }
        }

        //목표 오브젝트를 카운트 이상 습득하면 이벤트 발생 (라이프 증가는 코루틴에 넣으니 2개씩 증가하여 if문에 삽입했습니다.)
        if (count > 9)
        {
            count = 0;
            if (life < 5)
            {
                life++;
            }
            SceneManager.LoadScene("Scene_event");
        }

        //왼쪽 Ctrl로 3초간 무적 스킬 사용, 한번 사용하면 이벤트 발생 전에 다시 사용할 수 없음.
        float untouchable = Input.GetAxisRaw("Fire1");
        if (untouchable == 1)
        {
            if (skillcount > 0)
            {
                skillcount = 0;
                StartCoroutine("Skill");
            }
        }

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        //땅에 닿을 때 점프 가능하게 하기
        if (collision.gameObject.tag == "floor")
        {
            if (rb.velocity.y == 0)
            {
                isGrounded = 1;
                anim.SetBool("isjump", false);
            }

        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    { 
        //오브젝트에 닿을 때 오브젝트 제거 및 점수 획득
        if (collision.gameObject.tag == "score")
        {
            if (noscore == false)
            {
                Destroy(collision.gameObject);

                if (stage == 1)
                {
                    StartCoroutine("GetErda");
                }
                else if (stage == 2)
                {
                    StartCoroutine("GetMiniErda");
                }
            }

        }
        //적에게 닿을 경우 라이프 감소, 라이프가 0이 되면 게임오버
        if (collision.gameObject.tag == "enemy")
        {
            if (nohit == false)
            {
                if (life > 1)
                {
                    life = life - 1;
                    StartCoroutine("Attacked");
                }
                else
                {
                    SceneManager.LoadScene("Gameover");
                }

            }
        }
    }
    //애니메이터의 Loop를 꺼도 계속 공중에서 점프 모션을 반복하길래, 원인을 알 수 없어 그냥 잠시 후 점프가 꺼지는 스크립트를 추가했습니다.
    //검색해보니 'Has Exit Time'을 체크하면 한 번은 동작을 하고 애니메이션이 변경된다고 해서, 점프가 끊기지 않도록 이 트랜지션에만 체크해 주었습니다.
    IEnumerator Jumpanim()
    {
        anim.SetBool("isjump", true);
        yield return new WaitForSeconds(0.05f);
        anim.SetBool("isjump", false);
    }
    //순식간에 게임오버되는 것을 방지하기 위해, 피격당했을 때 1초간 무적이 되며 빨간색으로 깜박거리게 함
    IEnumerator Attacked()
    {
        nohit = true;
        float nohit_time = 0;
        while (nohit_time < 1)
        {
            sr.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.05f);
            sr.color = new Color(1, 0, 0, 0.8f);
            yield return new WaitForSeconds(0.05f);
            nohit_time = nohit_time + 0.1f;
        }
        sr.color = new Color(1, 1, 1, 1);
        nohit = false;
    }
    //좌측 Ctrl 버튼을 누르면 3초간 무적
    IEnumerator Skill()
    {
        nohit = true;
        float nohit_time = 0;
        sr.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(2f);
        while (nohit_time < 1)
        {
            sr.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.05f);
            sr.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.05f);
            nohit_time = nohit_time + 0.1f;
        }
        sr.color = new Color(1, 1, 1, 1);
        nohit = false;
    }
    //오브젝트 획득 시 점수와 카운트가 증가한다. 이벤트 맵과 아닐 때의 점수 증가량이 상이하다.
    IEnumerator GetErda()
    {
        noscore = true;
        score = score + erdascore;
        count++;
        float x = Random.Range(-7.4f, 7.4f);
        float y = Random.Range(-4.5f, 4.3f);
        Instantiate(Erda, new Vector3(x, y, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.01f);
        noscore = false;
    }
    IEnumerator GetMiniErda()
    {
        noscore = true;
        score = score + minierdascore;
        yield return new WaitForSeconds(0.01f);
        noscore = false;
    }
    //이벤트 모드에 진입하면 점프 대신 상하좌우 이동이 가능해지며, 별도의 이동속도를 가지고 무중력 상태가 된다.
    //이벤트 종료 후 획득하는 점수량, 중력과 점프력, 이동속도, 적 이동속도가 증가하고, 스킬 사용이 1회 가능해진다.
    //이벤트 맵 진입 후 8초 후 자동으로 원래 맵으로 되돌아간다.
    IEnumerator EventMove()
    {
        xMove = Input.GetAxisRaw("Horizontal");
        yMove = Input.GetAxisRaw("Vertical");
        eventtime++;
        rb.gravityScale = 0f;
        isGrounded = 0;
        transform.Translate(xMove * eventSpeed * Time.deltaTime, yMove * 1.5f * eventSpeed * Time.deltaTime, 0);
        yield return new WaitForSeconds(8f);
        rb.gravityScale = 1.5f + 0.2f * eventtime;
        xSpeed = xSpeed + 0.15f;
        enemySpeed = enemySpeed + 0.065f;
        jump = jump + 0.07f;
        eventSpeed = eventSpeed + 0.3f;
        transform.Translate(xMove * xSpeed * Time.deltaTime, 0, 0);
        skillcount = 1;
        erdascore = erdascore + 5;
        minierdascore = minierdascore + 2;
        SceneManager.LoadScene("Scene_normal");
    }
    //좌측 상단에 점수 표시, 우측 상단에 라이프 표시. 가독성을 위해 일반 맵과 이벤트 맵에서의 색상을 다르게 설정.
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        if (stage == 1)
        {
            style.normal.textColor = Color.white;
        }
        else if (stage == 2)
        {
            style.normal.textColor = Color.black;
        }
        style.fontSize = 16;
        GUI.Label(new Rect(1, 1, 300, 50), "Score: " + score, style);
        GUI.Label(new Rect(1040, 1, 300, 50), "Life", style);
    }
}