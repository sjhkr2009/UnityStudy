using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
