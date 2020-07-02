using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EternalUnit : UnitBase
{
    protected override void Awake()
    {
        base.Awake();
        unitLevel = UnitLevel.Eternal;
    }
}
