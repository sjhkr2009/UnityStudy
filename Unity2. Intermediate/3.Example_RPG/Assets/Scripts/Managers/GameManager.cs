using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	static GameManager _instance;
	public static GameManager Instance { get { Init(); return _instance; } }

	InputManager _input = new InputManager();
	public static InputManager Input => Instance._input;

	private void Start() => Init();
	static void Init()
	{
		if (_instance == null)
		{
			GameObject go = GameObject.Find("@GameManager");
			if (go == null)
			{
				go = new GameObject("@GameManager");
				go.AddComponent<GameManager>();
			}

			DontDestroyOnLoad(go);
			_instance = go.GetComponent<GameManager>();
		}
	}

	private void Update()
	{
		Input.OnUpdate();
	}
}
