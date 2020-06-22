using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Launcher : MonoBehaviour
{
    [BoxGroup("Components For LaserManager")] public Transform shootPoint;
    [BoxGroup("Components For LaserManager")] public LineRenderer lineRenderer;

    public event Action<Launcher> EventOnLauncherClick = (n) => { };

    void Awake()
    {
        if (shootPoint == null) shootPoint = transform.Find("ShootPoint");
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnMouseUpAsButton()
    {
        EventOnLauncherClick(this);
    }
}
