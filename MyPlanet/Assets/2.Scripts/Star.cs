using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Star : MonoBehaviour
{
    [SerializeField] Transform planet;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float rotateSpeed;
    [SerializeField] float startSpeed;
    [SerializeField] float addSpeed;
    [SerializeField] float distanceFromPlanet;

    float getTargetRange = 0.4f;

    Vector3 mousePos;
    bool isShooting;
    bool isReturning;

    void Start()
    {
        isShooting = false;
        isReturning = false;
    }

    void Update()
    {
        Rotate();
        if (Input.GetMouseButtonDown(0))
        {
            StarShooting();
        }
    }

    void StarShooting()
    {
        isShooting = true;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        

        if (isShooting)
        {
            StartCoroutine(SmoothMove(mousePos));
            isReturning = false;
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

    void ReRotate() => isShooting = false;

    void ReturnToPlanet()
    {
        StartCoroutine(SmoothMove(ReturnPoint()));
        isReturning = true;
    }

    void Rotate()
    {
        if (!isShooting)
        {
            transform.RotateAround(planet.position, Vector3.forward, rotateSpeed);
            Debug.Log("공전 중");
        }
    }

    IEnumerator SmoothMove(Vector3 targetPoint)
    {
        Debug.Log($"Target: {targetPoint}");

        Vector2 _dir = targetPoint - transform.position;
        rb.velocity += _dir.normalized * startSpeed;

        while (getTargetRange < Vector2.Distance(transform.position, targetPoint))
        {
            _dir = targetPoint - transform.position;
            rb.velocity += _dir.normalized * addSpeed;
            //yield return new WaitForSeconds(0.1f);
            yield return null;
            Debug.Log($"목표까지의 거리: {Vector2.Distance(transform.position, targetPoint)}");
        }

        Debug.Log($"{targetPoint}에 도달함");

        if (isReturning)
        {
            rb.velocity *= 0f;
            ReRotate();
        }

        for (int i = 0; i < 20; i++)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, 0.1f);
            yield return new WaitForSeconds(0.02f);
        }

        Debug.Log($"{targetPoint}에서 감속 완료");

        if (!isReturning)
        {
            ReturnToPlanet();
        }
        else
        {
            ReRotate();
        }
    }
}
