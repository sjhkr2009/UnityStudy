using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private string targetType;
    [SerializeField] private string avoidType;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int damage;
    [SerializeField] private int healing;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetType))
        {
            Debug.Log("Success");
            GameManager.Instance.EnemyOnCollision(targetType, healing, false);
            gameObject.SetActive(false);
        }
        else if (other.CompareTag(avoidType))
        {
            Debug.Log("Fail");
            GameManager.Instance.EnemyOnCollision(avoidType, damage, true);
            gameObject.SetActive(false);
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
