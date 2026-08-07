using UnityEditor;
using UnityEngine;
using BlueShadeStudio.Core;
using System.Collections.Generic;

namespace BlueShadeStudio.Modules
{
    public class PresetsModule : BaseModule
    {
        public override string ModuleName => "Presets";
        public override int Order => 4;

        private bool presetOpen = true;
        private bool toolsOpen = true;

        public override void Draw()
        {
            DrawPresets();
            DrawTools();
        }

        void DrawPresets()
        {
            DrawSectionHeader(ref presetOpen, "Material Presets", false);
            if (presetOpen)
            {
                DrawBodyStart();

                EditorGUILayout.BeginHorizontal();
                string[] presets = BlueShadeStudioPresetManager.GetAllPresetNames();

                int cols = 2;
                int rows = Mathf.CeilToInt(presets.Length / (float)cols);

                for (int r = 0; r < rows; r++)
                {
                    EditorGUILayout.BeginVertical();
                    for (int c = 0; c < cols; c++)
                    {
                        int idx = r * cols + c;
                        if (idx < presets.Length)
                        {
                            if (GUILayout.Button(presets[idx], GUILayout.Height(24)))
                            {
                                BlueShadeStudioPresetManager.ApplyPreset(material, presets[idx]);
                            }
                        }
                    }
                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawTools()
        {
            DrawSectionHeader(ref toolsOpen, "Material Tools", false);
            if (toolsOpen)
            {
                DrawBodyStart();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Save Custom Preset", Theme.GetButtonStyle(), GUILayout.Height(24)))
                {
                    string name = EditorUtility.DisplayDialogComplex("Save Preset", "Enter preset name:", "Save", "Cancel", "") == 0 ? "Custom" : "";
                    if (!string.IsNullOrEmpty(name))
                    {
                        BlueShadeStudioPresetManager.SaveCustomPreset(material, name);
                    }
                }
                if (GUILayout.Button("Reset Material", Theme.GetButtonStyle(), GUILayout.Height(24)))
                {
                    if (EditorUtility.DisplayDialog("Reset Material", "Reset all material properties to defaults?", "Yes", "Cancel"))
                    {
                        BlueShadeStudioPresetManager.ResetMaterial(material);
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(4);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Copy Settings", Theme.GetButtonStyle(), GUILayout.Height(22)))
                {
                    BlueShadeStudioPresetManager.CopyMaterialSettings(material);
                }
                if (GUILayout.Button("Paste Settings", Theme.GetButtonStyle(), GUILayout.Height(22)))
                {
                    BlueShadeStudioPresetManager.PasteMaterialSettings(material);
                }
                EditorGUILayout.EndHorizontal();
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
