using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class Calculator : MonoBehaviour
{
    [BoxGroup("UI"), SerializeField] Toggle getManualUI;
    [BoxGroup("UI"), SerializeField] Dropdown eduAliveBonusUI;
    [BoxGroup("UI"), SerializeField] Dropdown eduLaborBonusUI;
    [BoxGroup("UI"), SerializeField] Dropdown workTypeUI;
    [BoxGroup("UI"), SerializeField] Dropdown playerLevelUI;
    [BoxGroup("UI"), SerializeField] Dropdown enemyLevelUI;
    [BoxGroup("UI"), SerializeField] Toggle isPaleUI;
    [BoxGroup("UI"), SerializeField] InputField startHpUI;
    [BoxGroup("UI"), SerializeField] InputField endHpUI;
    [BoxGroup("UI"), SerializeField] InputField peBoxUI;
    [BoxGroup("UI"), SerializeField] Text resultUI;
    [BoxGroup("UI"), SerializeField] GameObject onPaleUI;
    [BoxGroup("UI"), SerializeField] GameObject offPaleUI;

    [BoxGroup("계산 결과"), SerializeField, ReadOnly] private float result;
    public float Result
    {
        get => result;
        set
        {
            result = value;
            resultUI.text = value.ToString();
        }
    }


    [BoxGroup("최종 변수"), SerializeField, ReadOnly] private float _levelExp = 1f;
    public float LevelExp
    {
        get => _levelExp;
        set
        {
            _levelExp = value;
            FinalCalculate();
        }
    }
    [BoxGroup("최종 변수"), SerializeField, ReadOnly] private float _dmgExp = 1f;
    public float DmgExp
    {
        get => _dmgExp;
        set
        {
            _dmgExp = value;
            FinalCalculate();
        }
    }
    [BoxGroup("최종 변수"), SerializeField, ReadOnly] private float _specialExp = 1f;
    public float SpecialExp
    {
        get => _specialExp;
        set
        {
            _specialExp = value;
            FinalCalculate();
        }
    }
    [BoxGroup("최종 변수"), SerializeField, ReadOnly] private int _peBox = 0;
    public int PeBox
    {
        get => _peBox;
        set
        {
            _peBox = value;
            FinalCalculate();
        }
    }


    [BoxGroup("게임 내 변수"), SerializeField, ReadOnly] bool isJustice = false;
    public bool IsJustice
    {
        get => isJustice;
        set
        {
            isJustice = value;
            LevelExp = CalculateLevelExp(_playerLevel, _enemyLevel);
        }
    }
    [BoxGroup("게임 내 변수"), SerializeField, ReadOnly] bool isPale = false;
    public bool IsPale
    {
        get => isPale;
        set
        {
            isPale = value;
            startHpUI.readOnly = value;
            endHpUI.readOnly = value;
            onPaleUI.SetActive(value);
            offPaleUI.SetActive(!value);
            DmgExp = CalculateDmgExp(_startHp, _endHp);
        }
    }
    [BoxGroup("게임 내 변수"), SerializeField, ReadOnly] bool getManual = false;
    public bool GetManual
    {
        get => getManual;
        set
        {
            getManual = value;
            CalculateSpecialExp();
        }
    }
    [BoxGroup("게임 내 변수"), SerializeField, ReadOnly] private int eduAliveBonus = 0;
    public int EduAliveBonus
    {
        get => eduAliveBonus;
        set
        {
            eduAliveBonus = value;
            CalculateSpecialExp();
        }
    }
    [BoxGroup("게임 내 변수"), SerializeField, ReadOnly] private int eduLaborBonus = 0;
    public int EduLaborBonus
    {
        get => eduLaborBonus;
        set
        {
            eduLaborBonus = value;
            CalculateSpecialExp();
        }
    }



    //LevelExp 관련 변수
    [BoxGroup("직원 및 환상체 정보"), SerializeField, ReadOnly] private int _playerLevel = 1;
    public int PlayerLevel
    {
        get => _playerLevel;
        set
        {
            _playerLevel = value;
            LevelExp = CalculateLevelExp(_playerLevel, _enemyLevel);
        }
    }
    [BoxGroup("직원 및 환상체 정보"), SerializeField, ReadOnly] private int _enemyLevel = 1;
    public int EnemyLevel
    {
        get => _enemyLevel;
        set
        {
            _enemyLevel = value;
            LevelExp = CalculateLevelExp(_playerLevel, _enemyLevel);
        }
    }

    //DmgExp 관련 변수
    [BoxGroup("직원 및 환상체 정보"), SerializeField, ReadOnly] private int _startHp = 0;
    public int StartHp
    {
        get => _startHp;
        set
        {
            _startHp = value;
            DmgExp = CalculateDmgExp(_startHp, _endHp);
        }
    }
    [BoxGroup("직원 및 환상체 정보"), SerializeField, ReadOnly] private int _endHp = 0;
    public int EndHp
    {
        get => _endHp;
        set
        {
            _endHp = value;
            DmgExp = CalculateDmgExp(_startHp, _endHp);
        }
    }

    [Button] void SetPlayerLevel(int value) { PlayerLevel = value; }
    [Button] void SetEnemyLevel(int value) { EnemyLevel = value; }
    [Button] void SetStartHp(int value) { StartHp = value; }
    [Button] void SetEndHp(int value) { EndHp = value; }
    [Button] void SetPeBox(int value) { PeBox = value; }
    [Button] void SetIsJustice(bool value) { IsJustice = value; }

    private void Start()
    {
        
    }

    private float CalculateLevelExp(int player, int enemy)
    {
        int num = (player - enemy) + 3;
        float levelExp = 1;

        if (num <= 0) levelExp = 1.4f;
        else if (num == 1) levelExp = 1.2f;
        else if (num == 2) levelExp = 1.0f;
        else if (num == 3) levelExp = 1.0f;
        else if (num == 4) levelExp = 0.8f;
        else if (num == 5) levelExp = 0.6f;
        else if (num == 6) levelExp = 0.4f;
        else if (num >= 7) levelExp = 0.2f;

        if (player <= 1) levelExp *= 0.6f;
        else if (player == 2) levelExp *= 0.55f;
        else if (player == 3) levelExp *= 0.5f;
        else if (player == 4) levelExp *= 0.45f;
        else if (player >= 5) levelExp *= 0.4f;

        if (isJustice) levelExp *= 0.333f;

        return levelExp;
    }

    private float CalculateDmgExp(int start, int end)
    {
        if (isPale) return 1.5f;
        if (start == 0) return 0f;
        
        float playerHp = (float)end / (float)start;
        float dmgExp = 1f;

        if (playerHp >= 0.9f) dmgExp = 0.4f;
        else if (playerHp >= 0.8f) dmgExp = 0.6f;
        else if (playerHp >= 0.7f) dmgExp = 0.8f;
        else if (playerHp > 0.2f) dmgExp = 1f;
        else if (playerHp > 0.1f) dmgExp = 1.3f;
        else if (playerHp <= 0.1f) dmgExp = 1.5f;

        return dmgExp;
    }

    private void CalculateSpecialExp()
    {
        float specialExp = 1f;

        if (getManual) specialExp += 0.5f;

        if (eduAliveBonus == 1) specialExp += 0.01f;
        else if (eduAliveBonus == 2) specialExp += 0.03f;
        else if (eduAliveBonus >= 3) specialExp += 0.05f;

        if (eduLaborBonus == 1) specialExp += 0.05f;
        else if (eduLaborBonus == 2) specialExp += 0.1f;
        else if (eduLaborBonus >= 3) specialExp += 0.15f;

        SpecialExp = specialExp;
    }

    private void FinalCalculate()
    {
        if (_specialExp < 1f) _specialExp = 1f;

        float _result = (float)_peBox * _levelExp * _dmgExp * _specialExp;
        Result = Mathf.Round(_result * 100) * 0.01f;
    }

    public void InputGetMenual() { GetManual = getManualUI.isOn; }
    public void InputEduAlive() { EduAliveBonus = eduAliveBonusUI.value; }
    public void InputEduLabor() { EduLaborBonus = eduLaborBonusUI.value; }
    public void InputWorkType()
    {
        int value = workTypeUI.value;
        if (value == 0 && IsJustice) IsJustice = false;
        else if (value == 1 && !IsJustice) IsJustice = true;
    }
    public void InputPlayerLevel() { PlayerLevel = playerLevelUI.value + 1; }
    public void InputEnemyLevel() { EnemyLevel = enemyLevelUI.value + 1; }
    public void InputIsPale()
    { 
        IsPale = isPaleUI.isOn;
    }
    public void InputStartHp()
    {
        int num = 0;
        int.TryParse(startHpUI.text, out num);
        StartHp = num;
    }
    public void InputEndHp()
    {
        int num = 0;
        int.TryParse(endHpUI.text, out num);
        EndHp = num;
    }
    public void InputPeBox()
    {
        int num = 0;
        int.TryParse(peBoxUI.text, out num);
        PeBox = num;
    }
    
    public void DontTouchMe()
    {
        Application.Quit();
    }

    public void ReStart()
    {
        SceneManager.LoadScene(0);
    }
}
