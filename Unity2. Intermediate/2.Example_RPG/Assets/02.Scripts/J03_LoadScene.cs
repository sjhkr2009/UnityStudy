using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J03_LoadScene : H01_BaseScene
{
    public override void Clear()
    {
        throw new System.NotImplementedException();
    }

    protected override void Init()
    {
        base.Init();

        for (int i = 0; i < 5; i++)
        {
            A01_Manager.Resource.Instantiate("unitychan");
        }
    }
}
