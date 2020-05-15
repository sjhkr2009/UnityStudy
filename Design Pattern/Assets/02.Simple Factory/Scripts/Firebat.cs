using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firebat : BaseUnit
{
    
    void Start()
    {
        Debug.Log("파이어뱃 생성");
    }


    public override void Move()
    {
        Debug.Log("파이어뱃 이동");
    }
}
