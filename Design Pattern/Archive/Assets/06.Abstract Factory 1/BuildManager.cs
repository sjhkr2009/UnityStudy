using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    //Abstract Factory 1 에서는 추상 팩토리를 구현하지 않은 코드를 먼저 살펴본다.

    //Factories는 일반적으로 아주 합리적인 코드인데, 유닛 생산 건물과 유닛 상한을 늘리는 건물을 나누어 각각 서브 클래스를 통해 구현하고 있기 때문이다.

    public Race race;

    void Start()
    {
        AddUnitCapacity capacityBuilding = CapacityFactory.MakeCapacityBuilding(race);
        CreateUnit unitBuilding = CreateUnitFactory.CreateUnit(race);

        capacityBuilding.Expand();
        unitBuilding.MakeUnit();
    }


    //문제점:

    /*
    
    종족별 유닛/건물이 하나뿐이거나 같은 구조를 가지고 있다면,
    위 코드에서 테란 대신 프로토스 사용 시에는 14,15줄의 인자를 Race.Protoss로만 바꾸면 된다.
    
    하지만 종족마다 (배럭과 게이트웨이 말고도) 다양한 건물을 가지고 있다면, 각각의 건물마다 팩토리 클래스가 존재할 것이다.
    이 경우 건물 수만큼 줄의 내용을 바꿔야 한다.

    또한 새로운 종족인 Zerg를 추가해야 할 때,
    각 팩토리 클래스마다 switch 문을 찾아서 case Zerg: 를 추가해야 한다.

    이를 좀 더 간편하게 수정/추가하게 하고자 추상 클래스를 사용한다.

    */ 

    //해결책:

    /*
    엘리베이터가 A 회사 혼자 제작한 것이라면 각각의 부품들도 A회사 제작일 것이다.
    이렇게 여러 객체가 관련성을 갖는 경우라면,
    각 종류별로 Factory를 사용하는 대신,
    관련된 객체들을 일관성 있게 생성하는 추상 팩토리를 적용하는 것이 편리하다.


    */
}
