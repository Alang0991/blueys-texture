using UnityEditor;
using UnityEngine;
using BlueShadeStudio.Core;

namespace BlueShadeStudio.Modules
{
    public class RenderingModule : BaseModule
    {
        private bool renderOpen = true;
        private bool cullOpen = true;

        protected override string[] ManagedProperties => new[] { "_Cull" };

        public override void Draw()
        {
            DrawRenderingInfo();
            DrawCullSettings();
        }

        void DrawRenderingInfo()
        {
            DrawSectionHeader(ref renderOpen, "Rendering Settings", false);
            if (renderOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;

                EditorGUILayout.LabelField("Queue", material.renderQueue.ToString(), EditorStyles.wordWrappedLabel);
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

                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawCullSettings()
        {
            if (!material.HasProperty("_Cull")) return;
            DrawSectionHeader(ref cullOpen, "Culling", false);
            if (cullOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;

                EditorGUI.BeginChangeCheck();
                int cull = (int)material.GetFloat("_Cull");
                int newCull = EditorGUILayout.Popup("Cull Mode", cull, new[] { "Off (Double Sided)", "Front", "Back" });
                if (EditorGUI.EndChangeCheck())
                {
                    material.SetFloat("_Cull", newCull);
                    EditorUtility.SetDirty(material);
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        public override void LoadSectionStates(string prefix)
        {
            renderOpen = State.GetBool(prefix + "renderOpen", true);
            cullOpen = State.GetBool(prefix + "cullOpen", true);
        }

        public override void SaveSectionStates(string prefix)
        {
            State.SetBool(prefix + "renderOpen", renderOpen);
            State.SetBool(prefix + "cullOpen", cullOpen);
        }

        public override void ResetValues()
        {
            if (material.HasProperty("_Cull"))
                material.SetFloat("_Cull", 2f);
            EditorUtility.SetDirty(material);
        }

        protected override string GetTooltip(string propName)
        {
            switch (propName)
            {
                case "_Cull": return "Controls which faces are culled. Off = double-sided, Front = cull front faces, Back = cull back faces.";
                default: return string.Empty;
            }
        }
    }
}
