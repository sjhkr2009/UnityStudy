using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IWeapon
{
    public void Shoot(GameObject obj)
    {
        Debug.Log("총알 공격");

        GameObject gameObject = Instantiate(obj);
        gameObject.transform.position = transform.position;
    }
}
