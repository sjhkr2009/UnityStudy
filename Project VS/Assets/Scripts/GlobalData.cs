using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData {
    // TODO: 싱글톤들 GameManager로 이동, 외부에서 값을 직접 변경하지 못 하게 readonly로 정보를 반환하게 만들 것
    public static PlayerController Player { get; set; }
    public static GameController Controller { get; set; }
}
