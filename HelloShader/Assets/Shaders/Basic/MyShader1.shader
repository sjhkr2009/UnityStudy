// Unlit 셰이더는 빛의 영향을 안 받는 셰이더라는 뜻.
Shader "Unlit/MyShader1"
{
    // Properties: 속성. 여기 나열된 요소들은 값 설정이 가능하며 일반적으로 인스펙터에 노출됨.
    Properties
    {
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        // 효과를 위해 셰이더를 여러 번 부르기도 한다. 각 회차를 Pass로 구분한다.
        Pass
        {
            CGPROGRAM
            // vertex 셰이더는 vert 함수, fragment 셰이더는 frag 함수라는 선언
            #pragma vertex vert
            #pragma fragment frag
            // 유니티에서 제공하는 편의 기능들을 사용하기 위해 include
            #include "UnityCG.cginc"

            // 구조체 이름은 내 마음대로 정할 수 있다. TEXCOORD0, POSITION, COLOR 등의 정보 중 사용할 것을 변수로 받아준다. 
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

            // 버텍스 셰이더
            MyFragment_In vert (MyVertex_In v)
            {
                MyFragment_In o;

                // UnityObjectToClipPos(): 유니티 3차원 공간에 배치한 버텍스 좌표를 카메라에 픽셀화된 좌표로 바꿔준다.
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                return o;
            }

            // 프래그먼트 셰이더
            fixed4 frag (MyFragment_In i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
}
