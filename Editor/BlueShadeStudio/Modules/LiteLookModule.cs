using UnityEditor;
using UnityEngine;
using BlueShadeStudio.Core;

namespace BlueShadeStudio.Modules
{
    public class LiteLookModule : BaseModule
    {
        private bool lookOpen = true;

        protected override string[] ManagedProperties => new[]
        {
            "_Brightness", "_Contrast", "_Saturation", "_HueShift",
            "_Gamma", "_Vibrance", "_Sharpness"
        };

        public override void Draw()
        {
            DrawSectionHeader(ref lookOpen, "Texture Look", false);
            if (lookOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                DrawProp("_Brightness");
                DrawProp("_Contrast");
                DrawProp("_Saturation");
                DrawProp("_HueShift");
                DrawProp("_Gamma");
                DrawProp("_Vibrance");
                DrawProp("_Sharpness");
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        public override void LoadSectionStates(string prefix)
        {
            lookOpen = State.GetBool(prefix + "lookOpen", true);
        }

        public override void SaveSectionStates(string prefix)
        {
            State.SetBool(prefix + "lookOpen", lookOpen);
        }

        public override void ResetValues()
        {
            SetDefault("_Brightness", 1f);
            SetDefault("_Contrast", 1f);
            SetDefault("_Saturation", 1f);
            SetDefault("_HueShift", 0f);
            SetDefault("_Gamma", 1f);
            SetDefault("_Vibrance", 0f);
            SetDefault("_Sharpness", 0f);
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
                case "_Brightness": return "Brightens or darkens the texture.";
                case "_Contrast": return "Adjusts the difference between light and dark areas.";
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
