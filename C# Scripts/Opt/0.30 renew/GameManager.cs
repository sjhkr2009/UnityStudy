using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<GameManager>();
            if (_instance == null)
            {
                GameObject container = new GameObject("GameManager");
                _instance = container.AddComponent<GameManager>();
            }
            return _instance;
        }
    }

    [SerializeField] [BoxGroup("Scripts")] Player player;
    [SerializeField] [BoxGroup("Scripts")] JoyStickController joyStick;
    [SerializeField] [BoxGroup("Scripts")] LaserManager laserManager;
    [SerializeField] [BoxGroup("Scripts")] UiManager uiManager;


    private void Awake()
    {
        _instance = this;
        if(player == null) player = FindObjectOfType<Player>();
        if (joyStick == null) joyStick = FindObjectOfType<JoyStickController>();
        if (laserManager == null) laserManager = GetComponent<LaserManager>();
        if (uiManager == null) uiManager = GetComponent<UiManager>();
    }
    void Start()
    {
        if (_instance != null && _instance != this) Destroy(gameObject);

        player.EventOnHpChanged += GameoverCheck;

        joyStick.EventOnDrag += player.PlayerMove;

        laserManager.EventInHacking += uiManager.OnHacking;
        laserManager.EventOutHacking += uiManager.OffHacking;
    }

    private void OnDestroy()
    {
        player.EventOnHpChanged -= GameoverCheck;

        joyStick.EventOnDrag -= player.PlayerMove;

        laserManager.EventInHacking -= uiManager.OnHacking;
        laserManager.EventOutHacking -= uiManager.OffHacking;
    }


    void GameoverCheck(int playerHp)
    {
        if(playerHp <= 0f)
        {
            Debug.Log("Game Over");
        }
    }
}
