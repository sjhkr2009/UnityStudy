using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Animal 스크립트를 활용하여 코딩

public class AnimalTest : MonoBehaviour
{
    
    void Start()
    {
        Cat nate = new Cat(); //Cat 클래스의 변수를 하나 만든다.
        nate.name = "Nate"; //Cat 또는 Animal이 가진 변수나 함수를 사용할 수 있다.
        nate.weight = 1.5f;
        nate.year = 3;

        nate.Stealth();
        nate.Print();

        Dog jack = new Dog();
        jack.name = "Jack";
        jack.weight = 5f;
        jack.year = 2;

        jack.Hunt();
        jack.Print();

        ////////////////////////////////////////////////////////////////////////////////////////////////

        //다형성
        //상위 클래스에서 파생된 클래스에 속하는 변수들은 상위 클래스에도 해당한다. 동물이 생물에 속하는것과 비슷.

        Animal someAnimal = jack; //jack을 상위 클래스로 불러올수도 있다. 이러면 someAnimal은 Animal의 함수는 쓸 수 있지만, Dog의 고유 함수는 쓸 수 없다.
        someAnimal.Print();
        //someAnimal.Hunt(); //Dog의 함수는 사용 불가(에러)


        Dog someDog = (Dog)someAnimal; //Dog의 정보가 없어지는 건 아니기 때문에, 다시 하위 클래스로서 불러오면 다시 쓸 수 있다.
        //상위 클래스에 속하는 변수를 하위 클래스에 대입할 땐 앞에 (Dog)와 같이 강제로 대입할 것임을 알려준다. (Hello Unity 프로젝트의 'HelloCSharp.cs'파일 - 강제 형변환 항목 참고)

        //이를 활용하여, 서로 다른 클래스에 속하더라도 공통된 상위 클래스가 있다면, 아래와 같이 한번에 불러올 수 있다.
        Animal[] animals = new Animal[2];
        animals[0] = nate;
        animals[1] = jack;

        //이후 상위 클래스에서 사용하는 기능을 한꺼번에 적용한다. 예를 들어, 서로 다른 몬스터에게 몬스터의 공통된 기능을 적용할 때 사용할 수 있다.
        for (int i = 0; i < animals.Length; i++) //.Length: 해당 배열의 길이
        {
            animals[i].Print();
            // animals[i].Hunt(); //하위 클래스의 고유 기능은 사용 불가;
        }

        
    }

    
    void Update()
    {
        
    }
}
