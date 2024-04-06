using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFactoryController : MonoBehaviour
{
    SimpleFactory simpleFactory;
    BaseUnit unit1;
    BaseUnit unit2;

    int moveCount;

    void Start()
    {
        simpleFactory = GetComponent<SimpleFactory>();
        moveCount = 0;

        unit1 = simpleFactory.CreateUnit(UnitTypeOnSimpleFactory.Marine).GetComponent<BaseUnit>();
        unit2 = simpleFactory.CreateUnit(UnitTypeOnSimpleFactory.Firebat).GetComponent<BaseUnit>();

        StartCoroutine(nameof(UnitAction));
    }

    IEnumerator UnitAction()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f);

            if (moveCount % 2 == 0) unit1.Move();
            else unit2.Move();

            moveCount++;
        }
    }
}
