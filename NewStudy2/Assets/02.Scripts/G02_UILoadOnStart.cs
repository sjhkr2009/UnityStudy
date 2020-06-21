using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G02_UILoadOnStart : MonoBehaviour
{
    // 캔버스는 보통 프리팹으로 만들어서, 해당 UI가 필요할 때 띄우는 방식으로 사용된다.
    // 프리팹 폴더에 UI를 저장할 공간을 만든 후 넣어두고, 리소스 매니저를 통해 불러올 수 있다.
    void Awake()
    {
        A01_Manager.Resource.Instantiate("UI/G01_UI_Button");
    }

}
