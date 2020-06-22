using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Event: 유니티 이벤트와 마찬가지로, 특정 함수 목록을 만들어두고 조건 만족 시 자동으로 발동되게 한다.
    // 각 스크립트 간의 직접적인 연결을 끊고, 수정사항이 생겼을 때 많은 스크립트를 함께 수정해야 하는 일을 방지한다.


    //플레이어의 각 능력치를 담고 있으며, 유료 아이템(부스트 팩 등) 구입 시 구입할 때마다 능력치가 증가하도록 스크립트를 만들어보자.
    //능력치 증가는 Booster.cs 스크립트에서 처리한다.

    public string playerName = "Ho!";

    public float hp = 100;
    public float defense = 50;
    public float damage = 30;

    //기존 방식
    //public Booster booster; //부스터 스크립트를 불러와서

    private void Awake()
    {
        //booster.hpBooster(this); //부스터 스크립트의 hp부스터 함수에 자기 자신을 집어넣고 발동한다.

        //이렇게 하면 부스터가 적용될 때마다 코드가 수정되어야 한다.
        //플레이어 입장에서는 부스터의 기능을 잠재적으로 갖되, 그걸 어떤 것으로 발동시킬지는 부스터 매니저가 결정하게 하는 게 좋다.
    }


    //이벤트 사용 방식
    public delegate void Boost(Character player); //캐릭터 스크립트를 가진 오브젝트를 입력받는 함수를 대행할 델리게이트 생성.
    public event Boost playerBoost; //해당 델리게이트를 변수로 가져온다.

    //여기서 event는 없어도 되지만, 코드상의 실수를 방지하는 역할을 한다. 상세하게는,
        // 1. event로 선언된 델리게이트에 함수를 추가/삭제할 수는 있지만, 다른 값으로 덮어씌울 수 없다.
        // 2. event로 선언된 델리게이트는 public이라도 player.playerBoost(player) 이 스크립트 외부에서 같은 식으로 발동시킬 수 없다.

    private void Start()
    {
        playerBoost(this); //델리게이트에 이 오브젝트를 입력한다.
        //목록에 무엇이 들어갈지는 외부에서 결정하게 하고, 목록을 실행하는 역할만 맡는다.
        //부스터 관련 함수를 가진 Booster.cs에서 사용할 함수를 추가하면 되고, 플레이어가 부스터를 알고 있을 필요는 없다.
    }

    private void Update() //작동 확인 여부를 위해 스페이스바를 누를 때마다 부스터가 실행되게 해 보자.
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerBoost(this);
        }
    }
}
