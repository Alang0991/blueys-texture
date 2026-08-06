Shader "Blueys/BlueysTexture"
{
    Properties
    {
        [HideInInspector] _Cull ("Cull", Float) = 2

        _MainTex ("Main Image Texture", 2D) = "white" {}
        _MainTiling ("Main Tiling", Vector) = (1,1,0,0)
        _MainOffset ("Main Offset", Vector) = (0,0,0,0)
        _Color ("Texture Tint", Color) = (1,1,1,1)
        _Alpha ("Transparency", Range(0,1)) = 1

        [Toggle] _UseTextureBoost ("Texture Enhancement", Float) = 0
        _TextureStrength ("Texture Strength", Range(0,3)) = 1
        _Contrast ("Contrast", Range(0,3)) = 1
        _Brightness ("Brightness", Range(0,3)) = 1
        _Saturation ("Saturation", Range(0,3)) = 1
        _HueShift ("Hue Shift", Range(-180,180)) = 0
        _Gamma ("Gamma", Range(0.1,3)) = 1
        _Vibrance ("Vibrance", Range(-1,1)) = 0
        _Sharpness ("Sharpness", Range(0,2)) = 0

        [Toggle] _UseDetail ("Detail Overlay", Float) = 0
        _DetailTex ("Detail Texture", 2D) = "gray" {}
        _DetailStrength ("Detail Strength", Range(0,2)) = 0.2
        _DetailTiling ("Detail Tiling", Range(1,40)) = 8
        _DetailOffset ("Detail Offset", Vector) = (0,0,0,0)

        [Toggle] _UseNormal ("Normal Map", Float) = 0
        [Normal] _BumpMap ("Normal Map", 2D) = "bump" {}
        _BumpStrength ("Normal Strength", Range(0,2)) = 0.4

        [Toggle] _UseWetShine ("Wet Shine", Float) = 0
        _Smoothness ("Smoothness", Range(0,1)) = 1
        _SpecularStrength ("Specular Strength", Range(0,1)) = 0.5
        _MetallicMap ("Metallic Map", 2D) = "black" {}
        _MetallicStrength ("Metallic Strength", Range(0,1)) = 0
        _SmoothnessMap ("Smoothness Map", 2D) = "black" {}
        _SmoothnessStrength ("Smoothness Strength", Range(0,1)) = 0

        [Toggle] _UseEdgeGlow ("Edge Glow", Float) = 0
        _RimColor ("Edge Glow Color", Color) = (0.35,0.9,1,1)
        _RimPower ("Edge Tightness", Range(0.5,8)) = 3
        _RimStrength ("Edge Strength", Range(0,8)) = 2
        _EdgeAlphaBoost ("Edge Alpha Boost", Range(0,0.5)) = 0.1

        [Toggle] _UseDepth ("Deep Color", Float) = 0
        _DepthColor ("Deep Color", Color) = (0,0.16,0.75,1)
        _DepthStrength ("Deep Strength", Range(0,2)) = 0.5

        [Toggle] _UseInnerGlow ("Inner Glow", Float) = 0
        _InnerColor ("Inner Glow Color", Color) = (0.15,0.75,1,1)
        _InnerStrength ("Inner Strength", Range(0,5)) = 0.6
        _InnerPower ("Inner Softness", Range(0.5,8)) = 2

        [Toggle] _UseEmission ("Emission Texture", Float) = 0
        _EmissionMap ("Emission Texture", 2D) = "black" {}
        _EmissionColor ("Emission Color", Color) = (0.1,0.7,1,1)
        _EmissionStrength ("Emission Strength", Range(0,8)) = 1
        _PulseSpeed ("Pulse Speed", Range(0,10)) = 0
        _PulseMin ("Pulse Min", Range(0,1)) = 0.5
        _FlickerSpeed ("Flicker Speed", Range(0,20)) = 0
        _FlickerIntensity ("Flicker Intensity", Range(0,1)) = 0
        _ScrollSpeed ("Scroll Speed", Range(0,10)) = 0
        _ScrollDirection ("Scroll Direction", Range(0,360)) = 0

        [Toggle] _UseReflection ("Fake Reflection", Float) = 0
        _ReflectionColor ("Reflection Color", Color) = (0.7,0.95,1,1)
        _ReflectionStrength ("Reflection Strength", Range(0,3)) = 0.4
        _ReflectionPower ("Reflection Tightness", Range(0.5,10)) = 4
        _ReflectionMap ("Reflection Map", 2D) = "black" {}
        _ReflectionBlend ("Reflection Blend", Range(0,1)) = 0

        [Toggle] _UseOutline ("Outline", Float) = 0
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range(0,0.1)) = 0
        _OutlineThreshold ("Outline Threshold", Range(0,1)) = 0.1

        [Toggle] _UseDissolve ("Dissolve", Float) = 0
        _DissolveAmount ("Dissolve Amount", Range(0,1)) = 0
        _DissolveEdgeWidth ("Dissolve Edge Width", Range(0,0.1)) = 0.05
        _DissolveEdgeColor ("Dissolve Edge Color", Color) = (1,0.5,0,1)

        [Toggle] _UseMatcap ("Matcap", Float) = 0
        _MatcapTex ("Matcap", 2D) = "black" {}
        _MatcapStrength ("Matcap Strength", Range(0,1)) = 0

        [Toggle] _UseGradient ("Gradient", Float) = 0
        _GradientTex ("Gradient Texture", 2D) = "white" {}
        _GradientStrength ("Gradient Strength", Range(0,1)) = 0

        _OcclusionMap ("Occlusion Map", 2D) = "white" {}
        _OcclusionStrength ("Occlusion Strength", Range(0,1)) = 1

        _FinalGlowPower ("Final Glow Power", Range(0,3)) = 1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }

        LOD 400
        Cull [_Cull]
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        #pragma surface surf StandardSpecular alpha:fade
        #pragma target 3.0

        #pragma multi_compile _USE_TEXTURE_BOOST _USE_TEXTURE_BOOST_OFF
        #pragma multi_compile _USE_DETAIL _USE_DETAIL_OFF
        #pragma multi_compile _USE_NORMAL _USE_NORMAL_OFF
        #pragma multi_compile _USE_WET_SHINE _USE_WET_SHINE_OFF
        #pragma multi_compile _USE_EDGE_GLOW _USE_EDGE_GLOW_OFF
        #pragma multi_compile _USE_DEPTH _USE_DEPTH_OFF
        #pragma multi_compile _USE_INNER_GLOW _USE_INNER_GLOW_OFF
        #pragma multi_compile _USE_EMISSION _USE_EMISSION_OFF
        #pragma multi_compile _USE_REFLECTION _USE_REFLECTION_OFF
        #pragma multi_compile _USE_OUTLINE _USE_OUTLINE_OFF
        #pragma multi_compile _USE_DISSOLVE _USE_DISSOLVE_OFF
        #pragma multi_compile _USE_MATCAP _USE_MATCAP_OFF
        #pragma multi_compile _USE_GRADIENT _USE_GRADIENT_OFF

        sampler2D _MainTex;
        sampler2D _DetailTex;
        sampler2D _BumpMap;
        sampler2D _EmissionMap;
        sampler2D _ReflectionMap;
        sampler2D _OcclusionMap;
        sampler2D _MetallicMap;
        sampler2D _SmoothnessMap;
        sampler2D _MatcapTex;
        sampler2D _GradientTex;

        fixed4 _Color;
        half _Alpha;

        half _TextureStrength;
        half _Contrast;
        half _Brightness;
        half _Saturation;
        half _HueShift;
        half _Gamma;
        half _Vibrance;
        half _Sharpness;

        half _DetailStrength;
        half _DetailTiling;
        float4 _DetailOffset;

        half _BumpStrength;

        half _Smoothness;
        half _SpecularStrength;
        half _MetallicStrength;
        half _SmoothnessStrength;

        fixed4 _RimColor;
        half _RimPower;
        half _RimStrength;
        half _EdgeAlphaBoost;

        fixed4 _DepthColor;
        half _DepthStrength;

        fixed4 _InnerColor;
        half _InnerStrength;
        half _InnerPower;

        fixed4 _EmissionColor;
        half _EmissionStrength;
        half _PulseSpeed;
        half _PulseMin;
        half _FlickerSpeed;
        half _FlickerIntensity;
        half _ScrollSpeed;
        half _ScrollDirection;

        fixed4 _ReflectionColor;
        half _ReflectionStrength;
        half _ReflectionPower;
        half _ReflectionBlend;

        fixed4 _OutlineColor;
        half _OutlineWidth;
        half _OutlineThreshold;

        half _DissolveAmount;
        half _DissolveEdgeWidth;
        fixed4 _DissolveEdgeColor;

        half _MatcapStrength;

        half _GradientStrength;

        half _OcclusionStrength;

        half _FinalGlowPower;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_DetailTex;
            float2 uv_BumpMap;
            float2 uv_EmissionMap;
            float2 uv_ReflectionMap;
            float2 uv_OcclusionMap;
            float2 uv_MetallicMap;
            float2 uv_SmoothnessMap;
            float2 uv_MatcapTex;
            float2 uv_GradientTex;
            float3 viewDir;
            float3 worldPos;
            INTERNAL_DATA
        };

        fixed3 HueRotate(fixed3 c, half shift)
        {
            shift = shift / 360.0 + 1.0;
            fixed3 coshift = cos(shift * 6.2831853 + fixed3(0.0, 2.0943951, 4.1887902));
            half gray = dot(c, fixed3(0.299, 0.587, 0.114));
            return fixed3(
                gray + dot(c, fixed3(0.701, -0.587, -0.114) * coshift.x) + dot(c, fixed3(-0.299, -0.587, 0.701) * coshift.y) + dot(c, fixed3(-0.300, 0.587, -0.587) * coshift.z),
                gray + dot(c, fixed3(0.701, -0.587, -0.114) * coshift.x + 0.168736) + dot(c, fixed3(-0.299, -0.587, 0.701) * coshift.y + 0.328416) + dot(c, fixed3(-0.300, 0.587, -0.587) * coshift.z + 0.5),
                gray + dot(c, fixed3(0.701, -0.587, -0.114) * coshift.x + 0.5) + dot(c, fixed3(-0.299, -0.587, 0.701) * coshift.y + 0.330864) + dot(c, fixed3(-0.300, 0.587, -0.587) * coshift.z + 0.5)
            );
        }

        fixed3 GammaAdjust(fixed3 c, half g)
        {
            return pow(c, fixed3(1.0/g, 1.0/g, 1.0/g));
        }

        fixed3 VibranceAdjust(fixed3 c, half v)
        {
            half maxC = max(c.r, max(c.g, c.b));
            half minC = min(c.r, min(c.g, c.b));
            half sat = (maxC - minC) / (maxC + 0.0001);
            half boost = (1.0 - sat) * v;
            return c + boost * (maxC - c);
        }

        fixed3 ContrastAdjust(fixed3 c, half v)
        {
            return saturate((c - 0.5) * v + 0.5);
        }

        fixed3 SaturationAdjust(fixed3 c, half v)
        {
            half gray = dot(c, fixed3(0.299, 0.587, 0.114));
            return lerp(fixed3(gray, gray, gray), c, v);
        }

        fixed3 SharpnessAdjust(fixed3 c, half s, fixed3 blurred)
        {
            return lerp(blurred, c, 1.0 + s);
        }

        fixed3 ScrolledUV(fixed2 uv, half speed, half direction)
        {
            float rad = direction * 3.14159 / 180.0;
            fixed2 dir = fixed2(cos(rad), sin(rad));
            return tex2D(_MainTex, uv + dir * _Time.y * speed).rgb;
        }

        void surf(Input IN, inout SurfaceOutputStandardSpecular o)
        {
            fixed4 mainTex = tex2D(_MainTex, IN.uv_MainTex);
            fixed3 col = mainTex.rgb * _Color.rgb;

            #if _USE_TEXTURE_BOOST
                fixed3 boosted = mainTex.rgb;
                boosted = ContrastAdjust(boosted, _Contrast);
                boosted = SaturationAdjust(boosted, _Saturation);
                boosted *= _Brightness;
                boosted = HueRotate(boosted, _HueShift);
                boosted = GammaAdjust(boosted, _Gamma);
                boosted = VibranceAdjust(boosted, _Vibrance);
                col = lerp(col, boosted * _Color.rgb * 2.0, _TextureStrength);
            #endif

            #if _USE_DETAIL
                fixed2 detailUV = IN.uv_MainTex * _DetailTiling + _DetailOffset.xy;
                fixed3 detail = tex2D(_DetailTex, detailUV).rgb;
                col *= lerp(fixed3(1,1,1), detail * 2.0, _DetailStrength);
            #endif

            #if _USE_NORMAL
                fixed3 n = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
                o.Normal = lerp(fixed3(0,0,1), n, _BumpStrength);
            #endif

            half fresnel = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));

            #if _USE_DEPTH
                col = lerp(col, _DepthColor.rgb, fresnel * _DepthStrength);
            #endif

            fixed3 emission = fixed3(0,0,0);

            #if _USE_EDGE_GLOW
                half rim = pow(fresnel, _RimPower) * _RimStrength;
                emission += _RimColor.rgb * rim;
            #endif

            #if _USE_INNER_GLOW
                half inner = pow(fresnel, _InnerPower) * _InnerStrength;
                emission += _InnerColor.rgb * inner;
            #endif

            #if _USE_EMISSION
                fixed3 e = tex2D(_EmissionMap, IN.uv_EmissionMap).rgb;
                e *= _EmissionColor.rgb * _EmissionStrength;
                e *= _PulseSpeed > 0 ? _PulseMin + (1.0 - _PulseMin) * (0.5 + 0.5 * sin(_Time.y * _PulseSpeed * 6.28)) : 1.0;
                e *= _FlickerSpeed > 0 ? 1.0 - _FlickerIntensity * (0.5 + 0.5 * sin(_Time.y * _FlickerSpeed * 6.28)) * (0.5 + 0.5 * sin(_Time.y * _FlickerSpeed * 12.56)) : 1.0;
                fixed2 scrollUV = IN.uv_EmissionMap;
                float rad = _ScrollDirection * 3.14159 / 180.0;
                scrollUV += fixed2(cos(rad), sin(rad)) * _Time.y * _ScrollSpeed;
                e *= tex2D(_EmissionMap, scrollUV).rgb;
                emission += e;
            #endif

            #if _USE_REFLECTION
                half refl = pow(fresnel, _ReflectionPower) * _ReflectionStrength;
                fixed3 reflTex = tex2D(_ReflectionMap, IN.uv_ReflectionMap).rgb;
                emission += _ReflectionColor.rgb * refl * lerp(fixed3(1,1,1), reflTex, _ReflectionBlend);
            #endif

            #if _USE_MATCAP
                half2 matcapUV = o.Normal.xy * 0.5 + 0.5;
                emission += tex2D(_MatcapTex, matcapUV).rgb * _MatcapStrength;
            #endif

            #if _USE_GRADIENT
                fixed3 gradTex = tex2D(_GradientTex, IN.uv_MainTex).rgb;
                emission += gradTex * gradTex.r * _GradientStrength;
            #endif

            #if _USE_DISSOLVE
                half dissolve = mainTex.a - _DissolveAmount;
                clip(dissolve);
                if (dissolve < _DissolveEdgeWidth)
                {
                    emission += _DissolveEdgeColor.rgb * (1.0 - dissolve / _DissolveEdgeWidth);
                }
            #endif

            o.Albedo = saturate(col);

            #if _USE_WET_SHINE
                half metallic = tex2D(_MetallicMap, IN.uv_MetallicMap).r * _MetallicStrength;
                half smoothness = tex2D(_SmoothnessMap, IN.uv_SmoothnessMap).r * _SmoothnessStrength;
                half finalSmoothness = lerp(_Smoothness, smoothness, _SmoothnessStrength);
                o.Specular = fixed3(metallic, metallic, metallic);
                o.Specular.a = finalSmoothness;
            #else
                o.Specular = fixed3(_SpecularStrength, _SpecularStrength, _SpecularStrength);
                o.Specular.a = 0.5;
            #endif

            #if _USE_OCCLUSION
                half occ = tex2D(_OcclusionMap, IN.uv_OcclusionMap).r;
                o.Albedo *= lerp(1.0, occ, _OcclusionStrength);
            #endif

            o.Emission = emission * _FinalGlowPower;

            #if _USE_OUTLINE
                half edgeAlpha = fresnel * _OutlineWidth * 100.0;
                o.Alpha = saturate(mainTex.a * _Color.a * _Alpha + edgeAlpha);
            #else
                half edgeAlpha = fresnel * _EdgeAlphaBoost;
                o.Alpha = saturate(mainTex.a * _Color.a * _Alpha + edgeAlpha);
            #endif
        }
        ENDCG
    }

    FallBack "Transparent/Diffuse"
    CustomEditor "BlueysTextureGUI"
}
