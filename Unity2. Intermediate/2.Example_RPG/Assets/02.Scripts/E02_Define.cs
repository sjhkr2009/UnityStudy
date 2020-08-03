using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E02_Define
{
    // 카메라 모드를 비롯한 여러 모드 종류를 저장할 스크립트. 컴포넌트로 붙일 건 아니니 MonoBehavior는 불필요하다
    public enum CameraMode
    {
        QuarterView
    }
    public enum MouseEvent
    {
        Press,
        Click
    }

    // UI의 버튼 동작을 입력하는 enum 값
    // 기타 다른 여러 모드를 선택하는 enum 값들도 Define에 저장한다.

    public enum EventType
    {
        Click,
        Drag
    }

    public enum Scene
    {
        Unknown,
        H02_Login,
        Lobby,
        H01_Game
    }

    public enum Sound
    {
        BGM,
        Effect,
        AtPoint,
        Count
    }
}
