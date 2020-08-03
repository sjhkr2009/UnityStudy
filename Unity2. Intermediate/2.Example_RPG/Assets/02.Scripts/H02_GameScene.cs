using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H02_GameScene : H01_BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = E02_Define.Scene.H01_Game;

        A01_Manager.UI.OpenPopupUI<G03_UIBinder>();

    }
    public override void Clear()
    {
        throw new System.NotImplementedException();
    }
}
