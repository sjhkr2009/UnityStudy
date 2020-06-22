using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour {

    int count;
    public GameObject active;
    public GameObject nonActive;

    void Start () {
        nonActive.SetActive(false);

    }
	
	void Update () {

        count = PlayerMove.skillcount;
        if (count > 0)
        {
            nonActive.SetActive(false);
            active.SetActive(true);
        }
        else
        {
            active.SetActive(false);
            nonActive.SetActive(true);
        }


    }
}
