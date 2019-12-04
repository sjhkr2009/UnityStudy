using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager instance => _instance;

    [TabGroup("Scripts")] [SerializeField] Player player;
    [TabGroup("Scripts")] [SerializeField] SpawnManager spawnManager;
    UpgradeManager upgradeManager;

    float inputX = 0f;
    float inputY = 0f;
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
                    break;
                case State.Play:
                    _state = State.Play;
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
        upgradeManager = GetComponent<UpgradeManager>();
    }

    void Start()
    {
        //임시 코드
        state = State.Play;
    }

    void Update()
    {
        //Input
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15f));
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        switch (state)
        {
            case State.Play:
                PlayerControl();
                break;
        }
    }

    void PlayerControl()
    {
        player.PlayerMove(inputX, inputY);
        player.FollowMouse(mousePos);
    }

    public void PlayerShooting(Vector3 position, Quaternion rotation)
    {
        spawnManager.SpawnPlayerBullet(position, rotation);
    }
}
