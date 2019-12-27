using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Star : MonoBehaviour
{
    [SerializeField] Transform planet;
    [SerializeField] float rotateSpeed;

    Vector3 mousePos;
    bool isShooting;

    void Start()
    {
        isShooting = false;
    }

    void Update()
    {
        Rotate();
        //TestMove();
        if (Input.GetMouseButtonDown(0))
        {
            StarShooting();
        }
    }

    void StarShooting()
    {
        isShooting = true;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        Debug.Log(mousePos);

        if (isShooting)
        {
            transform.DOMove(mousePos, 0.35f);
            Invoke("ReturnToPlanet", 0.3f);
        }
    }

    void ReturnToPlanet()
    {
        //Vector3 dir = (planet.position - transform.position);
        //dir.Normalize();

        transform.DOMove(Vector2.Lerp(transform.position, planet.position, 0.5f), 0.35f);
        isShooting = false;
    }

    void Rotate()
    {
        //Debug.Log("이게 왜 안되지");
        if (!isShooting)
        {
            transform.RotateAround(planet.position, Vector3.forward, rotateSpeed);
        }
    }

    void TestMove()
    {
        transform.Translate(Vector2.up);
    }
}
