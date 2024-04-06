using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AddUnitCapacity
{
    //스타크래프트처럼 유닛 최대 생성에 제한이 있을 때, 특정 건물을 통해 그 제한을 늘릴 수 있다.

    public abstract void Expand();
}


public class SupplyDepot : AddUnitCapacity
{
    public override void Expand()
    {
        Debug.Log("테란 유닛 상한 + 8");
    }
}

public class Pylon : AddUnitCapacity
{
    public override void Expand()
    {
        Debug.Log("프로토스 유닛 상한 + 8");
    }
}
