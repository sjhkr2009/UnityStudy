using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public enum EnemyType { ToPlanet1, ToStar1 }

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyType enemyType;
    public EnemyType EnemyType => enemyType;

    [SerializeField] private string targetType;
    [SerializeField] private string avoidType;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int damage;
    [SerializeField] private int healing;

    public event Action<Enemy, int> EventContactCorrect;
    public event Action<Enemy, int> EventContactWrong;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetType))
        {
            EventContactCorrect(this, healing);
        }
        else if (other.CompareTag(avoidType))
        {
            EventContactWrong(this, damage);
        }
    }


    private void Move()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }


    private void Update()
    {
        Move();
    }
}
