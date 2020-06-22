using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OverrideTest2
{
    public class Override2 : MonoBehaviour
    {
        [Button]
        public void Init()
        {
            Cat cat = new Cat();
            Duck duck = new Duck();
            MecaCat mecaCat = new MecaCat();
            MecaDuck mecaDuck = new MecaDuck();
            /*
            Animal[] animalArray = new Animal[2];
            Machine[] machineArray = new Machine[2];

            animalArray[0] = cat;
            animalArray[1] = duck;
            machineArray[0] = mecaCat;
            machineArray[1] = mecaDuck;
            //동물과 기계를 둘 다 상속받을 수는 없음.
            */
            cat.Print();
            duck.Print();
            mecaCat.Print();
            mecaDuck.Print();
        }
    }

    public class Animal : IPrintable
    {
        public virtual void Drink()
        {
            Debug.Log("마신다");
        }

        public virtual void Print()
        {
            Debug.Log("출력한다");
        }
    }

    public class Cat : Animal
    {
        public override void Drink()
        {
            Debug.Log("고양이가");
            base.Drink(); //base는 부모 클래스를 의미. 부모 클래스의 Drink() 함수의 내용을 받아온 다음, 코드를 추가할 때 사용.
        }

        public override void Print() //인터페이스 상속받는 함수들은 무조건 public이어야 한다.
        {
            Debug.Log("고양이가 마신다.");
        }
    }

    public class Duck : Animal
    {
        public override void Drink()
        {
            Debug.Log("오리가");
            base.Drink();
        }

        public override void Print() //인터페이스 상속받는 함수들은 무조건 public이어야 한다.
        {
            Debug.Log("오리가 마신다.");
        }
    }

    public class Machine :IPrintable 
    {
        public virtual void Charge()
        {
            Debug.Log("충전한다");
        }

        public virtual void Print()
        {
            Debug.Log("츌력한다");
        }
    }

    public class MecaCat : Machine
    {
        public override void Charge()
        {
            Debug.Log("로봇 고양이가");
            base.Charge();
        }

        public override void Print()
        {
            Debug.Log("머신캣이 충전한다.");
        }
    }

    public class MecaDuck : Machine
    {
        public override void Charge()
        {
            Debug.Log("로봇 오리가");
            base.Charge();
        }
        public override void Print()
        {
            Debug.Log("머신덕이 충전한다.");
        }
    }

    //마신다는 동작을 일괄 출력하기 위해 Machine, Animal을 다중 상속할수는 없다.
    //대신 여러 기능을 상속받기 위해 인터페이스를 사용한다.

    interface IPrintable //기능 하나를 인터페이스로 묶는 경우가 많다.
    {
        //int a;
        //변수 생성 불가 - 다중 상속 문제처럼, 여러 개 상속 시 어떤 변수를 받아와야 하느냐는 문제가 발생한다.

        //함수 생성 가능, 구현 불가, 접근 한정자 지정 불가
        void Print(); // 이 함수를 가지고 있을 것이라는 약속.
    }
}
