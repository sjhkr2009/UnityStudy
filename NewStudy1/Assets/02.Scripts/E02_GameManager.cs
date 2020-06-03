using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class E02_GameManager : MonoBehaviour
{
    public enum GameState
    {
        None,
        Lobby,
        Town,
        Field
    }
    private GameState gameState = GameState.None;

}
