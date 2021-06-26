using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
	static Director _instance;
	public static Director Instance { get { Init(); return _instance; } }

	DataManager _data = new DataManager();
	InputManager _input = new InputManager();
	PoolManager _pool = new PoolManager();
	ResourceManager _resource = new ResourceManager();
	UiManager _ui = new UiManager();
	SceneManagerEx _scene = new SceneManagerEx();
	SoundManager _sound = new SoundManager();

	public static DataManager Data => Instance._data;
	public static InputManager Input => Instance._input;
	public static PoolManager Pool => Instance._pool;
	public static ResourceManager Resource => Instance._resource;
	public static UiManager UI => Instance._ui;
	public static SceneManagerEx Scene => Instance._scene;
	public static SoundManager Sound => Instance._sound;


	void Start() => Init();
	static void Init()
	{
		if (_instance != null)
			return;

		GameObject go = GameObject.Find(Define.DefaultName.GameManager);
		if (go == null)
		{
			go = new GameObject(Define.DefaultName.GameManager);
			go.AddComponent<Director>();
		}

		DontDestroyOnLoad(go);
		_instance = go.GetComponent<Director>();

		_instance._data.Init(); // First Order
		_instance._pool.Init();
		_instance._sound.Init();
	}

	public static void Clear()
	{
		Sound.Clear();
		Input.Clear();
		Scene.Clear();
		UI.Clear();

		// Last Order
		Pool.Clear();
	}

	public void Update()
	{
		Input.OnUpdate();
	}

	
}
