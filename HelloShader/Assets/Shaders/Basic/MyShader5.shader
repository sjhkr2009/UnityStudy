Shader "Unlit/MyShader5"
{
    Properties
    {
        _Color("Main Color", Color) = (1,1,1,1)
        _Tex("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            float4 _OutlineColor;
            sampler2D _Tex;

            // 버텍스 셰이더
            MyFragment_In vert (MyVertex_In v)
            {
                MyFragment_In o;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                // 아까 HLSL로 구현한 일렁이는 효과. 버텍스가 많은 구 형태의 3D 이미지로 보면 다른 느낌이 날 것이다.
                // 그래서 3D에서 사인/코사인 함수가 자주 사용됨.
                o.vertex.x += sin(_Time.w + v.vertex.x * 10) * 0.1; // 빠르게 일렁이게 하기 위해 사인함수 내에는 10을 곱해주고, 좌표가 많이 변하지 않게 결과값 좌표에는 0.1을 곱한다.
                o.vertex.y += cos(_Time.w + v.vertex.y * 10) * 0.1;
                return o;
            }

            // 프래그먼트 셰이더
            fixed4 frag (MyFragment_In i) : SV_Target
            {
                fixed4 col = tex2D(_Tex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
