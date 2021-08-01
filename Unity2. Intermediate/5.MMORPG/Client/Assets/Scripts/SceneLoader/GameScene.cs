using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init() {
        base.Init();
        SceneType = Scene.Game;
        
        Director.Map.LoadMap(1);
    }

    public override void Clear() {
        
    }
}
