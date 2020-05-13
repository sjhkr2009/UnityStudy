using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test02 : MonoBehaviour
{
    //메모리(램) 자료 = 스텍과 힙으로 구분

    //스텍: 하나씩 쌓여있는 형태. 먼저 들어오는 것(제일 아래에 있는 것)이 제일 마지막에 나간다
    //스텍을 하면 1MB의 메모리 공간이 할당되고, Main()을 제일 먼저 찾는다.
    //코드에서 사용하는 변수는 모두 스텍에서 사용된다.
    //int나 float 등 자료형을 선언하면 4바이트의 공간이 할당된다. (32비트 운영체제 기준)
    //함수 내에서 지역 변수를 만들 때도 공간이 할당되지만, 함수가 끝나면 추가된 지역 변수 부분은 사용할 필요가 없으므로 스텍에서 빠진다.
    //쌓여있는 양이 1MB를 넘어가면 스택 오버플로우 에러가 난다. (for문을 수억번씩 반복하는 등)

    //힙: 스텍을 제외한 메모리 자료. 참조형(new)으로 만드는 값이 동적으로 할당된다. 스텍은 1MB이므로 큰 파일은 힙에 할당한다.
    //클래스는 스텍에 값을 저장하지 않고, 힙의 특정 영역에 할당된 값을 참조한다.
    //따라서 스텍과 달리 사용 후에도 그 클래스가 참조하는 한 값이 사라지지 않기 때문에, 메모리 공간 확보를 위해 클래스는 사용 후 Delete로 제거해 주어야 한다.

    void Start()
    {
        //구조체(struct)와 클래스(class)의 차이
        //구조체는 값 형식, 클래스는 참조 형식.
        
        TestClass testClass = new TestClass();
        testClass.n = 1;
        testClass.n2 = 2;
        TestStruct testStruct = new TestStruct();
        testStruct.n = 3;
        testStruct.n2 = 4;

        TestClass testClass2 = new TestClass(); //n, n2를 가리키고 있는(참조하는) 새로운 클래스가 생성된다.
        testClass2 = testClass; //기존에 가리키는 곳 대신 testClass가 가리키는 곳을 참조하도록 한다. 
        testClass.n = 10;
        testClass.n2 = 20; //이렇게 바꾸면 testClass2의 n, n2 값도 똑같이 바뀐다. 두 클래스는 같은 곳을 참조하고 있기 때문이다.

        //testClasss2를 처음 정의할 때 참조하게 한 대상은, 이제 더 이상 가리키는 대상이 없다.
        //아무도 접근하지 않는 곳은 (참조 횟수를 체크해서) 윈도우의 가비지 컬렉터가 제거한다. (운영체제가 메모리 공간을 확보하는 것.)

        //가비지 컬렉터는 어느 정도 쓰레기 데이터가 쌓이면 일하기 시작해서 1~2초정도 시간을 투자하여 모두 정리하는데, 가비지 컬렉터가 활동할 때는 어떤 작업도 불가능하다.
        //게임은 스택에 변수들을 할당하고 힙에 데이터들을 넣어 가리키는 식으로 진행된다.
        //스테이지가 바뀌면 쓰레기 값들이 생기고 새로운 값들을 불러오게 된다. 가비지 컬렉터가 로딩중에 일하면 좋지만 일하는 타이밍은 지정할 수 없기 때문에,
        //스테이지 진행 중 가비지 컬렉터가 일하면 렉이 걸린다.
        //따라서 게임은 C#으로 잘 동작하지 않으며, 유니티도 빌드를 하면 내부적으로 2/3 이상은 C++로 작동한다.
        
        //오브젝트 풀링: 오브젝트를 매번 제거하고 재생성하지 않고, 지속적으로 재활용한다.
            //예를 들어 몬스터를 처치하면 사라지지 않고, 잠시 후 HP 등의 값이 바뀐 채 재등장한다.
        
        TestStruct testStruct2 = new TestStruct(); //n, n2의 값을 담고 있는 새로운 구조체가 생성된다.
        testStruct2 = testStruct; //기존 값에 testStruct에 담겨 있는 값인 n=3, n2=4 가 복사된다.
        testStruct.n = 30;
        testStruct.n2 = 40; //이렇게 바꿔도 testStruct2의 n, n2의 값은 바뀌지 않고 그대로 3,4다.
    }

    //참고: MonoBehaviour을 상속받는 클래스는 new로 생성하지 말 것.
}

public class TestClass
{
    public int n;
    public int n2;

    public void Test()
    {

    }
}

public struct TestStruct
{
    public int n;
    public int n2;

    public void Test()
    {

    }
}