using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //팩토리 메서드 패턴 1
    //메인 클래스는 생성 패턴이 몇 가지 있다는 것만 알고, 구체적인 생성 내용은 서브 클래스에 위임하는 패턴.
    //생성 패턴을 추가 또는 수정하고자 할 때 해당 서브 클래스만 수정하면 된다.

    //각 Generator는 추상 클래스를 통해 공통적으로, 유닛을 담을 리스트와 리스트에 생성된 개체를 추가하는 함수를 상속받는다.
    //어떤 개체를 생성할지는 각 Generator에서 정해준다. 이들은 생성 정보만을 담고 있으므로 public 으로 선언하거나 MonoBehaviour를 상속할 필요가 없다.

    public int numberOfPatterns = 2;

    GeneratorBase[] generators;


    private void Start()
    {
        generators = new GeneratorBase[numberOfPatterns];

        generators[0] = new GeneratorTypeA();
        generators[1] = new GeneratorTypeB();
    }

    public void SpawnTypeA()
    {
        generators[0].SpawnUnits();

        List<UnitBase> units = generators[0].GetUnits();
        foreach (var unit in units)
        {
            unit.Attack();
        }
    }

    public void SpawnTypeB()
    {
        generators[1].SpawnUnits();

        List<UnitBase> units = generators[1].GetUnits();
        foreach (var unit in units)
        {
            unit.Attack();
        }
    }
}
