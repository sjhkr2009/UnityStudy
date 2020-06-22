using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;

    [SerializeField, ReadOnly] UnitBase[] allUnitList;
    public List<UnitBase> requiredCommonList = new List<UnitBase>();

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        allUnitList = FindObjectsOfType<UnitBase>();
    }

    public void UnitCountReset()
    {
        foreach (var unit in allUnitList)
        {
            unit.testUnitNumber = unit.currentUnitNumber;
        }
        requiredCommonList.Clear();
    }
}
