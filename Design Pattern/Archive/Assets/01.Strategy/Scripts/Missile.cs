using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour, IWeapon
{
    public void Shoot(GameObject obj)
    {
        Debug.Log("미사일 공격");

        GameObject gameObject = Instantiate(obj);
        gameObject.transform.position = transform.position;
    }
}
