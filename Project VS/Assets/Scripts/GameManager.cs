using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    // TODO: 싱글톤 코드 구현하기. 일단 귀찮으니 이렇게 해둠... 
    // ISingleton<T> 에서 T CreateOrLoad() 를 제공하는건...?
    private static GameManager instance;
    public static GameManager Instance => instance;
    
    [SerializeField] private PlayerController player;

    public PlayerController Player => player;

    private void Awake() {
        instance = this;
    }

    private float timer = 0f;
    private void Update() {
        timer += Time.deltaTime;
        if (timer > 0.2f) {
            timer = 0f;
            PoolManager.Get($"Enemy{Random.Range(1, 3):00}");
        }
    }
}
