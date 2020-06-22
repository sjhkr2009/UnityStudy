using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemy : MonoBehaviour
{
    public GameObject bullet;
    public float shootingDelay = 3f;

    public Transform enemyLeftGun;
    public Transform enemyRightGun;
    Transform player;

    public float speed = 1f;
    bool isComing = true;
    bool isRightMoving = true;
    float destination = 11f;

    SpawnManager spawnManager;
    void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        player = FindObjectOfType<PlayerController>().transform;
    }

    private void OnEnable()
    {
        isComing = true;
        destination = Random.Range(9f, 14f);
        float movingMode = Random.Range(0, 2);
        if(movingMode < 1)
        {
            isRightMoving = true;
        }
        else
        {
            isRightMoving = false;
        }
        StartCoroutine("CoShoot");
    }
    
    private void OnDisable()
    {
        StopCoroutine("CoShoot");
    }

    void Update()
    {
        if (isComing)
        {
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        }
        else
        {
            if (isRightMoving)
            {
                transform.Translate(transform.right * speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(-transform.right * speed * Time.deltaTime);
            }
        }


        if (transform.position.z < destination)
        {
            isComing = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            gameObject.SetActive(false);
            other.gameObject.SetActive(false);
        }
    }

    IEnumerator CoShoot()
    {
        yield return new WaitForSeconds(shootingDelay);

        while (true)
        {
            yield return null;
            //Quaternion rotation = Quaternion.LookRotation(player.position);
            Vector3 leftDir = player.position - enemyLeftGun.position;
            Vector3 rightDir = player.position - enemyRightGun.position;
            spawnManager.EnemyBulletSpawn(enemyLeftGun.position, Quaternion.LookRotation(leftDir));
            yield return new WaitForSeconds(1f);
            spawnManager.EnemyBulletSpawn(enemyRightGun.position, Quaternion.LookRotation(rightDir));

            yield return new WaitForSeconds(shootingDelay);
        }
    }
}
