using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
	protected override void Init()
	{
		base.Init();

		SceneType = Define.Scene.Game;

		//GameManager.UI.ShowSceneUI<UI_Inventory>();
		gameObject.GetOrAddComponent<CursorController>();
	}

	public override void Clear()
	{
		
	}
}
