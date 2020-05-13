using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    //SetLaser
    List<GameObject> laserLaunchers = new List<GameObject>();
    public GameObject[] launcher;
    LineRenderer laserDraw;
    LineRenderer lastLaserDraw;
    bool isLaserOn;
    public LayerMask unitLayer;

    //Hacking
    public bool isHacking;
    GameObject originLauncher;
    GameObject changeLauncher;
    GameObject onClickUI;
    Vector2 originPos;
    Vector2 changePos;
    int originIndex;
    int changeIndex;

    //Color
    Color normalColor;
    Color invisibleColor;
    Color visibleColor;

    void Start()
    {
        isLaserOn = true;
        isHacking = false;

        for (int i = 0; i < launcher.Length; i++)
        {
            laserLaunchers.Add(launcher[i]);
        }

        normalColor = new Color(1, 75 / 255f, 0);
        invisibleColor = new Color(0, 1, 1, 0);
        visibleColor = new Color(0, 1, 1, 0.3f);
    }

    [System.Obsolete]
    void Update()
    {
        SetLaser();
    }

    public void Hacking(RaycastHit2D hit)
    {
        
        if (!isHacking)
        {
            isHacking = true;
            onClickUI = hit.collider.transform.Find("OnClick").gameObject;
            onClickUI.SetActive(true);

            originLauncher = hit.collider.gameObject;
            for (int i = 0; i < laserLaunchers.Count; i++)
            {
                if(laserLaunchers[i] == originLauncher)
                {
                    originIndex = i;
                }
            }

            //originPos = hit.collider.transform.position;
        }
        else if (isHacking)
        {
            isHacking = false;
            onClickUI.SetActive(false);

            if (!hit)
            {
                return;
            }

            if (hit.collider.tag == "LaserLauncher" && hit.collider.gameObject != originLauncher)
            {
                changeLauncher = hit.collider.gameObject;
                for (int i = 0; i < laserLaunchers.Count; i++)
                {
                    if (laserLaunchers[i] == changeLauncher)
                    {
                        changeIndex = i;
                    }
                }

                laserLaunchers[originIndex] = changeLauncher;
                Debug.Log(laserLaunchers[originIndex].name);
                laserLaunchers[changeIndex] = originLauncher;
                Debug.Log(laserLaunchers[changeIndex].name);
                Debug.Log("0번: " + laserLaunchers[0]);
                Debug.Log("1번: " + laserLaunchers[1]);
                Debug.Log("2번: " + laserLaunchers[2]);

                //isHacking = false;
                //changePos = hit.collider.transform.position;
                //originLauncher.transform.position = changePos;
                //hit.collider.transform.position = originPos;
            }
        }
    }
    
    Quaternion SetRotate(Vector2 now, Vector2 target)
    {
        Vector2 direction = target - now;
        float rotateLevel = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0, 0, rotateLevel));
    }

    [System.Obsolete]
    void SetLaser()
    {
        for (int i = 0; i < (laserLaunchers.Count - 1); i++)
        {
            Vector2 currentLauncher = laserLaunchers[i].transform.position;
            Vector2 nextLauncher = laserLaunchers[i + 1].transform.position;
            Launcher Lstate = laserLaunchers[i].GetComponent<Launcher>();

            Quaternion targetRotation = SetRotate(currentLauncher, nextLauncher);
            laserLaunchers[i].transform.rotation = Quaternion.Slerp(laserLaunchers[i].transform.rotation, targetRotation, 0.2f);

            if (i == laserLaunchers.Count - 2)
            {
                Quaternion lastRotation = SetRotate(nextLauncher, currentLauncher);
                laserLaunchers[i + 1].transform.rotation = Quaternion.Slerp(laserLaunchers[i + 1].transform.rotation, lastRotation, 0.2f);
            }

            if(Lstate.state == Launcher.State.Inactive)
            {
                continue;
            }

            laserDraw = laserLaunchers[i].GetComponent<LineRenderer>();
            laserDraw.SetWidth(0.1f, 0.1f);

            switch (Lstate.state)
            {
                case Launcher.State.Normal:
                    laserDraw.SetColors(normalColor, normalColor);
                    break;
                case Launcher.State.Invisible:
                    laserDraw.SetColors(invisibleColor, invisibleColor);
                    break;
                case Launcher.State.Visible:
                    laserDraw.SetColors(visibleColor, visibleColor);
                    break;
            }

            laserDraw.SetPosition(0, currentLauncher);
            laserDraw.SetPosition(1, nextLauncher);

            RaycastHit2D hit = Physics2D.Linecast(currentLauncher, nextLauncher, unitLayer);
            Debug.DrawLine(currentLauncher, nextLauncher);

            if (hit)
            {
                IUnit unit = hit.collider.gameObject.GetComponent<IUnit>();
                if(unit != null)
                {
                    unit.Attacked(1);
                }
            }
        }

        //마지막 레이저 발사기는 선이 안 보이게 처리
        lastLaserDraw = laserLaunchers[laserLaunchers.Count - 1].GetComponent<LineRenderer>();
        lastLaserDraw.SetWidth(0, 0);
        lastLaserDraw.SetPosition(0, laserLaunchers[laserLaunchers.Count - 1].transform.position);
        lastLaserDraw.SetPosition(1, laserLaunchers[laserLaunchers.Count - 1].transform.position);
    }
}