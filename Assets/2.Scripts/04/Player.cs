using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 3f;
    public GameObject bullet;
    public Transform bulletSpawnPos;
    public Transform bulletsGroup; //public으로 선언하기 싫은 경우 GameObject.Find("Bullets").transform 으로 찾아서 불러올 수 있다.
        // transform.Find() 를 사용하면 이 오브젝트의 하위 오브젝트 중에서 찾으므로, Bullets를 찾을 수 없다.

    void Update()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(inputH, 0f, inputV);
        if(Vector3.Distance(dir, Vector3.zero) > 1) //대각선 이동 시 속도가 (루트2배) 빨라지는 현상 방지
        {
            dir.Normalize();
        }
        //Debug.Log($"H: {dir.x} / V: {dir.z}");

        transform.Translate(dir * speed * Time.deltaTime);

        if (Input.GetButtonDown("Fire1"))
        {
            GameObject instance = Instantiate(bullet, bulletSpawnPos.position, transform.rotation, bulletsGroup); //생성할 오브젝트, 생성 위치, 회전, 부모 오브젝트
            Destroy(instance, 3f);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject, 1f);
        }
    }
}
