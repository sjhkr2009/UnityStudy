using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    GameObject[] ball = new GameObject[1000];
    int index = 0;
    int count = 0;
    public float delay = 0.5f;
    Coroutine spawn;

    bool isSpawnOn = true;

    private void Start()
    {
        spawn = StartCoroutine(Spawn());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BallSpawn();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            for (int i = 0; i < ball.Length; i++)
            {
                Destroy(ball[i]);
            }
            index = 0;
            count = 0;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            /*
            if(spawn != null)
            {
                StopCoroutine(spawn);
                spawn = null;
            }
            else
            {
                spawn = StartCoroutine(Spawn());
            }
            */
            if (isSpawnOn)
            {
                isSpawnOn = false;
            }
            else
            {
                isSpawnOn = true;
            }
        }
    }

    void BallSpawn()
    {
        if(index >= 1000)
        {
            index = 0;
        }

        Vector3 pos = new Vector3(Random.Range(-10f, 10f), 5f, Random.Range(-10f, 10f));
        ball[index] = Instantiate(prefab, pos, Quaternion.identity);
        index++;
        count++;
        Debug.Log("현재 공의 개수: " + count);
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if (!isSpawnOn)
            {
                continue;
            }
            BallSpawn();
        }
    }
}
