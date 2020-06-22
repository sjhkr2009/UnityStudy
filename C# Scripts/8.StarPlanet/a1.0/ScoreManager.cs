using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public event Action<int> EventOnScoreChanged = n => { };
    public event Action<int, bool> EventOnGameOver = (n,b) => { }; //최고 점수를 갱신했다면 true, uiManager에서 게임오버 화면에 점수를 출력하는 용도.

    [SerializeField, ReadOnly] private int _score;
    [SerializeField, ReadOnly] private int _bestScore;

    [SerializeField] private float addScorePerSecond;
    [SerializeField, ReadOnly] float timeScoreCount;
    [SerializeField] private int scoreTier1;
    [SerializeField] private int scoreTier2;
    [SerializeField] private int scoreTier3;
    [SerializeField] private int scoreTier4;
    [ReadOnly] public bool isCurrentScoreBest;

    public int BestScore
    {
        get => PlayerPrefs.GetInt(nameof(BestScore), 0);
        set => PlayerPrefs.SetInt(nameof(BestScore), value);
    }

    private void Awake()
    {
        _score = 0;
        timeScoreCount = 0f;
        isCurrentScoreBest = false;
        _bestScore = BestScore;

        EventOnScoreChanged += BestScoreCheck;
    }

    private void OnDestroy()
    {
        if(!isCurrentScoreBest) EventOnScoreChanged -= BestScoreCheck;
        else EventOnScoreChanged -= BestScoreSave;
    }

    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            EventOnScoreChanged(value);
        }
    }

    void AddScore(int score)
    {
        Score += score;
    }

    public void AddScorePerSecond()
    {
        timeScoreCount += addScorePerSecond;
        if(timeScoreCount >= 1.0f)
        {
            AddScore(Mathf.FloorToInt(timeScoreCount));
            timeScoreCount %= 1f;
        }
    }
    public void ScorePerSecondUpgrade()
    {
        addScorePerSecond = addScorePerSecond + Mathf.Min(addScorePerSecond * 0.1f, 0.88f);
    }

    int ScoreCalculator(Enemy enemy)
    {
        int score = 0;
        switch (enemy.EnemyType)
        {
            case EnemyType.ToPlanet1:
                score = scoreTier1;
                break;
            case EnemyType.ToStar1:
                score = scoreTier1;
                break;
            case EnemyType.ToPlanet2:
                score = scoreTier2;
                break;
            case EnemyType.ToStar2:
                score = scoreTier2;
                break;
            case EnemyType.ToPlanet3:
                score = scoreTier3 * 6;
                break;
            case EnemyType.TP3mini:
                score = scoreTier3 * 2;
                break;
            case EnemyType.ToStar3:
                score = scoreTier3;
                break;
            case EnemyType.ToPlanet4:
                score = scoreTier4;
                break;
            case EnemyType.ToStar4:
                score = scoreTier4;
                break;
        }
        return score;
    }

    public void GetScore(Enemy enemy, int _none) { AddScore(ScoreCalculator(enemy)); }
    public void GetScore(Enemy enemy) { AddScore(ScoreCalculator(enemy)); }

    public void OnGameStateChanged(GameState gameState) //게임 상태 변화에 따른 동작 설정
    {
        switch (gameState)
        {
            case GameState.Ready:
                _score = 0;
                break;
            case GameState.GameOver:
                OnGameOverScoreCheck();
                break;
            default:
                break;
        }
    }

    private void OnGameOverScoreCheck() { EventOnGameOver(Score, isCurrentScoreBest); }

    void BestScoreCheck(int currentScore)
    {
        if(currentScore > _bestScore)
        {
            isCurrentScoreBest = true;
            EventOnScoreChanged -= BestScoreCheck;
            EventOnScoreChanged += BestScoreSave;
        }
    }

    void BestScoreSave(int score)
    {
        BestScore = score;
    }
}
