using UnityEditor;
using UnityEngine;
using BlueShadeStudio.Core;

namespace BlueShadeStudio.Modules
{
    public class LightingModule : BaseModule
    {
        private bool normalOpen = false;
        private bool shineOpen = true;
        private bool occlusionOpen = false;

        protected override string[] ManagedProperties => new[]
        {
            "_UseNormal", "_BumpMap", "_BumpStrength",
            "_UseWetShine", "_Smoothness", "_SpecularStrength",
            "_MetallicMap", "_MetallicStrength",
            "_SmoothnessMap", "_SmoothnessStrength",
            "_UseOcclusion", "_OcclusionMap", "_OcclusionStrength"
        };

        public override void Draw()
        {
            DrawNormalMap();
            DrawWetShine();
            DrawOcclusion();
        }

        void DrawNormalMap()
        {
            if (!material.HasProperty("_UseNormal")) return;
            bool enabled = material.GetFloat("_UseNormal") > 0.5f;
            DrawSectionHeader(ref normalOpen, "Normal Map", true, enabled, "_UseNormal");
            if (normalOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (enabled)
                {
                    DrawProp("_BumpMap");
                    DrawProp("_BumpStrength");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Normal Map to access settings.", Theme.DisabledLabelStyle);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawWetShine()
        {
            if (!material.HasProperty("_UseWetShine")) return;
            bool enabled = material.GetFloat("_UseWetShine") > 0.5f;
            DrawSectionHeader(ref shineOpen, "Wet Shine (PBR)", true, enabled, "_UseWetShine");
            if (shineOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (enabled)
                {
                    DrawProp("_Smoothness");
                    DrawProp("_SpecularStrength");

                    DrawProp("_MetallicMap");
                    DrawProp("_MetallicStrength");
                    DrawTextureWarning("_MetallicMap");

                    DrawProp("_SmoothnessMap");
                    DrawProp("_SmoothnessStrength");
                    DrawTextureWarning("_SmoothnessMap");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Wet Shine to access PBR settings.", Theme.DisabledLabelStyle);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawOcclusion()
        {
            if (!material.HasProperty("_UseOcclusion")) return;
            bool enabled = material.GetFloat("_UseOcclusion") > 0.5f;
            DrawSectionHeader(ref occlusionOpen, "Ambient Occlusion", true, enabled, "_UseOcclusion");
            if (occlusionOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (enabled)
                {
                    DrawProp("_OcclusionMap");
                    MaterialProperty texProp = FindProperty("_OcclusionMap");
                    if (texProp != null && texProp.textureValue != null)
                        DrawTextureWarning("_OcclusionMap");
                    DrawProp("_OcclusionStrength");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Ambient Occlusion to access settings.", Theme.DisabledLabelStyle);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        public override void LoadSectionStates(string prefix)
        {
            normalOpen = State.GetBool(prefix + "lightingNormalOpen", false);
            shineOpen = State.GetBool(prefix + "lightingShineOpen", true);
            occlusionOpen = State.GetBool(prefix + "lightingOcclusionOpen", false);
        }

        public override void SaveSectionStates(string prefix)
        {
            State.SetBool(prefix + "lightingNormalOpen", normalOpen);
            State.SetBool(prefix + "lightingShineOpen", shineOpen);
            State.SetBool(prefix + "lightingOcclusionOpen", occlusionOpen);
        }

        public override void ResetValues()
        {
            SetDefault("_UseNormal", 0f);
            SetDefault("_BumpStrength", 0.4f);
            SetDefault("_UseWetShine", 0f);
            SetDefault("_Smoothness", 0.5f);
            SetDefault("_SpecularStrength", 0.1f);
            SetDefault("_MetallicStrength", 0f);
            SetDefault("_SmoothnessStrength", 0f);
            SetDefault("_UseOcclusion", 0f);
            SetDefault("_OcclusionStrength", 1f);
        }

        void SetDefault(string name, float value)
        {
            if (material.HasProperty(name))
            {
                material.SetFloat(name, value);
            }
        }

        protected override string GetTooltip(string propName)
        {
            switch (propName)
            {
                case "_BumpMap": return "Normal map for surface detail.";
                case "_BumpStrength": return "Strength of the normal map effect.";
                case "_Smoothness": return "Smoothness of the surface (higher = more reflective).";
                case "_SpecularStrength": return "Strength of the specular highlight.";
                case "_MetallicMap": return "Texture that defines metallic areas (red channel).";
                case "_MetallicStrength": return "Strength of the metallic map effect.";
                case "_SmoothnessMap": return "Texture that defines smoothness areas (red channel).";
                case "_SmoothnessStrength": return "Strength of the smoothness map effect.";
                case "_OcclusionMap": return "Texture that defines ambient occlusion.";
                case "_OcclusionStrength": return "Strength of the ambient occlusion.";
                default: return string.Empty;
            }
        }
    }
}
