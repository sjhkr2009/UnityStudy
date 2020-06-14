using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A01_Managers : MonoBehaviour
{
    // 매니저 클래스와 싱글톤 패턴

    // 유니티 사용 시, 컴포넌트처럼 쓸 스크립트와 그렇지 않은 스크립트로 구분하는 것이 좋다.
    // 오브젝트에 붙일 수 있는 모든 클래스는 Component를 상속받는다.

    // 다른 오브젝트나 컴포넌트 동작을 제어하는 매니저 클래스는 보통 빈 오브젝트에 붙여놓는다.

    // 매니저를 어디서든 호출할 수 있도록 하기 위해 싱글톤 패턴이 사용된다.

    static A01_Managers _instance;

    private void Start()
    {
        
    }
}
