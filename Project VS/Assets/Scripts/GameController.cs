using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour {
    public int level;
    public int kill;
    public int exp;

    public int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };

    public float gameTime;
    public float maxGameTime;
    
    private void Awake() {
        GlobalData.Controller = this;
    }

    private void Update() {
        gameTime += Time.deltaTime;
        if (gameTime > maxGameTime) gameTime = maxGameTime;
    }

    public void GainExp(int value) {
        exp += value;

        var requiredExp = level < nextExp.Length ? nextExp[level] : nextExp.Last();
        if (exp >= requiredExp) {
            level++;
            exp -= requiredExp;
        }
    }
}
