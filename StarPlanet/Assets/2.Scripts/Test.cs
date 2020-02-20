using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Test : MonoBehaviour
{
    [BoxGroup("Testing")] public bool isTesting;

    ParticleSystem ps;

    enum TestEnum { A, B, C, D }

    [ShowIf(nameof(isTesting))] public int number;

    [ShowIf(nameof(isTesting)), Button]
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

    [ShowIf(nameof(isTesting)), SerializeField] GameObject explosion;

    private void Start()
    {
        if (!isTesting)
        {
            gameObject.SetActive(false);
            return;
        }
        ps = GetComponent<ParticleSystem>();
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

    [Button("저장된 내용 리셋")]
    void ResetPlayerPrefs() { PlayerPrefs.DeleteAll(); }
}
