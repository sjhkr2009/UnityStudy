using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonUnit : UnitBase
{
    protected override void Awake()
    {
        base.Awake();
        unitLevel = UnitLevel.Common;
    }
}
