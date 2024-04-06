using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adapter {
    public interface IWeapon {
        float AttackPower { get; set; }
        float Coefficient { get; }
        float Range { get; set; }
    }

    public class Bow : IWeapon {
        public float AttackPower { get; set; }
        public float Coefficient => 1.3f;
        public float Range { get; set; } = 470f;
    }

    public class OneHandedSword : IWeapon {
        public float AttackPower { get; set; }
        public float Coefficient => 1.2f;
        public float Range { get; set; } = 175f;
    }
    
    public class Claw : IWeapon {
        public float AttackPower { get; set; }
        public float Coefficient => 1.75f;
        public float Range { get; set; } = 400f;
    }
    
    public class Cain : IWeapon {
        public const float BaseRange = 200f;

        public float AttackPower { get; set; }
        public float Coefficient => 1f;

        public float Range { get; set; } = BaseRange;
    }
}
