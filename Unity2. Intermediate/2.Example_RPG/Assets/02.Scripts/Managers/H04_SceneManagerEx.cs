using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class H04_SceneManagerEx
{
    public H01_BaseScene CurrentScene => Object.FindObjectOfType<H01_BaseScene>();

    public void LoadScene(E02_Define.Scene type)
    {
        A01_Manager.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }

    string GetSceneName(E02_Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(E02_Define.Scene), type);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
