Shader "Unlit/CustomMaterial"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Noise ("Noise", 2D) = "white" {}
        _Opacity ("Opacity", Range(0,1)) = 0.5 // Range()로 특정 범위의 숫자를 받을 수 있음
        _NoiseScale ("Noise Scale", Range(0.1, 5)) = 1
        _OutlineThickness ("Outline Thickness", Range(0, 0.1)) = 0.1
        [HDR] _OutlineColor("Outline Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { 
            "RenderType"="Transparent" // 투명
            "IgnoreProjector"="True"
            "Queue"="Transparent" // 불투명 효과는(아마 뒤의 요소를 안 그려도 되므로...?) 투명 효과와 렌더링 순서가 다르다. 그리는 순서를 투명 타입으로 한다.
            "PreviewType"="Plane"
        }
        Cull Off // 뒷면 안 그림 (2D)
        Lighting Off // 빛 안씀
        ZWrite Off // 깊이 무시 (2D)
        Blend SrcAlpha OneMinusSrcAlpha // 뒤에 그린 것과 앞에 그린 것을 블렌딩할 것
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct MyVertex_In
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct MyFragment_In
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _Noise;
            float _Opacity;
            float _NoiseScale;
            float _OutlineThickness;
            float4 _OutlineColor;

            // 버텍스 셰이더
            MyFragment_In vert (MyVertex_In v)
            {
                MyFragment_In o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); // 샘플링된 텍스쳐를 TRANSFORM_TEX 함수로 uv값에 적용해준다
                return o;
            }

            // 프래그먼트 셰이더
            fixed4 frag (MyFragment_In i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 noise = tex2D(_Noise, i.uv / _NoiseScale);

                // 노이즈 이미지의 밝고 어두운 정도에 따라 알파값을 바꿔줄 수 있다.
                // 노이즈는 rgb값이 동일하니 임의로 r 값을 대입한다. 곱연산하면 원본의 알파값이 0이면 곱해도 0일테니 그대로 투명할 것.
                // col.a *= noise.r;

                // 이걸 이용해서, 노이즈 밝기가 일정 값 이상일 때 원본 이미지를 안 보이게 그린다.
                // 위에서 선언한 Opacity 값을 서서히 줄여서 랜덤하게 사라지는(Desolve) 효과를 구현할 수 있다.
                col.a *= step(noise.r, _Opacity);

                // 사라질 때 외곽선을 추가한다. 외곽선은 (큰 이미지 - 작은 이미지) 로 생성할 수 있다. _Opacity 값에서 _Opacity보다 약간 작은 값을 빼면 아웃라인 영역이다.
                float outline = step(noise.r, _Opacity) - step(noise.r, _Opacity - _OutlineThickness);
                col.rgb += outline * _OutlineColor.rgb;
                
                return col;
            }
            ENDCG
        }
    }
}
