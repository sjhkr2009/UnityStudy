Shader "Unlit/MyShader4"
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

                // 유니티에서 시간 정보는 _Time 에 담겨있다.
                // _Time은 기본적으로 기준 시간의 0.05배, 1배, 2배, 3배 값이 x,y,z,w에 들어있다. 즉, _Time = (time/20, time, time*2, time*3)
                // 편의상 여기선 1배짜리 _Time.y 를 사용. 참고로 시간은 게임을 실행해야 제대로 볼 수 있다. (그렇지 않으면 키보드/마우스 이벤트가 있을 때만 1프레임씩 변함)
                o.vertex.x += sin(_Time.y);
                o.vertex.y += cos(_Time.y);
                return o;
            }

            // 프래그먼트 셰이더
            fixed4 frag (MyFragment_In i) : SV_Target
            {
                fixed4 col = tex2D(_Tex, i.uv);

                // 여기도 _Time을 사용해서 원형 그라데이션 회전 로직을 넣어본다.
                float2 p = i.uv;
                p.x += sin(_Time.y);
                p.y += cos(_Time.y);

                col.rgb += 1 - length(p - 0.5);
                return col;
            }
            ENDCG
        }
    }
}
