using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Database", menuName = "CustomDatabase/Database", order = int.MinValue + 2)]
public class ScriptableData : ScriptableObject
{
    [ShowInInspector] public Dictionary<string, int> customerLevel { get; private set; } = new Dictionary<string, int>() { };


    public void SetLevel(string id, int level)
    {
        if (customerLevel.ContainsKey(id)) customerLevel[id] = level;
        else customerLevel.Add(id, level);
    }
}
