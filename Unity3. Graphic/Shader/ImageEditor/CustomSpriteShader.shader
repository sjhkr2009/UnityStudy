Shader "Custom/CustomShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Color Overlay", Color) = (1,1,1,1)
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        
        [Space(5)]
        [Toggle()] _UseDoodleEffect("Use Doodle Animation?", float) = 0
        _DoodleAmount("Doodle Amount", Range(0, 20)) = 6.6
		_DoodleSpeed("Doodle Speed", Range(0, 15)) = 5
        
        [Space(5)]
        [Toggle()] _UseHsvEdit("Use HSV Editor?", float) = 0
        _HsvShift("Hue Shift", Range(0, 360)) = 0
        _HsvSaturation("Saturation", Range(0, 2)) = 1
        _HsvBright("Brightness", Range(0, 2)) = 1
        
        [Space(5)]
        [Toggle()] _UseGradient("Use Gradient?", float) = 0
        [Toggle()] _OverlayAlpha("Gradient Alpha Overlay?", float) = 0
        _GradBlend("Gradient Blend", Range(0,1)) = 1
        [Toggle()] _GradVertical("Use Vertical Gradient?", float) = 0
        [Toggle()] _GradHorizontal("Use Horizontal Gradient?", float) = 0
        [Toggle()] _GradRadiant("Use Radiant Gradient?", float) = 0
        _GradTopLeftCol("Top/Left/Outer", Color) = (1,0,0,1)
        _GradTopRightCol("Top/Right", Color) = (1, 1, 0, 1)
        _GradBotLeftCol("Bottom/Left/Inner", Color) = (0,0,1,1)
        _GradBotRightCol("Bottom/Right", Color) = (0, 1, 0, 1)
        _GradValueX("Horizontal Axis", Range(0, 5)) = 1
        _GradValueY("Vertical Axis", Range(0, 5)) = 1
        _GradValueR("Radient Axis", Range(0, 2)) = 0.75
        [Toggle()] _UseReverseGradient("Reverse?", float) = 0
        
        [Space(5)]
        [Toggle()] _UseShadow("Use Shadow?", float) = 0
        _ShadowX("Shadow X Axis", Range(-0.05, 0.05)) = 0.015
		_ShadowY("Shadow Y Axis", Range(-0.05, 0.05)) = -0.015
		_ShadowColor("Shadow Color", Color) = (0, 0, 0, 1)
        
        [Space(5)]
        [Toggle()] _UseShadowBlur("Use Shadow Blur?", float) = 0
        _ShadowBlur("Shadow Blur", Range(0, 0.01)) = 0.002
        
        [Space(5)]
        [Toggle()] _UseShineEffect("Use Shine Effect?", float) = 0
        _ShineColor("Shine Color", Color) = (1,1,1,1)
        _ShineSpeed("Shine Speed", Range(0, 5)) = 1
        _ShineInterval("Shine Interval", Range(1, 10)) = 1
        _ShineRotate("Rotate Angle", Range(0, 360)) = 0
        _ShineWidth("Shine Width", Range(0.001,1)) = 0.1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment CustomFrag
            #pragma target 2.0
            #include "UnitySprites.cginc"

            float _UseHsvEdit;
            half4 _MainTex_ST, _MainTex_TexelSize;
            half _HsvShift, _HsvSaturation, _HsvBright;

            float _UseGradient, _GradHorizontal, _GradVertical, _GradRadiant, _UseReverseGradient, _OverlayAlpha;
            half _GradBlend, _GradValueX, _GradValueY, _GradValueR;
			half4 _GradTopRightCol, _GradTopLeftCol, _GradBotRightCol, _GradBotLeftCol;
            
            half _ShadowX, _ShadowY;
            float _UseShadow;
            float _UseShadowBlur;
			half4 _ShadowColor;
            float _UseShineEffect;
            float _ShadowBlur;
            
            half4 _ShineColor;
            half _ShineSpeed, _ShineInterval, _ShineRotate, _ShineWidth;

            float _UseDoodleEffect;
            half _DoodleAmount, _DoodleSpeed;

            float3 RGBtoHSV(float3 c) {
                float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
                float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));
            
                float d = q.x - min(q.w, q.y);
                float e = 1.0e-10;
                return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }
            
            float3 HSVtoRGB(float3 hsv) {
                float h = hsv.x * 6; // 0 ~ 6
                float s = hsv.y;
                float v = hsv.z;
            
                int h_int = (int)h;
                float f = h - h_int;
                float p = v * (1 - s);
                float q = v * (1 - s * f);
                float t = v * (1 - s * (1 - f));
            
                float3 rgb;
            
                switch(h_int) {
                    case 0: rgb = float3(v, t, p); break;
                    case 1: rgb = float3(q, v, p); break;
                    case 2: rgb = float3(p, v, t); break;
                    case 3: rgb = float3(p, q, v); break;
                    case 4: rgb = float3(t, p, v); break;
                    case 5: default: rgb = float3(v, p, q); break;
                }
            
                return rgb;
            }
            
            half4 CustomFrag (v2f i) : SV_Target {
                //////////////////////////////////////
                /// 두들 효과
                //////////////////////////////////////

                if (_UseDoodleEffect > 0.0 && _DoodleSpeed > 0.0 && _DoodleAmount > 0.0)
                {
                    half randomValue = UNITY_ACCESS_INSTANCED_PROP(Props, 0);
                    half2 uvCopy = i.texcoord;
				    _DoodleSpeed = (floor((_Time.x + randomValue) * 20 * _DoodleSpeed) / _DoodleSpeed) * _DoodleSpeed;
				    uvCopy.x = sin((uvCopy.x * _DoodleAmount + _DoodleSpeed) * 4);
				    uvCopy.y = cos((uvCopy.y * _DoodleAmount + _DoodleSpeed) * 4);
				    i.texcoord = lerp(i.texcoord, i.texcoord + uvCopy, 0.0005 * _DoodleAmount);
                }

                //////////////////////////////////////
                
                half4 col = SampleSpriteTexture (i.texcoord) * i.color;
                col.rgb *= col.a;

                //////////////////////////////////////
                /// HSV 변환
                //////////////////////////////////////
                
                if(_UseHsvEdit > 0.0)
                {
                    float3 hsv = RGBtoHSV(col.rgb);
                    hsv.x += _HsvShift / 360.0;
                    hsv.y *= _HsvSaturation;
                    hsv.z *= _HsvBright;
				    
				    col.rgb = HSVtoRGB(hsv);
                }

                //////////////////////////////////////

                
                //////////////////////////////////////
                /// 그라데이션 오버레이
                //////////////////////////////////////

                if(_UseGradient > 0.0)
                {
                    half2 tiledUvGrad = half2(i.texcoord.x / _MainTex_ST.x, i.texcoord.y / _MainTex_ST.y);
                    half4 gradientResult;
                    bool isReverse = _UseReverseGradient > 0.0;
                    
                    if (_GradRadiant > 0.0)
                    {
                        half radialDist = 1 - length(tiledUvGrad - half2(0.5, 0.5));
                        radialDist *= (_MainTex_TexelSize.w / _MainTex_TexelSize.z);
                        radialDist = saturate(_GradValueR * radialDist);
                        gradientResult = isReverse ? lerp(_GradBotLeftCol, _GradTopLeftCol, radialDist) : lerp(_GradTopLeftCol, _GradBotLeftCol, radialDist);
                    }
                    else
                    {
                        if (_GradVertical > 0.0)
                        {
                            _GradTopRightCol = _GradTopLeftCol;
                            _GradBotRightCol = _GradBotLeftCol;
                        }
                        else if (_GradHorizontal > 0.0)
                        {
                            if (isReverse)
                            {
                                half4 temp = _GradTopRightCol;
                                _GradTopRightCol = _GradTopLeftCol;
                                _GradTopLeftCol = temp;
                            }
                            _GradBotRightCol = _GradTopRightCol;
                            _GradBotLeftCol =  _GradTopLeftCol;
                        }
                        else if (isReverse)
                        {
                            half4 temp = _GradTopRightCol;
                            _GradTopRightCol = _GradTopLeftCol;
                            _GradTopLeftCol = temp;

                            temp = _GradBotRightCol;
                            _GradBotRightCol = _GradBotLeftCol;
                            _GradBotLeftCol = temp;
                        }
                        
                        const half lerpX = saturate(pow(tiledUvGrad.x, _GradValueX));
                        const half lerpY = saturate(pow(tiledUvGrad.y, _GradValueY));
                        const half4 bottomValue = lerp(_GradBotLeftCol, _GradBotRightCol, lerpX);
                        const half4 topValue = lerp(_GradTopLeftCol, _GradTopRightCol, lerpX);
                        gradientResult = lerp(
                            isReverse ? topValue : bottomValue,
                            isReverse ? bottomValue : topValue,
                            lerpY
                        );
                    }
                    gradientResult = lerp(col, gradientResult, gradientResult.a * _GradBlend);
                    col.rgb = gradientResult.rgb * col.a;
                    if (_OverlayAlpha > 0.0) col.a *= gradientResult.a;
                }
                
                //////////////////////////////////////
                
                
                //////////////////////////////////////
                /// Shine Effect
                //////////////////////////////////////
                
                if(_UseShineEffect > 0.0 && _ShineColor.a > 0.0)
                {
                    float rotation = _ShineRotate * (UNITY_PI / 180.0);
                    half cosAngle = cos(rotation);
                    half sinAngle = sin(rotation);
                    half2x2 rotMatrix = half2x2(cosAngle, -sinAngle, sinAngle, cosAngle);

                    half2 shineCenter = i.texcoord;
                    shineCenter -= half2(0.5, 0.5);
                    shineCenter = mul(rotMatrix, shineCenter);
                    shineCenter += half2(0.5, 0.5);
                    
                    half shineUv = (shineCenter.x + shineCenter.y) / 2.0;
                    float shineMoveValue = (_Time.y * _ShineSpeed) % (_ShineInterval * _ShineSpeed);
                    half shinePower = 1 - (abs(shineUv - shineMoveValue) / _ShineWidth);
                    half shinedTextureAlpha = max(sign((shineMoveValue + _ShineWidth) - shineUv), 0.0) * max(sign(shineUv - (shineMoveValue - _ShineWidth)), 0.0);
                    col.rgb += col.a * shinePower * shinedTextureAlpha * _ShineColor.rgb * _ShineColor.a;
                }
                
                //////////////////////////////////////
                

                //////////////////////////////////////
                // 그림자 효과
                //////////////////////////////////////
                
                if(_UseShadow > 0.0)
                {
                    half shadowAlpha = 0;
                    if(_UseShadowBlur > 0 && _ShadowBlur > 0.0)
                    {
                        // 블러를 위한 텍스처 샘플링 오프셋
                        float2 baseOffset = half2(_ShadowX, _ShadowY);
                        float2 offsets[9] = {
                            baseOffset + half2(-1,  1) * _ShadowBlur, baseOffset + half2(0,  1), baseOffset + half2(1,  1) * _ShadowBlur,
                            baseOffset + half2(-1,  0) * _ShadowBlur, baseOffset,                baseOffset + half2(1,  0) * _ShadowBlur,
                            baseOffset + half2(-1, -1) * _ShadowBlur, baseOffset + half2(0, -1), baseOffset + half2(1, -1) * _ShadowBlur,
                        };
                        
                        // 주변 픽셀을 샘플링하여 평균 그림자 알파값을 계산
                        for (int j = 0; j < 9; j++) {
                            shadowAlpha += tex2D(_MainTex, i.texcoord - offsets[j]).a;
                        }
                        shadowAlpha /= 9.0;
                    }
                    else
                    {
                        shadowAlpha = tex2D(_MainTex, i.texcoord - half2(_ShadowX, _ShadowY)).a;
                    }
                    
				    col.rgb *= 1 - ((shadowAlpha - col.a) * (1 - col.a));
				    col.rgb += (_ShadowColor * shadowAlpha) * (1 - col.a);
				    col.a = max(shadowAlpha * _ShadowColor.a * i.color.a, col.a);
                    }

                //////////////////////////////////////
                

                return col;
            }
        ENDCG
        }
    }
    CustomEditor "CustomShaderEditor"
}