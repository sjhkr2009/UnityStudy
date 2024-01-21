Shader "Unlit/Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Thickness ("Thickness", Range(0, 0.02)) = 0.005
        [HDR] _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "PreviewType"="Plane"
        }
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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
            float _Thickness;
            float4 _Color;

            MyFragment_In vert (MyVertex_In v)
            {
                MyFragment_In o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (MyFragment_In i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float alpha = 0;
                // 외곽선 알파값 구하기: 원본 이미지 알파값에 약간 오른쪽으로 옮긴 이미지 앞파값을 뺀다. 같은 원리로 4방향에 모두 동일한 처리.
                alpha += col.a - tex2D(_MainTex, i.uv + float2(_Thickness, 0)).a;
                alpha += col.a - tex2D(_MainTex, i.uv + float2(-_Thickness, 0)).a;
                alpha += col.a - tex2D(_MainTex, i.uv + float2(0, _Thickness)).a;
                alpha += col.a - tex2D(_MainTex, i.uv + float2(0, -_Thickness)).a;

                // 다만 이대로 적용을 끝내면 대각선 4방향의 아웃라인은 빠뜨리게 되므로, 경계선에 계단 현상이 발생한다. 대각선 4방향도 계산해준다.
                alpha += col.a - tex2D(_MainTex, i.uv + float2(_Thickness, _Thickness)).a;
                alpha += col.a - tex2D(_MainTex, i.uv + float2(-_Thickness, _Thickness)).a;
                alpha += col.a - tex2D(_MainTex, i.uv + float2(_Thickness, -_Thickness)).a;
                alpha += col.a - tex2D(_MainTex, i.uv + float2(-_Thickness, -_Thickness)).a;

                // 8번 더했으니 0~1 안으로 알파값을 보정해야 한다. 그냥 8로 나눠도 되지만 (alpha /= 8), clamp로 범위를 제한할 수 있다.
                alpha = clamp(alpha, 0, 1); // 알파값 1 이상에 해당되는 영역은 모두 1로 표시하므로, 8로 나누는 것보다 외곽선이 좀더 선명해진다.

                // 원래 alpha가 음수값이면 원본 색상을 넣도록 보정해야 하지만...
                //if(alpha <= 0) alpha = col.a;

                // lerp라는 선형 보간 함수가 지원된다. alpha가 있을 때만 지정된 _Color에 가깝게 그린다.
                col.rgb = lerp(col, _Color, alpha);

                // 이건 선택이지만, 외곽선에도 알파값이 적용되게 하려면 alpha를 더해준다.
                col.a += alpha;
                
                return col;
            }
            ENDCG
        }
    }
}
