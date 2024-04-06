using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class GeneratorBase
{
    public List<UnitBase> units = new List<UnitBase>();

    public List<UnitBase> GetUnits()
    {
        return units;
    }

    public abstract void SpawnUnits();
}
