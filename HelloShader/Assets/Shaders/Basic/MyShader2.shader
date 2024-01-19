Shader "Unlit/MyShader2"
{
    Properties
    {
        // 변수를 사용하고 싶다면 Properties에 이름과 타입을 정해 노출시킨다.
        _MyValue("My Value", Float) = 1.0
        _MyColor("My Color", Color) = (1,1,1,1)
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
                float4 color : COLOR;
            };

            struct MyFragment_In
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            // Properties의 변수를 사용하려면, 해당 변수를 담을 수 있는 자료형으로 Properties와 똑같은 이름의 변수를 선언한다.
            float _MyValue;
            float4 _MyColor;

            // 버텍스 셰이더
            MyFragment_In vert (MyVertex_In v)
            {
                MyFragment_In o;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color * _MyValue;
                return o;
            }

            // 프래그먼트 셰이더
            fixed4 frag (MyFragment_In i) : SV_Target
            {
                return i.color * _MyColor;
            }
            ENDCG
        }
    }
}
