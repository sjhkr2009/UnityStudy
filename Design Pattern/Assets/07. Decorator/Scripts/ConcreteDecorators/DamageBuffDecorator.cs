using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Decorator {
    public class DamageBuffDecorator : PlayerStatDecorator {
        private readonly float buffRate;
        public DamageBuffDecorator(float rate, IPlayerModel playerModel) : base(playerModel) {
            buffRate = rate;
        }

        public override PlayerStat GetStat() {
            var origin = base.GetStat().Clone();
            origin.attackPower *= buffRate;
            return origin;
        }
    }
}
