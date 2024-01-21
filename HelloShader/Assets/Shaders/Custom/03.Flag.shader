Shader "Custom/Flag" // Unlit이 아닌 Standard Surface Shader로 생성한다.
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        // Lambert 모델을 사용하겠다고 선언한다. 아래의 UNITY_INITIALIZE_OUTPUT()에서 Lambert 모델에 관한 연산을 알아서 해 준다.
        #pragma surface surf Lambert vertex:vert // 기본값은 Standard fullforwardshadows : 내 모델링 전방에 그려지는 요소들에 영향을 준다
        #pragma target 3.0
        
        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };
        
        fixed4 _Color;

        // 위에서 버텍스 모델을 쓰겠다고 선언(vertex:vert)했으니 vert 함수를 쓸 수 있다.
        void vert (inout appdata_base v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o); // 들어온 입력을 o로 내보내는 처리. Lambert 모델에 관한 처리는 자동으로 수행됨.

            // 버텍스를 추가로 조정하려면 아래에서 조작

            // 깃발 펄럭이는 연출을 하고싶으니 사인함수에 따라 위아래로 움직이되, 버텍스의 x 좌표에 따라 움직임에 차이를 둔다.
            //v.vertex.y += sin(_Time.y + v.vertex.x);

            // 깃발은 특정 지점에 매달려서, 매달린 부분에 가까우면 움직임이 적고 멀수록 움직임이 커진다.
            // appdata_base에 기본적으로 텍스쳐 정보가 들어 있으므로, uv 좌표에 따라 변화량을 다르게 준다. 
            v.vertex.y += sin(_Time.w + v.vertex.x) * v.texcoord.x;
        }

        // 유니티 Standard Surface Shader에는 Vertex, Fragment 대신 Surface가 있다.
        // 픽셀 셰이더 전후로 처리되는 조명과 그림자 효과를 계산하기가 너무 어려워서, 유니티가 Light Model, Fragment, Shadow 처리를 합쳐놓은 편의 기능이 Surface 개념.
        // 예를 들어 Albedo는 주변 광에 따라 달리 보이는 색상을 의미한다. 다만 유니티도 URP 도입 이후로는 Surface 모델을 안 쓴다.

        // 옛날에 사용하던 Diffuse(난반사), Specular(정반사), Ambient(태양광) 개념 대신 Albedo, Metallic, Smoothness 표기로 대체.

        // Lambert 모델에서는 SurfaceOutputStandard 대신 간소화된 SurfaceOutput 으로 쓴다.
        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // 별도로 리턴하지 않아도 out (또는 inout) 으로 나간다.
        }
        ENDCG
    }
    FallBack "Diffuse"
}
