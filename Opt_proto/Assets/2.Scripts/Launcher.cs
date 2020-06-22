using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Launcher : MonoBehaviour
{
    [BoxGroup("Components For LaserManager")] public Transform shootPoint;
    [BoxGroup("Components For LaserManager")] public LineRenderer lineRenderer;
    [BoxGroup("Components For LaserManager")] public GameObject startParticle;
    [BoxGroup("Components For LaserManager")] public GameObject hitParticle;
    [BoxGroup("Components For LaserManager"), ReadOnly] public Material lineMaterial;
    public bool isInvisible;

    public event Action<Launcher> EventOnLauncherClick = (n) => { };

    void Awake()
    {
        if (shootPoint == null) shootPoint = transform.Find("ShootPoint");
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
        lineMaterial = GetComponent<LineRenderer>().material;
        lineMaterial.SetColor("_TintColor", new Color(lineMaterial.GetColor("_TintColor").r, lineMaterial.GetColor("_TintColor").g, lineMaterial.GetColor("_TintColor").b, 0.5f));
    }

    private void OnEnable()
    {
        if(isInvisible) StartCoroutine(nameof(InvisibleLaser));
    }

    private void OnMouseUpAsButton()
    {
        if(GameManager.Instance.gameState == GameState.Playing) EventOnLauncherClick(this);
    }

    public void ParticleOnHit(Vector2 pos)
    {
        //if (from != this) return;
        if (!hitParticle.activeSelf) hitParticle.SetActive(true);
        hitParticle.transform.position = pos;
    }

    public void ParticleOff()
    {
        if (hitParticle.activeSelf) hitParticle.SetActive(false);
    }

    IEnumerator InvisibleLaser()
    {
        while (true)
        {
            yield return null;
            
            for (float lineColorAlpha = 0.5f; lineColorAlpha > 0f; lineColorAlpha -= 0.05f)
            {
                lineMaterial.SetColor("_TintColor", new Color(lineMaterial.GetColor("_TintColor").r, lineMaterial.GetColor("_TintColor").g, lineMaterial.GetColor("_TintColor").b, lineColorAlpha));
                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitForSeconds(2f);

            for (float lineColorAlpha = 0f; lineColorAlpha < 0.5f; lineColorAlpha += 0.05f)
            {
                lineMaterial.SetColor("_TintColor", new Color(lineMaterial.GetColor("_TintColor").r, lineMaterial.GetColor("_TintColor").g, lineMaterial.GetColor("_TintColor").b, lineColorAlpha));
                yield return new WaitForSeconds(0.05f);
            }
        }


    }
}
