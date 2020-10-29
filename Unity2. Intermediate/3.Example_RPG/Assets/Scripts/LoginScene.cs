using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
	protected override void Init()
	{
		base.Init();

		SceneType = Define.Scene.Login;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
			GameManager.Scene.LoadScene(Define.Scene.Game);
	}

	public override void Clear()
	{
		Debug.Log("Login Scene Cleared!");
	}
}
