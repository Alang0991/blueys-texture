using UnityEditor;
using UnityEngine;
using BlueShadeStudio.Core;

namespace BlueShadeStudio.Modules
{
    public class RenderingModule : BaseModule
    {
        public override string ModuleName => "Rendering";
        public override int Order => 3;

        private bool renderOpen = true;

        public override void Draw()
        {
            DrawSectionHeader(ref renderOpen, "Rendering Settings", false);
            if (renderOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;

                EditorGUILayout.LabelField("Render Queue", material.renderQueue.ToString());
                EditorGUILayout.LabelField("Render Type", material.GetTag("RenderType", false, "Unknown"));
                EditorGUILayout.LabelField("Shader", material.shader.name);
                EditorGUILayout.Space(8);

                EditorGUI.BeginChangeCheck();
                int queue = EditorGUILayout.IntField("Custom Render Queue", material.renderQueue);
                if (EditorGUI.EndChangeCheck())
                {
                    material.renderQueue = queue;
                    EditorUtility.SetDirty(material);
                }

                if (material.HasProperty("_Cull"))
                {
                    EditorGUI.BeginChangeCheck();
                    bool doubleSided = material.GetFloat("_Cull") == 0;
                    bool newDoubleSided = EditorGUILayout.Toggle("Double Sided", doubleSided);
                    if (EditorGUI.EndChangeCheck())
                    {
                        material.SetFloat("_Cull", newDoubleSided ? 0f : 2f);
                        EditorUtility.SetDirty(material);
                    }
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        protected override string GetTooltip(string propName)
        {
            return string.Empty;
        }
    }
}
