using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeSubmaterialWindow : OnSwipe
{

    protected override void Swipe()
    {
        base.Swipe();

        SelectMaterialUI root = transform.root.GetComponent<SelectMaterialUI>();

        if (root != null)
            root.MoveWindow(dragLeft);
    }
}
