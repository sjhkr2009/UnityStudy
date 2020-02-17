using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public enum EnemyType { ToPlanet1, ToStar1, ToPlanet2, ToStar2, ToPlanet3, ToStar3, ToPlanet4, ToStar4 }

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyType enemyType;
    public EnemyType EnemyType => enemyType;

    [BoxGroup("Basic"), SerializeField] private string targetType;
    [BoxGroup("Basic"), SerializeField] private string avoidType;
    [BoxGroup("Basic"), SerializeField] private int damage;
    [BoxGroup("Basic"), SerializeField] private int healing;

    [TabGroup("Direct Move")] public float moveSpeed;

    [TabGroup("Rotate Move"), SerializeField] private float radiusReduceSpeed;
    [TabGroup("Rotate Move"), SerializeField] private float angulerSpeed;

    [TabGroup("Divide"), SerializeField] private float divideMinDistance;
    private float divideDistance;

    public event Action<Enemy, int> EventContactCorrect;
    public event Action<Enemy, int> EventContactWrong;
    public event Action<Enemy> EventOnExplosion;
    public event Action<Vector3> EventOnDivide;

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
        if(targetType != "Star" && targetType != "Planet")
        {
            if (enemyType == EnemyType.ToPlanet1 || enemyType == EnemyType.ToPlanet2 || enemyType == EnemyType.ToPlanet3 || enemyType == EnemyType.ToPlanet4) targetType = "Planet";
            else if (enemyType == EnemyType.ToStar1 || enemyType == EnemyType.ToStar2 || enemyType == EnemyType.ToStar3 || enemyType == EnemyType.ToStar4) targetType = "Star";
        }
        if (avoidType != "Star" && avoidType != "Planet")
        {
            if (enemyType == EnemyType.ToPlanet1 || enemyType == EnemyType.ToPlanet2 || enemyType == EnemyType.ToPlanet3 || enemyType == EnemyType.ToPlanet4) avoidType = "Star";
            else if (enemyType == EnemyType.ToStar1 || enemyType == EnemyType.ToStar2 || enemyType == EnemyType.ToStar3 || enemyType == EnemyType.ToStar4) avoidType = "Planet";
        }

        if (enemyType == EnemyType.ToPlanet4)
        {
            divideDistance = UnityEngine.Random.Range(divideMinDistance, divideMinDistance * 1.5f);
            transform.localScale = Vector3.one;
        }
        //else if (enemyType == EnemyType.ToStar4) moveSpeed = 1f;
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


    protected virtual void Update()
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
                Move();
                break;
            case EnemyType.ToStar3:
                RotateMove(false);
                break;
            case EnemyType.ToPlanet4:
                if (Vector3.Distance(transform.position, Vector3.zero) > divideDistance) Move();
                else Divide();
                break;
            case EnemyType.ToStar4:
                moveSpeed = Mathf.Clamp(18f / Vector3.Distance(transform.position, Vector3.zero), 1.5f, 6f);
                Move();
                break;
        }
    }

    private void Divide()
    {
        transform.DORotate(new Vector3(0f, 1080f, 0f), 2f).SetEase(Ease.InQuad);
        transform.DOScale(0.1f, 2f).SetEase(Ease.InCirc).OnComplete(OnDivideSpawn);
    }
    void OnDivideSpawn()
    {
        EventOnDivide(transform.position);
        gameObject.SetActive(false);
    }
}
