using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Decorator {
    public class HpBuffDecorator : PlayerStatDecorator {
        private readonly float buffValue;
        public HpBuffDecorator(float addValue, IPlayerModel playerModel) : base(playerModel) {
            buffValue = addValue;
        }

        public override PlayerStat GetStat() {
            var origin = base.GetStat().Clone();
            origin.hp += buffValue;
            return origin;
        }
    }
}
