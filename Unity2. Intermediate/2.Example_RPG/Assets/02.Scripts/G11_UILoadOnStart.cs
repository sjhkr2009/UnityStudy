using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G11_UILoadOnStart : MonoBehaviour
{
    // UI 호출 테스트용
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            A01_Manager.UI.OpenPopupUI<G03_UIBinder>();
        }
    }
}
