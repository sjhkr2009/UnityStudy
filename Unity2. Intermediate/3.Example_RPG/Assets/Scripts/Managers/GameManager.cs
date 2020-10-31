using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	static GameManager _instance;
	public static GameManager Instance { get { Init(); return _instance; } }

	InputManager _input = new InputManager();
	ResourceManager _resource = new ResourceManager();
	UiManager _ui = new UiManager();
	SceneManagerEx _scene = new SceneManagerEx();
	SoundManager _sound = new SoundManager();

	public static InputManager Input => Instance._input;
	public static ResourceManager Resource => Instance._resource;
	public static UiManager UI => Instance._ui;
	public static SceneManagerEx Scene => Instance._scene;
	public static SoundManager Sound => Instance._sound;

	private void Start() => Init();
	static void Init()
	{
		if (_instance != null)
			return;

		GameObject go = GameObject.Find(Define.DefaultName.GameManager);
		if (go == null)
		{
			go = new GameObject(Define.DefaultName.GameManager);
			go.AddComponent<GameManager>();
		}

		DontDestroyOnLoad(go);
		_instance = go.GetComponent<GameManager>();

		_instance._sound.Init();
	}

	public static void Clear()
	{
		Sound.Clear();
		Input.Clear();
		Scene.Clear();
		UI.Clear();
	}

	public void Update()
	{
		Input.OnUpdate();
	}

	
}
