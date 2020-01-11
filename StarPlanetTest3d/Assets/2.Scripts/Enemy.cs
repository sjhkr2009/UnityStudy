using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private string targetType;
    [SerializeField] private float moveSpeed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetType))
        {
            Debug.Log("골든 성공");
            gameObject.SetActive(false);
        }
        else if (!other.CompareTag("Enemy"))
        {
            Debug.Log("실패");
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