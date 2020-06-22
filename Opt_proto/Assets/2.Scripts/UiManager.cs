using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class UiManager : MonoBehaviour
{
    [BoxGroup("Hacking"), SerializeField] GameObject hackingMessage;
    [BoxGroup("Hacking"), SerializeField] GameObject hackingTargetImage;
    [BoxGroup("Pop-Up"), SerializeField] GameObject optionWindow;
    [BoxGroup("Pop-Up"), SerializeField] Text countdownText;

    private void Awake()
    {
        hackingMessage.SetActive(false);
        hackingTargetImage.SetActive(false);
        StartReadyCountdown();
    }

    public void StartReadyCountdown()
    {
        StartCoroutine(nameof(StartCountdown));
    }

    public void OnHacking(Launcher target)
    {
        hackingMessage.SetActive(true);
        hackingTargetImage.SetActive(true);
        hackingTargetImage.transform.position = target.transform.position;
    }

    public void ExitHacking()
    {
        hackingMessage.SetActive(false);
        hackingTargetImage.SetActive(false);
    }

    public void OnPause()
    {
        optionWindow.SetActive(true);
    }

    public void ExitPause()
    {
        if(optionWindow.activeSelf) optionWindow.SetActive(false);
    }

    IEnumerator StartCountdown()
    {
        countdownText.gameObject.SetActive(true);
        countdownText.text = "3";
        yield return new WaitForSeconds(0.8f);
        countdownText.text = "2";
        yield return new WaitForSeconds(0.8f);
        countdownText.text = "1";
        yield return new WaitForSeconds(0.8f);
        countdownText.text = "Start!!";
        yield return new WaitForSeconds(0.6f);
        countdownText.gameObject.SetActive(false);
        GameManager.Instance.gameState = GameState.Playing;
    }
}
