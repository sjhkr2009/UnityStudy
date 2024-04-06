using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class GeneratorTypeA : GeneratorBase
{
    public override void SpawnUnits()
    {
        units.Add(new Ork());
        units.Add(new Ork());
        units.Add(new Ork());
    }
}
