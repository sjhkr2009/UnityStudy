using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx : SceneManager
{
	private BaseScene _currentScene;
	public BaseScene CurrentScene
	{
		get
		{
			if (_currentScene == null)
				_currentScene = Object.FindObjectOfType<BaseScene>();

			return _currentScene;
		}
	}

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
		_currentScene = null;
	}
}
