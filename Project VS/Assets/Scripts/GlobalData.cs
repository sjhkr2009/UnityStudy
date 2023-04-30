using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData {
    public static PlayerController Player { get; set; }
    
    // TODO: Enemy에서 직접 호출하는 대신 이벤트를 통해 호출하도록 변경
    public static GameController Controller { get; set; }
}
