using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed;


    void Update()
    {
        Move();

        if(transform.position.z > 10f)
        {
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);
    }
}
