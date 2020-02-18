using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TimeManager : MonoBehaviour
{
    public event Action<float> EventOnTimeChanged = f => { };
    public event Action EventPerOneSecond = () => { };

    [SerializeField, ReadOnly] bool isPlaying;
    [SerializeField, ReadOnly] float timeCount;
    [SerializeField, ReadOnly] int secondCount;
    int prevSecond;

    private void Awake()
    {
        isPlaying = false;
        timeCount = 0f;
        secondCount = 0;
    }

    void Update()
    {
        if (isPlaying) TimeCount();
    }

    public void OnGameStateChanged(GameState gameState) 
    {
        switch (gameState)
        {
            case GameState.Ready:
                if (isPlaying) isPlaying = false;
                timeCount = 0f;
                secondCount = 0;
                break;
            case GameState.Playing:
                isPlaying = true;
                break;
            case GameState.Pause:
                isPlaying = false;
                break;
            case GameState.GameOver:
                isPlaying = false;
                break;
        }
    }

    void TimeCount()
    {
        timeCount += Time.deltaTime;
        secondCount = Mathf.FloorToInt(timeCount);

        EventOnTimeChanged(timeCount);
        if(secondCount != prevSecond) EventPerOneSecond();

        prevSecond = secondCount;
    }

}
