using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    enum Upgrade1 { None, Fast, Power, AddShoot }
    Upgrade1 _up1;
    Upgrade1 up1
    {
        get { return _up1; }
        set
        {
            switch (value)
            {
                case 0:
                    _up1 = 0;
                    break;
                case Upgrade1.Fast:
                    _up1 = Upgrade1.Fast;
                    break;
                case Upgrade1.Power:
                    _up1 = Upgrade1.Power;
                    break;
                case Upgrade1.AddShoot:
                    _up1 = Upgrade1.AddShoot;
                    break;
            }
        }
    }

    void Start()
    {
        up1 = 0;
    }

    void Update()
    {
        
    }
}
