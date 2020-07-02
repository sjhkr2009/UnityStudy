using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class HiddenUnit : UnitBase
{
    protected override void Awake()
    {
        base.Awake();
        unitLevel = UnitLevel.Hidden;
    }
}
