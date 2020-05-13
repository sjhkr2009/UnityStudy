using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    //SetLaser
    List<GameObject> laserLaunchers = new List<GameObject>();
    public GameObject[] launcher;
    LineRenderer laserDraw;
    bool isLaserOn;
    public LayerMask unitLayer;

    //Hacking
    public bool isHacking;
    GameObject originLauncher;
    GameObject changeLauncher;
    GameObject onClickUI;
    Vector2 originPos;
    Vector2 changePos;

    void Start()
    {
        isLaserOn = true;
        isHacking = false;

        for (int i = 0; i < launcher.Length; i++)
        {
            laserLaunchers.Add(launcher[i]);
        }
    }

    void Update()
    {
        if (isLaserOn)
        {
            SetLaser();
        }
    }

    public void Hacking(RaycastHit2D hit)
    {
        if (!isHacking)
        {
            isHacking = true;

            onClickUI = hit.collider.transform.GetChild(0).gameObject;
            onClickUI.SetActive(true);

            originLauncher = hit.collider.gameObject;
            originPos = hit.collider.transform.position;
        }
        else if (isHacking)
        {
            isHacking = false;

            onClickUI.SetActive(false);

            if (hit.collider.tag == "LaserLauncher" && hit.collider.gameObject != originLauncher)
            {
                changePos = hit.collider.transform.position;
                originLauncher.transform.position = changePos;
                hit.collider.transform.position = originPos;
            }
        }
    }
    
    Quaternion SetRotate(Vector2 now, Vector2 target)
    {
        Vector2 direction = target - now;
        float rotateLevel = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0, 0, rotateLevel));
    }

    void SetLaser()
    {
        for (int i = 0; i < (laserLaunchers.Count - 1); i++)
        {
            Vector2 currentLauncher = laserLaunchers[i].transform.position;
            Vector2 nextLauncher = laserLaunchers[i + 1].transform.position;

            Quaternion targetRotation = SetRotate(currentLauncher, nextLauncher);
            laserLaunchers[i].transform.rotation = Quaternion.Slerp(laserLaunchers[i].transform.rotation, targetRotation, 0.2f);

            if (i == laserLaunchers.Count - 2)
            {
                Quaternion lastRotation = SetRotate(nextLauncher, currentLauncher);
                laserLaunchers[i + 1].transform.rotation = Quaternion.Slerp(laserLaunchers[i + 1].transform.rotation, lastRotation, 0.2f);
            }

            laserDraw = laserLaunchers[i].GetComponent<LineRenderer>();
            laserDraw.SetWidth(0.1f, 0.1f);

            laserDraw.SetPosition(0, currentLauncher);
            laserDraw.SetPosition(1, nextLauncher);

            RaycastHit2D hit = Physics2D.Linecast(currentLauncher, nextLauncher, unitLayer);

            if (hit)
            {
                IUnit unit = hit.collider.gameObject.GetComponent<IUnit>();
                if(unit != null)
                {
                    unit.Attacked(1);
                }
            }
        }
    }
}