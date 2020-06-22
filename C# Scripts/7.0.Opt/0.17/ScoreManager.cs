using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private void Awake()
    {
        instance = this;
    }

    public Text timeText;
    public Player player;

    float countTime;
    public int countHacking;
    public bool itemUse; //아이템 사용이 감지되면 true로 변경

    public int stageNumber;
    public float limitTime;
    public int limitHacking;
    public int limitLife;
    public int limitItem;

    void Start()
    {
        countTime = 0;
        countHacking = 0;
        itemUse = false;
    }

    void Update()
    {
        if (StageManager.instance.isStageEnd)
        {
            return;
        }

        TimeCount();
        timeText.text = $"{countTime:N2}" + " " + countHacking.ToString();
    }

    void TimeCount()
    {
        countTime += Time.deltaTime;
    }

    public void GetScore()
    {
        GameManager.instance.playerGold += GoldCalc(stageNumber, countTime, countHacking, player.hp);
    }

    int GoldCalc(int stageNumber, float time, int hacking, int remainingLife)
    {
        int clearGold = Random.Range(stageNumber * 8, stageNumber * 12);
        int timeBonus = 0;
        int hackingBonus = 0;

        if (time < limitTime)
        {
            timeBonus = (int)(10 * Mathf.Sqrt(10 * stageNumber) * Mathf.Min((limitTime / time), 3f));
        }
        if(hacking < limitHacking)
        {
            hackingBonus = (int)(10 * Mathf.Sqrt(10 * stageNumber) * Mathf.Min((limitHacking / hacking), 3f));
        }

        int lifeBonus = Mathf.Min(((remainingLife - 1) * (remainingLife - 1)), 10) * stageNumber;

        return clearGold + timeBonus + hackingBonus + lifeBonus;
    }
}
