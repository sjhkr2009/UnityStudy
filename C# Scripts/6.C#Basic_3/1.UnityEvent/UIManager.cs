using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text playerState;

    public void OnPlayerDead()
    {
        playerState.text = "YOU DIED";
    }
}
