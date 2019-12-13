using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Player : BaseUnit
{
    Rigidbody rb;
    [TabGroup("Child Components")] [SerializeField] Transform weapon;
    [TabGroup("Child Components")] [SerializeField] Transform mainShootPoint;
    [TabGroup("Child Components")] [SerializeField] GameObject soulShooter;
    [TabGroup("Stats")] [SerializeField] float shooingDelay = 0.2f;
    [TabGroup("Stats")] [SerializeField] int homingMissileCount = 8;
    [TabGroup("Stats")] [SerializeField] float skillCooldown = 5f;

    bool canShooting = true;
    bool _canHomingSkill = false;
    bool canHomingSkill
    {
        set
        {
            if (value)
            {
                soulShooter.SetActive(true);
                _canHomingSkill = true;
            }
            else if (!value)
            {
                soulShooter.SetActive(false);
                _canHomingSkill = false;
            }
        }
        get
        {
            return _canHomingSkill;
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //weapon = transform.Find("WeaponCenter");
    }

    private void Start()
    {
        StartCoroutine(SkillCooldown());
    }


    void Update()
    {
        DeadCheck();

        if (Input.GetButtonDown("Fire1") && canShooting)
        {
            GameManager.instance.PlayerShooting(mainShootPoint.position, weapon.rotation);
            StartCoroutine(ShootingCooldown());
        }

        if (Input.GetKeyDown(KeyCode.Space) && canHomingSkill)
        {
            GameManager.instance.PlayerHomingSkill(transform.position, homingMissileCount);
            StartCoroutine(SkillCooldown());
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

    IEnumerator ShootingCooldown()
    {
        canShooting = false;

        yield return new WaitForSeconds(shooingDelay);

        canShooting = true;
    }

    IEnumerator SkillCooldown()
    {
        canHomingSkill = false;

        yield return new WaitForSeconds(skillCooldown);

        canHomingSkill = true;
    }
}
