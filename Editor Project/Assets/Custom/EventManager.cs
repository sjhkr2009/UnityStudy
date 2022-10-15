using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(EventManager), menuName = "CustomParamManager/Type1")]
public class EventManager : ScriptableObject {
    public int unlockLevel;
    public int goalCount;
}
