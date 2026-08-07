using UnityEditor;
using UnityEngine;
using BlueShadeStudio.Core;
using System.Collections.Generic;

namespace BlueShadeStudio.Modules
{
    public class OptimizationModule : BaseModule
    {
        public override string ModuleName => "Optimization";
        public override int Order => 5;

        private bool perfOpen = true;
        private bool validationOpen = true;
        private bool textureInfoOpen = false;

        public override void Draw()
        {
            DrawPerformance();
            DrawValidation();
            DrawTextureInfo();
        }

        void DrawPerformance()
        {
            DrawSectionHeader(ref perfOpen, "Performance Information", false);
            if (perfOpen)
            {
                DrawBodyStart();

                int keywordCount = CountActiveKeywords();

                EditorGUILayout.LabelField("Active Keywords", keywordCount.ToString());
                EditorGUILayout.LabelField("Shader", material.shader.name);
                EditorGUILayout.LabelField("Render Queue", material.renderQueue.ToString());
                EditorGUILayout.LabelField("Render Type", material.GetTag("RenderType", false, "Unknown"));

                string perfRating = BlueShadeStudioUtils.GetPerformanceRating(keywordCount);
                Color ratingColor = BlueShadeStudioUtils.GetPerformanceColor(keywordCount);

                EditorGUILayout.Space(4);
                Rect ratingRect = EditorGUILayout.GetControlRect(false, 24);
                EditorGUI.DrawRect(ratingRect, ratingColor * 0.3f);
                GUIStyle ratingStyle = new GUIStyle(EditorStyles.boldLabel);
                ratingStyle.alignment = TextAnchor.MiddleCenter;
                ratingStyle.normal.textColor = ratingColor;
                GUI.Label(ratingRect, "Performance: " + perfRating, ratingStyle);

                if (keywordCount > 5)
                {
                    EditorGUILayout.HelpBox("This material has many active features. Consider disabling unused features for better VRChat performance.", MessageType.Warning);
                }

                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawValidation()
        {
            DrawSectionHeader(ref validationOpen, "Material Validator", false);
            if (validationOpen)
            {
                DrawBodyStart();

                List<string> warnings = new List<string>();
                List<string> fixes = new List<string>();

                if (material.GetTexture("_MainTex") == null)
                {
                    warnings.Add("Main texture is missing.");
                    fixes.Add("Assign a Main PNG Texture.");
                }

                if (material.GetFloat("_UseEmission") > 0.5f && material.GetTexture("_EmissionMap") == null)
                {
                    warnings.Add("Emission is enabled but no emission texture is assigned.");
                    fixes.Add("Assign an emission texture or disable emission.");
                }

                if (warnings.Count == 0)
                {
                    EditorGUILayout.HelpBox("Material looks good. No warnings found.", MessageType.Info);
                }
                else
                {
                    for (int i = 0; i < warnings.Count; i++)
                    {
                        EditorGUILayout.HelpBox(warnings[i], MessageType.Warning);
                        if (GUILayout.Button("Fix: " + fixes[i], Theme.GetButtonStyle()))
                        {
                            ApplyFix(material, i);
                        }
                    }
                }

                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawTextureInfo()
        {
            DrawSectionHeader(ref textureInfoOpen, "Texture Information", false);
            if (textureInfoOpen)
            {
                DrawBodyStart();

                string[] texProps = new string[]
                {
                    "_MainTex", "_DetailTex", "_BumpMap", "_EmissionMap",
                    "_ReflectionMap", "_OcclusionMap", "_MetallicMap", "_SmoothnessMap",
                    "_MatcapTex", "_GradientTex"
                };

                foreach (string prop in texProps)
                {
                    if (!material.HasProperty(prop)) continue;
                    Texture tex = material.GetTexture(prop);
                    if (tex == null) continue;

                    EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(prop), tex.name);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.LabelField("Resolution", tex.width + " x " + tex.height);
                    EditorGUILayout.LabelField("Type", tex.GetType().Name);
                    EditorGUILayout.LabelField("VRAM", BlueShadeStudioUtils.FormatVRAM(tex));
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space(4);
                }

                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        int CountActiveKeywords()
        {
            int count = 0;
            if (material.IsKeywordEnabled("_USE_TEXTURE_BOOST")) count++;
            if (material.IsKeywordEnabled("_USE_DETAIL")) count++;
            if (material.IsKeywordEnabled("_USE_NORMAL")) count++;
            if (material.IsKeywordEnabled("_USE_WET_SHINE")) count++;
            if (material.IsKeywordEnabled("_USE_EDGE_GLOW")) count++;
            if (material.IsKeywordEnabled("_USE_DEPTH")) count++;
            if (material.IsKeywordEnabled("_USE_INNER_GLOW")) count++;
            if (material.IsKeywordEnabled("_USE_EMISSION")) count++;
            if (material.IsKeywordEnabled("_USE_REFLECTION")) count++;
            if (material.IsKeywordEnabled("_USE_MATCAP")) count++;
            if (material.IsKeywordEnabled("_USE_GRADIENT")) count++;
            if (material.IsKeywordEnabled("_USE_DISSOLVE")) count++;
            if (material.IsKeywordEnabled("_USE_OUTLINE")) count++;
            if (material.IsKeywordEnabled("_USE_SOLID_OVERLAY")) count++;
            if (material.IsKeywordEnabled("_USE_RIM_GLOW")) count++;
            if (material.IsKeywordEnabled("_USE_CUTOUT")) count++;
            return count;
        }

        void ApplyFix(Material mat, int index)
        {
            switch (index)
            {
                case 0:
                    EditorUtility.DisplayDialog("Fix Required", "Please assign a main texture manually.", "OK");
                    break;
                case 1:
                    mat.SetFloat("_UseEmission", 0);
                    EditorUtility.SetDirty(mat);
                    break;
            }
        }

        protected override string GetTooltip(string propName)
        {
            return string.Empty;
        }
    }
}
