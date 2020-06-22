using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedLaserLauncher : MonoBehaviour
{
    LineRenderer laserDraw;
    Enemy enemy;
    public GameObject target;
    
    public LayerMask unitLayer;
    Color startColor;
    Color endColor;

    void Start()
    {
        laserDraw = GetComponent<LineRenderer>();
        enemy = GetComponent<Enemy>();
        startColor = new Color(0, 1, 0, 1);
        endColor = new Color(0, 1, 0, 0.5f);
    }

    void Update()
    {
        if (enemy.state == Enemy.State.Idle)
        {
            SetLaser();
        }
        if (enemy.state == Enemy.State.Destroyed)
        {
            if(laserDraw != null)
            {
                Destroy(laserDraw);
            }
        }
    }

    [System.Obsolete]
    void SetLaser()
    {
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = target.transform.position;

        laserDraw.SetWidth(0.2f, 0.01f);
        laserDraw.SetColors(startColor, endColor);
        laserDraw.SetPosition(0, currentPosition);
        laserDraw.SetPosition(1, targetPosition);

        RaycastHit2D[] hits = Physics2D.LinecastAll(currentPosition, targetPosition, unitLayer);
        Debug.DrawLine(currentPosition, targetPosition, Color.red);
        
        if (hits.Length >= 1)
        {
            for(int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.tag == "Player")
                {
                    IUnit unit = hits[i].collider.gameObject.GetComponent<IUnit>();
                    if (unit != null)
                    {
                        unit.Attacked(1);
                    }
                }
            }
        }
        
        Quaternion targetRotation = SetRotate(currentPosition, targetPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.2f);
    }

    Quaternion SetRotate(Vector2 now, Vector2 target)
    {
        Vector2 direction = target - now;
        float rotateLevel = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0, 0, rotateLevel));
    }
}
