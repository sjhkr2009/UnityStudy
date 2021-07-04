using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TestDotween : MonoBehaviour
{
    private PathType pathtype = PathType.CubicBezier;
    [SerializeField] private DOTweenPath dotpath;
    void Start()
    {
        dotpath = GetComponent<DOTweenPath>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Key A Pressed");
            if(DOTween.IsTweening(transform, true)) transform.DOKill();
            transform.DOPath(dotpath.path, 5f);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Key S Pressed");
            transform.DOKill();
            transform.DOPath(new Vector3[]
            {
                new Vector3(0, 0, 0),
                new Vector3(3, 0, 0),
                new Vector3(0, 3, 0)
            }, 5f);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Key D Pressed");
            transform.DOKill();
            transform.DOPath(new Vector3[]
            {
                new Vector3(0, 0, 0),
                new Vector3(3, 0, 0),
                new Vector3(0, 3, 0)
            }, 5f, PathType.CubicBezier);
        }
    }
}
