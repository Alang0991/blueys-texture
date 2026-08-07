using UnityEditor;
using UnityEngine;
using BlueShadeStudio.Core;

namespace BlueShadeStudio.Modules
{
    public class MainModule : BaseModule
    {
        public override string ModuleName => "Main";
        public override int Order => 0;

        private bool mainOpen = true;
        private bool textureOpen = true;

        public override void Draw()
        {
            DrawMainTexture();
            DrawTextureLook();
        }

        void DrawMainTexture()
        {
            DrawSectionHeader(ref mainOpen, "Main Texture", false);
            if (mainOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                DrawProp("_MainTex");
                DrawProp("_Color");
                DrawProp("_MainTiling");
                DrawProp("_MainOffset");
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.PropertySpacing);
        }

        void DrawTextureLook()
        {
            DrawSectionHeader(ref textureOpen, "Texture Look", true, material.GetFloat("_UseTextureBoost") > 0.5f, "_UseTextureBoost");
            if (textureOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (material.GetFloat("_UseTextureBoost") > 0.5f)
                {
                    DrawProp("_TextureStrength");
                    DrawProp("_Contrast");
                    DrawProp("_Brightness");
                    DrawProp("_Saturation");
                    DrawProp("_HueShift");
                    DrawProp("_Gamma");
                    DrawProp("_Vibrance");
                    DrawProp("_Sharpness");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Texture Enhancement to access look controls", Theme.GetDisabledLabelStyle());
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
                case "_MainTex": return "The main image texture for the material.";
                case "_Color": return "Tint colour applied to the main texture.";
                case "_MainTiling": return "Tiling of the main texture UVs.";
                case "_MainOffset": return "Offset of the main texture UVs.";
                case "_TextureStrength": return "Strength of the texture enhancement effect.";
                case "_Contrast": return "Adjusts the difference between light and dark areas.";
                case "_Brightness": return "Brightens or darkens the texture.";
                case "_Saturation": return "Adjusts colour intensity.";
                case "_HueShift": return "Rotates the colour hue of the texture.";
                case "_Gamma": return "Adjusts gamma/brightness curve.";
                case "_Vibrance": return "Intelligently boosts colour saturation.";
                case "_Sharpness": return "Enhances edge detail.";
                default: return string.Empty;
            }
        }
    }
}
