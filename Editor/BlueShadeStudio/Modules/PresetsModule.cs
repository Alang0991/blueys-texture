using UnityEditor;
using UnityEngine;
using BlueShadeStudio.Core;
using System.Collections.Generic;

namespace BlueShadeStudio.Modules
{
    public class PresetsModule : BaseModule
    {
        private bool presetOpen = true;
        private bool toolsOpen = true;

        private bool savingPreset = false;
        private string presetNameInput = "Custom";

        protected override string[] ManagedProperties => new string[0];

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

                string[] presets = BlueShadeStudioPresetManager.GetAllPresetNames();

                int cols = 2;
                int rows = Mathf.CeilToInt(presets.Length / (float)cols);

                for (int r = 0; r < rows; r++)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int c = 0; c < cols; c++)
                    {
                        int idx = r * cols + c;
                        if (idx < presets.Length)
                        {
                            if (GUILayout.Button(presets[idx], Theme.ButtonStyle, GUILayout.Height(26)))
                            {
                                BlueShadeStudioPresetManager.ApplyPreset(material, presets[idx]);
                            }
                        }
                        else
                        {
                            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(26));
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }

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
                if (GUILayout.Button("Reset Material", Theme.ButtonStyle, GUILayout.Height(24)))
                {
                    if (EditorUtility.DisplayDialog("Reset Material", "Reset all material properties to their defaults?", "Yes", "Cancel"))
                    {
                        BlueShadeStudioPresetManager.ResetMaterial(material);
                    }
                }

                if (GUILayout.Button(savingPreset ? "Cancel" : "Save Custom Preset", Theme.ButtonStyle, GUILayout.Height(24)))
                {
                    savingPreset = !savingPreset;
                    presetNameInput = "Custom";
                }
                EditorGUILayout.EndHorizontal();

                if (savingPreset)
                {
                    EditorGUILayout.Space(4);
                    presetNameInput = EditorGUILayout.TextField("Preset Name:", presetNameInput);

                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Save", Theme.ButtonStyle, GUILayout.Height(22)))
                    {
                        if (!string.IsNullOrEmpty(presetNameInput.Trim()))
                        {
                            BlueShadeStudioPresetManager.SaveCustomPreset(material, presetNameInput.Trim());
                            savingPreset = false;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "Please enter a preset name.", "OK");
                        }
                    }
                    if (GUILayout.Button("Cancel", Theme.ButtonStyle, GUILayout.Height(22)))
                    {
                        savingPreset = false;
                    }
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.Space(8);

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Copy Settings", Theme.ButtonStyle, GUILayout.Height(22)))
                {
                    BlueShadeStudioPresetManager.CopyMaterialSettings(material);
                }
                if (GUILayout.Button("Paste Settings", Theme.ButtonStyle, GUILayout.Height(22)))
                {
                    BlueShadeStudioPresetManager.PasteMaterialSettings(material);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        public override void LoadSectionStates(string prefix)
        {
            presetOpen = State.GetBool(prefix + "presetsPresetOpen", true);
            toolsOpen = State.GetBool(prefix + "presetsToolsOpen", true);
        }

        public override void SaveSectionStates(string prefix)
        {
            State.SetBool(prefix + "presetsPresetOpen", presetOpen);
            State.SetBool(prefix + "presetsToolsOpen", toolsOpen);
        }

        public override void ResetValues() { }

        protected override string GetTooltip(string propName)
        {
            return string.Empty;
        }
    }
}
