using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    ScoreManager scoreManager;
    StageManager stageManager;

    //SetLaser
    public List<GameObject> laserLaunchers = new List<GameObject>();
    public GameObject[] launcher;
    LineRenderer laserDraw;
    LineRenderer lastLaserDraw;
    //bool isLaserOn; //추후 모든 레이저 끄는 효과 추가 시 사용. 단, SetLaser()를 실행하지 않아도 라인렌더러는 남아 있으니 별도로 꺼 준다.
    public LayerMask unitLayer;

    //Hacking
    public bool isHacking;
    GameObject originLauncher;
    GameObject changeLauncher;
    GameObject onClickUI;
    int originIndex;
    int changeIndex;

    //Color
    Color normalColor;
    Color invisibleColor;
    Color visibleColor;

    void Start()
    {
        //isLaserOn = true;
        isHacking = false;

        for (int i = 0; i < launcher.Length; i++)
        {
            laserLaunchers.Add(launcher[i]);
        }

        normalColor = new Color(1, 75 / 255f, 0);
        invisibleColor = new Color(0, 1, 1, 0);
        visibleColor = new Color(0, 1, 1, 0.3f);

        scoreManager = GetComponent<ScoreManager>();
        stageManager = GetComponent<StageManager>();
    }

    [System.Obsolete]
    void Update()
    {
        SetLaser();
    }

    public void ItemUsing()
    {
        HackingToIdle();
        for (int i = 0; i < laserLaunchers.Count; i++)
        {
            onClickUI = laserLaunchers[i].transform.Find("OnClick").gameObject;
            if (onClickUI)
            {
                onClickUI.SetActive(false);
            }
        }
    }

    public void HackingToIdle()
    {
        isHacking = false;
        onClickUI.SetActive(false);
    }

    public void Hacking(GameObject _launcher)
    {
        if (!isHacking)
        {
            isHacking = true;
            stageManager.state = StageManager.State.Hacking;
            onClickUI = _launcher.transform.Find("OnClick").gameObject;
            onClickUI.SetActive(true);

            originLauncher = _launcher;
            for (int i = 0; i < laserLaunchers.Count; i++)
            {
                if(laserLaunchers[i] == originLauncher)
                {
                    originIndex = i;
                }
            }
        }
        else if (isHacking)
        {
            HackingToIdle();

            if (_launcher != originLauncher)
            {
                changeLauncher = _launcher;
                for (int i = 0; i < laserLaunchers.Count; i++)
                {
                    if (laserLaunchers[i] == changeLauncher)
                    {
                        changeIndex = i;
                    }
                }
                laserLaunchers[originIndex] = changeLauncher;
                laserLaunchers[changeIndex] = originLauncher;
                scoreManager.countHacking++;
            }
        }
    }
    
    //현재 좌표에서 타겟의 좌표를 물체의 Y축으로 바라보도록 하는 회전값을 쿼터니언으로 반환한다.
    Quaternion SetRotate(Vector2 now, Vector2 target)
    {
        Vector2 direction = target - now;
        float rotateLevel = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        return Quaternion.Euler(new Vector3(0, 0, rotateLevel));
    }

    [System.Obsolete]
    void SetLaser()
    {
        for (int i = 0; i < (laserLaunchers.Count - 1); i++)
        {
            //발사기 별 타겟 설정 및 회전 조절
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

            //상태 체크 및 레이저 그리기 (비활성화면 다음 발사기로)
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

            //레이저 활성화
            RaycastHit2D hit = Physics2D.Linecast(currentLauncher, nextLauncher, unitLayer);

            if (hit)
            {
                laserDraw.SetPosition(1, hit.collider.transform.position);
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