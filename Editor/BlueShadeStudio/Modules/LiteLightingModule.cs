using UnityEditor;
using UnityEngine;
using BlueShadeStudio.Core;

namespace BlueShadeStudio.Modules
{
    public class LiteLightingModule : BaseModule
    {
        private bool pbrOpen = true;
        private bool metallicMapOpen = true;
        private bool smoothnessMapOpen = true;

        protected override string[] ManagedProperties => new[]
        {
            "_Smoothness", "_Metallic",
            "_UseMetallicMap", "_MetallicMap", "_MetallicStrength",
            "_UseSmoothnessMap", "_SmoothnessMap", "_SmoothnessStrength"
        };

        public override void Draw()
        {
            DrawPBR();
            DrawMetallicMap();
            DrawSmoothnessMap();
        }

        void DrawPBR()
        {
            DrawSectionHeader(ref pbrOpen, "PBR Materials", false);
            if (pbrOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                DrawProp("_Smoothness");
                DrawProp("_Metallic");
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawMetallicMap()
        {
            bool enabled = material.GetFloat("_UseMetallicMap") > 0.5f;
            DrawSectionHeader(ref metallicMapOpen, "Metallic Map", true, enabled, "_UseMetallicMap");
            if (metallicMapOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (enabled)
                {
                    DrawProp("_MetallicMap");
                    DrawProp("_MetallicStrength");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Metallic Map to access settings.", Theme.DisabledLabelStyle);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawSmoothnessMap()
        {
            bool enabled = material.GetFloat("_UseSmoothnessMap") > 0.5f;
            DrawSectionHeader(ref smoothnessMapOpen, "Smoothness Map", true, enabled, "_UseSmoothnessMap");
            if (smoothnessMapOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (enabled)
                {
                    DrawProp("_SmoothnessMap");
                    DrawProp("_SmoothnessStrength");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Smoothness Map to access settings.", Theme.DisabledLabelStyle);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        public override void LoadSectionStates(string prefix)
        {
            pbrOpen = State.GetBool(prefix + "litePbrOpen", true);
            metallicMapOpen = State.GetBool(prefix + "liteMetallicMapOpen", true);
            smoothnessMapOpen = State.GetBool(prefix + "liteSmoothnessMapOpen", true);
        }

        public override void SaveSectionStates(string prefix)
        {
            State.SetBool(prefix + "litePbrOpen", pbrOpen);
            State.SetBool(prefix + "liteMetallicMapOpen", metallicMapOpen);
            State.SetBool(prefix + "liteSmoothnessMapOpen", smoothnessMapOpen);
        }

        public override void ResetValues()
        {
            SetDefault("_Smoothness", 0.5f);
            SetDefault("_Metallic", 0f);
            SetDefault("_UseMetallicMap", 0f);
            SetDefault("_MetallicStrength", 0f);
            SetDefault("_UseSmoothnessMap", 0f);
            SetDefault("_SmoothnessStrength", 0f);
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
                case "_Smoothness": return "Smoothness of the surface (higher = more reflective).";
                case "_Metallic": return "Metallic value of the surface (0 = dielectric, 1 = metal).";
                case "_UseMetallicMap": return "Use a metallic map texture for per-pixel metallic control.";
                case "_MetallicMap": return "Texture that defines metallic areas (red channel).";
                case "_MetallicStrength": return "Strength of the metallic map effect.";
                case "_UseSmoothnessMap": return "Use a smoothness map texture for per-pixel smoothness control.";
                case "_SmoothnessMap": return "Texture that defines smoothness areas (red channel).";
                case "_SmoothnessStrength": return "Strength of the smoothness map effect.";
                default: return string.Empty;
            }
        }
    }
}
