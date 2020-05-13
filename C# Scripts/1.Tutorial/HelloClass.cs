using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloClass : MonoBehaviour
{
    
    //변수는 실재하는 대상(객체)을 가리키는 참고자(reference)이며, 실제 존재하는 것(혹은 객체 자체)이 아니다.
    //변수를 통해 무언가를 '가져와서' 쓴다는 개념을 이해하는 것이 중요!

    void Start()
    {
        Animal jack = new Animal(); //'new 클래스명()'으로 해당 클래스의 속성을 갖는 객체를 생성한다.
        jack.name = "JACK"; //jack이 가리키는 대상의 속성을 정의한다(해당 클래스가 가진 속성이어야 함)
        jack.sound = "Bark";
        jack.weight = 4.5f;

        Animal nate = new Animal();
        nate.name = "NATE";
        nate.sound = "Nyaa";
        nate.weight = 1.2f;

        Animal annie = new Animal();
        annie.name = "ANNIE";
        annie.sound = "Wee";
        annie.weight = 0.8f;

        Debug.Log(jack.name);
        Debug.Log(nate.sound);
        Debug.Log(annie.weight);


        nate = jack; //jack이 가리키던 오브젝트를 nate도 가리키게 된다. 사실상 하나의 객체를 두 이름이 가리키고 있다.
        //이 시점에서 nate가 원래 가리키던 대상의 속성에는 누구도 접근할 수 없다. 철학적으로 말하면 존재의 빛이 닿지 않는 존재자로, 사실상 없어진 것과 같다.
        //이런 속성은 더 이상 쓰이지 않는 데이터이므로 가비지 컬렉터(GC)에서 주기적으로 삭제한다.
        Debug.Log(nate.name);
        Debug.Log(jack.name);

        nate.name = "Yaong"; //여기서 nate가 가리키는 개체의 속성을 바꾸면, jack이 가리키는 개체의 속성도 바뀐다. 둘은 같은 개체를 가리키고 있기 때문이다.
        Debug.Log(nate.name);
        Debug.Log(jack.name);
    }
}

public class Animal
{
    public string name; //public이 없으면 클래스 외부에서 인식 불가 (즉 위 11번째 줄에서 jack.name 등을 지정할 수 없음)
    public string sound;
    public float weight;
}