using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : Player
{
    [SerializeField] private float rotateSpeed;

    
    protected override void Start()
    {
        base.Start();
    }
    

    public override void Processing()
    {
        Rotate();
    }

    void Rotate()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }

}
