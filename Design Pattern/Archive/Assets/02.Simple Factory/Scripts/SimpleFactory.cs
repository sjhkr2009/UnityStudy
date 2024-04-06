using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitTypeOnSimpleFactory { Marine, Firebat }

public class SimpleFactory : MonoBehaviour
{
    public GameObject marine;
    public GameObject firebat;

    public GameObject CreateUnit(UnitTypeOnSimpleFactory type)
    {
        GameObject unit = null;

        float x = Random.Range(0f, 6f);
        float z = Random.Range(0f, 6f);

        switch (type)
        {
            case UnitTypeOnSimpleFactory.Marine:
                unit = Instantiate(marine, new Vector3(x, 1f, z), Quaternion.identity);
                break;
            case UnitTypeOnSimpleFactory.Firebat:
                unit = Instantiate(firebat, new Vector3(x, 2f, z), Quaternion.identity);
                break;
        }

        return unit;
    }
}
