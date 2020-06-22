using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    //빌보드
    //풀과 같은 오브젝트들을 3D로 렌더링하지 않고, 텍스처로 구현한다.
    //해당 텍스쳐의 회전은 플레이어의 카메라 방향과 동일하게 유지해야 한다.
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
