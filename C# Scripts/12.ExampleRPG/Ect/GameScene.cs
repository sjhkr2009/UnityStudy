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

		GameManager.Game.Spawn(Define.ObjectType.Player, "UnityChan");
		GameManager.Game.Spawn(Define.ObjectType.Monster, "Knight");

		Camera.main.gameObject.GetOrAddComponent<CameraController>().SetCameraPos();
		SpawningPool pool = gameObject.GetOrAddComponent<SpawningPool>();
		pool.SetMaxCount(5);
	}

	public override void Clear()
	{
		
	}
}
