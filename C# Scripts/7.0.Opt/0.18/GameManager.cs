using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public bool[] firstClear;
    public bool[] timeStarPoint;
    public bool[] hackingStarPoint;
    public bool[] lifeStarPoint;
    public bool[] itemStarPoint;

    public int playerStarPoint;
    public int playerGold;


    void Start()
    {
        DataLoad();
    }

    void Update()
    {
        
    }

    public void GetCredit(int gold, int starPoint)
    {
        playerGold += gold;
        playerStarPoint += starPoint;
        PlayerPrefs.SetInt("Gold", playerGold);
        PlayerPrefs.SetInt("StarPoint", playerStarPoint);
        PlayerPrefs.Save();
    }

    void DataLoad()
    {
        playerGold = PlayerPrefs.GetInt("Gold");
        playerStarPoint = PlayerPrefs.GetInt("StarPoint");

        for (int i = 0; i < firstClear.Length; i++)
        {
            int checkBool = PlayerPrefs.GetInt("firstClear" + i.ToString());
            if (checkBool == 1)
            {
                firstClear[i] = true;
            }
            else
            {
                firstClear[i] = false;
            }
        }

        for (int i = 0; i < timeStarPoint.Length; i++)
        {
            int checkBool = PlayerPrefs.GetInt("timeStarPoint" + i.ToString());
            if (checkBool == 1)
            {
                timeStarPoint[i] = true;
            }
            else
            {
                timeStarPoint[i] = false;
            }
        }

        for (int i = 0; i < hackingStarPoint.Length; i++)
        {
            int checkBool = PlayerPrefs.GetInt("hackingStarPoint" + i.ToString());
            if (checkBool == 1)
            {
                hackingStarPoint[i] = true;
            }
            else
            {
                hackingStarPoint[i] = false;
            }
        }

        for (int i = 0; i < lifeStarPoint.Length; i++)
        {
            int checkBool = PlayerPrefs.GetInt("lifeStarPoint" + i.ToString());
            if (checkBool == 1)
            {
                lifeStarPoint[i] = true;
            }
            else
            {
                lifeStarPoint[i] = false;
            }
        }

        for (int i = 0; i < itemStarPoint.Length; i++)
        {
            int checkBool = PlayerPrefs.GetInt("itemStarPoint" + i.ToString());
            if (checkBool == 1)
            {
                itemStarPoint[i] = true;
            }
            else
            {
                itemStarPoint[i] = false;
            }
        }
    }
}
