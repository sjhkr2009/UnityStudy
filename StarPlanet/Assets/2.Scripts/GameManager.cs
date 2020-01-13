using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    public enum GameState { Ready, Playing, Pause, GameOver }
    private GameState _gameState;
    public GameState gameState
    {
        get => _gameState;
        set
        {
            switch (value)
            {
                case GameState.Ready:
                    _gameState = GameState.Ready;
                    break;
                case GameState.Playing:
                    _gameState = GameState.Playing;
                    StartCoroutine(spawnManager.EnemySpawn());
                    Time.timeScale = 1f;
                    break;
                case GameState.Pause:
                    _gameState = GameState.Pause;
                    StopAllCoroutines();
                    Time.timeScale = 0f;
                    break;
                case GameState.GameOver:
                    _gameState = GameState.GameOver;
                    Debug.Log("Game Over");
                    break;
            }
        }
    }

    [BoxGroup("Scripts")] [SerializeField] UIManager uiManager;
    [BoxGroup("Scripts")] [SerializeField] Star star;
    [BoxGroup("Scripts")] [SerializeField] SpawnManager spawnManager;
    ParticleManager particleManager;

    Vector3 mousePos;
    public event Action<Vector3> EventOnClick;


    private void Awake()
    {
        _instance = this;
        gameState = GameState.Ready;
        particleManager = GetComponent<ParticleManager>();
    }

    void Start()
    {
        if(_instance != null && _instance != this) { Destroy(gameObject);}
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));

        if (Input.GetMouseButton(0))
        {
            EventOnClick(mousePos);
        }
    }

}
