using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager instance => _instance;

    [TabGroup("Scripts")] public Player player;
    [TabGroup("Scripts")] public SpawnManager spawnManager;
    [TabGroup("Scripts")] public EnemySpawner enemySpawner;
    UIManager uiManager;

    [TabGroup("Level")] [SerializeField] int[] expList;

    int _requiredExp = 1;
    public int requiredExp => _requiredExp;

    int _currentExp;
    public int currentExp
    {
        get { return _currentExp; }
        set
        {
            _currentExp = value;
            if (value >= requiredExp)
            {
                if(expPerLevel.Count <= player.level)
                {
                    return;
                }
                
                currentExp = value - requiredExp;
                _requiredExp = expPerLevel[player.level];
                player.level++;
                uiManager.requiredExp = requiredExp;
                uiManager.maxHp = player.maxHp;
                uiManager.LevelUp();
                enemySpawner.SpawnDelayReduce(0.88f);

                if(player.level == 5)
                {
                    uiManager.SkillGuide(true);
                }
            }
        }
    }

    List<int> expPerLevel = new List<int>();

    float inputX = 0f;
    float inputY = 0f;
    float inputRawX = 0f;
    float inputRawY = 0f;
    Vector3 mousePos;

    public enum State
    {
        Ready, Play, Upgrade, Pause, Gameover
    }

    State _state;

    public State state
    {
        get { return _state; }
        set
        {
            switch (value)
            {
                case State.Ready:
                    _state = State.Ready;
                    uiManager.StartLoading();
                    break;
                case State.Play:
                    Time.timeScale = 1f;
                    _state = State.Play;
                    enemySpawner.Spawn();
                    break;
                case State.Upgrade:
                    _state = State.Upgrade;
                    break;
                case State.Pause:
                    _state = State.Pause;
                    Time.timeScale = 0f;
                    break;
                case State.Gameover:
                    _state = State.Gameover;
                    break;
            }
        }
    }

    private void Awake()
    {
        _instance = this;
        uiManager = GetComponent<UIManager>();
        SetExpLevel();
        state = State.Ready;
        _requiredExp = expPerLevel[0];
    }

    void Update()
    {
        //Input
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15f));
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        inputRawX = Input.GetAxisRaw("Horizontal");
        inputRawY = Input.GetAxisRaw("Vertical");

        switch (state)
        {
            case State.Play:
                PlayerControl();
                break;
        }
    }

    void PlayerControl()
    {
        if(player.level <= 3)
        {
            player.PlayerMove(inputX, inputY);
        }
        else
        {
            player.PlayerMove(inputRawX, inputRawY);
        }
        player.FollowMouse(mousePos);
    }

    public void PlayerShooting(Vector3 position, Quaternion rotation)
    {
        spawnManager.SpawnPlayerBullet(position, rotation);
    }

    public void PlayerHomingSkill(Vector3 position, int count)
    {
        
        for (int i = 0; i < count; i++)
        {
            Vector3 vecRotation = new Vector3(0f, (1f/count)*i*360f, 0f);
            Quaternion rotation = Quaternion.Euler(vecRotation);
            spawnManager.SpawnPlayerHomingBullet(position, rotation);
        }

        uiManager.SkillGuide(false);
    }

    public void EnemyBulletShooting(Vector3 position, Quaternion rotation)
    {
        spawnManager.SpawnEnemyBullet(position, rotation);
    }

    void SetExpLevel()
    {
        for (int i = 0; i < expList.Length; i++)
        {
            expPerLevel.Add(expList[i]);
            if(expPerLevel[i] == 0)
            {
                expPerLevel[i] = 1;
            }
        }
    }
}
