using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    // Property: 외부에선 변수처럼 사용하지만, 어떤 변수를 대입하느냐에 따라 특정 함수를 거쳐가게 하는 처리.
    // 다른 오브젝트가 PointTest.cs 스크립트를 통해 여기서 점수를 받아간다고 가정하자.

    //public int point; //이렇게만 하면 어디서든 포인트를 가져가 자유롭게 조정할 수 있다.
    //하지만 포인트가 마이너스가 되거나, 해킹으로 지나치게 큰 값이 되는 등 의도치 않은 값으로 바뀐다면?

    //이를 방지하기 위해, 포인트는 무조건 함수를 거쳐서 조정하도록 해 보자.

    //기존 방식은 아래와 같다.

        /*
    private int point; //포인트를 직접 조정할 수는 없게 하고

    //포인트의 최저/최고값을 넘어가는 범위의 수를 필터링해준다.
    public void SetPoint(int newPoint) //포인트의 최저/최고값을 넘어가는 범위의 수를 필터링해준다.
    {
        if(newPoint < 0)
        {
            point = 0;
        }
        else if(newPoint > 10000)
        {
            point = 10000;
        }
        else
        {
            point = newPoint;
        }
    }
    //포인트를 반환하며 로그를 띄운다.
    public int GetPoint()
    {
        Debug.Log(point);
        return point;
    }
        */

    //이후 다른 스크립트에서 pointManager.SetPoint();, int myPoint = pointManager.GetPoint(); 와 같은 방식으로 포인트를 세팅한 후 가져간다.
    //하지만 함수를 거쳐가는 방식은 번거롭다.

    //이럴 때 아래와 같이 프로퍼티를 사용한다.

    private int myPoint; // 역시 포인트를 직접 조정할 수는 없게 private로 선언한다.

    public int point //변수를 선언하고 중괄호를 사용한다. 
    {
        set //set: 외부에서 point 변수에 어떤 수를 대입하면, 그 수를 'value' 라는 이름으로 받아 set이 발동된다.
        {
            if(value < 0)
            {
                myPoint = 0; //대입한 값이 0 미만이면 0으로 처리
            }
            else
            {
                myPoint = value; //대입한 값이 0 이상이면 그대로 myPoint로 반환한다.
            }
        }
        get //get: 외부에서 다른 변수에 point 변수를 대입하거나 사용하면, point 변수는 get에서 반환시킨 값으로 대체된다.
        {
            return myPoint;
        }
    }
    //이렇게 하면 외부에서는 pointManager.point 로 변수를 조정하듯이 간단하게 불러올 수 있다.



    //참고: get만 있고 set이 없다면 외부에서 point 변수에 직접 어떤 값을 갖게 할 수 없다.
    
    //예를 들어, 테스트 오브젝트의 개수를 실시간으로 출력한다고 가정하자.
    //이 때 실수로 다른 곳에서 pointManager.count = 0; 과 같이 직접 값을 바꿔서 덮어써버리면, 오브젝트 개수를 제대로 출력할 수 없다.

    //따라서 변수 조정은 막으면서, 다른 곳에서 오브젝트 개수를 필요로 할 때만 get으로 수를 세서 반환시키도록 하자.
    public int count
    {
        get //외부에서 count 변수를 호출할 때
        {
            PointTest[] pt = FindObjectsOfType<PointTest>(); //이 스크립트를 가진 변수를 모두 찾고

            return pt.Length; //찾은 개수를 반환한다.
        }
    }
    // 이후 외부에서 pointManager.count 로 사용하면 되지만, 'pointManager.point = 5;'와 같이 등호를 통한 대입은 set이 없으므로 불가능하다.

}
