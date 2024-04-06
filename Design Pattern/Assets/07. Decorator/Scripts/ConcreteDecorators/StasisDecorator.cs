using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Decorator {
    public class StasisDecorator : PlayerStatDecorator {
        public StasisDecorator(IPlayerModel playerModel) : base(playerModel) { }

        public override BehaviorFlag GetBehaviorStatus() {
            // cannot move, attack, skill
            return BehaviorFlag.Invulnerable;
        }
    }
}
