using System;
using UnityEngine;

namespace InternalData {
    [Serializable]
    public class EnemyExpData {
        public EnemyTier targetTier = EnemyTier.Normal;
        public int targetLevel = 0;
        public int gainExp = 1;
    }
}
