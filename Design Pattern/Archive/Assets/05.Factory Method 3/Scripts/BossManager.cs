using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 보스 몬스터가 특정 조건에서 이벤트 보스로 변경되어야 할 때, 기존 코드의 수정 없이 이벤트 보스 코드만 추가해서 구현할 수 있다.
// 예를 들어 설날 이벤트 때 코스튬 보스를 등장시키고 싶다면 이를 통해 기존 보스 자리에 고유의 스펙과 외형, 패턴을 가진 보스를 소환 가능하다. 
public class BossManager : MonoBehaviour
{
    BossGenerator factory;
    public Text text;

    private void Start()
    {
        factory = GetComponent<BossGenerator>();

        //보스 타입에 맞춰 UI를 바꿔준다.
        if(factory.type == BossType.Normal)         text.text = "Normal Boss";
        else if(factory.type == BossType.Special)   text.text = "Special Boss";

        //무엇이 만들어질지 여기서는 모르지만, 이 위치에(혹은 다른 트랜스폼을 지정할수도 있음) 보스를 소환한다.
        //이벤트 기간에 맞춰 BossGenerator 클래스에서 타입이 변경되었다면 다른 보스가 소환될 것이다.
        factory.CreateBoss(transform);
    }
}
