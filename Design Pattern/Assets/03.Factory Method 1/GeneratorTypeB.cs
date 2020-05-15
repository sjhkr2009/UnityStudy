using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class GeneratorTypeB : GeneratorBase
{
    public override void SpawnUnits()
    {
        units.Add(new Ork());
        units.Add(new Slime());
        units.Add(new Slime());
        units.Add(new Slime());
        units.Add(new Slime());
        units.Add(new Slime());
    }
}
