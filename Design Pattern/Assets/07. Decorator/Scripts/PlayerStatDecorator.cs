using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Decorator {
    public abstract class PlayerStatDecorator : IPlayerModel {
        private IPlayerModel PlayerModel { get; }

        protected PlayerStatDecorator(IPlayerModel playerModel) {
            PlayerModel = playerModel;
        }
        
        public virtual PlayerStat GetStat() {
            return PlayerModel.GetStat();
        }

        public virtual BehaviorFlag GetBehaviorStatus() {
            return PlayerModel.GetBehaviorStatus();
        }
    }
}
