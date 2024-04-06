using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FactoryMethod {
    public static class DragonCreator {
        static DragonCreator() {
            DragonFactory.Initialize(MakeDragon);
        }
        
        public static DragonBase MakeDragon(int type) {
            if (type > (int)Enum.GetValues(typeof(DragonType)).Cast<DragonType>().Max()) {
                throw new ArgumentOutOfRangeException($"DragonType에 {type}번 타입이 정의되어있지 않습니다.");
            }

            var dragonType = (DragonType) type;
            DragonBase dragon = dragonType switch {
                DragonType.Infernal => new InfernalDragon(),
                DragonType.Cloud => new CloudDragon(),
                DragonType.Ocean => new OceanDragon(),
                DragonType.Mountain => new MountainDragon(),
                DragonType.Elder => new ElderDragon(),
                _ => null
            };

            if (dragon == null) {
                Debug.LogError($"팩토리가 {dragonType} 타입을 생성할 수 없습니다.");
            }

            return dragon;
        }
    }
}

