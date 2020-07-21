using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Scripts", menuName = "CustomDatabase/Texts", order = int.MinValue + 1)]
public class ScriptableTexts : ScriptableObject
{
    [ShowInInspector] public Dictionary<int, List<string>> eagleScripts { get; private set; } = new Dictionary<int, List<string>>();
}