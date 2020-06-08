using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Reflection;

public class C16_Reflection : MonoBehaviour
{
    // 16. 리플렉션(Reflection)

    // 1. 개념

    // 클래스가 가진 변수나 함수 등을, 런타임 도중에 분석할 수 있다. using System.Reflection 필요.
    // 클래스에 X-Ray를 찍는 것과 비슷한 개념.

    // 툴을 만들 때 유용하다. 당장 유니티 인스펙터에서 실시간으로 값을 변경할 수 있도록 되어있는 것도 리플렉션 기능을 이용한 것이다.
    // C#에는 있으나 C++에는 아직 구현되지 않아서, C++ 기반의 언리얼 엔진에서 툴을 만드려면 일일이 파일을 만들어서 구현해야 한다.


    // 1.1. 속성값(Attribute)
    // 컴퓨터가 알아낼 수 있는 주석. 선언된 변수나 함수 앞에 [Attribute 이름] 으로 나타낸다. 프로퍼티라고도 한다.
    // 어트리뷰트는 변수 자체에 영향을 주지는 않으나, 어트리뷰트에 따라 해당 변수나 함수를 구분한 후 리플렉션할 때 표기를 달리할 수 있다.
    //      예를 들어 Button 어트리뷰트는 인스펙터에 표시해주는 기능과 인스펙터에서 눌러 실행할 수 있는 기능을 가지고 있다.
    //          추가로 Name을 입력해서 인스펙터상의 이름을 바꿀 수 있다. 추가 기능들은 어트리뷰트에서 F12를 눌러서 확인할 수 있다.
    //      대표적으로 SerializeField는 private 변수를 인스펙터상에 보여주는 기능이 있다. 변수 자체엔 영향이 없다(즉 어트리뷰트가 있든 없든 접근 한정자는 내부적으로 계속 private이다.)
     

    [Button(Name = "테스트")]
    void Test01()
    {
        C16_Monster monster = new C16_Monster();

        Type type = monster.GetType(); //GetType은 System이나 UnityEngine의 모든 클래스에서 상속받는 Object 클래스에 존재하는 함수로, 모든 클래스에서 사용할 수 있다.

        var fields = type.GetFields(BindingFlags.Public     //GetFields에서 BindingFlags.형식 을 통해 가져올 것들을 지정할 수 있다. 여러 종류를 가져올때는 |(or)을 통해 구분한다.
            | BindingFlags.NonPublic
            | BindingFlags.Static
            | BindingFlags.Instance);

        foreach (FieldInfo field in fields)                 // 가져온 필드들을 하나씩 보며 해당 변수의 접근 한정자, 타입, 변수명 등을 알아낼 수 있다.
        {
            Debug.Log($"{field.Attributes} 변수 - {field.FieldType.Name} 타입의 {field.Name} 변수"); //각각 접근 한정자(Static 포함), 변수 타입, 변수명을 뜻한다.
            //protected는 Family, float는 single이라고도 부른다.

            if (field.IsStatic) Debug.Log($"{field.Name} 은 static 입니다."); //field.Is~~ 을 통해 특정 접근 한정자인지 체크할 수 있다.

            var attribute = field.GetCustomAttribute<C16_IsImportant>(); //어트리뷰트를 가져오려면 GetCustomAttribute<Attribute를 상속받은 클래스명> 으로 가져온다. 없다면 가져오지 못한다(null)
            if (attribute != null) Debug.Log($"{field.Name}의 Attribute는 다음과 같습니다 : {attribute.message}");
            else Debug.Log("Attribute는 없습니다.");

            Debug.Log($"-------------------------------------------------");
        }
    }
}

class C16_Monster
{
    [C16_IsImportant("이름을 입력하는 변수입니다")]
    public string name;
    protected int hp = 100;
    [C16_IsImportant("Static 입니다")] static float power; //float는 single이라고도 부른다.
    private float speed = 3f;

    void Attack() { }
}
class C16_IsImportant : Attribute //Attribute를 상속받을 경우 Attribute로서 사용할 수 있다.
{
    public string message;
    public C16_IsImportant(string text) //생성자를 통해 Attribute를 어떤 식으로 사용하게 할 지 정할 수 있다.
    {
        message = text;
    }
}