using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// 씬 변경 애니메이션을 호출하며 씬을 변경합니다. 필요한 FadeOut 이미지와 로딩 관련 UI를 이 스크립트가 적용된 오브젝트의 하위에 생성해 두어야 합니다.
/// 추후 스플래시 화면의 씬 상에 배치하여 싱글톤으로 설정, 씬 변경 시에도 파괴되지 않고 게임 내내 지속됩니다. 적용 시 SceneManager를 대체하도록 GameManager와 TitleMenu의 코드를 변경해 주세요.
/// </summary>
public class SceneLoadController : MonoBehaviour
{
    private static SceneLoadController _instance;
    public static SceneLoadController Instance => _instance;

    [SerializeField] Image sceneFadeOutImage;
    [SerializeField] float fadeDuration;

    [Header("Loading UI")]
    [SerializeField] GameObject loadingUI;
    [SerializeField] Text loadingText;

    private void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        if (_instance == null) _instance = this;
        if (_instance != null && _instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        sceneFadeOutImage.gameObject.SetActive(true);
        if (sceneFadeOutImage.color.a != 0f) sceneFadeOutImage.DOFade(0f, 0f);

        sceneFadeOutImage.DOFade(1f, fadeDuration).OnComplete(() =>
        {
            StartCoroutine(Loading(sceneName));
        });
    }

    IEnumerator Loading(string sceneName)
    {
        loadingUI.SetActive(true);

        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        float loadingRate = 0f;
        float loadingTime = 0f;

        while (!scene.isDone)
        {
            yield return null;
            loadingTime += Time.deltaTime;

            if (loadingRate < 90f)
            {
                loadingRate = Mathf.Lerp(loadingRate, scene.progress * 100f, loadingTime);
            }
            else if (loadingRate < 100f)
            {
                loadingRate += Time.deltaTime * UnityEngine.Random.Range(10, 30);
                if (loadingRate > 100f) loadingRate = 100f;
            }
            else if (loadingRate == 100f)
            {
                scene.allowSceneActivation = true;
                DOVirtual.DelayedCall(fadeDuration * 0.5f, OnSceneLoaded);
                break;
            }

            loadingText.text = loadingRate.ToString("0") + "%";
        }
    }

    void OnSceneLoaded()
    {
        loadingUI.SetActive(false);
        sceneFadeOutImage.DOFade(0f, fadeDuration).OnComplete(() =>
        {
            sceneFadeOutImage.gameObject.SetActive(false);
        });
    }
}
