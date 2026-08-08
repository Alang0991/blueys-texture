using UnityEditor;
using UnityEngine;
using BlueShadeStudio.Core;
using System.Collections.Generic;

namespace BlueShadeStudio.Modules
{
    public class OptimizationModule : BaseModule
    {
        private bool perfOpen = true;
        private bool validationOpen = true;
        private bool textureInfoOpen = false;

        protected override string[] ManagedProperties => new string[0];

        public override void Draw()
        {
            DrawPerformance();
            DrawValidation();
            DrawTextureInfo();
        }

        void DrawPerformance()
        {
            DrawSectionHeader(ref perfOpen, "Performance", false);
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
                EditorGUI.DrawRect(ratingRect, ratingColor * 0.25f);
                Theme.RatingStyle.normal.textColor = ratingColor;
                GUI.Label(ratingRect, "Performance: " + perfRating, Theme.RatingStyle);

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
                List<string> fixProps = new List<string>();

                if (material.GetTexture("_MainTex") == null)
                {
                    warnings.Add("Main texture is missing.");
                    fixProps.Add(null);
                }

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

                    if (!BlueShadeStudioUtils.IsValidTexture(tex, out string w))
                    {
                        warnings.Add($"\"{tex.name}\" ({prop}) has issues: {w}");
                        fixProps.Add(prop);
                    }
                }

                if (material.GetFloat("_UseEmission") > 0.5f && material.GetTexture("_EmissionMap") == null)
                {
                    warnings.Add("Emission is enabled but no emission texture is assigned.");
                    fixProps.Add(null);
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
                        if (fixProps[i] == null)
                        {
                             string buttonLabel = warnings[i].Contains("Emission") ? "Fix: Disable Emission" : "Fix: Assign Texture";
                             if (GUILayout.Button(buttonLabel, Theme.ButtonStyle, GUILayout.Height(20)))
                             {
                                 if (warnings[i].Contains("Emission"))
                                 {
                                     material.SetFloat("_UseEmission", 0f);
                                     material.DisableKeyword(BlueShadeStudioUtils.TogglePropertyToKeyword("_UseEmission"));
                                 }
                                 EditorUtility.SetDirty(material);
                             }
                        }
                        else
                        {
                            Texture tex = material.GetTexture(fixProps[i]);
                            if (GUILayout.Button($"Auto-fix \"{tex?.name}\"", Theme.ButtonStyle, GUILayout.Height(20)))
                            {
                                if (tex != null)
                                    BlueShadeStudioUtils.ApplyFixTexture(tex);
                            }
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
                    EditorGUILayout.LabelField("Details", BlueShadeStudioUtils.GetTextureInfo(tex));
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
            if (material.IsKeywordEnabled("_USE_OUTLINE")) count++;
            if (material.IsKeywordEnabled("_USE_DISSOLVE")) count++;
            if (material.IsKeywordEnabled("_USE_MATCAP")) count++;
            if (material.IsKeywordEnabled("_USE_GRADIENT")) count++;
            if (material.IsKeywordEnabled("_USE_OCCLUSION")) count++;
            if (material.IsKeywordEnabled("_USE_METALLIC_MAP")) count++;
            if (material.IsKeywordEnabled("_USE_SMOOTHNESS_MAP")) count++;
            if (material.IsKeywordEnabled("_USE_SOLID_OVERLAY")) count++;
            if (material.IsKeywordEnabled("_USE_RIM_GLOW")) count++;
            if (material.IsKeywordEnabled("_USE_CUTOUT")) count++;
            return count;
        }

        public override void LoadSectionStates(string prefix)
        {
            perfOpen = State.GetBool(prefix + "perfOpen", true);
            validationOpen = State.GetBool(prefix + "validationOpen", true);
            textureInfoOpen = State.GetBool(prefix + "textureInfoOpen", false);
        }

        public override void SaveSectionStates(string prefix)
        {
            State.SetBool(prefix + "perfOpen", perfOpen);
            State.SetBool(prefix + "validationOpen", validationOpen);
            State.SetBool(prefix + "textureInfoOpen", textureInfoOpen);
        }

        public override void ResetValues() { }

        protected override string GetTooltip(string propName)
        {
            return string.Empty;
        }
    }
}
