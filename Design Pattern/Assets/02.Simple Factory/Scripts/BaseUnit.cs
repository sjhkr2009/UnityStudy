using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
    public abstract void Move(); //이 추상 클래스를 상속받은 클래스는 Move를 구현해야 한다. 이후 BaseUnit을 통해 모든 종류의 유닛의 Move()를 제어할 수 있다.
}
