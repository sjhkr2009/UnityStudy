using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    //Prop에 필요한 기능
    // 1. HP를 가지고 있으며, 폭발에 의해 피해를 받고, HP가 0이 되면 파괴된다.
    // 2. 파괴되었다는 사실을 알려 점수가 오르게 한다.

    public int score = 5; //파괴 시 올라갈 점수
    public ParticleSystem explosionParticle; //나중에 파티클을 프리팹에서 넣어줘서 새로 생성되게 할 것임.
    public float hp = 10f; //체력

    public void TakeDamage(float damage) //피해를 주는 함수. 피해를 주는 쪽에서 발동시켜야 하므로 public으로 선언하며, 데미지 변수를 받는다.
    {
        hp -= damage; //받은 데미지 변수를 체력에서 뺀다.

        if (hp <= 0) //체력이 0 이하가 되면
        {
            ParticleSystem instance = Instantiate(explosionParticle, transform.position, transform.rotation); //파티클(파괴 시 효과)을 생성한다.
            //괄호에는 (생성할 오브젝트, 생성 위치, 회전) 이 들어가며, 위치와 회전은 선택사항이나 여기선 이 오브젝트와 같은 위치와 회전으로 폭발 효과가 나도록 정해준다. (안 정하면 원점 혹은 랜덤)
            instance.Play();

            AudioSource expAudio = instance.GetComponent<AudioSource>(); //생성된 파티클의 컴포넌트 중 오디오를 이 스크립트로 불러온다. (Play an Awake를 껐으니 수동으로 재생시켜줘야 함)
            expAudio.Play(); //오디오 재생

            GameManager.instance.AddScore(score); //파괴되기 전에 게임 매니저의 점수 증가 함수를 발동시킨다.

            Destroy(instance.gameObject, instance.duration); //"파괴한다 (이 파티클 효과를, 파티클 재생이 끝나면)"
            gameObject.SetActive(false); //이 오브젝트는 게임 재시작마다 수십 개씩 생성되므로, 파괴/생성은 렉이 많이 걸리니 SetActive로 켜고 끄는 방식을 택한다.
        }
    }
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}
