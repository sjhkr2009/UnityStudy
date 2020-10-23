using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F03_SetAnimState : MonoBehaviour
{
    Animator anim;

    Vector3 prevPos;

    private void Start()
    {
        anim = GetComponent<Animator>();
        prevPos = transform.position;
    }
    private void Update()
    {
        float nowSpeed = (transform.position - prevPos).magnitude;
        anim.SetFloat("Speed", nowSpeed * 10);

        prevPos = transform.position;

    }
}
