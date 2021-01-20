using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define.Default;

/// <summary>
/// Singleton이 적용되어 모든 스크립트는 GameManager를 통해 주요 클래스에 접근할 수 있습니다.
/// 매니저 클래스들의 초기화와 해제, 업데이트를 담당합니다.
/// </summary>
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

	private SceneBase _currentScene;
	public SceneBase CurrentScene
	{
		get
		{
			if (_currentScene == null)
				_currentScene = FindObjectOfType<SceneBase>();

			return _currentScene;
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

		Camera.SetSizeByAspect();
	}

	public static void Clear()
	{
		Input.Clear();
		Resource.Clear();
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
