using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    bool[] saveTest;
    int count;
    int getIntCount;

    int a;
    int b;
    float c;

    void Start()
    {
        a = 3;
        b = 5;
        c = (float)a / (float)b;
        Debug.Log(c);

        saveTest = new bool[5];
        count = 0;
        getIntCount = 0;

        for (int i = 0; i < saveTest.Length; i++)
        {
            int checkBool = PlayerPrefs.GetInt("Test" + i.ToString());
            if (checkBool == 1)
            {
                saveTest[i] = true;
            }
            else
            {
                saveTest[i] = false;
            }
        }

        Debug.Log("시작");
        for (int i = 0; i < saveTest.Length; i++)
        {
            Debug.Log(saveTest[i]);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            saveTest[count] = true;
            PlayerPrefs.SetInt("Test" + count.ToString(), 1);
            PlayerPrefs.Save();
            for(int i = 0; i < saveTest.Length; i++)
            {
                Debug.Log(saveTest[i]);
            }
            count++;
            
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("데이터 전체 삭제 완료");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ReturnOne();
        }
    }

    int ReturnOne()
    {
        getIntCount++;
        Debug.Log(getIntCount);
        return 1;
    }
}
