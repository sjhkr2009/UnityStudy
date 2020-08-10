using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 데이터만 전문적으로 저장할 스크립트

[Serializable]
public class StatData : ILoader<int, Stat>
{
    public List<Stat> stats = new List<Stat>();
    public Dictionary<int, Stat> MakeDict()
    {
        Dictionary<int, Stat> _dict = new Dictionary<int, Stat>();

        foreach (Stat stat in stats)
            _dict.Add(stat.level, stat);

        return _dict;
    }
}

[Serializable]
public class Stat
{
    public int level;
    public int hp;
    public int attack;
}