using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    ScoreManager scoreManager;
    LaserManager laserManager;
    public GameObject launcher;
    public Camera cam;
    Vector3 clickPoint;

    GameObject selectLauncherUI;
    GameObject selectItemUI;
    public bool isStageEnd;

    public enum State
    {
        Idle,Hacking,ItemUsing,GameOver,StageClear
    }
    public State state
    {
        get
        {
            return stageState;
        }
        set
        {
            switch (value)
            {
                case State.Idle:
                    stageState = value;
                    selectLauncherUI.SetActive(false);
                    selectItemUI.SetActive(false);
                    break;
                case State.Hacking:
                    stageState = value;
                    selectLauncherUI.SetActive(true);
                    break;
                case State.ItemUsing:
                    stageState = value;
                    selectItemUI.SetActive(true);
                    laserManager.ItemUsing();
                    break;
                case State.GameOver:
                    stageState = value;
                    isStageEnd = true;
                    Debug.Log("Game Over!"); //게임오버 창 출력
                    break;
                case State.StageClear:
                    stageState = value;
                    isStageEnd = true;
                    scoreManager.GetScore();
                    Debug.Log("Clear!"); //클리어 창 출력
                    break;
            }
        }
    }
    private State stageState;

    void Start()
    {
        selectLauncherUI = transform.Find("SelectLauncherUI").gameObject;
        selectItemUI = transform.Find("SelectItemUI").gameObject;
        laserManager = GetComponent<LaserManager>();
        scoreManager = GetComponent<ScoreManager>();
        state = State.Idle;
        isStageEnd = false;
    }


    void Update()
    {
        if (isStageEnd)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            clickPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPoint, Vector2.zero);

            switch (state)
            {
                case State.Idle:
                    if (hit.collider == null)
                    {
                        Debug.Log("빈 공간");
                        return;
                    }

                    switch (hit.collider.tag)
                    {
                        /*case "LaserLauncher":
                            state = State.Hacking;
                            laserManager.Hacking(hit.collider.gameObject);
                            break;*/
                        default:
                            Debug.Log("할당된 동작 없음");
                            break;
                    }
                    break;
                    
                case State.Hacking:
                    state = State.Idle;
                    if(hit.collider == null || hit.collider.tag != "LaserLauncher")
                    {
                        laserManager.HackingToIdle();
                    }
                    break;

            }
        }
    }
}
