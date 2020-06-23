using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class E02_GameManager : MonoBehaviour
{
    public enum GameState
    {
        None,
        Create,
        Lobby,
        Town,
        Field,
        Battle,
        BattleEnd
    }
    private GameState _gameState;

    [SerializeField] Text log;
    Player player;
    Monster monster;

    string logText
    {
        set
        {
            Debug.Log(value);
            log.text += "\n" + value;
        }
    }
    #region 안내 메시지
    [SerializeField, TextArea, ReadOnly] string startText = "캐릭터 생성을 시작합니다. 원하시는 직업을 선택하세요." +
        "\n[1] 전사" +
        "\n[2] 궁수" +
        "\n[3] 마법사" +
        "\n[4] 도적";
    [SerializeField, TextArea, ReadOnly] string lobbyText = "로비에 입장하셨습니다." +
        "\n[1] 마을로 가기" +
        "\n[2] 필드로 가기" +
        "\n[3] 종료하기";
    [SerializeField, TextArea, ReadOnly] string townText = "마을입니다. HP가 최대치로 회복됩니다." +
        "\n[1] 로비로 가기" +
        "\n[2] 필드로 가기";
    [SerializeField, TextArea, ReadOnly] string fieldText = "필드에 진입했습니다.";
    [SerializeField, TextArea, ReadOnly] string battleEndText = "[1] 마을로 돌아가기" +
        "\n[2] 필드로 가기";
    #endregion

    float evasionChance
    {
        get
        {
            float playerHp = (float)player.GetHp() / (float)player.maxHp;
            float chance = (50f * playerHp) + ((player.evasion - monster.speed) / 2f);
            return Mathf.Clamp(chance, 1f, 100f);
        }
    }
    GameState gameState
    {
        get => _gameState;
        set
        {
            _gameState = value;
            logText = "----------------------------";
            switch (value)
            {
                case GameState.None:
                    logText = "시작하려면 [1] 키를 누르세요.";
                    break;
                case GameState.Create:
                    logText = startText;
                    break;
                case GameState.Lobby:
                    logText = lobbyText;
                    break;
                case GameState.Town:
                    player.HealTo(player.maxHp);
                    logText = townText;
                    break;
                case GameState.Field:
                    logText = fieldText;
                    FindMonster();
                    logText = $"{monster.name} 이(가) 나타났습니다. 어떻게 하시겠습니까?" +
                        $"\n[1] 전투를 시작한다." +
                        $"\n[2] 도망친다. (도주 성공률 : {evasionChance.ToString("0.0")} %)";
                    break;
                case GameState.Battle:
                    logText = "전투를 시작합니다.";
                    StartCoroutine(nameof(Battle));
                    break;
                case GameState.BattleEnd:
                    logText = $"전투가 종료되었습니다. (남은 체력 : {player.GetHp()} )";
                    logText = battleEndText;
                    break;
            }
        }
    }

    private void Start()
    {
        if (log == null) log = FindObjectOfType<Text>();
        gameState = GameState.Create;
    }

    #region Input 관련 메서드
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) InputController(1);
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) InputController(2);
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) InputController(3);
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) InputController(4);
    }

    void InputController(int input)
    {
        switch (gameState)
        {
            case GameState.None:
                InputOnNone(input);
                break;
            case GameState.Create:
                InputOnCreate(input);
                break;
            case GameState.Lobby:
                InputOnLobby(input);
                break;
            case GameState.Town:
                InputOnTown(input);
                break;
            case GameState.Field:
                InputOnField(input);
                break;
            case GameState.BattleEnd:
                InputOnBattleEnd(input);
                break;
            default:
                break;
        }
    }
    void InputOnNone(int input)
    {
        if (input == 1) gameState = GameState.Create;
    }
    void InputOnCreate(int input)
    {
        player = CreatePlayer(input);
        logText = $"[{player.name}] 캐릭터 생성이 완료되었습니다. {player.GetHp()}의 체력과 {player.GetPower()}의 공격력을 갖습니다.";
        gameState = GameState.Lobby;
    }
    void InputOnLobby(int input)
    {
        switch (input)
        {
            case 1:
                gameState = GameState.Town;
                break;
            case 2:
                gameState = GameState.Field;
                break;
            case 3:
                logText = "게임을 종료합니다.";
                gameState = GameState.None;
                break;
        }
    }
    void InputOnTown(int input)
    {
        switch (input)
        {
            case 1:
                gameState = GameState.Lobby;
                break;
            case 2:
                gameState = GameState.Field;
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
                if (Evasion())
                {
                    logText = "도주에 성공했습니다. 마을로 돌아갑니다.";
                    gameState = GameState.Town;
                }
                else
                {
                    logText = "도주에 실패했습니다.";
                    gameState = GameState.Battle;
                }
                break;
        }
    }
    void InputOnBattleEnd(int input)
    {
        switch (input)
        {
            case 1:
                gameState = GameState.Town;
                break;
            case 2:
                gameState = GameState.Field;
                break;
        }
    }
    #endregion

    const int KnightClassNumer = 1;
    const int ArchorClassNumer = 2;
    const int MageClassNumer = 3;
    const int ThiefClassNumer = 4;

    Player CreatePlayer(int classNumber)
    {
        Player player = null;
        switch (classNumber)
        {
            case KnightClassNumer:
                player = new Knight();
                break;
            case ArchorClassNumer:
                player = new Archor();
                break;
            case MageClassNumer:
                player = new Mage();
                break;
            case ThiefClassNumer:
                player = new Thief();
                break;
        }
        return player;
    }
    void FindMonster()
    {
        float value = Random.value;
        if (value < 0.4f) monster = new Slime();
        else if (value < 0.7f) monster = new Ork();
        else if (value < 0.92f) monster = new Skeleton();
        else monster = new Boss();

        if (monster == null) monster = new Slime();
    }
    bool Evasion()
    {
        float chance = evasionChance;
        float successPoint = Random.Range(0f, 100f);
        if (chance > successPoint) return true;
        else return false;
    }

    IEnumerator Battle()
    {
        while (true)
        {
            yield return null;

            int playerDamage = GetDamage(player);
            monster.OnDamaged(playerDamage);
            logText = $"플레이어 {player.name} 이(가) {monster.name} 을(를) 공격하여 {playerDamage}의 피해를 입혔습니다.";

            if (monster.GetHp() == 0)
            {
                BattleEnd(true);
                break;
            }

            yield return new WaitForSeconds(0.5f);

            int monsterDamage = GetDamage(monster);
            player.OnDamaged(monster.GetPower());
            logText = $"{monster.name} 에게 공격받아 {monsterDamage}의 피해를 받았습니다.";

            if (player.GetHp() == 0)
            {
                BattleEnd(false);
                break;
            }

            logText = $"플레이어 남은 체력 : {player.GetHp()} / {player.maxHp}  |  적 남은 체력 : {monster.GetHp()} / {monster.maxHp}\n";
            yield return new WaitForSeconds(1.5f);
        }
    }
    int GetDamage(Creature attacker)
    {
        int damage = Mathf.RoundToInt(attacker.GetPower() * Random.Range(1f - attacker.damageDifference, 1f + attacker.damageDifference));
        return damage;
    }
    void BattleEnd(bool isWin)
    {
        if (isWin)
        {
            logText = "승리했습니다!";
            gameState = GameState.BattleEnd;
        }
        else
        {
            logText = "패배했습니다. 마을로 돌아갑니다.";
            gameState = GameState.Town;
        }

    }
}
