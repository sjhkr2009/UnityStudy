using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define.Default;

public class GameManager : MonoBehaviour
{
	static GameManager _instance;
	public static GameManager Instance { get { Init(); return _instance; } }

	private ResourceManager _resource = new ResourceManager();
	private InputManager _input = new InputManager();
	private CameraManager _camera = new CameraManager();
	public static ResourceManager Resource => Instance._resource;
	public static InputManager Input => Instance._input;
	public static CameraManager Camera => Instance._camera;


	static void Init()
	{
		if (_instance != null)
			return;

		GameObject go = GameObject.Find(Name.GameManager);
		if (go == null)
		{
			go = new GameObject(Name.GameManager);
			go.AddComponent<GameManager>();
		}

		DontDestroyOnLoad(go);
		_instance = go.GetComponent<GameManager>();
	}

	public static void Clear()
	{
		Input.Clear();
	}

	private void Awake()
	{
		Init();
		if (_instance != this)
		{
			DestroyImmediate(gameObject);
		}
	}

    private void Update()
    {
		Input.OnUpdate();
	}
}
