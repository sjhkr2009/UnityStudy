using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F04_AnimationEventTest : MonoBehaviour
{
    //애니메이션 이벤트: 애니메이션의 특정 지점에서 이벤트를 뿌리는 것

    // public이 아니라도 인스펙터 창에서 선택 가능. 애니메이션이 재생되는 오브젝트에 이 스크립트가 컴포넌트로 붙어 있어야 적용 가능하다.
    void CubeEvent()
    {
        Debug.Log("큐브 회전 시작");
    }

    // 캐릭터가 걷는 애니메이션에서 칼을 휘두를 때 피해를 입히거나, 땅에 발이 닿을 때마다 이벤트로 발자국 소리를 내 주는 등으로 이용할 수 있다.
    // 여기서는 플레이어가 달릴 때 땅에 발이 닿으면 이벤트를 발동시켜 본다. 아래 함수는 유니티짱에서 발동된다.
    // (이벤트 설정은 애니메이션 'unitychan_RUN00_F' 파일 클릭 후 인스펙터 창 하단 Event 탭에서 설정할 수 있다. Function의 함수명과 일치해야 하며, 필요에 따라 인자를 넘겨줄 수 있다.)

    int walkCount = 0;
    void OnRunEvent(int walk)
    {
        walkCount += walk;
        Debug.Log($"뚜벅뚜벅... ({walkCount} 걸음째)");
    }
}
