using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class A01_Manager : MonoBehaviour
{
    // 매니저 클래스와 싱글톤 패턴

    // 유니티 사용 시, 컴포넌트처럼 쓸 스크립트와 그렇지 않은 스크립트로 구분하는 것이 좋다.
    // 오브젝트에 붙일 수 있는 모든 클래스는 Component를 상속받는다.

    // 다른 오브젝트나 컴포넌트 동작을 제어하는 매니저 클래스는 보통 빈 오브젝트에 붙여놓는다.

    // 매니저가 여럿 존재하지 않도록 하면서, 매니저를 어디서든 호출할 수 있도록 하기 위해 싱글톤 패턴이 사용된다.

    static A01_Manager _instance; // 유일성이 보장된다.
    //public static A01_Manager Instance { get { Init(); return _instance; } } //유일한 매니저를 다른 곳에서 가져올 수 있게 한다.
    public static A01_Manager Instance => _instance;

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
            _instance = obj.GetComponent<A01_Manager>();

            DontDestroyOnLoad(obj);

            //Init 내에서 프로퍼티인 Instance를 호출하지 않도록 주의한다.
            _instance._sound.Init();
            _instance._pool.Init();
            _instance._data.Init();
        }
    }

    //----------------------------------------------------------------------------------

    // 다른 매니저 클래스 가져오기
    B02_InputManager _input = new B02_InputManager();
    public static B02_InputManager Input => Instance._input;
    // Instance._input을 호출함으로써, 이 매니저를 호출하지 않았더라도 Init()이 발동될 것이다.

    L01_DataManager _data = new L01_DataManager();
    C02_ResourceManager _resource = new C02_ResourceManager();
    G10_UIManager _ui = new G10_UIManager();
    H04_SceneManagerEx _scene = new H04_SceneManagerEx();
    I01_SoundManager _sound = new I01_SoundManager();
    J01_PoolManager _pool = new J01_PoolManager();

    public static L01_DataManager Data => Instance._data;
    public static C02_ResourceManager Resource => Instance._resource;
    public static G10_UIManager UI => Instance._ui;
    public static H04_SceneManagerEx Scene => Instance._scene;
    public static I01_SoundManager Sound => Instance._sound;
    public static J01_PoolManager Pool => Instance._pool;

    private void Update()
    {
        Input.OnUpdate();   // 매니저 클래스에서 한 번만 키 입력을 체크하면 된다.
    }


    // 저장중인 사운드나 UI정보, 이벤트 등 씬에 종속된 정보를 초기화한다. 씬 매니저에서 씬이 바뀔 때 실행한다.
    public static void Clear()
    {
        Input.Clear();
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear(); // 오브젝트 제거는 일반적으로 맨 마지막에 넣어준다.
    }

}
