using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Define {
    public static bool IsUnityEditor =>
#if UNITY_EDITOR
        true;
#else
        false;
#endif
    
    // 추후 디파인심볼 등으로 빌드에서도 테스트 환경을 정의할 수 있음
    public static bool IsTesting => IsUnityEditor;
    
    public static class Tag {
        public const string Ground = "Ground";
        public const string ScreenArea = "ScreenArea";
        public const string Enemy = "Enemy";
        public const string Player = "Player";
        public const string ItemGainer = "ItemGainer";
        public const string Projectile = "Projectile";
    }
     
    public static class Layer {
        public const string Enemy = "Enemy";
    }
    
    public static class EnvironmentSetting {
        public const int TileMapSize = 40;
    }

    public static class DataSetting {
        public const int ItemDefaultMaxLevel = 1;
    }
}
