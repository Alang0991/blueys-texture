using UnityEditor;
using UnityEngine;
using BlueShadeStudio.Core;

namespace BlueShadeStudio.Modules
{
    public class LiteMainModule : BaseModule
    {
        private bool mainOpen = true;

        protected override string[] ManagedProperties => new[]
        {
            "_MainTex", "_Color", "_MainTiling", "_MainOffset"
        };

        public override void Draw()
        {
            DrawMainTexture();
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

                MaterialProperty texProp = FindProperty("_MainTex");
                if (texProp != null && texProp.textureValue != null)
                    DrawTextureWarning("_MainTex");

                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        public override void LoadSectionStates(string prefix)
        {
            mainOpen = State.GetBool(prefix + "liteMainOpen", true);
        }

        public override void SaveSectionStates(string prefix)
        {
            State.SetBool(prefix + "liteMainOpen", mainOpen);
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
                case "_MainTex": return "The main PNG texture — the foundation of the material. All effects build on top of this.";
                case "_Color": return "Tint colour applied multiplicatively to the main texture.";
                case "_MainTiling": return "Tiling (scale) of the main texture UVs.";
                case "_MainOffset": return "Offset (translation) of the main texture UVs.";
                default: return string.Empty;
            }
        }
    }
}
