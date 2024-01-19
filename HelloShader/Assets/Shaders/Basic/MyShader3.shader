Shader "Unlit/MyShader3"
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
            
            float4 _Color;
            sampler2D _Tex;

            // 버텍스 셰이더
            MyFragment_In vert (MyVertex_In v)
            {
                MyFragment_In o;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
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
