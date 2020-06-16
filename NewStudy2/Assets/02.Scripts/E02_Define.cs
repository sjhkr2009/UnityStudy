using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E02_Define
{
    // 카메라의 여러 모드를 저장할 스크립트. 컴포넌트로 붙일 건 아니니 MonoBehavior는 불필요하다.

    public enum CameraMode
    {
        QuarterView
    }
    public enum MouseEvent
    {
        Press,
        Click
    }
}
