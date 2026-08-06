using System.Collections.Generic;
using UnityEngine;

public static class BlueysTextureBuiltinPresets
{
    public static readonly Dictionary<string, MaterialPreset> Presets = new Dictionary<string, MaterialPreset>
    {
        {
            "Wet Fur", new MaterialPreset
            {
                name = "Wet Fur",
                settings = new Dictionary<string, object>
                {
                    { "_UseWetShine", 1f },
                    { "_Smoothness", 0.9f },
                    { "_SpecularStrength", 0.8f },
                    { "_MetallicStrength", 0.1f },
                    { "_UseTextureBoost", 1f },
                    { "_Brightness", 1.1f },
                    { "_Contrast", 1.2f },
                    { "_Saturation", 0.9f },
                    { "_Vibrance", 0.3f }
                }
            }
        },
        {
            "Toon", new MaterialPreset
            {
                name = "Toon",
                settings = new Dictionary<string, object>
                {
                    { "_UseTextureBoost", 1f },
                    { "_Contrast", 1.8f },
                    { "_Brightness", 1.0f },
                    { "_Saturation", 1.5f },
                    { "_Sharpness", 1.0f },
                    { "_Smoothness", 0f },
                    { "_MetallicStrength", 0f },
                    { "_SpecularStrength", 0f },
                    { "_FinalGlowPower", 1.0f }
                }
            }
        },
        {
            "Realistic", new MaterialPreset
            {
                name = "Realistic",
                settings = new Dictionary<string, object>
                {
                    { "_UseWetShine", 1f },
                    { "_Smoothness", 0.5f },
                    { "_MetallicStrength", 0.3f },
                    { "_SmoothnessStrength", 0.5f },
                    { "_BumpStrength", 0.5f },
                    { "_Brightness", 1.0f },
                    { "_Contrast", 1.1f },
                    { "_Saturation", 1.0f },
                    { "_OcclusionStrength", 0.8f }
                }
            }
        },
        {
            "Wet", new MaterialPreset
            {
                name = "Wet",
                settings = new Dictionary<string, object>
                {
                    { "_UseWetShine", 1f },
                    { "_Smoothness", 0.95f },
                    { "_SpecularStrength", 0.8f },
                    { "_MetallicStrength", 0.1f },
                    { "_UseEdgeGlow", 1f },
                    { "_RimStrength", 1.0f },
                    { "_RimPower", 2f },
                    { "_Brightness", 1.1f },
                    { "_Contrast", 1.2f },
                    { "_Saturation", 0.9f },
                    { "_Vibrance", 0.3f },
                    { "_FinalGlowPower", 1.2f }
                }
            }
        },
        {
            "Plastic", new MaterialPreset
            {
                name = "Plastic",
                settings = new Dictionary<string, object>
                {
                    { "_UseWetShine", 1f },
                    { "_Smoothness", 0.7f },
                    { "_SpecularStrength", 0.5f },
                    { "_MetallicStrength", 0f },
                    { "_UseTextureBoost", 1f },
                    { "_Brightness", 1.0f },
                    { "_Contrast", 1.1f },
                    { "_Saturation", 1.2f },
                    { "_Vibrance", 0.1f }
                }
            }
        },
        {
            "Metal", new MaterialPreset
            {
                name = "Metal",
                settings = new Dictionary<string, object>
                {
                    { "_UseWetShine", 1f },
                    { "_Smoothness", 0.9f },
                    { "_SpecularStrength", 0.7f },
                    { "_MetallicStrength", 1.0f },
                    { "_UseReflection", 1f },
                    { "_ReflectionStrength", 0.6f },
                    { "_UseTextureBoost", 1f },
                    { "_Brightness", 1.0f },
                    { "_Contrast", 1.1f },
                    { "_Saturation", 0.7f }
                }
            }
        },
        {
            "Glass", new MaterialPreset
            {
                name = "Glass",
                settings = new Dictionary<string, object>
                {
                    { "_Alpha", 0.3f },
                    { "_Smoothness", 0.95f },
                    { "_SpecularStrength", 0.9f },
                    { "_MetallicStrength", 0f },
                    { "_Brightness", 1.2f },
                    { "_Contrast", 1.0f },
                    { "_UseEdgeGlow", 1f },
                    { "_RimStrength", 0.5f },
                    { "_RimColor", new Color(1f, 1f, 1f, 1f) }
                }
            }
        },
        {
            "Neon", new MaterialPreset
            {
                name = "Neon",
                settings = new Dictionary<string, object>
                {
                    { "_UseEmission", 1f },
                    { "_EmissionStrength", 2.5f },
                    { "_EmissionColor", new Color(0.1f, 0.9f, 1f, 1f) },
                    { "_UseEdgeGlow", 1f },
                    { "_RimStrength", 2.0f },
                    { "_RimColor", new Color(0.1f, 0.9f, 1f, 1f) },
                    { "_Brightness", 1.3f },
                    { "_Contrast", 1.4f },
                    { "_Saturation", 1.3f },
                    { "_FinalGlowPower", 1.5f },
                    { "_Smoothness", 0.8f },
                    { "_SpecularStrength", 0.6f }
                }
            }
        },
        {
            "Emissive", new MaterialPreset
            {
                name = "Emissive",
                settings = new Dictionary<string, object>
                {
                    { "_UseEmission", 1f },
                    { "_EmissionStrength", 3.0f },
                    { "_EmissionColor", new Color(0.8f, 0.2f, 1f, 1f) },
                    { "_Brightness", 0.5f },
                    { "_Contrast", 1.5f },
                    { "_Saturation", 1.2f },
                    { "_FinalGlowPower", 2.0f },
                    { "_Smoothness", 0.3f },
                    { "_MetallicStrength", 0f }
                }
            }
        }
    };
}

public class MaterialPreset
{
    public string name;
    public Dictionary<string, object> settings;
}
