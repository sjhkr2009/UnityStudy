using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "CustomDatabase/StageData")]
public class StageData : ScriptableObject
{
    public int width;
    public int height;
    public int[] blocks;
}
