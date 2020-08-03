using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

[CreateAssetMenu(fileName = "Scripts", menuName = "CustomDatabase/Texts", order = int.MinValue + 1)]
public class ScriptableTexts : ScriptableObject
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
        eagleScripts.Add(1, new List<string>()
        {
            "허허~ 분위기 좋구만~!! 여기 쐬주 한잔에,,,,",
            "읍,,따고~? 요즘 메뉴는,, 알아먹기도 힘들구만!!"
        });
    }
}