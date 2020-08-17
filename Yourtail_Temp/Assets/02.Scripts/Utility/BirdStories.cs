using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

[CreateAssetMenu(fileName = "Scripts", menuName = "CustomDatabase/Texts", order = int.MinValue + 1)]
public class BirdStories : ScriptableObject
{
    [ShowInInspector] public Dictionary<int, List<string>> eagleScripts { get; private set; } = new Dictionary<int, List<string>>();

    
    public List<string> GetDialog(Customers customer, int level = -1)
    {
        if (level == -1) level = customer.Level;
        List<string> result = null;

        switch (customer.CustomerType)
        {
            case Define.CustomerType.Eagle:
                return eagleScripts.TryGetValue(level, out result) ? result : null;
            default:
                Debug.Log("해당 캐릭터와 호감도에 맞는 텍스트가 없습니다.");
                return null;
        }
    }

    public void DialogSetting()
    {
        SetEagelScript();
    }

    void SetEagelScript()
    {
        if (!eagleScripts.ContainsKey(1))
        {
            eagleScripts.Add(1, new List<string>()
            {
                "(독수리 스토리 1단계 - 1)",
                "(독수리 스토리 1단계 - 2)",
                "(독수리 스토리 1단계 - 3)"
            });
        }
        if (!eagleScripts.ContainsKey(2))
        {
            eagleScripts.Add(2, new List<string>()
            {
                "(독수리 스토리 2단계 - 1)",
                "(독수리 스토리 2단계 - 2)"
            });
        }
        if (!eagleScripts.ContainsKey(3))
        {
            eagleScripts.Add(3, new List<string>()
            {
                "(독수리 스토리 3단계 - 1)",
                "(독수리 스토리 3단계 - 2)"
            });
        }
    }
}