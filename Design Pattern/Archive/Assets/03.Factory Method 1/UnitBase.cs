using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UnitTypeOnFactoryMethod { Ork, Slime }


abstract class UnitBase
{
    protected UnitTypeOnFactoryMethod unitType;
    protected string name;
    protected int hp;
    protected int exp;
    public abstract void Attack();
}
