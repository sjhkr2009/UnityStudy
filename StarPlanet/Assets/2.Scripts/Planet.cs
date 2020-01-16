using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : Player
{
    [SerializeField] private float rotateSpeed;
    
    protected override void Start()
    {
        base.Start();
    }

    void GetDamage(string targetTag, int damage)
    {
        if (!gameObject.CompareTag(targetTag))
        {
            return;
        }
        
        Hp -= damage;
    }

    void GetEnergy(string targetTag, int energy)
    {
        if (!gameObject.CompareTag(targetTag))
        {
            return;
        }
        Hp += energy;
    }

    public override void Processing()
    {
        Rotate();
    }

    void Rotate()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }
}
