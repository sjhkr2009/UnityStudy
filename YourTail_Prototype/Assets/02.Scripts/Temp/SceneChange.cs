using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SceneChange : MonoBehaviour
{
    public string sceneName;

    public void Change(string sceneName)
    {
        SceneTweenManager.Instance.ChangeScene(sceneName);
    }
}
