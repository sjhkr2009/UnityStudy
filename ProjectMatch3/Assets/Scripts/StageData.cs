using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 각 스테이지의 초기 정보를 담고 있는 ScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "StageData", menuName = "CustomDatabase/StageData")]
public class StageData : ScriptableObject
{
    public int width;
    public int height;
    public int[] blocks;
}
