using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [BoxGroup("Playing UI")] [SerializeField] Text countdownText;
    [BoxGroup("Playing UI")] [SerializeField] GameObject orbitalRadiusUI;

    [BoxGroup("Object")] [SerializeField] Star star;


    private void Awake()
    {
        StartCoroutine(CountdownToPlay());
    }

    private void Start()
    {
        star.EventRadiusChange += RadiusChange;
    }

    void RadiusChange(float radius)
    {
        orbitalRadiusUI.transform.localScale = Vector3.one * radius * 2;
    }

    IEnumerator CountdownToPlay()
    {
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
        GameManager.Instance.gameState = GameManager.GameState.Playing;
    }
}
