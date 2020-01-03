using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Star : MonoBehaviour
{
    [SerializeField] Transform planet;
    [SerializeField] float rotateSpeed;
    [SerializeField] float distanceFromPlanet;

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
            transform.DOMove(mousePos, 0.5f);
            Invoke("ReturnToPlanet", 0.3f);
        }
    }

    Vector3 ReturnPoint()
    {
        Vector3 dir = (transform.position - planet.position);
        dir.Normalize();

        Vector3 dirReverse = new Vector3(-dir.y, dir.x, dir.z).normalized;
        Vector3 returnPoint = (dirReverse * distanceFromPlanet);

        return returnPoint;
    }

    void ReturnToPlanet()
    {
        transform.DOMove(ReturnPoint(), 0.5f);
        isShooting = false;
    }

    void Rotate()
    {
        transform.RotateAround(planet.position, Vector3.forward, rotateSpeed);
    }

    void TestMove()
    {
        transform.Translate(Vector2.up);
    }
}
