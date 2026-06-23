Shader "Blueys/BlueysTextureSimple"
{
    Properties
    {
        _MainTex ("Main PNG Texture", 2D) = "white" {}
        _Color ("PNG Tint", Color) = (1,1,1,1)

        _Brightness ("PNG Brightness", Range(0,5)) = 1
        _Contrast ("PNG Contrast", Range(0,3)) = 1
        _Saturation ("PNG Saturation", Range(0,3)) = 1

        _Smoothness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0

        [Toggle] _UseSolidOverlay ("Use Colour Overlay", Float) = 0
        _SolidColor ("Overlay Colour", Color) = (1,1,1,1)
        _SolidStrength ("Overlay Strength", Range(0,1)) = 0

        [Toggle] _UseEmission ("Use Emission", Float) = 0
        _EmissionMap ("Emission Image", 2D) = "white" {}
        _EmissionMask ("Emission Mask", 2D) = "white" {}
        _EmissionColor ("Emission Colour", Color) = (0.2,0.7,1,1)
        _EmissionStrength ("Emission Strength", Range(0,20)) = 1
        _EmissionUsesPNG ("Emission Uses Main PNG", Range(0,1)) = 1

        [Toggle] _UseRimGlow ("Use Rim Glow", Float) = 1
        _RimColor ("Rim Glow Colour", Color) = (0.35,0.8,1,1)
        _RimPower ("Rim Tightness", Range(0.5,10)) = 3
        _RimStrength ("Rim Strength", Range(0,10)) = 1

        [Toggle] _UseCutout ("Use PNG Cutout Shape", Float) = 0
        _AlphaCutoff ("PNG Alpha Cutoff", Range(0,1)) = 0.05
    }

    SubShader
    {
        Tags
        {
            "Queue"="Geometry"
            "RenderType"="Opaque"
        }

        LOD 400
        Cull Back
        ZWrite On

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _EmissionMap;
        sampler2D _EmissionMask;

        fixed4 _Color;

        half _Brightness;
        half _Contrast;
        half _Saturation;

        half _Smoothness;
        half _Metallic;

        half _UseSolidOverlay;
        fixed4 _SolidColor;
        half _SolidStrength;

        half _UseEmission;
        fixed4 _EmissionColor;
        half _EmissionStrength;
        half _EmissionUsesPNG;

        half _UseRimGlow;
        fixed4 _RimColor;
        half _RimPower;
        half _RimStrength;

        half _UseCutout;
        half _AlphaCutoff;

        struct Input
        {
            float2 uv_MainTex;
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

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 png = tex2D(_MainTex, IN.uv_MainTex) * _Color;

            if (_UseCutout > 0.5)
            {
                clip(png.a - _AlphaCutoff);
            }

            fixed3 col = png.rgb;

            col = ContrastAdjust(col, _Contrast);
            col = SaturationAdjust(col, _Saturation);
            col *= _Brightness;

            if (_UseSolidOverlay > 0.5)
            {
                col = lerp(col, _SolidColor.rgb, _SolidStrength);
            }

            fixed3 emission = fixed3(0,0,0);

            if (_UseEmission > 0.5)
            {
                fixed3 emissionImage = tex2D(_EmissionMap, IN.uv_MainTex).rgb;
                fixed3 emissionMask = tex2D(_EmissionMask, IN.uv_MainTex).rgb;

                fixed3 emissionBase = lerp(emissionImage, emissionImage * png.rgb, _EmissionUsesPNG);
                emission += emissionBase * emissionMask * _EmissionColor.rgb * _EmissionStrength;
            }

            if (_UseRimGlow > 0.5)
            {
                half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
                emission += _RimColor.rgb * pow(rim, _RimPower) * _RimStrength;
            }

            o.Albedo = saturate(col);
            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness;
            o.Emission = emission;
            o.Alpha = 1;
        }
        ENDCG
    }

    FallBack "Diffuse"
    CustomEditor "BlueysTextureSimpleGUI"
}