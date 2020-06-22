using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 인터페이스 구현에 사용될 스크립트. 플레이어에 대한 정보를 담고 있다.

    //hp와 골드를 아이템으로 증가시킬 수 있다고 가정한다.
    //각각 hp, 골드를 올릴 수 있는 2개 이상의 아이템이 존재할 때, 인터페이스를 통해 구현하는 법을 알아본다.
    public float hp = 50;
    public int gold = 1000;

    private Rigidbody rb;
    private float speed = 3f;
    private float jumpSpeed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update() //플레이어 이동 구현
    {
        Vector3 velocity = rb.velocity;

        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        velocity.x = inputX * speed;
        velocity.z = inputZ * speed;

        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpSpeed;
        }

        rb.velocity = velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        GoldItem goldItem = other.GetComponent<GoldItem>(); //충돌한 대상으로부터 골드 획득 함수를 가진 스크립트를 불러온다. (없을 경우 null이 됨)

        if (goldItem != null) //충돌한 대상이 이 스크립트를 가지고 있다면
        {
            goldItem.Use(); //골드 획득 함수를 발동시킨다.
        }

        HpItem hpItem = other.GetComponent<HpItem>();

        if (hpItem != null)
        {
            hpItem.Use();
        }
        */

        //위와 같이 아이템을 구현할 경우, 아이템 개수만큼 if문을 작성해야 한다.
        //아이템의 종류가 많아질수록 비효율적.

        //아이템은 모두 Use() 함수를 가지고 있다.
        //따라서 인터페이스를 통해 각 아이템들의 Use() 함수를 연결할 수 있다.
        //인터페이스에 대한 설명은 IItem.cs 스크립트 참고.

        IItem item = other.GetComponent<IItem>(); //충돌한 대상으로부터 IItem 스크립트를 불러온다. 이를 상속받은 하위 오브젝트도 불러올 수 있다. 
                                                  //IItem으로 불러왔으므로, 이 스크립트에 있는 Use() 외에 각 아이템의 고유 함수나 변수는 쓸 수 없다.
                                                  //(하위 클래스를 상위 클래스로 불러오는 것에 대해서는 AnimalTest.cs 의 다형성 관련 설명 참고)

        if (item != null) //IItem 스크립트를 불러왔다면 Use() 함수를 무조건 가지고 있을 것이므로,
        {
            item.Use(); //Use() 함수를 발동시킨다.
        }
    }

}
