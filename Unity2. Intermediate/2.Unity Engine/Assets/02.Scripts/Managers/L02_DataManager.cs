using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 데이터 클래스가 상속받는다. 데이터를 저장할 딕셔너리를 반환하는 함수를 반드시 포함해야 한다.
/// </summary>
public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}


// 실제로 데이터를 저장하고 불러와보자.
// 데이터는 별도의 스크립트 L03_Data_Contents 에 저장한다.
public class L02_DataManager
{
    // 특정 데이터를 알고 싶을 때, GameManager.Data.StatDict와 같이 딕셔너리를 불러와서 알 수 있다.
    // 모든 데이터를 이렇게 저장하는 건 아니고, 강화확률 등 숨기고 싶은 정보는 서버-클라 공용이 아닌 서버에서만 불러오도록 설정하기도 한다.
    public Dictionary<int, Stat> StatDict { get; private set; } = new Dictionary<int, Stat>();

    public void Init()
    {
        StatDict = LoadJson<StatData, int, Stat>("StatData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = A01_Manager.Resource.Load<TextAsset>($"Data/{path}");
        Loader loader = JsonUtility.FromJson<Loader>(textAsset.text);
        return loader;
    }
}
