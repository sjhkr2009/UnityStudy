using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetEvent : MonoBehaviour
{
    
    
    private void Awake()
    {
        //Start 보다 먼저 실행
        //(현재 오브젝트에 붙어 있는 컴포넌트 등) 이 오브젝트에 관한 내용 선언 
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }


    private void LateUpdate()
    {
        //카메라 관련 스크립트 넣기
        //카메라가 Update에 있으면, 한 오브젝트의 위치만 변하고 다른 오브젝트가 변하기 전에 카메라로 해당 장면을 내보냄으로써, 보이는 것과 실제 위치가 다르게 보일 수 있다.
        //추적 중인 오브젝트가 먼저 앞으로 이동하고 카메라가 한 프레임 늦게 따라가는 것으로 보일 수도 있음.
    }

    private void OnBecameVisible()
    {
        //오브젝트가 화면상에 보일 때 실행. 보이지 않게 되면 OnBecameInvisible() 실행.
        Debug.Log("In");
    }
    private void OnBecameInvisible()
    {
        Debug.Log("Out");
    }

    private void OnApplicationPause(bool pause)
    {
        //일시정지 시 실행.
        //모바일에서는 Home키, PC에선 전체화면으로 플레이 중 다른 창으로 이동할 경우 자동으로 일시정지 상태가 된다.
        //일반적으로 이 때 자동으로 저장을 한다.
    }

#if UNITY_EDITOR //전처리. 유니티 에디터 내에서만 실행하며, 빌드할 때 이 부분은 삭제한다.
    private void OnDrawGizmos()
    {
        //기즈모 만들 때 
        //게임에 영향은 없으나 빌드하면 계속 실행되어 리소스를 낭비하므로, 전처리를 해 준다.
    }
#endif
}
