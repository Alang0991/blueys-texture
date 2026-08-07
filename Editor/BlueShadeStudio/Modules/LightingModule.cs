using UnityEditor;
using UnityEngine;
using BlueShadeStudio.Core;

namespace BlueShadeStudio.Modules
{
    public class LightingModule : BaseModule
    {
        public override string ModuleName => "Lighting";
        public override int Order => 1;

        private bool normalOpen = false;
        private bool shineOpen = true;

        public override void Draw()
        {
            DrawNormalMap();
            DrawWetShine();
        }

        void DrawNormalMap()
        {
            if (!material.HasProperty("_UseNormal")) return;
            DrawSectionHeader(ref normalOpen, "Normal Map", true, material.GetFloat("_UseNormal") > 0.5f, "_UseNormal");
            if (normalOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (material.GetFloat("_UseNormal") > 0.5f)
                {
                    DrawProp("_BumpMap");
                    DrawProp("_BumpStrength");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Normal Map to access settings", Theme.GetDisabledLabelStyle());
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawWetShine()
        {
            if (!material.HasProperty("_UseWetShine")) return;
            DrawSectionHeader(ref shineOpen, "Wet Shine", true, material.GetFloat("_UseWetShine") > 0.5f, "_UseWetShine");
            if (shineOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (material.GetFloat("_UseWetShine") > 0.5f)
                {
                    DrawProp("_Smoothness");
                    DrawProp("_SpecularStrength");
                    if (material.HasProperty("_MetallicMap")) DrawProp("_MetallicMap");
                    if (material.HasProperty("_MetallicStrength")) DrawProp("_MetallicStrength");
                    if (material.HasProperty("_SmoothnessMap")) DrawProp("_SmoothnessMap");
                    if (material.HasProperty("_SmoothnessStrength")) DrawProp("_SmoothnessStrength");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Wet Shine to access PBR settings", Theme.GetDisabledLabelStyle());
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        protected override string GetTooltip(string propName)
        {
            switch (propName)
            {
                case "_BumpMap": return "Normal map for surface detail.";
                case "_BumpStrength": return "Strength of the normal map effect.";
                case "_Smoothness": return "Smoothness of the surface.";
                case "_SpecularStrength": return "Strength of the specular highlight.";
                case "_MetallicMap": return "Texture that defines metallic areas.";
                case "_MetallicStrength": return "Strength of the metallic map effect.";
                case "_SmoothnessMap": return "Texture that defines smoothness areas.";
                case "_SmoothnessStrength": return "Strength of the smoothness map effect.";
                default: return string.Empty;
            }
        }
    }
}
