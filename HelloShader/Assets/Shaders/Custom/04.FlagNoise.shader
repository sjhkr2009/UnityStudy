Shader "Custom/FlagNoise"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _Noise ("Noise", 2D) = "white" {}
        _NoiseScaleX ("Noise Scale X", float) = 10
        _NoiseScaleY ("Noise Scale Y", float) = 10
        _TimeScaleX ("Time Scale X", Range(0, 0.1)) = 0.1
        _TimeScaleY ("Time Scale Y", Range(0, 0.1)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _Noise;
        fixed4 _Color;
        float _NoiseScaleX;
        float _NoiseScaleY;
        float _TimeScaleX;
        float _TimeScaleY;

        struct Input
        {
            float2 uv_MainTex;
        };
        
        void vert (inout appdata_base v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);

            // vertex shader에서는 기본적으로 텍스쳐2D를 못 쓰지만, LOD는 제어할 수 있으므로 이걸 이용한다. 
            // LOD(Level of Detail): 카메라와의 거리에 따라 어떤 텍스쳐를 사용할 지 결정하는 영역

            // 사용법은 tex2D와 비슷하지만, float4(uv, 0, lodLevel) 를 매개변수로 써야 한다. LOD level은 별도로 구분하지 않았으니 여기선 0으로 한다.
            float2 uv = v.texcoord;

            // 자연스러운 움직임으로 커스텀하기 위해 노이즈 크기나 속도는 X,Y 변수를 따로 둔다.
            uv *= float2(1 / _NoiseScaleX, 1 / _NoiseScaleY); // 깃팔이 너무 많이 구겨지지 않게 노이즈 이미지를 크게 적용한다
            uv += float2(_Time.y * _TimeScaleX, _Time.y * _TimeScaleY); // 노이즈를 시간에 따라 흘러가게 한다.
            
            float noise = tex2Dlod(_Noise, float4(uv, 0, 0)).r;
            float offset = noise * 2 - 1; // 0~1 범위인 noise를 -1 ~ 1 범위로 바꾼다.

            // 묶여있는 부분에서 멀수록 많이 펄럭이게 한다. Flag 셰이더와 같은데 깃발 방향이 바뀌었으니 x 를 (1 - x)로 바꾼 것.
            offset *= (1 - v.texcoord.x);
            v.vertex.y += offset;

            // 참고로 좀더 자연스러운 움직임을 위해 x,z 축에 약간의 조정을 주면, 깃발이 약간 왜곡되면서 펄럭이는 효과를 줄 수 있다.
            v.vertex.x += offset * 0.1;
            v.vertex.z += offset * 0.1;
        }
        
        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
