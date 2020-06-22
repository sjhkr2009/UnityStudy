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
        
    }

    void Update()
    {
        
    }
}
