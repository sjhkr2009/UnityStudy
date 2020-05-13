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
    public GameObject clearUI;
    public Text[] ClearGoldUI = new Text[5];
    public Image[] starSlider = new Image[5];
    public Image[] starIcon = new Image[5];

    float countTime;
    public int countHacking;
    public bool itemUse; //아이템 사용이 감지되면 true로 변경

    public int stageNumber;
    public float limitTime;
    public int limitHacking;
    public int limitLife;
    public int limitItem;

    int clearGold;
    int timeBonus;
    int hackingBonus;
    int lifeBonus;

    int firstClearStar;
    int timeStar;
    int hackingStar;
    int lifeStar;
    int itemStar;

    enum StarPointState
    {
        alreadyGotten, newGet, none
    }
    StarPointState clearStarState;
    StarPointState timeStarState;
    StarPointState hackingStarState;
    StarPointState lifeStarState;
    StarPointState itemStarState;

    void StarPointCheck(int stageNumber)
    {
        if (GameManager.instance.firstClear[stageNumber])
        {
            clearStarState = StarPointState.alreadyGotten;
        }
        else
        {
            clearStarState = StarPointState.none;
        }

        if (GameManager.instance.timeStarPoint[stageNumber])
        {
            timeStarState = StarPointState.alreadyGotten;
        }
        else
        {
            timeStarState = StarPointState.none;
        }

        if (GameManager.instance.hackingStarPoint[stageNumber])
        {
            hackingStarState = StarPointState.alreadyGotten;
        }
        else
        {
            hackingStarState = StarPointState.none;
        }

        if (GameManager.instance.lifeStarPoint[stageNumber])
        {
            lifeStarState = StarPointState.alreadyGotten;
        }
        else
        {
            lifeStarState = StarPointState.none;
        }

        if (GameManager.instance.itemStarPoint[stageNumber])
        {
            itemStarState = StarPointState.alreadyGotten;
        }
        else
        {
            itemStarState = StarPointState.none;
        }
    }

    void Start()
    {
        countTime = 0;
        countHacking = 0;
        itemUse = false;
        clearGold = 0;
        timeBonus = 0;
        hackingBonus = 0;
        lifeBonus = 0;

        firstClearStar = 0;
        timeStar = 0;
        hackingStar = 0;
        lifeStar = 0;
        itemStar = 0;

        StarPointCheck(stageNumber);
        clearUI.SetActive(false);
    }

    void Update()
    {
        if (StageManager.instance.isStageEnd)
        {
            return;
        }

        TimeCount();
        timeText.text = $"{countTime:N2}" + " " + countHacking.ToString() + $"\n" + "Time: " + limitTime.ToString() +" / Hacking: " + limitHacking.ToString() ;
    }

    void TimeCount()
    {
        countTime += Time.deltaTime;
    }

    public void GetScore()
    {
        clearUI.SetActive(true);

        int getGold = GoldCalc(stageNumber, countTime, countHacking, player.hp);
        int getStarPoint = StarPointCalc(stageNumber, countTime, countHacking, player.hp, itemUse);
        GameManager.instance.GetCredit(getGold, getStarPoint);

        ClearGoldUISet(getGold);
        ClearStarPointUISet(clearStarState, 0);
        ClearStarPointUISet(timeStarState, 1);
        ClearStarPointUISet(hackingStarState, 2);
        ClearStarPointUISet(lifeStarState, 3);
        ClearStarPointUISet(itemStarState, 4);
    }

    int GoldCalc(int stageNumber, float time, int hacking, int remainingLife)
    {
        clearGold = Random.Range(stageNumber * 8, stageNumber * 12);

        if (time < limitTime)
        {
            timeBonus = (int)(10 * Mathf.Sqrt(stageNumber) * Mathf.Min((limitTime / time), 3f));
        }
        if(hacking < limitHacking)
        {
            if (hacking == 0)
            {
                hackingBonus = (int)(30 * Mathf.Sqrt(stageNumber));
            }
            else
            {
                hackingBonus = (int)(10 * Mathf.Sqrt(stageNumber) * Mathf.Min((limitHacking / hacking), 3f));
            }
        }

        lifeBonus = Mathf.Min(((remainingLife - 1) * (remainingLife - 1)), 10) * stageNumber;

        return clearGold + timeBonus + hackingBonus + lifeBonus;
    }

    int StarPointCalc(int stageNumber, float time, int hacking, int remainingLife, bool itemUse)
    {
        if (!GameManager.instance.firstClear[stageNumber])
        {
            GameManager.instance.firstClear[stageNumber] = true;
            PlayerPrefs.SetInt("firstClear" + stageNumber.ToString(), 1);
            firstClearStar = stageNumber;
            clearStarState = StarPointState.newGet;
        }
        if (!GameManager.instance.timeStarPoint[stageNumber])
        {
            if(countTime < limitTime)
            {
                GameManager.instance.timeStarPoint[stageNumber] = true;
                PlayerPrefs.SetInt("timeStarPoint" + stageNumber.ToString(), 1);
                timeStar = stageNumber;
                timeStarState = StarPointState.newGet;
            }
        }
        if (!GameManager.instance.hackingStarPoint[stageNumber])
        {
            if(countHacking < limitHacking)
            {
                GameManager.instance.hackingStarPoint[stageNumber] = true;
                PlayerPrefs.SetInt("hackingStarPoint" + stageNumber.ToString(), 1);
                hackingStar = stageNumber;
                hackingStarState = StarPointState.newGet;
            }
        }
        if (!GameManager.instance.lifeStarPoint[stageNumber])
        {
            if(player.hp >= 3)
            {
                GameManager.instance.lifeStarPoint[stageNumber] = true;
                PlayerPrefs.SetInt("lifeStarPoint" + stageNumber.ToString(), 1);
                lifeStar = stageNumber;
                lifeStarState = StarPointState.newGet;
            }
        }
        if (!GameManager.instance.itemStarPoint[stageNumber])
        {
            if (!itemUse)
            {
                GameManager.instance.itemStarPoint[stageNumber] = true;
                PlayerPrefs.SetInt("itemStarPoint" + stageNumber.ToString(), 1);
                itemStar = stageNumber;
                itemStarState = StarPointState.newGet;
            }
        }

        return firstClearStar + timeStar + hackingStar + lifeStar + itemStar;
    }

    void ClearGoldUISet(int totalGold)
    {
        ClearGoldUI[0].text = clearGold.ToString();
        ClearGoldUI[1].text = timeBonus.ToString();
        ClearGoldUI[2].text = hackingBonus.ToString();
        ClearGoldUI[3].text = lifeBonus.ToString();
        ClearGoldUI[4].text = totalGold.ToString();
    }

    void ClearStarPointUISet(StarPointState state, int index)
    {
        if (state == StarPointState.alreadyGotten)
        {
            starSlider[index].fillAmount = 1.0f;
            starIcon[index].gameObject.SetActive(true);
        }
        else if (state == StarPointState.newGet)
        {
            StartCoroutine(StarPointGetAnim(index));
        }
        else if(state == StarPointState.none)
        {
            StartCoroutine(StarPointNoneAnim(index));
        }
    }

    IEnumerator StarPointGetAnim(int index)
    {
        starSlider[index].fillAmount = 0f;
        starIcon[index].gameObject.SetActive(false);

        Debug.Log(index + " 포인트 획득!");

        while (true)
        {
            starSlider[index].fillAmount += 0.01f;
            yield return new WaitForSeconds(0.02f);

            if(starSlider[index].fillAmount >= 1.0f)
            {
                break;
            }
        }

        starIcon[index].gameObject.SetActive(true);
    }

    IEnumerator StarPointNoneAnim(int index)
    {
        float gauge;
        starSlider[index].fillAmount = 0f;
        starIcon[index].gameObject.SetActive(false);

        switch (index)
        {
            case 0:
                gauge = 0f;
                break;
            case 1:
                gauge = limitTime / countTime;
                Debug.Log("시간 포인트 달성율: " + gauge);
                break;
            case 2:
                gauge = (float)limitHacking / (float)countHacking;
                Debug.Log("해킹 포인트 달성율: " + gauge);
                break;
            case 3:
                gauge = (float)player.hp / 3f;
                Debug.Log("라이프 포인트 달성율: " + gauge);
                break;
            case 4:
                gauge = 0f;
                break;
            default:
                gauge = 0f;
                break;
        }

        while (gauge >= starSlider[index].fillAmount)
        {
            starSlider[index].fillAmount += 0.01f;
            yield return new WaitForSeconds(0.02f);
        }

    }
}
