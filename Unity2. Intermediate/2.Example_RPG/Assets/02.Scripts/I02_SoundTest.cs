using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I02_SoundTest : MonoBehaviour
{
    int i = 10;
    private void OnTriggerEnter(Collider other)
    {
        i++;
        i = (i % 10) + 10;
        A01_Manager.Sound.Play($"Voice/univ00{i}");
    }
}
