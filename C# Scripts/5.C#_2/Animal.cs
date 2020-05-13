using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//상속: 특정 클래스를 불러와 그것을 기반으로 새로운 기능을 추가한다.

public class Animal //뒤에 붙는 : MonoBehavior 역시 상속의 일종이다. MonoBehavior의 기능을 가진 채로 코드를 짜기 시작하는 것.
    //유니티의 기본 기능들이 MonoBehavior에 들어 있다. 예를 들어 GetComponent<>(), Start(), Enabled(), transform.position 등이 여기 속한다. 이것이 없으면 오브젝트에 드래그 앤 드롭으로 붙일 수 없다.
    //여기선 상속을 활용해보기 위해 Animal을 기반으로 하는 다른 클래스를 만들 예정이므로, 불필요한 MonoBehavior는 코드의 간결함을 위해 지워준다. 
{
    //동물의 이름,무게,나이를 담고 있으며, 동물의 정보를 출력하고 속도를 계산하여 반환하는 기능을 가진 Animal 함수 작성.

    public string name;
    public float weight;
    public int year;

    public void Print()
    {
        Debug.Log(name + " 몸무게: " + weight + " 나이: " + year);
    }
    
    public float GetSpeed()
    {
        float speed = 100f / (weight * year);
        return speed;
    }

    //만약 계산식을 숨기고 값만 반환하고 싶다면, 아래와 같이 계산식을 private로 선언하고 반환된 값만 public 함수로 가져와 다시 반환한다.
    private float CalcPower()
    {
        float power = weight / year;
        return power;
    }

    //만약 외부에서 접근 불가하게 하되 하위(자식) 클래스들만 접근 가능하게 하려면, protected 접근 한정자를 사용한다.
    protected float GetPower()
    {
        return CalcPower();
    }

}

//Animal을 이용하여 다른 동물 스크립트 작성 시 몸무게, 이름 등은 작성하지 않아도 됨.
public class Dog: Animal //Dog 클래스는 Animal의 기능을 모두 가진 채 시작한다.
{
    //먹이를 사냥하는 기능 추가
    public void Hunt()
    {
        float speed = GetSpeed(); // 따로 정의하지 않고도 Animal 내의 함수를 활용할 수 있다.
        float power = GetPower(); //CalcPower는 private이므로 사용할 수 없다. protected는 자식 오브젝트이므로 작동한다.
        Debug.Log(speed + " 의 속도로 달려가 " + power + " 의 힘으로 사냥했다.");

        weight += 0.5f; //name, weight 등의 변수도 접근 및 수정이 가능하다.
    }
}

public class Cat: Animal
{
    public void Stealth()
    {
        Debug.Log("숨었다");
    }
}
