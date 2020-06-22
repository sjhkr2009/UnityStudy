using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LaserManager : MonoBehaviour
{
    public event Action<Launcher> EventInHacking = (L) => { };
    public event Action EventOutHacking = () => { };
    public event Action<RaycastHit2D> EventOnLaserHit = (H) => { };
    public event Action EventExitLaserHit = () => { };

    [BoxGroup("Launchers"), SerializeField] List<Launcher> laserLaunchers = new List<Launcher>();
    [BoxGroup("Launchers"), SerializeField] float damage;
    [BoxGroup("Launchers"), SerializeField] LayerMask laserTarget;

    [BoxGroup("Hacking Info"), SerializeField, ReadOnly] Launcher hackingTarget;
    [BoxGroup("Hacking Info"), SerializeField, ReadOnly] int targetIndex;
    [BoxGroup("Hacking Info"), SerializeField, ReadOnly] bool _isHacking = false;
    [BoxGroup("Hacking Info"), SerializeField, ReadOnly] RaycastHit2D hit;

    bool isHackingPause = false;

    public bool IsHacking
    {
        get => _isHacking;
        set
        {
            _isHacking = value;
            if (value) EventInHacking(hackingTarget);
            else if (!value) EventOutHacking();
        }
    }

    void Awake()
    {
        if(laserLaunchers == null || laserLaunchers[0] == null)
        {
            laserLaunchers.Clear();
            Launcher[] launchers = FindObjectsOfType<Launcher>();
            foreach (var launcher in launchers) laserLaunchers.Add(launcher);
        }

        foreach (var launcher in laserLaunchers)
        {
            launcher.EventOnLauncherClick += Hacking;
        }

        EventOnLaserHit += SetDamage;
    }

    void Update()
    {
        SetLaser();
    }

    Quaternion SetRotateYToTarget(Vector2 now, Vector2 target)
    {
        Vector2 direction = target - now;
        float rotateLevel = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        return Quaternion.Euler(new Vector3(0, 0, rotateLevel));
    }

    void SetLaser()
    {
        for (int i = 0; i < (laserLaunchers.Count - 1); i++)
        {
            //발사기 별 타겟 설정 및 회전 조절
            Vector2 currentLauncher = laserLaunchers[i].shootPoint.position;
            Vector2 nextLauncher = laserLaunchers[i + 1].shootPoint.position;

            if (!laserLaunchers[i].startParticle.activeSelf) laserLaunchers[i].startParticle.SetActive(true);

            Quaternion targetRotation = SetRotateYToTarget(currentLauncher, nextLauncher);
            laserLaunchers[i].transform.rotation = Quaternion.Slerp(laserLaunchers[i].transform.rotation, targetRotation, 0.2f);

            if (i == laserLaunchers.Count - 2)
            {
                Quaternion lastRotation = SetRotateYToTarget(nextLauncher, currentLauncher);
                laserLaunchers[i + 1].transform.rotation = Quaternion.Slerp(laserLaunchers[i + 1].transform.rotation, lastRotation, 0.2f);
                laserLaunchers[i + 1].startParticle.SetActive(false);
            }
            
            LineRenderer laserDraw = laserLaunchers[i].lineRenderer;
            if (!laserDraw.enabled) laserDraw.enabled = true;
            laserDraw.startWidth = 0.2f;
            laserDraw.endWidth = 0.2f;

            laserDraw.SetPosition(0, currentLauncher);
            laserDraw.SetPosition(1, nextLauncher);


            //레이저 활성화
            hit = Physics2D.Linecast(currentLauncher, nextLauncher, laserTarget);
            
            if (hit && !isHackingPause)
            {
                laserDraw.SetPosition(1, hit.point);
                laserLaunchers[i].ParticleOnHit(hit.point);
                timeCount = 0.2f;
                EventOnLaserHit(hit);
            }
            else
            {
                laserLaunchers[i].ParticleOff();
                EventExitLaserHit();
            }
        }
        laserLaunchers[laserLaunchers.Count - 1].lineRenderer.enabled = false;
    }

    public void Hacking(Launcher launcher)
    {
        if (!IsHacking)
        {
            hackingTarget = launcher;
            targetIndex = FindLauncherInList(hackingTarget);
        }
        else if (IsHacking)
        {
            if (launcher == hackingTarget)
            {
                HackingCancel();
                IsHacking = false;
                return;
            }

            int newHackingIndex = FindLauncherInList(launcher);
            laserLaunchers[targetIndex] = launcher;
            laserLaunchers[newHackingIndex] = hackingTarget;
            HackingCancel();
            StartCoroutine(OnHackingPause());
        }
        IsHacking = !IsHacking;
    }

    int FindLauncherInList(Launcher launcher)
    {
        int index = 0;
        for (int i = 0; i < laserLaunchers.Count; i++)
        {
            if (laserLaunchers[i] == launcher) index = i;
        }
        return index;
    }

    void HackingCancel()
    {
        hackingTarget = null;
        targetIndex = 0;
    }

    IEnumerator OnHackingPause()
    {
        isHackingPause = true;
        yield return new WaitForSeconds(0.2f);
        isHackingPause = false;
    }

    float timeCount = 0f;
    void SetDamage(RaycastHit2D hitObject)
    {
        timeCount += Time.deltaTime;
        if (timeCount < 0.2f) return;
        BaseUnit unit = hitObject.collider.GetComponent<BaseUnit>();
        unit.GetDamage(damage);
    }
}
