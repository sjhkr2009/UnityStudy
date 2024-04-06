using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adapter {
    public interface ISkillHandler<T> {
        void Activate(T weapon);
    }
}
