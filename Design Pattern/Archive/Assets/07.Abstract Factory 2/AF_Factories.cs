using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AF_Race
{
    Terran,
    Zerg,
    Protoss
}

public abstract class RaceFactory
{
    public abstract UnitGeneratorGround GetUnitGeneratorGround();
    public abstract CapacityBuilding GetCapacityBuilding();
}

public class TerranFactory : RaceFactory
{
    public override CapacityBuilding GetCapacityBuilding()
    {
        return new T_SupplyDepot();
    }

    public override UnitGeneratorGround GetUnitGeneratorGround()
    {
        return new T_Barrack();
    }
}

public class ProtossFactory : RaceFactory
{
    public override CapacityBuilding GetCapacityBuilding()
    {
        return new P_Pylon();
    }

    public override UnitGeneratorGround GetUnitGeneratorGround()
    {
        return new P_Gateway();
    }
}