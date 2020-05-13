using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //'접근지정자 컴포넌트명 변수'로 다른 컴포넌트를 불러온다. 변수는 public으로 선언한 후 해당 컴포넌트를 가진 오브젝트를 드래그 앤 드롭.
    public ParticleSystem explosionParticle;
    public AudioSource explosionAudio;


    //공이 폭발할 때 주위 물건들에 피해를 주고 밀어내며, 0이 되면 오브젝트가 파괴되도록 만들 것이다.
    public float maxDamage = 100f; //최대 피해량
    public float explosionForce = 1000f; //주변 물체를 밀어내는 힘
    public float explosionRadius = 20f; //폭발 반경

    //공이 맵 바깥으로 날아가 영원히 폭발하지 않는 경우 방지
    public float lifeTime = 10f; //10초간 파괴되지 않으면 자동으로 사라지게 설정할 예정

    //폭발 반경만한 크기의 가상의 원을 그려서, 닿은 Prop들에 데미지를 주고 밀어내야 한다.
    //이 때, 닿은 대상이 Prop인지 다른 대상인지 구분해야 한다. 구분 방법엔 tag와 layer가 있다. (tag는 '만약 A라면' 같은 1:1비교만 가능, layer는 'A 또는 B,C,D' 같은 여러 개의 필터링 가능)
    public LayerMask whatIsProp; //LayerMask: 원하는 레이어만 선택하게 해 준다. tag가 아닌 layer이므로 유니티에서 여러 개의 선택이 가능하다. (단, 여기서는 prop만 선택할 것이다)

    void Start()
    {
        //Destroy(대상,지연시간 n): 대상 오브젝트를 파괴한다. 지연시간이 있다면 n초 후에, 없다면 즉시 파괴한다. (지연시간은 선택사항)
        Destroy(gameObject, lifeTime); //이 오브젝트를 10초 후에 삭제시킨다.
    }

    private void OnTriggerEnter(Collider other) //공이 바닥에 충돌하면
    {
        //폭발 반경만한 크기로 가상의 구를 만든다. 모든 물리 처리는 Physics를 사용한다.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, whatIsProp);
        //OverlapSphere(위치, 반지름, 레이어 필터링): 해당 위치에 가상의 구를 생성해, 범위 내의 오브젝트를 모두 가져온다. 레이어 필터링은 선택사항으로, 해당 레이어의 오브젝트만 가져온다.

        for (int i = 0; i < colliders.Length; i++) // (.Length는 개수를 세는 기능. 파이썬의 .len() 과 비슷함.)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>(); //해당 오브젝트(Prop)의 Rigidbody를 불러온다.

            targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius); //.AddExplosionForce(힘, 폭심, 반경): 폭발에 의한 힘을 계산하여 자동으로 적용시켜준다. 

            Prop targetProp = colliders[i].GetComponent<Prop>(); //해당 오브젝트의 Prop 함수를 가져온다.

            float damage = CalcDamage(colliders[i].transform.position); //데미지 계산 함수에 오브젝트의 위치를 입력하여, 데미지를 계산한다.
            targetProp.TakeDamage(damage); //해당 오브젝트에 적용된, 데미지를 받는 함수에 damage를 넣고 작동시킴.
        }


        explosionParticle.transform.parent = null; //파티클 효과가 파괴되지 않도록, 파티클의 부모 오브젝트(현재는 공)를 없앤다.

        explosionParticle.Play();
        explosionAudio.Play();

        GameManager.instance.BallDestroy(); //게임 매니저에 있는, '공 파괴 시 실행할 함수'를 실행시킨다.

        //파티클이 재생된 후에도 계속 메모리에 남아 있으므로, 재생이 끝나면 파괴해 준다.
        Destroy(explosionParticle.gameObject, explosionParticle.duration); //파티클을 가진 오브젝트 파괴, 지연시간은 파티클의 재생시간만큼.
        Destroy(gameObject);
    }

    float CalcDamage(Vector3 targetposition) //위치에 따른 데미지를 계산하는 함수(해당 오브젝트의 위치 입력)
    {
        Vector3 expToTarget = targetposition - transform.position; //오브젝트의 위치 - 폭발 중심지 위치의 Vector3 값

        float distanceFromMiddle = expToTarget.magnitude; //해당 벡터값을 거리로 환산 (magnitude: 벡터를 길이로 변환)
        //여기서 distnaceFromMiddle은 폭발 중심에서 대상까지 거리

        float distance = explosionRadius - distanceFromMiddle; //distance는 폭발 외곽에서 대상까지 거리. 즉 짧을수록 피해를 적게 받음.
        float percentage = distance / explosionRadius; //외곽에서 떨어진 수치/폭발 반경 -> 0~1 사이의 수(0%~100%)로 변환
        float damage = maxDamage * percentage; //해당 퍼센트에 비례한 데미지 환산

        damage = Mathf.Max(0, damage); // Mathf.Max(a,b): a,b 중 더 큰 값을 반환.
        //거리가 폭발 반경보다 더 떨어져 있는데 살짝 겹친 경우, 체력이 오히려 회복되는 상황 방지하기 위해 데미지는 0 미만이 되지 않게 한다.

        return damage; //데미지를 반환
    }
}
