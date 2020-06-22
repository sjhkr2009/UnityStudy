﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public enum EnemyType { ToPlanet1, ToStar1, ToPlanet2, ToStar2, ToPlanet3, ToStar3, ToPlanet4, ToStar4, TP3mini }
public enum EnemyTarget { ToPlanet, ToStar }

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyType enemyType;
    [SerializeField] EnemyTarget enemyTarget;
    public EnemyType EnemyType => enemyType;
    public EnemyTarget EnemyTarget => enemyTarget;

    [BoxGroup("Basic"), SerializeField] private string targetType;
    [BoxGroup("Basic"), SerializeField] private string avoidType;
    [BoxGroup("Basic"), SerializeField] private int damage;
    [BoxGroup("Basic"), SerializeField] private int healing;

    [TabGroup("Direct Move")] public float moveSpeed;

    [TabGroup("Rotate Move"), SerializeField] private float radiusReduceSpeed;
    [TabGroup("Rotate Move"), SerializeField] private float angulerSpeed;

    [TabGroup("Divide"), SerializeField] private float divideMinDistance;
    [TabGroup("Divide"), SerializeField] private float divideMaxDistance;

    [TabGroup("Random Move"), SerializeField] private float maxAngulerSpeed;
    [TabGroup("Random Move"), SerializeField] private float minMoveRadius;
    [TabGroup("Random Move"), SerializeField] private float radiusReductionOnMove;

    private float divideDistance;
    private float moveRadius;
    private bool isDivided;
    private bool isLinearMove;

    public event Action<Enemy, int> EventContactCorrect;
    public event Action<Enemy, int> EventContactWrong;
    public event Action<Enemy> EventOnExplosion;
    public event Action<Transform, ObjectPool> EventOnDivide;
    public event Action<Enemy> EventOnDistanceOver;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fever"))
        {
            EventContactCorrect(this, healing);
            return;
        }
        
        if (other.CompareTag(targetType))
        {
            EventContactCorrect(this, healing);
        }
        else if (other.CompareTag(avoidType))
        {
            EventContactWrong(this, damage);
        }
        else if (other.CompareTag("HexagonExplosion") && Vector3.Distance(transform.position, Vector3.zero) > other.transform.localScale.x * 0.66f)
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
        if (targetType != "Star" && targetType != "Planet")
        {
            if (EnemyTarget == EnemyTarget.ToPlanet) targetType = "Planet";
            else if (EnemyTarget == EnemyTarget.ToStar) targetType = "Star";
        }
        if (avoidType != "Star" && avoidType != "Planet")
        {
            if (EnemyTarget == EnemyTarget.ToPlanet) avoidType = "Star";
            else if (EnemyTarget == EnemyTarget.ToStar) avoidType = "Planet";
        }

        if (enemyType == EnemyType.ToPlanet3)
        {
            divideDistance = UnityEngine.Random.Range(divideMinDistance, divideMaxDistance);
            transform.localScale = Vector3.one;
            isDivided = false;
        }
        else if (enemyType == EnemyType.ToStar4)
        {
            moveRadius = UnityEngine.Random.Range(minMoveRadius * 0.8f, minMoveRadius * 1.2f);
            isLinearMove = true;
        }
        else if(enemyType == EnemyType.TP3mini)
        {
            moveSpeed = 1.5f;
            StartCoroutine(nameof(DistanceOverCheck));
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

    Vector3 RandomMoveTarget() //Enemy TP4의 목표 위치를 설정한다. 중심까지의 거리를 좁히며 랜덤한 지점으로 이동하되 Planet로 돌진하지 않도록 각도를 제어한다.
    {
        float currentRadius = Vector3.Distance(transform.position, Vector3.zero);
        float currentAngle = Mathf.Atan2(transform.position.z, transform.position.x);

        if(currentRadius < moveRadius) return Vector3.zero;

        float targetAngle = 0f;
        while (Mathf.Abs(targetAngle - currentAngle) < 30f * Mathf.Deg2Rad)
        {
            targetAngle = UnityEngine.Random.Range(currentAngle - maxAngulerSpeed * Mathf.Deg2Rad, currentAngle + maxAngulerSpeed * Mathf.Deg2Rad);
            if(Mathf.Abs(maxAngulerSpeed) < 50f)
            {
                targetAngle = 0f;
                break;
            }
        }
        Debug.Log($"Enemy TS4 회전각: {targetAngle * Mathf.Rad2Deg}");
        float targetRadius = currentRadius - UnityEngine.Random.Range(radiusReductionOnMove * 0.85f, radiusReductionOnMove * 1.15f);

        float targetPosX = targetRadius * Mathf.Cos(targetAngle);
        float targetPosY = targetRadius * Mathf.Sin(targetAngle);
        Vector3 targetPos = new Vector3(targetPosX, transform.position.y, targetPosY);

        return targetPos;
    }   

    void RandomMove()
    {
        if (!gameObject.activeSelf) return;
        if(isLinearMove) isLinearMove = false;

        transform.DOMove(RandomMoveTarget(), 0.55f)
            .OnComplete(() =>
            {
                if (gameObject.activeSelf) DOVirtual.DelayedCall(0.85f, RandomMove, false);
            });
    }


    void Update()
    {
        if (GameManager.Instance.gameState != GameState.Playing) return;

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
            case EnemyType.ToPlanet3:
                if (Vector3.Distance(transform.position, Vector3.zero) > divideDistance) Move();
                else if (!isDivided)
                {
                    isDivided = true;
                    Divide();
                }
                break;
            case EnemyType.ToStar3:
                RotateMove(false);
                break;
            case EnemyType.ToPlanet4:
                Move();
                break;
            case EnemyType.ToStar4:
                if (isLinearMove)
                {
                    if (Vector3.Distance(transform.position, Vector3.zero) > Camera.main.orthographicSize * 0.8f) Move();
                    else RandomMove();
                }
                break;
            case EnemyType.TP3mini:
                moveSpeed = Mathf.Clamp(moveSpeed + 1f * Time.deltaTime, 1.5f, 5f);
                Move();
                break;
        }
    }

    private void Divide()
    {
        transform.DORotate(new Vector3(0f, 1440f, 0f), 2f).SetEase(Ease.InQuad);
        transform.DOScale(0.1f, 2f).SetEase(Ease.InCirc).OnComplete(() =>
        {
            if (gameObject.activeSelf)
            {
                EventOnDivide(transform, ObjectPool.EnemyTP3mini);
                gameObject.SetActive(false);
            }
        });
    }

    IEnumerator DistanceOverCheck()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(1f);
            if (Vector3.Distance(transform.position, Vector3.zero) > Camera.main.orthographicSize * 1.5f) EventOnDistanceOver(this);
        }
    }

    private void OnDisable()
    {
        DOTween.Kill(gameObject);
        StopCoroutine(nameof(DistanceOverCheck));
    }
    private void OnDestroy()
    {
        DOTween.Kill(gameObject);
        StopCoroutine(nameof(DistanceOverCheck));
    }
}
