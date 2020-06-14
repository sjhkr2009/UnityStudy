using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using DG.Tweening;

public class SceneTweenManager : MonoBehaviour
{
    public static SceneTweenManager Instance { get; private set; }

    private void Start()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (fadeImage == null) fadeImage = GetComponentInChildren<CanvasGroup>();
        if (loadingUI == null) loadingUI = transform.Find("LoadingUI").gameObject;
        if(loadingText == null) loadingText = loadingUI.GetComponentInChildren<Text>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //------------------------------------------------------------------------

    [SerializeField] CanvasGroup fadeImage;
    [SerializeField] float duration = 1f;

    [SerializeField] GameObject loadingUI;
    [SerializeField] Text loadingText;

    public void ChangeScene(string sceneName)
    {
        fadeImage.DOFade(1f, duration)
            .OnStart(() =>
            {
                fadeImage.blocksRaycasts = true;
            })
            .OnComplete(() =>
            {
                StartCoroutine(nameof(SceneLoading), sceneName);
            });
    }

    IEnumerator SceneLoading(string nextScene)
    {
        loadingUI.SetActive(true);

        AsyncOperation async = SceneManager.LoadSceneAsync(nextScene);
        async.allowSceneActivation = false;

        float percentage = 0f;
        float lerpTime = 0f;

        while (!async.isDone)
        {
            yield return null;

            lerpTime += Time.deltaTime;
            if (percentage < 99f)
            {
                percentage = Mathf.Lerp(percentage, (async.progress * 100f) + 9f, lerpTime);
                if (percentage >= 99f) lerpTime = 0f;
            }
            else percentage = Mathf.Lerp(percentage, 100f, lerpTime);

            if (percentage >= 100f) async.allowSceneActivation = true;

            loadingText.text = $"{Mathf.RoundToInt(percentage)}%";

        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        fadeImage.DOFade(0f, duration)
            .OnStart(() => loadingUI.SetActive(false))
            .OnComplete(() => fadeImage.blocksRaycasts = false);
    }

}
