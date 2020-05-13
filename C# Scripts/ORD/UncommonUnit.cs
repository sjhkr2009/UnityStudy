using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UncommonUnit : UnitBase
{
    protected override void Awake()
    {
        base.Awake(); 
        unitLevel = UnitLevel.Uncommon;
    }
}
