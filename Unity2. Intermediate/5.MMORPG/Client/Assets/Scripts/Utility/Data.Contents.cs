using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    #region Stat
    [Serializable]
    public class StatData : ILoader<int, Stat>
    {
        public List<Stat> stats = new List<Stat>();

        public Dictionary<int, Stat> LoadData()
        {
            Dictionary<int, Stat> dict = new Dictionary<int, Stat>();

            foreach (Stat stat in stats)
                dict.Add(stat.level, stat);

            return dict;
        }
    }

    [Serializable]
    public class Stat
    {
        // 데이터 클래스에는 Serializable이 필요하며, Json 파일의 Key 값과 변수명이 일치해야 한다.
        // value 형태는 무관하지만, 해당 타입에 들어갈 수 없는 값이면 크래시가 나니 주의.
        public int level;
        public int hp;
        public int attack;
        public int totalExp;
    }
    #endregion
}