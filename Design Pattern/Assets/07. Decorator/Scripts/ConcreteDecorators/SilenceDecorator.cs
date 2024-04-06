using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Decorator {
    public class SilenceDecorator : PlayerStatDecorator {
        public SilenceDecorator(IPlayerModel playerModel) : base(playerModel) { }

        public override BehaviorFlag GetBehaviorStatus() {
            var origin = base.GetBehaviorStatus();
            return (origin & BehaviorFlag.CanSkill) > 0 ? 
                origin ^ BehaviorFlag.CanSkill : origin;
        }
    }
}
