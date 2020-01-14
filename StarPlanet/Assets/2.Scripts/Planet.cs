using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : Player
{
    [SerializeField] private float rotateSpeed;
    
    protected override void Start()
    {
        base.Start();
        GameManager.Instance.EventGetDamage += GetDamage;
        GameManager.Instance.EventGetEnergy += GetEnergy;
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

    private void Update()
    {
        if(GameManager.Instance.gameState == GameManager.GameState.Playing)
        {
            Rotate();
        }
    }

    void Rotate()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }
}
