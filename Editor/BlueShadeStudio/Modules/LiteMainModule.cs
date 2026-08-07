using UnityEditor;
using UnityEngine;
using BlueShadeStudio.Core;

namespace BlueShadeStudio.Modules
{
    public class LiteMainModule : BaseModule
    {
        public override string ModuleName => "Main";
        public override int Order => 0;

        private bool mainOpen = true;

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
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        protected override string GetTooltip(string propName)
        {
            switch (propName)
            {
                case "_MainTex": return "The main PNG texture for the material.";
                case "_Color": return "Tint colour applied to the main texture.";
                case "_MainTiling": return "Tiling of the main texture UVs.";
                case "_MainOffset": return "Offset of the main texture UVs.";
                default: return string.Empty;
            }
        }
    }
}
