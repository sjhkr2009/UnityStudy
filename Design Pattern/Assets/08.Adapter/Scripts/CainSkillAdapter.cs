using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adapter {
    public class CainSkillAdapter : MonoBehaviour {
        private T Convert<T>(Cain origin) where T : IWeapon, new() {
            T ret = new T();
            
            // 모방할 무기의 공격력 계수에 맞게 공격력을 조정한다.
            ret.AttackPower = origin.AttackPower * (origin.Coefficient / ret.Coefficient);

            return ret;
        }

        public void ActivateByCain<T>(ISkillHandler<T> handler, Cain cain) where T : IWeapon, new() {
            handler.Activate(Convert<T>(cain));
        }
    }
}
