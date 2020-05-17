using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossType
{
    Normal,
    Special
}

abstract class BossBase : MonoBehaviour
{
    protected BossType type;
    protected int hp;
    protected int exp;
    public abstract void Attack();
}
