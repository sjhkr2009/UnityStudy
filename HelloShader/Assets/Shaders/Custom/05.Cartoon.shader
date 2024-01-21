Shader "Unlit/Cartoon" // 복잡한 라이트 효과를 사용하지 않으며, 조명 처리를 직접 연산하기 위해 Unlit으로 만든다.
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _ShadowSmooth ("Smooth Shadow", Range(0, 0.1)) = 0.01
        [HDR] _AmbientColor ("Ambient Color", Color) = (0.4, 0.4, 0.4, 1)
        [HDR] _SpecularColor ("Specular Color", Color) = (0.9, 0.9, 0.9, 1)
        _SpecularSmoothness ("Specular Smoothness", float) = 32
        [HDR] _RimColor ("Rim Color", Color) = (1, 1, 1, 1)
        _RimPower ("Rim Power", Range(0, 1)) = 0.5
        _RimThreshold ("Rim Threshold", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque"
            "LightMode" = "ForwardBase" // 내가 그려진 이후의 요소들에게만 영향을 준다
            "PassFlags" = "OnlyDirectional" // 오직 Direction Light에만 영향을 받음 (태양광)
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            #include "Lighting.cginc" // 빛에 관한 정보를 받아오기 위해 사용. (아래 _LightColor0 값을 받아오기 위함)

            struct MyVertex_In
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL; // NORMAL: 버텍스의 방향 정보. 일반적으로 해당 점에서 물체의 표면을 향하는 방향이 들어온다.
            };

            struct MyFragment_In
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
                float3 viewDir : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _AmbientColor;
            float4 _SpecularColor;
            float4 _RimColor;
            float _ShadowSmooth;
            float _SpecularSmoothness;
            float _RimPower;
            float _RimThreshold;

            MyFragment_In vert (MyVertex_In v)
            {
                MyFragment_In o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                // UnityObjectToWorldNormal() 으로 버텍스의 노말 값을 픽셀 셰이더 값으로 변환
                o.worldNormal = UnityObjectToWorldNormal(v.normal);

                // 월드 공간에서의 방향을 normalize 해서 픽셀 셰이더에 넘긴다.
                o.viewDir = normalize(UnityWorldSpaceViewDir(v.vertex));
                
                return o;
            }

            fixed4 frag (MyFragment_In i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Phong Shading : 빛의 방향과 내 normal의 내적을 구한다. 빛을 받는 부분은 양수, 반대쪽은 음수가 된다.
                float3 normal = normalize(i.worldNormal);
                float NdotL = dot(normal, _WorldSpaceLightPos0); // 월드 공간의 빛의 위치가 _WorldSpaceLightPos0 으로 들어온다.

                // two-tone Shading : 빛의 방향을 0과 1의 양극단으로 구분한다.
                float lightIntensity = NdotL > 0 ? 1 : 0;
                // 다만 빛과 그림자가 칼같이 변하게 하기보다, 부드럽게 전환되도록 약간 보간해준다.
                lightIntensity = smoothstep(0, _ShadowSmooth, NdotL); // 0보다 작으면 0, _SmoothNess보다 크면 1, 그 사이일 경우만 0~1의 보간된 값.

                // 현재 광원의 색상에도 영향을 받아야 한다. _LightColor0 으로 빛의 색상을 받아온다.
                float4 lightColor = _LightColor0 * lightIntensity;

                // + 추가) Blinn-Phong Shading: 좀더 날카로운 광원 효과를 사용.
                // 공식은 링크 참고 >> https://en.wikipedia.org/wiki/Blinn%E2%80%93Phong_reflection_model
                float3 halfDir = normalize(_WorldSpaceLightPos0 + i.viewDir);
                float NdotH = dot(normal, halfDir);
                float specularIntensity = pow(NdotH * lightIntensity, _SpecularSmoothness * _SpecularSmoothness);
                float specularIntensitySmooth = smoothstep(0, 0.01, specularIntensity);
                float4 specular = specularIntensitySmooth * _SpecularColor;

                // ++ 추가) Rim Light : 카메라에서 바라보는 정면 부분은 Rim Light를 적용할 필요가 없다. 표면만 밝게 외곽선처럼 처리한다.
                // 빛을 받는 지점에서 멀수록 더 많이 적용해야 하므로, (1 - 내적) 값을 계산한다.
                float rimDot = 1 - dot(i.viewDir, normal); // rimDot: 빛을 받아야 하는 부분에 가까운 정도
                // 외곽선의 세기는 빛을 받는 방향일수록 강해져야 하므로, rimDot에 (빛을 받는 정도 * _RimThreshold 제곱) 을 곱한다. 
                float rimIntensity = rimDot * pow(NdotL, _RimThreshold);
                // 모든 부분에 적용할 필요는 없으므로, 결과값이 _RimPower 이상일 때만 Rim light를 적용한다. 위와 동일하게 smoothstep로 부드럽게 보간해서 적용.
                rimIntensity = smoothstep(_RimPower - 0.01, _RimPower + 0.01, rimIntensity);
                float rim = _RimColor * rimIntensity;
                
                // _AmbientColor를 Additive 연산해서 밝은 효과를 준다.
                // + 추가) Blinn-Phong Shading 사용하려면 specular 값까지 더해야 함.
                // ++ 추가) Rim Light 적용을 위해 rim 값도 더한다.
                return col * _Color * (_AmbientColor + lightColor + specular + rim);
            }
            ENDCG
        }
    }
}
