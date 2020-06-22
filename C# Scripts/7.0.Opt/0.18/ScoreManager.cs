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
        int getGold = GoldCalc(stageNumber, countTime, countHacking, player.hp);
        int getStarPoint = StarPointCalc(stageNumber, countTime, countHacking, player.hp, itemUse);
        GameManager.instance.GetCredit(getGold, getStarPoint);
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

    int StarPointCalc(int stageNumber, float time, int hacking, int remainingLife, bool itemUse)
    {
        int firstClearStar = 0;
        int timeStar = 0;
        int hackingStar = 0;
        int lifeStar = 0;
        int itemStar = 0;

        if (!GameManager.instance.firstClear[stageNumber])
        {
            GameManager.instance.firstClear[stageNumber] = true;
            PlayerPrefs.SetInt("firstClear" + stageNumber.ToString(), 1);
            firstClearStar = stageNumber;
        }
        if (!GameManager.instance.timeStarPoint[stageNumber])
        {
            if(countTime < limitTime)
            {
                GameManager.instance.timeStarPoint[stageNumber] = true;
                PlayerPrefs.SetInt("timeStarPoint" + stageNumber.ToString(), 1);
                timeStar = stageNumber;
            }
        }
        if (!GameManager.instance.hackingStarPoint[stageNumber])
        {
            if(countHacking < limitHacking)
            {
                GameManager.instance.hackingStarPoint[stageNumber] = true;
                PlayerPrefs.SetInt("hackingStarPoint" + stageNumber.ToString(), 1);
                hackingStar = stageNumber;
            }
        }
        if (!GameManager.instance.lifeStarPoint[stageNumber])
        {
            if(player.hp >= 3)
            {
                GameManager.instance.lifeStarPoint[stageNumber] = true;
                PlayerPrefs.SetInt("lifeStarPoint" + stageNumber.ToString(), 1);
                lifeStar = stageNumber;
            }
        }
        if (!GameManager.instance.itemStarPoint[stageNumber])
        {
            if (!itemUse)
            {
                GameManager.instance.itemStarPoint[stageNumber] = true;
                PlayerPrefs.SetInt("itemStarPoint" + stageNumber.ToString(), 1);
                itemStar = stageNumber;
            }
        }

        return firstClearStar + timeStar + hackingStar + lifeStar + itemStar;
    }
}
