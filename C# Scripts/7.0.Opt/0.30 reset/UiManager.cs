using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class UiManager : MonoBehaviour
{
    [SerializeField] GameObject hackingMessage;
    [SerializeField] GameObject hackingTargetImage;

    private void Start()
    {
        hackingMessage.SetActive(false);
        hackingTargetImage.SetActive(false);
    }

    public void OnHacking(Launcher target)
    {
        hackingMessage.SetActive(true);
        hackingTargetImage.SetActive(true);
        hackingTargetImage.transform.position = target.transform.position;
    }

    public void OffHacking()
    {
        hackingMessage.SetActive(false);
        hackingTargetImage.SetActive(false);
    }
}
