using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class A01_Manager : MonoBehaviour
{
    // 매니저 클래스와 싱글톤 패턴

    // 유니티 사용 시, 컴포넌트처럼 쓸 스크립트와 그렇지 않은 스크립트로 구분하는 것이 좋다.
    // 오브젝트에 붙일 수 있는 모든 클래스는 Component를 상속받는다.

    // 다른 오브젝트나 컴포넌트 동작을 제어하는 매니저 클래스는 보통 빈 오브젝트에 붙여놓는다.

    // 매니저가 여럿 존재하지 않도록 하면서, 매니저를 어디서든 호출할 수 있도록 하기 위해 싱글톤 패턴이 사용된다.

    static A01_Manager _instance; // 유일성이 보장된다.
    public static A01_Manager Instance { get { Init(); return _instance; } } //유일한 매니저를 다른 곳에서 가져올 수 있게 한다.

    private void Awake()
    {
        Init();
    }

    static void Init()
    {
        if(_instance == null)
        {
            GameObject obj = GameObject.Find("GameManager");
            if(obj == null)
            {
                obj = new GameObject("GameManager");
                obj.AddComponent<A01_Manager>();
            }

            DontDestroyOnLoad(obj);
            _instance = obj.GetComponent<A01_Manager>();
        }
    }

    //----------------------------------------------------------------------------------

    // 다른 매니저 클래스 가져오기
    B02_InputManager _inputManager = new B02_InputManager();
    public static B02_InputManager InputManager => Instance._inputManager;
    // 그냥 _inputManager가 아니라 Instance._inputManager를 호출함으로써, 이 매니저를 호출하지 않았더라도 Init()이 발동될 것이다.

    private void Update()
    {
        _inputManager.OnUpdate();   // 매니저 클래스에서 한 번만 키 입력을 체크하면 된다.
    }


}
