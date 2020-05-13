using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZRotator : BaseRotator //BaseRotator에서 이미 MonoBehavior를 상속받았으므로, 다시 적어줄 필요는 없다.
{
    // BaseRotator를 상속받아서, speed 변수와 Rotate()함수를 사용할 수 있다.
    // Rotate()는 virtual void이므로 여기서 override void로 덮어쓸 수 있다.

    protected override void Rotate()
    {
        //부모의 함수를 그대로 가져와서 기능을 추가하기만 하고 싶다면, base.Rotate(); 를 적어준다.
        
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}
