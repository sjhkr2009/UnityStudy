using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AF_BuildManager : MonoBehaviour
{
    public AF_Race race;

    RaceFactory raceFactory;

    UnitGeneratorGround unitGeneratorGround;
    CapacityBuilding capacityBuilding;

    private void Start()
    {
        switch (race)
        {
            case AF_Race.Terran:
                raceFactory = new TerranFactory();
                break;
            case AF_Race.Protoss:
                raceFactory = new ProtossFactory();
                break;
        }

        unitGeneratorGround = raceFactory.GetUnitGeneratorGround();
        capacityBuilding = raceFactory.GetCapacityBuilding();

        unitGeneratorGround.MakeUnit();
        capacityBuilding.MakeUnit();
    }
}
