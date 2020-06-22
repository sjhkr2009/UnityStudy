using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    SpawnManager spawnManager;
    public Transform playerGunPos;
    public float speed = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(inputX, 0f, inputY);
        if(Vector3.Distance(dir, Vector3.zero) > 1)
        {
            dir.Normalize();
        }

        rb.velocity = dir * speed * Time.deltaTime * 60f;

        if (Input.GetButtonDown("Fire1"))
        {
            spawnManager.BulletSpawn(playerGunPos.position);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other);
            Destroy(gameObject, 1f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject, 1f);
        }
    }
}
