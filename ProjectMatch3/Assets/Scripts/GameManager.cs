using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define.Default;

public class GameManager : MonoBehaviour
{
	static GameManager _instance;
	public static GameManager Instance { get { Init(); return _instance; } }

	ResourceManager _resource = new ResourceManager();
	public static ResourceManager Resource => Instance._resource;


	private Camera _camera;
	public Camera Camera
	{
		get
		{
			if (_camera == null)
				_camera = Camera.main;

			return _camera;
		}
	}

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

	}

	private void Awake()
	{
		Init();
		if (_instance != this)
		{
			DestroyImmediate(gameObject);
		}
	}
}
