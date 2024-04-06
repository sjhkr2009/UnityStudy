using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CapacityBuilding
{
    public abstract void MakeUnit();
}

public class T_SupplyDepot : CapacityBuilding
{
    public override void MakeUnit()
    {
        Debug.Log("서플라이 건설");
    }
}

public class P_Pylon : CapacityBuilding
{
    public override void MakeUnit()
    {
        Debug.Log("파일런 건설");
    }
}