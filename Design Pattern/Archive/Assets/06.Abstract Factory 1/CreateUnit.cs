using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CreateUnit
{
    public abstract void MakeUnit();
}

public class Barrackks : CreateUnit
{
    public override void MakeUnit()
    {
        Debug.Log("테란 유닛 생산");
    }
}

public class Gateway : CreateUnit
{
    public override void MakeUnit()
    {
        Debug.Log("프로토스 유닛 생산");
    }
}