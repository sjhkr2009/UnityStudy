using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BaseSubWeapon : MonoBehaviour
{
    [TabGroup("Basic")] [SerializeField] protected GameObject player;
    [TabGroup("Basic")] [SerializeField] protected Transform shootPoint;
    [TabGroup("Basic")] public float shootDelay = 1f;
    [TabGroup("Basic")] [SerializeField] protected float speed;
    
    protected void RoundMove()
    {
        transform.RotateAround(player.transform.position, Vector3.down, speed);
    }
}
