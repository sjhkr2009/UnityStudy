using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace OverrideTest1 //네임스페이스는 이름을 저장하는 공간. 네임스페이스가 다르면 클래스, 변수명 등이 겹치지 않음.
{
    public class Override : MonoBehaviour
    {

        [SerializeField] //에디터에서는 보이되 다른 코드에서는 접근하지 못 하게 한다. 변수는 이 방식이 사실상 권장된다.
        private float moveSpeed;
        void Start()
        {
            Init();
        }


        void Update()
        {

        }

        [Button] //Odin의 기능. 에디터에서 버튼으로 함수 실행 가능.
        public void Init()
        {
            Animal animal = new Animal("None");
            animal.Print();
            Cat cat = new Cat("Cat", 100);
            cat.Print();
            Dog dog = new Dog("Dog", 6);
            dog.Print();

            //다형성 (upcasting)
            Animal animalCat = cat; //자식 클래스를 부모 클래스로서 불러온다. 
            Animal animalDog = dog;

            //Dog downDog = animal; //반대로는 불가.
            Cat downCat = (Cat)animalCat; //업캐스팅한 것을 다시 다운캐스팅하는 것은 가능하다.

            //Dog downDog = (Dog)animalCat; //문법적 오류는 없으나, Cat을 Dog로 바꾸려는 것이므로 제대로 동작하지 않음.

            //이런 업캐스팅을 통해, 몬스터 클래스에 공격 기능을 넣고, 오크/골렘/고블린 등의 개별 몬스터를 몬스터로서 배열에 불러온 다음 공격 기능을 일괄 적용할 수 있다.
            //이 때 공격력이나 공격 형태 등등 공격 기능의 서로 다른 부분은 override를 통해 설정해줄 수 있다.


            //다형성이 특히 빛을 발하는 부분: 인터페이스

        }

    }

    public class Animal
    {
        private string _name; //내부 변수는 가급적 노출하지 않는다. 숨겨진 변수는 _를 붙이는 게 권장된다.
        //또는 베이스 코드이므로 자식들이 쓸 수 있게 protected로 선언해도 된다.
        public string name
        {
            get { return _name; } //이렇게 하면 animal.name을 받아올 수는 있지만 값을 변경할수는 없다. (set이 없으므로)
            //set { name = value; }
        }

        /*
        public Animal(int n) //이 클래스를 기본형인 new Animal()로 생성하지 못 하게 한다.
        {
            name = "None";
        }
        */

        public Animal(string name)
        {
            this._name = name;
        }

        public virtual void Print() //자식 클래스에서 수정하여 쓰기 위해 virtual - override를 사용한다.
        {
            Debug.Log($"{name}은 아무것도 아니다.");
        }
    }

    public class Cat : Animal
    {
        private int _height;

        public int height => _height; //위 animal처럼 내부 변수를 보호하는 방법을 축약한 것.
        // public int height{ get{return height;} } 의 축약어. get 함수는 아래와 같이 축약할 수 있고, get만 있을 경우 이렇게 2차 축약된다.

        /*public int height
        {
            height => _height; //get{return height;} 의 축약
        }*/

        public Cat(string name, int height) : base(name) //base는 상속받은 상위 클래스를 의미
        {
            this._height = height;
        }

        public void Print()
        {
            Debug.Log($"{name}는 고양이고 키는 {height}."); //이렇게 부모 클래스와 같은 이름의 함수를 선언하면, 부모 클래스의 Print()는 호출되지 않는다.
            //단, 업캐스팅해서 Animal로 불러올 경우 "Cat은 아무것도 아니다" 라고 부모 클래스의 Print()가 출력된다. 
        }
    }

    public class Dog : Animal
    {
        private int _age;
        public Dog(string name, int age) : base(name)
        {
            this._age = age;
        }

        public override void Print() //이렇게 부모 클래스의 virtual 함수를 받아와서 오버라이드해서 쓰면, 업캐스팅해서 부모 클래스로서 Print()를 불러도 오버라이드된 이 함수가 출력된다.
                                     //virtual 함수를 보면 컴퓨터는 override된 실제 함수를 찾으려 하고, 내부적으로 Lookup Table을 통해 컴퓨터가 자동으로 Dog의 함수임을 인식한다.
        {
            Debug.Log($"{name}는 개고 나이는 {_age}."); // (Dog에서는 변수를 보호하지 않았으니 _age를 직접 조작)
        }

        //일반 함수보다 virtual 함수가 좀 더 동작이 무거우므로, 불필요한 경우엔 만들지 않는다.
    }
}