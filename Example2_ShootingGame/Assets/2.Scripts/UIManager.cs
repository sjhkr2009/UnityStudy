using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [TabGroup("Ready")] [SerializeField] GameObject loadingBackground;
    [TabGroup("Ready")] [SerializeField] Slider loadingBar;
    [TabGroup("Player")] [SerializeField] Slider hpBar;
    [TabGroup("Player")] [SerializeField] Slider expBar;
    [TabGroup("Player")] [SerializeField] Text playerLevel;
    [TabGroup("Player")] [SerializeField] GameObject skillGuideText;


    public float maxHp;
    public int requiredExp;

    private void Start()
    {
        maxHp = GameManager.instance.player.maxHp;
        requiredExp = GameManager.instance.requiredExp;
    }

    private void Update()
    {
        float currentHp = GameManager.instance.player.currentHp;
        int currentExp = GameManager.instance.currentExp;

        hpBar.value = currentHp / maxHp;
        expBar.value = (float)currentExp / (float)requiredExp;
    }

    IEnumerator Loading()
    {
        loadingBackground.SetActive(true);
        loadingBar.gameObject.SetActive(true);

        float currentLoading = 0f;

        while(true)
        {
            yield return new WaitForSeconds(0.05f);

            if (currentLoading < 50f)
            {
                currentLoading += 25f;
                loadingBar.value = currentLoading;
            } else if(currentLoading < 60f)
            {
                currentLoading += 0.4f;
                loadingBar.value = currentLoading;
            } else if(currentLoading < 96f)
            {
                currentLoading += 4f;
                loadingBar.value = currentLoading;
            }
            else if(currentLoading < 100f)
            {
                currentLoading += 0.8f;
                loadingBar.value = currentLoading;
            }
            else
            {
                break;
            }
        }
        yield return new WaitForSeconds(1f);

        loadingBackground.SetActive(false);
        loadingBar.gameObject.SetActive(false);
        GameManager.instance.state = GameManager.State.Play;
    }

    public void StartLoading()
    {
        StartCoroutine(Loading());
    }
    public void LevelUp()
    {
        playerLevel.text = GameManager.instance.player.level.ToString();
    }

    public void SkillGuide(bool isOn)
    {
        if (isOn)
        {
            skillGuideText.SetActive(true);
        }
        else
        {
            skillGuideText.SetActive(false);
        }
    }
}
