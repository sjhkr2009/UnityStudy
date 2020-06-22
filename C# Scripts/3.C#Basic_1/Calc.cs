using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calc : MonoBehaviour
{
    //오버로드: 같은 기능을 하는 함수의 여러 가지 버전을 만드는 것.

    //여기서는 덧셈을 담당하는 스크립트를 써 보기로 한다.
    public int Sum(int a, int b)
    {
        return a + b; //'Sum(a,b)'를 입력하면 두 값의 합이 출력되는 함수.
    }

    //이 함수로는 2개의 수를 더하는 것밖에 할 수 없다.
    //그러나 2개,3개,4개,... 를 더하는 함수를 모두 다르게 만들면 외우기 복잡하며 헷갈릴 것.
    //받은 입력에 따라서 다른 결과를 내는 동일한 이름의 함수를 추가로 만들 수 있다.

    //함수 명은 같게 하되, 입력이나 출력을 달리하면 자동으로 오버로드가 구현된다.

    public int Sum(int a, int b, int c) //이름은 같게, 입력은 다르게
    {
        return a + b + c;
    }
    //이렇게 하면 Sum 뒤에 숫자 2개만 쓰면 위의 Sum이, 3개를 쓰면 아래의 Sum이 실행된다. 

    public float Sum(float a, float b, float c) //이름은 같게, 입력은 다르게(정확히는 위에선 int, 여기선 float로 출력했으므로 출력이 다름)
    {
        return a + b + c;
    }

    void Start()
    {
        Debug.Log(Sum(1, 1));
        Debug.Log(Sum(1, 2, 3));
        Debug.Log(Sum(3.14f, 3.34f, 3.54f));
    }

}
