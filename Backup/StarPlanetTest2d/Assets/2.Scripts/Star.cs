using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Star : MonoBehaviour
{
    //enum State { Rotate, Shoot, Return }
    
    [SerializeField] Transform planet;
    [SerializeField] Rigidbody2D rb;

    [SerializeField] float originOrbitalRadius;
    float currentRadius;
    [SerializeField] float radiusChangeSpeed;
    float currentAngle;
    [SerializeField] float angulerSpeed;

    [SerializeField] float startSpeed;
    [SerializeField] float addSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float distanceFromPlanet;

    float getTargetRange = 0.4f;
    float currentSpeed;
    float CurrentSpeed
    {
        get => currentSpeed;
        set => currentSpeed = Mathf.Min(value, maxSpeed);
    }

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
            isShooting = true;
            StarShooting();
        }
    }

    void StarShooting()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        StartCoroutine(SmoothMove(mousePos));
        isReturning = false;
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
        //현재 중심부로부터의 길이와 각도 구하기
        currentRadius = Vector2.Distance(transform.position, Vector2.zero);
        currentAngle = Mathf.Atan2(transform.position.y, transform.position.x);

        //회전속도에 따라 변화할 각도를, 원래의 반지름으로 돌아가는 속도에 따라 변화할 반지름 길이를 연산
        float _targetAngle = currentAngle + angulerSpeed * Mathf.Deg2Rad * Time.deltaTime;
        float _targetRadius = Mathf.Lerp(currentRadius, originOrbitalRadius, radiusChangeSpeed);

        //다음 프레임에 이동할 위치 출력
        float _nextPosX = _targetRadius * Mathf.Cos(_targetAngle);
        float _nextPosY = _targetRadius * Mathf.Sin(_targetAngle);
        Vector2 _nextPos = new Vector2(_nextPosX, _nextPosY);

        //적용
        transform.position = _nextPos;
    }

    IEnumerator SmoothMove(Vector3 targetPoint)
    {
        Debug.Log($"Target: {targetPoint}");

        //currentSpeed = startSpeed;
        //Vector2 _dir = transform.up;
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
