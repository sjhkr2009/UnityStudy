using UnityEngine;
using UnityEditor;
using UnityEngine.Scripting;

[Preserve]
public class CustomShaderEditor : ShaderGUI {
    private bool showRawValue = false;

    enum GradientMode {
        Color4,
        Horizontal,
        Vertical,
        Radiant
    }
    
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties) {
        materialEditor.ShaderProperty(FindProperty("_Color", properties), "Base Color");
        
        EditorGUILayout.HelpBox("Doodle Animation, Shine Effect 등 시간에 따라 변화하는 효과는, 게임 실행중에만 정확한 간격으로 표시됩니다.\n" +
                        "에디터에선 키보드나 마우스 이동/클릭 등 이벤트를 감지했을때만 변화합니다.", MessageType.Info);

        // Doodle Animation Section
        GUILayout.Label("Doodle Animation", EditorStyles.boldLabel);
        var useDoodleEffect = FindProperty("_UseDoodleEffect", properties);
        materialEditor.ShaderProperty(useDoodleEffect, "두들 애니메이션 사용");
        
        if (useDoodleEffect.floatValue > 0f) {
            var doodleAmount = FindProperty("_DoodleAmount", properties);
            var doodleSpeed = FindProperty("_DoodleSpeed", properties);
            
            GUILayout.Space(3);
            GUILayout.Label("애니메이션 효과는 게임 실행중에만 정확한 간격으로 표시됩니다.");
            materialEditor.ShaderProperty(doodleAmount, "움직임 강도");
            materialEditor.ShaderProperty(doodleSpeed, "속도");
        }
        GUILayout.Space(10);

        // HSV Edit Section
        GUILayout.Label("HSV Edit", EditorStyles.boldLabel);
        var useHsvEdit = FindProperty("_UseHsvEdit", properties);
        materialEditor.ShaderProperty(useHsvEdit, "HSV 에디터 사용");

        if (useHsvEdit.floatValue > 0f) {
            var hsvShift = FindProperty("_HsvShift", properties);
            var hsvSaturation = FindProperty("_HsvSaturation", properties);
            var hsvBright = FindProperty("_HsvBright", properties);
            materialEditor.ShaderProperty(hsvShift, "색조 (Hue)");
            materialEditor.ShaderProperty(hsvSaturation, "채도 (Saturation)");
            materialEditor.ShaderProperty(hsvBright, "명도 (Value)");
        }
        GUILayout.Space(10);

        // Gradient Section
        GUILayout.Label("Gradient Overlay", EditorStyles.boldLabel);
        MaterialProperty useGradient = FindProperty("_UseGradient", properties);
        materialEditor.ShaderProperty(useGradient, "색상 오버레이 사용");
        if (useGradient.floatValue > 0.0f) {
            var gradOverlayAlpha = FindProperty("_OverlayAlpha", properties);
            var gradBlend = FindProperty("_GradBlend", properties);
            var gradVertical = FindProperty("_GradVertical", properties);
            var gradHorizontal = FindProperty("_GradHorizontal", properties);
            var gradRadiant = FindProperty("_GradRadiant", properties);
            var gradTopLeftCol = FindProperty("_GradTopLeftCol", properties);
            var gradTopRightCol = FindProperty("_GradTopRightCol", properties);
            var gradBotLeftCol = FindProperty("_GradBotLeftCol", properties);
            var gradBotRightCol = FindProperty("_GradBotRightCol", properties);
            var gradValueX = FindProperty("_GradValueX", properties);
            var gradValueY = FindProperty("_GradValueY", properties);
            var gradValueR = FindProperty("_GradValueR", properties);
            var useReverseGradient = FindProperty("_UseReverseGradient", properties);

            GradientMode mode = GradientMode.Color4;
            
            materialEditor.ShaderProperty(gradBlend, "오버레이 비율");
            materialEditor.ShaderProperty(gradVertical, "세로 그라디언트");
            if (IsOn(gradVertical)) {
                mode = GradientMode.Vertical;
                gradHorizontal.floatValue = 0f;
                gradRadiant.floatValue = 0f;
            }
            materialEditor.ShaderProperty(gradHorizontal, "가로 그라디언트");
            if (IsOn(gradHorizontal)) {
                mode = GradientMode.Horizontal;
                gradVertical.floatValue = 0f;
                gradRadiant.floatValue = 0f;
            }
            materialEditor.ShaderProperty(gradRadiant, "원형 그라디언트");
            if (IsOn(gradRadiant)) {
                mode = GradientMode.Radiant;
                gradHorizontal.floatValue = 0f;
                gradVertical.floatValue = 0f;
            }
            
            GUILayout.Space(5);
            GUILayout.Label("색상 설정");
            materialEditor.ShaderProperty(gradTopLeftCol, ColorLabel(1, mode));
            if (mode is GradientMode.Color4 or GradientMode.Horizontal)
                materialEditor.ShaderProperty(gradTopRightCol, ColorLabel(2, mode));
            if (mode is GradientMode.Color4 or GradientMode.Vertical or GradientMode.Radiant)
                materialEditor.ShaderProperty(gradBotLeftCol, ColorLabel(3, mode));
            if (mode is GradientMode.Color4)
                materialEditor.ShaderProperty(gradBotRightCol, ColorLabel(4, mode));
            
            GUILayout.Space(5);
            GUILayout.Label("색상 위치 조정");
            if (mode is GradientMode.Color4 or GradientMode.Horizontal)
                materialEditor.ShaderProperty(gradValueX, "가로 중심점");
            if (mode is GradientMode.Color4 or GradientMode.Vertical)
                materialEditor.ShaderProperty(gradValueY, "세로 중심점");
            if (mode is GradientMode.Radiant)
                materialEditor.ShaderProperty(gradValueR, "원형 범위");
            
            GUILayout.Space(5);
            materialEditor.ShaderProperty(useReverseGradient, "그라디언트 반전");
            materialEditor.ShaderProperty(gradOverlayAlpha, "투명도 블렌딩");
        }
        GUILayout.Space(10);

        // Shadow Section
        GUILayout.Label("Shadow", EditorStyles.boldLabel);
        var useShadow = FindProperty("_UseShadow", properties);
        materialEditor.ShaderProperty(useShadow, "그림자 사용");
        var useShadowBlur = FindProperty("_UseShadowBlur", properties);
        if (useShadow.floatValue > 0.0f) {
            var shadowX = FindProperty("_ShadowX", properties);
            var shadowY = FindProperty("_ShadowY", properties);
            var shadowColor = FindProperty("_ShadowColor", properties);
            materialEditor.ShaderProperty(shadowX, shadowX.displayName);
            materialEditor.ShaderProperty(shadowY, shadowY.displayName);
            materialEditor.ShaderProperty(shadowColor, shadowColor.displayName);

            materialEditor.ShaderProperty(useShadowBlur, "부드러운 그림자 사용");
            var shadowBlur = FindProperty("_ShadowBlur", properties);
            if (useShadowBlur.floatValue > 0.0f) {
                materialEditor.ShaderProperty(shadowBlur, "가장자리 보간");
            }
        }
        GUILayout.Space(10);
        
        // Shine Effect Section
        GUILayout.Label("Shine Effect", EditorStyles.boldLabel);
        var useShineEffect = FindProperty("_UseShineEffect", properties);
        materialEditor.ShaderProperty(useShineEffect, "빛 효과 사용");
        if (useShineEffect.floatValue > 0.0f) {
            var shineColor = FindProperty("_ShineColor", properties);
            var shineSpeed = FindProperty("_ShineSpeed", properties);
            var shineInterval = FindProperty("_ShineInterval", properties);
            var shineRotate = FindProperty("_ShineRotate", properties);
            var shineWidth = FindProperty("_ShineWidth", properties);
            
            materialEditor.ShaderProperty(shineColor, shineColor.displayName);
            materialEditor.ShaderProperty(shineWidth, shineWidth.displayName);
            materialEditor.ShaderProperty(shineRotate, shineRotate.displayName);
            
            GUILayout.Space(3);
            GUILayout.Label("빛 움직임은 게임 실행중에만 정확한 간격으로 표시됩니다.");
            materialEditor.ShaderProperty(shineSpeed, shineSpeed.displayName);
            materialEditor.ShaderProperty(shineInterval, shineInterval.displayName);
        }
        GUILayout.Space(10);
        
        GUILayout.Space(20);
        showRawValue = EditorGUILayout.Foldout(showRawValue, "Show Raw Value");
        if (showRawValue) base.OnGUI(materialEditor, properties);
        else if (GUI.changed) EditorUtility.SetDirty(materialEditor.target);
    }

    bool IsOn(MaterialProperty boolProperty) {
        return boolProperty.floatValue > 0f;
    }

    string ColorLabel(int oneBasedIndex, GradientMode mode) {
        if (oneBasedIndex == 1) {
            return mode switch {
                GradientMode.Color4 => "[↖] Top-Left",
                GradientMode.Horizontal => "[◀] Left",
                GradientMode.Vertical => "[▲] Top",
                GradientMode.Radiant => "[◯] Outer",
                _ => string.Empty
            };
        }
        if (oneBasedIndex == 2) {
            return mode switch {
                GradientMode.Color4 => "[↗] Top-Right",
                GradientMode.Horizontal => "[▶] Right",
                _ => string.Empty
            };
        }
        if (oneBasedIndex == 3) {
            return mode switch {
                GradientMode.Color4 => "[↙] Bottom-Left",
                GradientMode.Vertical => "[▼] Bottom",
                GradientMode.Radiant => "[●] Inner",
                _ => string.Empty
            };
        }
        if (oneBasedIndex == 4) {
            return mode switch {
                GradientMode.Color4 => "[↘] Bottom-right",
                _ => string.Empty
            };
        }

        return string.Empty;
    }
}
