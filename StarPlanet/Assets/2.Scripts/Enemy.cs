using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public enum EnemyType { ToPlanet1, ToStar1, ToPlanet2, ToStar2 }

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyType enemyType;
    public EnemyType EnemyType => enemyType;

    [BoxGroup("Basic"), SerializeField] private string targetType;
    [BoxGroup("Basic"), SerializeField] private string avoidType;
    [BoxGroup("Basic"), SerializeField] private int damage;
    [BoxGroup("Basic"), SerializeField] private int healing;

    [TabGroup("Type 1"), SerializeField] private float moveSpeed;

    [TabGroup("Type 2"), SerializeField] private float radiusReduceSpeed;
    [TabGroup("Type 2"), SerializeField] private float angulerSpeed;

    public event Action<Enemy, int> EventContactCorrect;
    public event Action<Enemy, int> EventContactWrong;
    public event Action<Enemy> EventOnExplosion;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"거리: {Vector3.Distance(transform.position, Vector3.zero)} / 무효 범위: {other.transform.localScale.x * 0.7f}");
        
        if (other.CompareTag(targetType))
        {
            EventContactCorrect(this, healing);
        }
        else if (other.CompareTag(avoidType))
        {
            EventContactWrong(this, damage);
        }
        else if (other.CompareTag("HexagonExplosion") && Vector3.Distance(transform.position, Vector3.zero) > other.transform.localScale.x * 0.7f)
        {
            EventOnExplosion(this);
        }
        else if (other.CompareTag("Explosion"))
        {
            EventOnExplosion(this);
        }
    }



    private void OnEnable()
    {
        if(targetType != "Star" && targetType != "Planet")
        {
            if (enemyType == EnemyType.ToPlanet1 || enemyType == EnemyType.ToPlanet2) targetType = "Planet";
            else if (enemyType == EnemyType.ToStar1 || enemyType == EnemyType.ToStar2) targetType = "Star";
        }
        if (avoidType != "Star" && avoidType != "Planet")
        {
            if (enemyType == EnemyType.ToPlanet1 || enemyType == EnemyType.ToPlanet2) avoidType = "Star";
            else if (enemyType == EnemyType.ToStar1 || enemyType == EnemyType.ToStar2) avoidType = "Planet";
        }
    }


    private void Move()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    void RotateMove(bool isClockwise)
    {
        float currentRadius = Vector3.Distance(transform.position, Vector3.zero);
        float currentAngle = Mathf.Atan2(transform.position.z, transform.position.x);

        float _targetAngle;
        if (isClockwise) _targetAngle = currentAngle - angulerSpeed * Mathf.Deg2Rad * Time.deltaTime;
        else _targetAngle = currentAngle + angulerSpeed * Mathf.Deg2Rad * Time.deltaTime;

        float targetRadius = currentRadius - radiusReduceSpeed * Time.deltaTime;

        float _nextPosX = targetRadius * Mathf.Cos(_targetAngle);
        float _nextPosY = targetRadius * Mathf.Sin(_targetAngle);
        Vector3 _nextPos = new Vector3(_nextPosX, transform.position.y, _nextPosY);

        transform.rotation = Quaternion.LookRotation(_nextPos);
        transform.position = _nextPos;
    }


    private void Update()
    {
        switch (enemyType)
        {
            case EnemyType.ToPlanet1:
                Move();
                break;
            case EnemyType.ToStar1:
                Move();
                break;
            case EnemyType.ToPlanet2:
                RotateMove(true);
                break;
            case EnemyType.ToStar2:
                RotateMove(false);
                break;
        }
    }
}
