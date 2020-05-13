using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YRotator : BaseRotator //설명은 ZRotator.cs 참고
{
    protected override void Rotate()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
