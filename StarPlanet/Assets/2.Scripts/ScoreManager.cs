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

    private void Awake()
    {
        score = 0;
        timeScoreCount = 0f;
        _bestScore = bestScore;
    }

    public int bestScore
    {
        get => PlayerPrefs.GetInt(nameof(bestScore), 0);
        set => PlayerPrefs.SetInt(nameof(bestScore), value);
    }

    public int score
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
        this.score += score;
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
                score = scoreTier3;
                break;
            case EnemyType.ToStar3:
                score = (int)(scoreTier3 / 4f);
                break;
            case EnemyType.ToPlanet4:
                score = scoreTier4;
                break;
            case EnemyType.ToStar4:
                score = scoreTier4 * 2;
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
                break;
            case GameState.Playing:
                break;
            case GameState.Pause:
                break;
            case GameState.GameOver:
                BestScoreCheck();
                break;
        }
    }

    private void BestScoreCheck()
    {
        _bestScore = bestScore;

        if (score > bestScore)
        {
            bestScore = score;
            EventOnGameOver(score, true);
        }
        else
        {
            EventOnGameOver(score, false);
        }

    }
}
