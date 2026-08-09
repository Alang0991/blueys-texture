using UnityEditor;
using UnityEngine;
using BlueShadeStudio.Core;

namespace BlueShadeStudio.Modules
{
    public class MainModule : BaseModule
    {
        private bool mainOpen = true;
        private bool textureOpen = true;

        protected override string[] ManagedProperties => new[]
        {
            "_MainTex", "_Color", "_MainTiling", "_MainOffset", "_Alpha",
            "_UseTextureBoost", "_TextureStrength", "_Contrast", "_Brightness",
            "_Saturation", "_HueShift", "_Gamma", "_Vibrance", "_Sharpness"
        };

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
                DrawProp("_Alpha");

                MaterialProperty texProp = FindProperty("_MainTex");
                if (texProp != null && texProp.textureValue != null)
                    DrawTextureWarning("_MainTex");

                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.PropertySpacing);
        }

        void DrawTextureLook()
        {
            bool boostOn = material.GetFloat("_UseTextureBoost") > 0.5f;
            DrawSectionHeader(ref textureOpen, "Texture Enhancement", true, boostOn, "_UseTextureBoost");
            if (textureOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (boostOn)
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
                    EditorGUILayout.LabelField("Enable Texture Enhancement to access look controls.", Theme.DisabledLabelStyle);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        public override void LoadSectionStates(string prefix)
        {
            mainOpen = State.GetBool(prefix + "mainOpen", true);
            textureOpen = State.GetBool(prefix + "textureOpen", true);
        }

        public override void SaveSectionStates(string prefix)
        {
            State.SetBool(prefix + "mainOpen", mainOpen);
            State.SetBool(prefix + "textureOpen", textureOpen);
        }

        public override void ResetValues()
        {
            SetDefault("_UseTextureBoost", 0f);
            SetDefault("_TextureStrength", 1f);
            SetDefault("_Contrast", 1f);
            SetDefault("_Brightness", 1f);
            SetDefault("_Saturation", 1f);
            SetDefault("_HueShift", 0f);
            SetDefault("_Gamma", 1f);
            SetDefault("_Vibrance", 0f);
            SetDefault("_Sharpness", 0f);
            SetDefault("_Alpha", 1f);
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
                case "_MainTex": return "The main PNG texture — the foundation of the material. All effects build on top of this.";
                case "_Color": return "Tint colour applied multiplicatively to the main texture.";
                case "_MainTiling": return "Tiling (scale) of the main texture UVs.";
                case "_MainOffset": return "Offset (translation) of the main texture UVs.";
                case "_Alpha": return "Overall transparency of the material (0 = fully transparent, 1 = fully opaque).";
                case "_TextureStrength": return "Blend between original and enhanced texture look.";
                case "_Contrast": return "Adjusts the difference between light and dark areas.";
                case "_Brightness": return "Brightens or darkens the texture.";
                case "_Saturation": return "Adjusts colour intensity.";
                case "_HueShift": return "Rotates the colour hue of the texture in degrees.";
                case "_Gamma": return "Adjusts the gamma/brightness curve.";
                case "_Vibrance": return "Intelligently boosts colour saturation in less saturated areas.";
                case "_Sharpness": return "Enhances edge detail.";
                default: return string.Empty;
            }
        }
    }
}
