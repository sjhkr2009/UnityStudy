using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    //충돌을 감지하여, 골인지점에 도달 시 색상 바꾸기

    private Renderer myRenderer; //나중에 색상을 바꿔주기 위해, 자신의 Renderer를 불러온다.
    private Color originalColor; //원래 색상을 지정하기 위한 변수
    public Color endColor; //골인 지점 도달 시 바뀌는 색상을 지정하기 위한 변수. 유니티에서 색상을 지정할 것이므로 public으로 한다.

    public bool isOveraped = false; //GameManager에서 사용할 변수이므로 다른 곳에서 쓸 수 있게 public으로 한다. '충돌 여부'라는 정보를 담고 있다.


    void Start()
    {
        //private으로 선언했으므로 드래그 앤 드롭을 통해 Renderer 컴포넌트를 가져올 수 없다.
        //따라서 게임 시작 시 myRenderer에 넣을 컴포넌트를 가져오게 한다.
        myRenderer = GetComponent<Renderer>();

        originalColor = myRenderer.material.color; //원래 컬러에는 시작 시점에 이 오브젝트가 가진 색상을 저장해 둔다.
    }
    
    void Update()
    {
        
    }

    //충돌 감지 함수
    //트리거인 물체에 접촉했을 때 자동으로 실행되는 함수
    private void OnTriggerEnter(Collider other) //other은 충돌한 대상. other이 아닌 다른 것으로 바꿔도 됨.
    {
        if(other.tag == "EndPoint") //충돌한 대상(other)의 태그를 감지
        {
            myRenderer.material.color = endColor;
            //렌더러의 material(색상)에서 color 옵션을 바꿔준다.
            //미리 지정된 색상으로 바꿀 때는 Color.red 와 같이 바꾼다. 지정된 색상으로는 red, green, blue, cyan, magenta, yellow, black, white, gray 등이 있다.
            //미리 지정되지 않은 색으로 바꿀 땐 new Color(x,y,z) 로 바꾼다. x,y,z는 RGB값을 0~1 사이로 나타내므로, RGB가 (132,93,230)인 색상을 쓰려면 (132/255f, 93/255f, 230/255f)로 입력하면 된다.

            isOveraped = true; //충돌했다고 변경
        }
    }

    //충돌 해제 감지 함수
    //트리거인 물체에 접촉이 해제되었을 때 자동으로 실행되는 함수
    private void OnTriggerExit(Collider other)
    {
        myRenderer.material.color = originalColor;

        isOveraped = false; //충돌하지 않았다고 변경
    }

    //트리거인 물체에 접촉중일 때 지속적으로 실행되는 함수도 있다. 여기서는 OnTriggerEnter 대신 써도 된다.
    private void OnTriggerStay(Collider other)
    {
        myRenderer.material.color = endColor;
        isOveraped = true;
    }


    //트리거가 아닌, 충돌 가능한 물체에 접촉했을 때 실행되는 함수. 여기선 사용하지 않음.
    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
