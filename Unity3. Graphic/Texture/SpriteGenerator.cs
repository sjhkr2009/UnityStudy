using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// 셰이더를 입힌 이미지를 생성해서 텍스쳐 파일로 저장하는 에디터 스크립트
/// </summary>
public class SpriteGenerator {
    private const string ShaderPath = "Custom/ColorWheelShader";
    private const string SpriteName = "GeneratedSprite";
    
    [MenuItem("Assets/Create/Create Sprite by Shader")]
    public static void CreateColorWheelSprite() {
        // 대상 셰이더를 사용한 material과 그것을 입힐 텍스쳐 생성 
        var targetShader = Shader.Find(ShaderPath);
        if (targetShader == null) {
            Debug.LogError("Color Wheel Shader not found.");
            return;
        }

        var customMaterial = new Material(targetShader);
        var createdTexture = new Texture2D(256, 256, TextureFormat.ARGB32, false) {
            name = SpriteName
        };

        // 렌더 텍스처 생성
        var renderTexture = RenderTexture.GetTemporary(createdTexture.width, createdTexture.height);
        Graphics.Blit(createdTexture, renderTexture, customMaterial);

        // 렌더 텍스처에서 텍스처로 픽셀 읽기
        var previous = RenderTexture.active;
        RenderTexture.active = renderTexture;
        createdTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        createdTexture.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTexture);

        // png로 저장
        byte[] pngData = createdTexture.EncodeToPNG();
        if (pngData != null) {
            string filePath = EditorUtility.SaveFilePanel("Save Color Wheel Texture", "", "ColorWheel.png", "png");
            if (!string.IsNullOrEmpty(filePath)) {
                File.WriteAllBytes(filePath, pngData);
                AssetDatabase.Refresh();
            }
        }

        // 메모리 정리
        Object.DestroyImmediate(createdTexture);
        Object.DestroyImmediate(customMaterial);
    }
}
