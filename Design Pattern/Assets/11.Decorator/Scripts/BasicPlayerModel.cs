using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Decorator {
    public class BasicPlayerModel : IPlayerModel {
        private PlayerStat status;
        private BehaviorFlag behaviorFlag;
        
        public BasicPlayerModel() {
            status = new PlayerStat() {
                attackPower = 50,
                defensePower = 30,
                hp = 500
            };
            behaviorFlag = BehaviorFlag.Normal;
        }
        public PlayerStat GetStat() {
            return status;
        }

        public BehaviorFlag GetBehaviorStatus() {
            return behaviorFlag;
        }
    }
}
