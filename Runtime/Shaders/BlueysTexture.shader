Shader "Blueys/BlueysTexture"
{
    Properties
    {
        [HideInInspector] _Cull ("Cull", Float) = 2

        _MainTex ("Main Image Texture", 2D) = "white" {}
        _Color ("Texture Tint", Color) = (1,1,1,1)
        _Alpha ("Transparency", Range(0,1)) = 1

        _UseTextureBoost ("Use Texture Boost", Float) = 0
        _TextureStrength ("Texture Strength", Range(0,3)) = 1
        _Contrast ("Contrast", Range(0,3)) = 1
        _Brightness ("Brightness", Range(0,3)) = 1
        _Saturation ("Saturation", Range(0,3)) = 1

        _UseDetail ("Use Detail Overlay", Float) = 0
        _DetailTex ("Detail Texture", 2D) = "gray" {}
        _DetailStrength ("Detail Strength", Range(0,2)) = 0.2
        _DetailTiling ("Detail Tiling", Range(1,40)) = 8

        _UseNormal ("Use Normal Map", Float) = 0
        [Normal] _BumpMap ("Normal Map", 2D) = "bump" {}
        _BumpStrength ("Normal Strength", Range(0,2)) = 0.4

        _UseWetShine ("Use Wet Shine", Float) = 0
        _Smoothness ("Smoothness", Range(0,1)) = 1
        _SpecularStrength ("Specular Strength", Range(0,1)) = 0.5

        _UseEdgeGlow ("Use Edge Glow", Float) = 0
        _RimColor ("Edge Glow Color", Color) = (0.35,0.9,1,1)
        _RimPower ("Edge Tightness", Range(0.5,8)) = 3
        _RimStrength ("Edge Strength", Range(0,8)) = 2
        _EdgeAlphaBoost ("Edge Alpha Boost", Range(0,0.5)) = 0.1

        _UseDepth ("Use Deep Color", Float) = 0
        _DepthColor ("Deep Color", Color) = (0,0.16,0.75,1)
        _DepthStrength ("Deep Strength", Range(0,2)) = 0.5

        _UseInnerGlow ("Use Inner Glow", Float) = 0
        _InnerColor ("Inner Glow Color", Color) = (0.15,0.75,1,1)
        _InnerStrength ("Inner Strength", Range(0,5)) = 0.6
        _InnerPower ("Inner Softness", Range(0.5,8)) = 2

        _UseEmission ("Use Emission Texture", Float) = 0
        _EmissionMap ("Emission Texture", 2D) = "black" {}
        _EmissionColor ("Emission Color", Color) = (0.1,0.7,1,1)
        _EmissionStrength ("Emission Strength", Range(0,8)) = 1

        _UseReflection ("Use Fake Reflection", Float) = 0
        _ReflectionColor ("Reflection Color", Color) = (0.7,0.95,1,1)
        _ReflectionStrength ("Reflection Strength", Range(0,3)) = 0.4
        _ReflectionPower ("Reflection Tightness", Range(0.5,10)) = 4

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

        sampler2D _MainTex;
        sampler2D _DetailTex;
        sampler2D _BumpMap;
        sampler2D _EmissionMap;

        fixed4 _Color;
        half _Alpha;

        half _UseTextureBoost;
        half _TextureStrength;
        half _Contrast;
        half _Brightness;
        half _Saturation;

        half _UseDetail;
        half _DetailStrength;
        half _DetailTiling;

        half _UseNormal;
        half _BumpStrength;

        half _UseWetShine;
        half _Smoothness;
        half _SpecularStrength;

        half _UseEdgeGlow;
        fixed4 _RimColor;
        half _RimPower;
        half _RimStrength;
        half _EdgeAlphaBoost;

        half _UseDepth;
        fixed4 _DepthColor;
        half _DepthStrength;

        half _UseInnerGlow;
        fixed4 _InnerColor;
        half _InnerStrength;
        half _InnerPower;

        half _UseEmission;
        fixed4 _EmissionColor;
        half _EmissionStrength;

        half _UseReflection;
        fixed4 _ReflectionColor;
        half _ReflectionStrength;
        half _ReflectionPower;

        half _FinalGlowPower;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_DetailTex;
            float2 uv_BumpMap;
            float2 uv_EmissionMap;
            float3 viewDir;
        };

        fixed3 ContrastAdjust(fixed3 c, half v)
        {
            return saturate((c - 0.5) * v + 0.5);
        }

        fixed3 SaturationAdjust(fixed3 c, half v)
        {
            half gray = dot(c, fixed3(0.299, 0.587, 0.114));
            return lerp(fixed3(gray, gray, gray), c, v);
        }

        void surf(Input IN, inout SurfaceOutputStandardSpecular o)
        {
            fixed4 mainTex = tex2D(_MainTex, IN.uv_MainTex);
            fixed3 col = mainTex.rgb * _Color.rgb;

            if (_UseTextureBoost > 0.5)
            {
                fixed3 boosted = mainTex.rgb;
                boosted = ContrastAdjust(boosted, _Contrast);
                boosted = SaturationAdjust(boosted, _Saturation);
                boosted *= _Brightness;
                col = lerp(col, boosted * _Color.rgb * 2.0, _TextureStrength);
            }

            if (_UseDetail > 0.5)
            {
                fixed3 detail = tex2D(_DetailTex, IN.uv_MainTex * _DetailTiling).rgb;
                col *= lerp(fixed3(1,1,1), detail * 2.0, _DetailStrength);
            }

            if (_UseNormal > 0.5)
            {
                fixed3 n = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
                o.Normal = lerp(fixed3(0,0,1), n, _BumpStrength);
            }

            half fresnel = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));

            if (_UseDepth > 0.5)
            {
                col = lerp(col, _DepthColor.rgb, fresnel * _DepthStrength);
            }

            fixed3 emission = fixed3(0,0,0);

            if (_UseEdgeGlow > 0.5)
            {
                half rim = pow(fresnel, _RimPower) * _RimStrength;
                emission += _RimColor.rgb * rim;
            }

            if (_UseInnerGlow > 0.5)
            {
                half inner = pow(fresnel, _InnerPower) * _InnerStrength;
                emission += _InnerColor.rgb * inner;
            }

            if (_UseEmission > 0.5)
            {
                fixed3 e = tex2D(_EmissionMap, IN.uv_EmissionMap).rgb;
                emission += e * _EmissionColor.rgb * _EmissionStrength;
            }

            if (_UseReflection > 0.5)
            {
                half refl = pow(fresnel, _ReflectionPower) * _ReflectionStrength;
                emission += _ReflectionColor.rgb * refl;
            }

            o.Albedo = saturate(col);
            o.Specular = _UseWetShine > 0.5 ? fixed3(_SpecularStrength, _SpecularStrength, _SpecularStrength) : fixed3(0.2,0.2,0.2);
            o.Smoothness = _UseWetShine > 0.5 ? _Smoothness : 0.5;
            o.Emission = emission * _FinalGlowPower;

            half edgeAlpha = (_UseEdgeGlow > 0.5) ? fresnel * _EdgeAlphaBoost : 0;
            o.Alpha = saturate(mainTex.a * _Color.a * _Alpha + edgeAlpha);
        }
        ENDCG
    }

    FallBack "Transparent/Diffuse"
    CustomEditor "BlueysTextureGUI"
}