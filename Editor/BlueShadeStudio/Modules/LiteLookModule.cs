using UnityEditor;
using UnityEngine;
using BlueShadeStudio.Core;

namespace BlueShadeStudio.Modules
{
    public class LiteLookModule : BaseModule
    {
        public override string ModuleName => "Look";
        public override int Order => 1;

        private bool lookOpen = true;

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

        protected override string GetTooltip(string propName)
        {
            switch (propName)
            {
                case "_Brightness": return "Brightens or darkens the texture.";
                case "_Contrast": return "Adjusts the difference between light and dark areas.";
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
