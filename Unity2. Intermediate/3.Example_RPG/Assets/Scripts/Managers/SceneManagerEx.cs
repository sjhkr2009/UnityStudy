using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx : SceneManager
{
    public BaseScene CurrentScene => Object.FindObjectOfType<BaseScene>();

	public void LoadScene(Define.Scene type)
	{
		GameManager.Clear();
		LoadScene(GetSceneName(type));
	}

	string GetSceneName(Define.Scene type)
	{
		return System.Enum.GetName(typeof(Define.Scene), type);
	}

	public void Clear()
	{
		CurrentScene.Clear();
	}
}
