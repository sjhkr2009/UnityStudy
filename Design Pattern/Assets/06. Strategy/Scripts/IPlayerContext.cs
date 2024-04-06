using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategy {
    public interface IPlayerContext {
        float Range { get; set; }
        float LifeSteal { get; set; }
    }
}
