using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public event Action<int> EventOnScoreChanged = n => { };

    [SerializeField, ReadOnly] private int _score;
    [SerializeField, ReadOnly] private int bestScore;

    [SerializeField] private int scoreTier1;
    [SerializeField] private int scoreTier2;
    [SerializeField] private int scoreTier3;
    [SerializeField] private int scoreTier4;

    private void Awake()
    {
        score = 0;
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
        }
        return score;
    }

    public void GetScore(Enemy enemy, int _none) { AddScore(ScoreCalculator(enemy)); }
    public void GetScore(Enemy enemy) { AddScore(ScoreCalculator(enemy)); }
}
