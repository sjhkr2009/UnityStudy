using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Player : BaseUnit
{
    Rigidbody rb;
    [BoxGroup("Child Components")] [SerializeField] Transform weapon;
    [BoxGroup("Child Components")] [SerializeField] Transform mainShootPoint;
    [BoxGroup("Skill")] [SerializeField] int homingMissileCount;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //weapon = transform.Find("WeaponCenter");
    }

    
    void Update()
    {
        DeadCheck();

        if (Input.GetButtonDown("Fire1"))
        {
            GameManager.instance.PlayerShooting(mainShootPoint.position, weapon.rotation);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("여기에 실험중인 함수 입력");
            GameManager.instance.PlayerHomingSkill(transform.position, homingMissileCount);
        }

        
    }

    public void PlayerMove(float xMove, float yMove)
    {
        Vector3 dir = new Vector3(xMove, 0f, yMove);
        if(1.0f < Vector2.Distance(dir, Vector2.zero))
        {
            dir.Normalize();
        }

        rb.velocity = dir * speed * Time.deltaTime;
    }

    public void FollowMouse(Vector3 mousePos)
    {
        //Ray가 허공을 뚫는단걸 깨닫기 전까지 삽질한 시간 30분

        Vector3 dir = mousePos - transform.position;
        weapon.rotation = Quaternion.LookRotation(dir);
    }
}
