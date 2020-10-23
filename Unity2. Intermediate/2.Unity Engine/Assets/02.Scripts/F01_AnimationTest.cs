using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F01_AnimationTest : MonoBehaviour
{
    Animator anim;

    Vector3 prevPos;
    float blendValue;

    private void Start()
    {
        anim = GetComponent<Animator>();
        prevPos = transform.position;
    }
    private void Update()
    {
        // 이전 좌표와 현재 좌표에 차이가 있으면 캐릭터가 움직인 것으로 간주하여 달리기 모션을 실행한다.
        if ((transform.position - prevPos).magnitude >= Time.deltaTime * 1f)
        {
            SetMove();
        }
        else
        {
            SetWait();
        }
        prevPos = transform.position;

        //이런 식으로 애니메이션마다 if문으로 상태를 체크하면, 수십 개의 애니메이션이 추가될 경우 관리하기가 매우 어렵다.
        //따라서 애니메이션은 State 패턴을 주로 사용한다. 이는 F02 스크립트 참고.
    }
    void SetMove()
    {
        if (blendValue < 0.999f) blendValue = Mathf.Min(Mathf.Lerp(blendValue, 1f, 10 * Time.deltaTime), 0.999f);
        anim.SetFloat("WAIT_RUN", blendValue);
        anim.Play("WAIT_RUN");
        //anim.Play("RUN");
    }

    void SetWait()
    {
        if (blendValue > 0.001f) blendValue = Mathf.Max(Mathf.Lerp(blendValue, 0f, 10 * Time.deltaTime), 0.001f);
        anim.SetFloat("WAIT_RUN", blendValue);
        anim.Play("WAIT_RUN");
        //anim.Play("WAIT");
    }
}
