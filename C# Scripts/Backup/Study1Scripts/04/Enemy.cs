using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bullet;
    public Transform enemyLeftGun;
    public Transform enemyRightGun;
    public Transform enemyBulletsGroup;

    public float speed = 1.5f;

    public PoolManager poolManager;
    void Start()
    {
        
    }

    void Update()
    {
        /*
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject instance1 = Instantiate(bullet, enemyLeftGun.position, transform.rotation, enemyBulletsGroup);
            Destroy(instance1, 3f);
            GameObject instance2 = Instantiate(bullet, enemyRightGun.position, transform.rotation, enemyBulletsGroup);
            Destroy(instance2, 3f);
        }
        */

        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        if(transform.position.z < -5f)
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet")) //other.tag == "Bullet" 보다 훨씬 처리속도가 빠르니 가급적 CompareTag() 사용
        {
            Debug.Log("총알에 맞음");
            //Destroy(gameObject); //이 오브젝트를 지우고
            //Destroy(other.gameObject); //총알도 지운다

            //오브젝트 풀링 방식
            gameObject.SetActive(false);
            other.gameObject.SetActive(false);
        }
    }
}
