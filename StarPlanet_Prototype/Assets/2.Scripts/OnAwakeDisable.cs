using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAwakeDisable : MonoBehaviour
{
    void Awake() { if (gameObject.activeSelf) gameObject.SetActive(false); }
}
