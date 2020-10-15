using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraResolution : MonoBehaviour
{
    [SerializeField] float horizontal = 16f;
    [SerializeField] float vertical = 9f;

    private void Awake()
    {
        SetResolution();
    }

    public void SetResolution()
    {
        float targetResolution = horizontal / vertical;
        float currentResolution = (float)Screen.width / Screen.height;

        float aspectRatio = currentResolution / targetResolution;
        Rect rect = Camera.main.rect;

        if (aspectRatio < 1f)
        {
            rect.height = aspectRatio;
            rect.y = (1f - aspectRatio) / 2f;
        }
        else
        {
            float rAspectRatio = 1f / aspectRatio;
            rect.width = rAspectRatio;
            rect.x = (1f - rAspectRatio) / 2f;
        }

        Camera.main.rect = rect;
    }

    //void OnPreCull()
    //{
    //    Rect wp = Camera.main.rect;
    //    Rect nr = new Rect(0, 0, 1, 1);

    //    Camera.main.rect = nr;
    //    GL.Clear(true, true, Color.black);

    //    Camera.main.rect = wp;
    //}
}
