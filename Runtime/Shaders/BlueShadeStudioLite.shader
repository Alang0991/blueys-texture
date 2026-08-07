Shader "Blueys/BlueShade Lite"
{
    Properties
    {
        _MainTex ("Main PNG Texture", 2D) = "white" {}
        _MainTiling ("Main Tiling", Vector) = (1,1,0,0)
        _MainOffset ("Main Offset", Vector) = (0,0,0,0)
        _Color ("PNG Tint", Color) = (1,1,1,1)

        _Brightness ("PNG Brightness", Range(0,5)) = 1
        _Contrast ("PNG Contrast", Range(0,3)) = 1
        _Saturation ("PNG Saturation", Range(0,3)) = 1
        _HueShift ("Hue Shift", Range(-180,180)) = 0
        _Gamma ("Gamma", Range(0.1,3)) = 1
        _Vibrance ("Vibrance", Range(-1,1)) = 0
        _Sharpness ("Sharpness", Range(0,2)) = 0

        _Smoothness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0
        _MetallicMap ("Metallic Map", 2D) = "black" {}
        _MetallicStrength ("Metallic Strength", Range(0,1)) = 0
        _SmoothnessMap ("Smoothness Map", 2D) = "black" {}
        _SmoothnessStrength ("Smoothness Strength", Range(0,1)) = 0

        [Toggle] _UseSolidOverlay ("Use Colour Overlay", Float) = 0
        _SolidColor ("Overlay Colour", Color) = (1,1,1,1)
        _SolidStrength ("Overlay Strength", Range(0,1)) = 0

        [Toggle] _UseEmission ("Use Emission", Float) = 0
        _EmissionMap ("Emission Image", 2D) = "white" {}
        _EmissionMask ("Emission Mask", 2D) = "white" {}
        _EmissionColor ("Emission Colour", Color) = (0.2,0.7,1,1)
        _EmissionStrength ("Emission Strength", Range(0,20)) = 1
        _EmissionUsesPNG ("Emission Uses Main PNG", Range(0,1)) = 1
        _PulseSpeed ("Pulse Speed", Range(0,10)) = 0
        _PulseMin ("Pulse Min", Range(0,1)) = 0.5
        _FlickerSpeed ("Flicker Speed", Range(0,20)) = 0
        _FlickerIntensity ("Flicker Intensity", Range(0,1)) = 0
        _ScrollSpeed ("Scroll Speed", Range(0,10)) = 0
        _ScrollDirection ("Scroll Direction", Range(0,360)) = 0

        [Toggle] _UseRimGlow ("Use Rim Glow", Float) = 1
        _RimColor ("Rim Glow Colour", Color) = (0.35,0.8,1,1)
        _RimPower ("Rim Tightness", Range(0.5,10)) = 3
        _RimStrength ("Rim Strength", Range(0,10)) = 1

        [Toggle] _UseCutout ("Use PNG Cutout Shape", Float) = 0
        _AlphaCutoff ("PNG Alpha Cutoff", Range(0,1)) = 0.05

        [Toggle] _UseMatcap ("Matcap", Float) = 0
        _MatcapTex ("Matcap", 2D) = "black" {}
        _MatcapStrength ("Matcap Strength", Range(0,1)) = 0

        [Toggle] _UseGradient ("Gradient", Float) = 0
        _GradientTex ("Gradient Texture", 2D) = "white" {}
        _GradientStrength ("Gradient Strength", Range(0,1)) = 0

        _OcclusionMap ("Occlusion Map", 2D) = "white" {}
        _OcclusionStrength ("Occlusion Strength", Range(0,1)) = 1
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

        #pragma multi_compile _USE_SOLID_OVERLAY _USE_SOLID_OVERLAY_OFF
        #pragma multi_compile _USE_EMISSION _USE_EMISSION_OFF
        #pragma multi_compile _USE_RIM_GLOW _USE_RIM_GLOW_OFF
        #pragma multi_compile _USE_CUTOUT _USE_CUTOUT_OFF
        #pragma multi_compile _USE_MATCAP _USE_MATCAP_OFF
        #pragma multi_compile _USE_GRADIENT _USE_GRADIENT_OFF

        sampler2D _MainTex;
        sampler2D _EmissionMap;
        sampler2D _EmissionMask;
        sampler2D _MetallicMap;
        sampler2D _SmoothnessMap;
        sampler2D _MatcapTex;
        sampler2D _GradientTex;
        sampler2D _OcclusionMap;

        fixed4 _Color;

        half _Brightness;
        half _Contrast;
        half _Saturation;
        half _HueShift;
        half _Gamma;
        half _Vibrance;
        half _Sharpness;

        half _Smoothness;
        half _Metallic;
        half _MetallicStrength;
        half _SmoothnessStrength;

        half _UseSolidOverlay;
        fixed4 _SolidColor;
        half _SolidStrength;

        half _UseEmission;
        fixed4 _EmissionColor;
        half _EmissionStrength;
        half _EmissionUsesPNG;
        half _PulseSpeed;
        half _PulseMin;
        half _FlickerSpeed;
        half _FlickerIntensity;
        half _ScrollSpeed;
        half _ScrollDirection;

        half _UseRimGlow;
        fixed4 _RimColor;
        half _RimPower;
        half _RimStrength;

        half _UseCutout;
        half _AlphaCutoff;

        half _MatcapStrength;
        half _GradientStrength;
        half _OcclusionStrength;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_EmissionMap;
            float2 uv_MetallicMap;
            float2 uv_SmoothnessMap;
            float2 uv_MatcapTex;
            float2 uv_GradientTex;
            float2 uv_OcclusionMap;
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

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 png = tex2D(_MainTex, IN.uv_MainTex) * _Color;

            #if _USE_CUTOUT
                clip(png.a - _AlphaCutoff);
            #endif

            fixed3 col = png.rgb;

            col = ContrastAdjust(col, _Contrast);
            col = SaturationAdjust(col, _Saturation);
            col *= _Brightness;
            col = HueRotate(col, _HueShift);
            col = GammaAdjust(col, _Gamma);
            col = VibranceAdjust(col, _Vibrance);

            #if _USE_SOLID_OVERLAY
                col = lerp(col, _SolidColor.rgb, _SolidStrength);
            #endif

            #if _USE_OCCLUSION
                half occ = tex2D(_OcclusionMap, IN.uv_OcclusionMap).r;
                col *= lerp(1.0, occ, _OcclusionStrength);
            #endif

            fixed3 emission = fixed3(0,0,0);

            #if _USE_EMISSION
                fixed3 emissionImage = tex2D(_EmissionMap, IN.uv_MainTex).rgb;
                fixed3 emissionMask = tex2D(_EmissionMask, IN.uv_MainTex).rgb;

                fixed3 emissionBase = lerp(emissionImage, emissionImage * png.rgb, _EmissionUsesPNG);
                emission += emissionBase * emissionMask * _EmissionColor.rgb * _EmissionStrength;

                emission *= _PulseSpeed > 0 ? _PulseMin + (1.0 - _PulseMin) * (0.5 + 0.5 * sin(_Time.y * _PulseSpeed * 6.28)) : 1.0;
                emission *= _FlickerSpeed > 0 ? 1.0 - _FlickerIntensity * (0.5 + 0.5 * sin(_Time.y * _FlickerSpeed * 6.28)) * (0.5 + 0.5 * sin(_Time.y * _FlickerSpeed * 12.56)) : 1.0;

                fixed2 scrollUV = IN.uv_MainTex;
                float rad = _ScrollDirection * 3.14159 / 180.0;
                scrollUV += fixed2(cos(rad), sin(rad)) * _Time.y * _ScrollSpeed;
                emission *= tex2D(_EmissionMap, scrollUV).rgb;
            #endif

            #if _USE_RIM_GLOW
                half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
                emission += _RimColor.rgb * pow(rim, _RimPower) * _RimStrength;
            #endif

            #if _USE_MATCAP
                half2 matcapUV = o.Normal.xy * 0.5 + 0.5;
                emission += tex2D(_MatcapTex, matcapUV).rgb * _MatcapStrength;
            #endif

            #if _USE_GRADIENT
                fixed3 gradTex = tex2D(_GradientTex, IN.uv_MainTex).rgb;
                emission += gradTex * gradTex.r * _GradientStrength;
            #endif

            o.Albedo = saturate(col);

            #if _USE_METALLIC_MAP
                half metallic = tex2D(_MetallicMap, IN.uv_MetallicMap).r * _MetallicStrength;
                o.Metallic = lerp(_Metallic, metallic, _MetallicStrength);
            #else
                o.Metallic = _Metallic;
            #endif

            #if _USE_SMOOTHNESS_MAP
                half smoothness = tex2D(_SmoothnessMap, IN.uv_SmoothnessMap).r * _SmoothnessStrength;
                o.Smoothness = lerp(_Smoothness, smoothness, _SmoothnessStrength);
            #else
                o.Smoothness = _Smoothness;
            #endif

            o.Emission = emission;
            o.Alpha = 1;
        }
        ENDCG
    }

    FallBack "Diffuse"
    CustomEditor "BlueShadeStudioLiteGUI"
}
