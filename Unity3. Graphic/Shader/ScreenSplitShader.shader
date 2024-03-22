Shader "Custom/ScreenSplitShader" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        
        _OffsetX ("Offset X", Float) = 5.0
        _OffsetY ("Offset Y", Float) = 0
        _SplitPoint ("Split Point", Range(0, 1)) = 0.5
        
        _BorderWidth ("Border Width", Range(0, 0.5)) = 0.01
        _BorderColor ("Border Color", Color) = (0,0,0,1)
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        // 현재 화면 캡처
        GrabPass {
            "_GrabTexture"
        }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _GrabTexture;
            float _OffsetX;
            float _OffsetY;
            float _SplitPoint;
            float _BorderWidth;
            half4 _BorderColor;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // 화면 해상도에 따라 오프셋 조정
                float offsetX = _OffsetX / _ScreenParams.y;
                float offsetY = _OffsetY / _ScreenParams.y;

                float borderMin = _SplitPoint - _BorderWidth;
                float borderMax = _SplitPoint + _BorderWidth;
                if (i.uv.y > borderMin && i.uv.y < borderMax) return _BorderColor;
                
                if (i.uv.y > _SplitPoint) {
                    i.uv.x += offsetX;
                    i.uv.y += offsetY;
                } else {
                    i.uv.x -= offsetX;
                    i.uv.y -= offsetY;
                }
                
                return tex2D(_GrabTexture, i.uv);
            }
            ENDCG
        }
    }
}
