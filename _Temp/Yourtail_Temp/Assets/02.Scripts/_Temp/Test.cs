using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test : MonoBehaviour
{
    DOTweenAnimation anim;
    void Awake()
    {
        anim = gameObject.GetOrAddComponent<DOTweenAnimation>();
        anim.isFrom = !anim.isFrom;
    }

    void Update()
    {
        
    }
}
