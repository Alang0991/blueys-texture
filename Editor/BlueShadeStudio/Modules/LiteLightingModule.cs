using UnityEditor;
using UnityEngine;
using BlueShadeStudio.Core;

namespace BlueShadeStudio.Modules
{
    public class LiteLightingModule : BaseModule
    {
        public override string ModuleName => "Lighting";
        public override int Order => 2;

        private bool smoothnessOpen = true;

        public override void Draw()
        {
            DrawSmoothnessMetallic();
        }

        void DrawSmoothnessMetallic()
        {
            DrawSectionHeader(ref smoothnessOpen, "Smoothness & Metallic", true, material.GetFloat("_UseWetShine") > 0.5f, "_UseWetShine");
            if (smoothnessOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (material.GetFloat("_UseWetShine") > 0.5f)
                {
                    DrawProp("_Smoothness");
                    DrawProp("_Metallic");
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
                case "_Smoothness": return "Smoothness of the surface.";
                case "_Metallic": return "Metallic value of the surface.";
                case "_MetallicMap": return "Texture that defines metallic areas.";
                case "_MetallicStrength": return "Strength of the metallic map effect.";
                case "_SmoothnessMap": return "Texture that defines smoothness areas.";
                case "_SmoothnessStrength": return "Strength of the smoothness map effect.";
                default: return string.Empty;
            }
        }
    }
}
