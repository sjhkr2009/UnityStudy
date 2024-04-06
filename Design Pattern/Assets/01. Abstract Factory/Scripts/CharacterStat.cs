using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbstractFactory {
    public class CharacterStat {
        public float attackDamage = 50;
        public float abilityPower = 0;
        public float movementSpeed = 330;
        public float health = 500;
        public float cost = 350;
        public float armor = 24;
        public float magicResistance = 30;
        public float abilityHaste = 0;

        public static CharacterStat AllZero => new CharacterStat() {
            attackDamage = 0,
            abilityPower = 0,
            movementSpeed = 0,
            health = 0,
            cost = 0,
            armor = 0,
            magicResistance = 0,
            abilityHaste = 0
        };
    }
}


