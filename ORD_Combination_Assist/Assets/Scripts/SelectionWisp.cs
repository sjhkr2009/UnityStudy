using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionWisp : UnitBase
{
    protected override void Awake()
    {
        base.Awake();
        unitLevel = UnitLevel.SelectionWisp;
    }
}
