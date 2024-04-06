using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marine : BaseUnit
{
    
    void Start()
    {
        Debug.Log("마린 생성");
    }

    public override void Move()
    {
        Debug.Log("마린 이동");
    }
}
