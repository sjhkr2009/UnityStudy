using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class BossFactory : MonoBehaviour
{
    public abstract void CreateBoss(Transform trans);
}
