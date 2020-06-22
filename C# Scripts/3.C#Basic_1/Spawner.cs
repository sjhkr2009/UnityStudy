using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //인스턴스화: 특정 게임 오브젝트를 게임 내에서 실시간으로 생성시킨다. 원본을 등록해두고 원할 때 계속 찍어낼 수 있다.
    //이런 오브젝트는 미리 만들어 배치해두기보다, 주로 프리팹(Prefab)으로 만들어놓고 등록하여, 코드를 통해 생성한다.

    public GameObject target; //원본이 될 게임오브젝트 변수를 만들고, 유니티 내에서 드래그 앤 드롭으로 넣어준다.
    //대상을 GameObject가 아니라 Rigidbody 등 다른 이름으로 불러와도 잘 생성되지만, return 때 Rigidbody로 반환된다.
    //따라서 public Rigidbody target 을 썼다면, 아래에서 이 오브젝트를 가리킬 때도 'Rigidbody inst = Instantiate(target, spawnPosition.position, spawnPosition.rotation);'로 지정하면 되고,
    //이 때 생성된 'inst'라는 이름의 변수는 Rigidbody로 반환되었으므로 inst.AddForce(x,y,z) 와 같이 Rigidbody 내장 함수를 바로 쓸 수 있다.

    public Transform spawnPosition; //생성 위치를 나타낸다. 보통 빈 오브젝트를 만들어 위치를 정해준다.

    void Start()
    {
        //Instantiate(오브젝트,위치,회전) 순으로 작성되며, 위치와 회전은 선택사항이다.
        Instantiate(target); //해당 오브젝트를 생성한다.
        Instantiate(target, spawnPosition.position, spawnPosition.rotation); //해당 위치와 회전을 가진 오브젝트를 생성한다.
        GameObject inst = Instantiate(target, spawnPosition.position, spawnPosition.rotation); //해당 오브젝트를 변수로 가리킬 수도 있음.
        Debug.Log(inst.name); //(테스트용)

        //여기서 inst는 Rigidbody가 아니라 GameObject로 가져온 것이므로
        inst.GetComponent<Rigidbody>().AddForce(0, 500, 0);
        //이렇게 Rigidbody 컴포넌트를 가져온 다음 Rigidbody 내장 함수를 써야 한다.
    }
    
    void Update()
    {
        
    }
}
