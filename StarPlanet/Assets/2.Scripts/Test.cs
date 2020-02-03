using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Test : MonoBehaviour
{
    [BoxGroup("Testing...")] public bool isTesting;

    enum TestEnum { A, B, C, D }

    public int number;

    [Button]
    public void TestFunction()
    {
        Debug.Log(TestEnum.A);
        Debug.Log((int)TestEnum.B);
        N10(number = 10);
    }

    void N10(int n)
    {
        Debug.Log(n + 10);
    }

    [SerializeField] GameObject explosion;

    private void Start()
    {
        explosion.SetActive(false);
    }

    private void Update()
    {
        if (!isTesting) return;
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            Boom(1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Boom(2);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Boom(3);
        }
    }

    void Boom(int n)
    {
        switch (n)
        {
            case 1:
                explosion.transform.localScale = new Vector3(3, 3, 3);
                explosion.SetActive(true);
                break;
            case 2:
                explosion.transform.localScale = new Vector3(5, 5, 5);
                explosion.SetActive(true);
                break;
            case 3:
                explosion.transform.localScale = new Vector3(7, 7, 7);
                explosion.SetActive(true);
                break;
        }
    }
}
