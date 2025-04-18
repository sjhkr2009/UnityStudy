﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageManager : MonoBehaviour
{
    
    public LaserManager laserManager;
    public Camera cam;
    Vector3 clickPoint;

    GameObject selectUI;
    public bool isStageend;

    public static StageManager instance;

    public enum State
    {
        Idle,Hacking,GameOver,StageClear
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
                    selectUI.SetActive(false);
                    break;
                case State.Hacking:
                    stageState = value;
                    selectUI.SetActive(true);
                    break;
                case State.GameOver:
                    stageState = value;
                    isStageend = true;
                    Debug.Log("Game Over!"); //게임오버 창 출력
                    break;
                case State.StageClear:
                    stageState = value;
                    isStageend = true;
                    Debug.Log("Clear!"); //클리어 창 출력
                    break;
            }
        }
    }
    private State stageState;

    void Start()
    {
        instance = this;
        selectUI = transform.GetChild(0).gameObject;
        state = State.Idle;
        isStageend = false;
    }


    void Update()
    {
        if (isStageend)
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
                        case "LaserLauncher":
                            laserManager.Hacking(hit);
                            state = State.Hacking;
                            break;

                        default:
                            Debug.Log("할당된 동작 없음");
                            break;
                    }
                    break;

                case State.Hacking:
                    laserManager.Hacking(hit);
                    state = State.Idle;
                    break;

            }
        }
    }
}
