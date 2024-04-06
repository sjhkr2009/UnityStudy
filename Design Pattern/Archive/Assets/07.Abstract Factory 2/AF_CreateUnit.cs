using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitGeneratorGround
{
    public abstract void MakeUnit();
}

public class T_Barrack : UnitGeneratorGround
{
    public override void MakeUnit()
    {
        Debug.Log("배럭 생성");
    }
}

public class P_Gateway : UnitGeneratorGround
{
    public override void MakeUnit()
    {
        Debug.Log("게이트웨이 생성");
    }
}