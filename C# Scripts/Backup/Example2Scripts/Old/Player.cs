﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Player : BaseUnit
{
    Rigidbody rb;
    [TabGroup("Child Components")] [SerializeField] Transform weapon;
    [TabGroup("Child Components")] [SerializeField] Transform mainShootPoint;
    [TabGroup("Child Components")] [SerializeField] GameObject soulShooter;
<<<<<<< HEAD
    [TabGroup("Child Components")] [SerializeField] SubWeaponFairy fairy;
=======
>>>>>>> parent of 96586eaa... Fairy Update
    [TabGroup("Stats")] public float maxHp = 5f;
    [TabGroup("Stats")] [SerializeField] float shooingDelay = 0.2f;
    [TabGroup("Stats")] [SerializeField] int homingMissileCount = 8;
    [TabGroup("Stats")] [SerializeField] float skillCooldown = 5f;

    public float currentHp => hp;

    int _level = 1;
    public int level
    {
        get { return _level; }
        set
        {
            _level = value;
<<<<<<< HEAD

            switch (value)
            {
                case 2:
                    maxHp++;
                    hp += maxHp/2f;
                    shootingDelay = 0.2f;
                    break;
                case 3:
                    fairy.gameObject.SetActive(true);
                    break;
                case 4:
                    maxHp++;
                    hp += maxHp / 2f;
                    speed += 50f;
                    //이동 조작감 개선
                    break;
                case 5:
                    canHomingSkill = true;
                    break;
                case 6:
                    maxHp += 2.5f;
                    hp += maxHp / 2f;
                    shootingDelay = 0.15f;
                    homingMissileCount++;
                    fairy.shootDelay -= 1f;
                    break;
                case 7:
                    shootingDelay = 0.12f;
                    speed += 50f;
                    homingMissileCount++;
                    //연사 가능
                    break;
                case 8:
                    maxHp += 2.5f;
                    StartCoroutine(AutoHealing());
                    skillCooldown -= 2f;
                    homingMissileCount++;
                    break;
                case 9:
                    skillCooldown -= 2f;
                    homingMissileCount++;
                    speed += 50f;
                    fairy.shootDelay -= 1f;
                    break;
                case 10:
                    maxHp += 5f;
                    hpAutoHeal *= 2f;
                    shootingDelay = 0.07f;
                    homingMissileCount += 3;
                    speed += 50f;
                    break;
            }
=======
>>>>>>> parent of 96586eaa... Fairy Update
        }
    }

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

    protected override void Awake()
    {
        base.Awake();
        hp = maxHp;
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
<<<<<<< HEAD
        canHomingSkill = false;
        fairy.gameObject.SetActive(false);
=======
        StartCoroutine(SkillCooldown());
>>>>>>> parent of 96586eaa... Fairy Update
    }


    void Update()
    {
        if(GameManager.instance.state != GameManager.State.Play)
        {
            return;
        }
        
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

<<<<<<< HEAD
        if(hp > maxHp)
        {
            hp = maxHp;
        }

        MoveAreaLimit();
=======
        
>>>>>>> parent of 96586eaa... Fairy Update
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.CompareTag("Enemy"))
        {
            HitParticle();
            BaseUnit targetObject = other.transform.parent.gameObject.GetComponent<BaseUnit>();
            targetObject.Attacked(1);
            Attacked(1);
        }
    }
<<<<<<< HEAD

    IEnumerator AutoHealing()
    {
        while (gameObject.activeSelf)
        {
            hp += hpAutoHeal;
            yield return new WaitForSeconds(0.5f);
        }
    }

    void MoveAreaLimit()
    {
        if (transform.position.x > 17.5f)
        {
            transform.position = new Vector3(17.5f, 0f, transform.position.z);
        }
        if (transform.position.x < -17.5f)
        {
            transform.position = new Vector3(-17.5f, 0f, transform.position.z);
        }
        if (transform.position.z > 14.5f)
        {
            transform.position = new Vector3(transform.position.x, 0f, 14.5f);
        }
        if (transform.position.z < -4.8f)
        {
            transform.position = new Vector3(transform.position.x, 0f, -4.8f);
        }
    }
=======
>>>>>>> parent of 96586eaa... Fairy Update
}
