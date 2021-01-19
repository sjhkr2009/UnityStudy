using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SplashController : MonoBehaviour
{
    [SerializeField] SpriteRenderer baseImage;
    [SerializeField] SpriteRenderer featherImage;

    void Start()
    {
        DOVirtual.DelayedCall(1.5f, () =>
        {
            baseImage.DOKill();
            baseImage.DOFade(0f, 1f);
        });

        DOVirtual.DelayedCall(2.25f, () =>
        {
            featherImage.DOKill();
            featherImage.DOFade(0f, 0.5f);
        });

        DOVirtual.DelayedCall(3f, () =>
        {
            DOTween.Clear();
            SceneManager.LoadScene("02_TitleMenu");
        });
    }
}
