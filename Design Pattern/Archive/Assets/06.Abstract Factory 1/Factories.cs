using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Race
{
    Terrran,
    Protoss,
    Zerg
}

public class CapacityFactory
{
    public static AddUnitCapacity MakeCapacityBuilding(Race type)
    {
        AddUnitCapacity unitCapacity = null;

        switch (type)
        {
            case Race.Terrran:
                unitCapacity = new SupplyDepot();
                break;
            case Race.Protoss:
                unitCapacity = new Pylon();
                break;

        }

        return unitCapacity;
    }
}


public class CreateUnitFactory
{
    public static CreateUnit CreateUnit(Race type)
    {
        CreateUnit building = null;

        switch (type)
        {
            case Race.Terrran:
                building = new Barrackks();
                break;
            case Race.Protoss:
                building = new Gateway();
                break;
        }
        return building;
    }
}
