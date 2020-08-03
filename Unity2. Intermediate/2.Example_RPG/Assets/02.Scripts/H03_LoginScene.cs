using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class H03_LoginScene : H01_BaseScene
{
    public override void Clear()
    {
        Debug.Log("씬 종료");
    }

    protected override void Init()
    {
        base.Init();

        SceneType = E02_Define.Scene.H02_Login;
    }

    public void LoadGameScene()
    {
        A01_Manager.Scene.LoadScene(E02_Define.Scene.H01_Game);
    }
}
