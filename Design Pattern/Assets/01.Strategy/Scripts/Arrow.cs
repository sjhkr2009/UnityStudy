using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour, IWeapon
{
    //투사체 소환 동작
    public void Shoot(GameObject obj)
    {
        Debug.Log("화살 공격");

        GameObject gameObject = Instantiate(obj);
        gameObject.transform.position = transform.position;
    }
}
