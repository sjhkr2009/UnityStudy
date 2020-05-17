using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BossGenerator : BossFactory
{
    public BossType type;
    public GameObject normalBoss;
    public GameObject specialBoss;
    
    public override void CreateBoss(Transform trans)
    {
        GameObject boss = null;

        switch (type)
        {
            case BossType.Normal:

                boss = Instantiate(normalBoss);
                boss.transform.position = trans.position;
                boss.transform.rotation = trans.localRotation;
                break;

            case BossType.Special:
                boss = Instantiate(specialBoss);
                boss.transform.position = trans.position;
                boss.transform.rotation = trans.localRotation;
                break;
        }

    }
}
