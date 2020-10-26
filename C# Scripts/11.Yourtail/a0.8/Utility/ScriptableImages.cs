using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Images", menuName = "CustomDatabase/Images", order = int.MinValue)]
public class ScriptableImages : ScriptableObject
{
    public List<Sprite> sprites = new List<Sprite>();
}
