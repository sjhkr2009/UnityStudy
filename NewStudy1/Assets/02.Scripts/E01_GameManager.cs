using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class E01_GameManager : MonoBehaviour
{
    public Text log;

    enum GameState { Create, Lobby, HomeTown, Field, Battle }
    enum Class { Warrior, Mage, Archor, Thief }
    enum MonsterType { Slime, Ork, Skeleton, Boss }
    public struct Player
    {
        public string name;
        public int hp;
        public int maxHp;
        public int power;
        public int evasion;
    }
    public struct Monster
    {
        public string name;
        public int hp;
        public int power;
        public int speed;
    }
    float evasionChance => Mathf.Min(Mathf.Max(player.evasion - currentMonster.speed, 1f) * Mathf.Max(((float)player.hp / (player.maxHp * 0.5f)), 0.33f), 100f);
    string logMessage
    {
        set
        {
            Debug.Log(value);
            log.text += "\n" + value;
        }
    }

    Player player;
    Monster currentMonster;

    [SerializeField, ReadOnly] private GameState _gameState;
    GameState gameState
    {
        get => _gameState;
        set
        {
            _gameState = value;
            logMessage = "";
            switch (value)
            {
                case GameState.Create:
                    OnCreate();
                    break;
                case GameState.Lobby:
                    OnLobby();
                    break;
                case GameState.HomeTown:
                    OnHomeTown();
                    break;
                case GameState.Field:
                    OnField();
                    break;
                case GameState.Battle:
                    OnBattle();
                    break;

            }
        }
    }

    void OnCreate()
    {
        logMessage = "캐릭터 생성을 시작합니다.\n" +
            "해당하는 숫자를 눌러주세요.\n" +
            "[1] 전사\n" +
            "[2] 마법사\n" +
            "[3] 궁수\n" +
            "[4] 도적";
    }
    void OnLobby()
    {
        logMessage = "로비에 입장하셨습니다.\n" +
            "입장할 지역을 선택하세요.\n" +
            "[1] 마을로 가기\n" +
            "[2] 필드로 가기\n" +
            "[3] 게임 종료";
    }
    void OnHomeTown()
    {
        player.hp = player.maxHp;
        logMessage = "마을에 입장하셨습니다.\n" +
            "HP가 최대치로 회복됩니다.\n" +
            "[1] 필드로 가기\n" +
            "[2] 로비로 돌아가기";
    }
    void OnField()
    {
        MonsterCreate();
        logMessage = $"{currentMonster.name} 이(가) 등장했습니다.\n" +
            "어떻게 하시겠습니까?\n" +
            "[1] 전투를 시작한다\n" +
            $"[2] 마을로 도망친다 (성공률 : {evasionChance.ToString("0.0")}%)\n";
    }
    void OnBattle()
    {
        StartCoroutine(nameof(Battle));
    }
    IEnumerator Battle()
    {
        int originEnemyHp = currentMonster.hp;
        int playerDev = Mathf.RoundToInt(player.power * 0.33f);
        int playerDamage = 0;
        int enemyDev = Mathf.RoundToInt(currentMonster.power * 0.25f);
        int enemyDamage = 0;

        logMessage = $"{currentMonster.name} 와(과) 전투를 시작합니다.";
        while (true)
        {
            yield return new WaitForSeconds(1f);
            playerDamage = Random.Range(player.power - playerDev, player.power + playerDev + 1);
            currentMonster.hp -= playerDamage;
            if (currentMonster.hp <= 0)
            {
                currentMonster.hp = 0;
                logMessage = $"플레이어 {player.name} 이(가) {currentMonster.name} 을(를) 공격해 {playerDamage}의 피해를 입혔습니다. (적 남은 체력 : {currentMonster.hp} / {originEnemyHp})\n" +
                    $"적을 처치했습니다!\n" +
                    $"필드로 돌아갑니다.";
                gameState = GameState.Field;
                break;
            }
            logMessage = $"플레이어 {player.name} 이(가) {currentMonster.name} 을(를) 공격해 {playerDamage}의 피해를 입혔습니다. (적 남은 체력 : {currentMonster.hp} / {originEnemyHp})";


            yield return new WaitForSeconds(0.5f);


            enemyDamage = Random.Range(currentMonster.power - enemyDev, currentMonster.power + enemyDev + 1);
            player.hp -= enemyDamage;
            if (player.hp <= 0)
            {
                player.hp = 0;
                logMessage = $"{currentMonster.name} 에게 공격당해 {enemyDamage}의 피해를 받았습니다. (플레이어 남은 체력 : {player.hp} / {player.maxHp})\n" +
                    $"적에게 당했습니다.\n" +
                    $"마을로 돌아갑니다.";
                gameState = GameState.HomeTown;
                break;
            }
            logMessage = $"{currentMonster.name} 에게 공격당해 {enemyDamage}의 피해를 받았습니다. (플레이어 남은 체력 : {player.hp} / {player.maxHp})";
        }
    }
    Player MakePlayer(Class myClass)
    {
        Player newPlayer = new Player();
        switch (myClass)
        {
            case Class.Warrior:
                newPlayer.name = "전사";
                newPlayer.maxHp = 175;
                newPlayer.hp = newPlayer.maxHp;
                newPlayer.power = 10;
                newPlayer.evasion = 20;
                break;
            case Class.Mage:
                newPlayer.name = "마법사";
                newPlayer.maxHp = 60;
                newPlayer.hp = newPlayer.maxHp;
                newPlayer.power = 30;
                newPlayer.evasion = 55;
                break;
            case Class.Archor:
                newPlayer.name = "궁수";
                newPlayer.maxHp = 100;
                newPlayer.hp = newPlayer.maxHp;
                newPlayer.power = 20;
                newPlayer.evasion = 70;
                break;
            case Class.Thief:
                newPlayer.name = "도적";
                newPlayer.maxHp = 80;
                newPlayer.hp = newPlayer.maxHp;
                newPlayer.power = 17;
                newPlayer.evasion = 90;
                break;
            default:
                newPlayer.name = null;
                break;
        }
        return newPlayer;
    }
    void MonsterCreate()
    {
        float chance = Random.value;
        if (chance < 0.05f) currentMonster = MakeMonster(MonsterType.Boss);
        else if (chance < 0.45f) currentMonster = MakeMonster(MonsterType.Slime);
        else if (chance < 0.75f) currentMonster = MakeMonster(MonsterType.Ork);
        else currentMonster = MakeMonster(MonsterType.Skeleton);
    }

    Monster MakeMonster(MonsterType type)
    {
        Monster monster = new Monster();
        switch (type)
        {
            case MonsterType.Slime:
                monster.name = "슬라임";
                monster.hp = 25;
                monster.power = 4;
                monster.speed = 10;
                break;
            case MonsterType.Ork:
                monster.name = "오크";
                monster.hp = 55;
                monster.power = 12;
                monster.speed = 0;
                break;
            case MonsterType.Skeleton:
                monster.name = "스켈레톤";
                monster.hp = 35;
                monster.power = 14;
                monster.speed = 25;
                break;
            case MonsterType.Boss:
                monster.name = "보스 몬스터";
                monster.hp = 100;
                monster.power = 15;
                monster.speed = 70;
                break;
        }
        return monster;
    }

    void InputController(int inputKey)
    {
        switch (gameState)
        {
            case GameState.Create:
                InputOnCreate(inputKey);
                break;
            case GameState.Lobby:
                InputOnLobby(inputKey);
                break;
            case GameState.HomeTown:
                InputOnHomeTown(inputKey);
                break;
            case GameState.Field:
                InputOnField(inputKey);
                break;
        }
    }

    void InputOnCreate(int input)
    {
        player = MakePlayer((Class)(input - 1));
        if (player.name != null)
        {
            logMessage = $"{player.name} 이(가) 생성되었습니다.";
            gameState = GameState.Lobby;
        }
        else
        {
            logMessage = "올바른 숫자를 입력하십시오.";
            gameState = GameState.Create;
        }
    }
    void InputOnLobby(int input)
    {
        switch (input)
        {
            case 1:
                gameState = GameState.HomeTown;
                break;
            case 2:
                gameState = GameState.Field;
                break;
            case 3:
                logMessage = "게임을 종료합니다.";
                player.name = null;
                gameState = GameState.Create;
                return;
        }
    }
    void InputOnHomeTown(int input)
    {
        switch (input)
        {
            case 1:
                gameState = GameState.Field;
                break;
            case 2:
                gameState = GameState.Lobby;
                break;
        }
    }
    void InputOnField(int input)
    {
        switch (input)
        {
            case 1:
                gameState = GameState.Battle;
                break;
            case 2:
                float requiredEvasion = Random.Range(0f, 100f);
                if (evasionChance > requiredEvasion)
                {
                    logMessage = "도망에 성공했습니다!";
                    gameState = GameState.HomeTown;
                }
                else
                {
                    logMessage = "도망에 실패했습니다.";
                    gameState = GameState.Battle;
                }
                break;
        }
    }


    void Start()
    {
        player.name = null;
        gameState = GameState.Create;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) InputController(1);
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) InputController(2);
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) InputController(3);
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) InputController(4);
    }
}
