using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRotator : BaseRotator //설명은 ZRotator.cs 참고
{
    protected override void Rotate()
    {
        transform.Rotate(speed * Time.deltaTime, 0, 0);
    }
}
