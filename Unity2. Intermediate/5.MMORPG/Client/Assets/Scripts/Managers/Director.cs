using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour {
	static Director _instance;

	public static Director Instance {
		get {
			Init();
			return _instance;
		}
	}

	#region Common

	private DataManager _data = new DataManager();
	private InputManager _input = new InputManager();
	private PoolManager _pool = new PoolManager();
	private ResourceManager _resource = new ResourceManager();
	private UiManager _ui = new UiManager();
	private SceneManagerEx _scene = new SceneManagerEx();
	private SoundManager _sound = new SoundManager();

	public static DataManager Data => Instance._data;
	public static InputManager Input => Instance._input;
	public static PoolManager Pool => Instance._pool;
	public static ResourceManager Resource => Instance._resource;
	public static UiManager UI => Instance._ui;
	public static SceneManagerEx Scene => Instance._scene;
	public static SoundManager Sound => Instance._sound;

	#endregion

	#region Contents

	private MapManager _map = new MapManager();
	private ObjectManager _object = new ObjectManager();
	private NetworkManager _network = new NetworkManager();

	public static MapManager Map => Instance._map;
	public static ObjectManager Object => Instance._object;
	public static NetworkManager Network => Instance._network;

	#endregion

	void Start() => Init();

	static void Init() {
		if (_instance != null)
			return;

		GameObject go = GameObject.Find(Define.DefaultName.GameManager);
		if (go == null) {
			go = new GameObject(Define.DefaultName.GameManager);
			go.AddComponent<Director>();
		}

		DontDestroyOnLoad(go);
		_instance = go.GetComponent<Director>();

		_instance._data.Init(); // First Order
		_instance._pool.Init();
		_instance._sound.Init();
		_instance._network.Init();
	}

	public static void Clear() {
		Sound.Clear();
		Input.Clear();
		Scene.Clear();
		UI.Clear();

		// Last Order
		Pool.Clear();
	}

	public void Update() {
		Input.OnUpdate();
		_network.Init();
	}
}
