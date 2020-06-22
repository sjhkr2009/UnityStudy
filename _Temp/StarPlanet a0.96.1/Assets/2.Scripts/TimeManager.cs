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

    /// <summary>
    /// 게임 상태 변화에 따른 동작을 서술합니다.
    /// Ready 상태일 때 시간을 초기화하고, Playing 상태에서는 시간을 흐르게, 아닐 경우엔 흐르지 않게 조정합니다.
    /// </summary>
    /// <param name="gameState"></param>
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

    /// <summary>
    /// 시간을 측정하고 시간 변화에 따른 이벤트를 발동합니다. 시간은 매 프레임마다 측정되며, 별도로 초 단위로도 측정합니다. 이벤트도 두 경우에 별개로 발동됩니다.
    /// 게임이 플레이 중일 때만 호출됩니다.
    /// </summary>
    void TimeCount()
    {
        timeCount += Time.deltaTime;
        secondCount = Mathf.FloorToInt(timeCount);

        EventOnTimeChanged(timeCount);
        if(secondCount != prevSecond) EventPerOneSecond();

        prevSecond = secondCount;
    }

}
