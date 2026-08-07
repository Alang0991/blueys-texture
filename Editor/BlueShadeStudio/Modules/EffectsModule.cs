using UnityEditor;
using UnityEngine;
using BlueShadeStudio.Core;

namespace BlueShadeStudio.Modules
{
    public class EffectsModule : BaseModule
    {
        public override string ModuleName => "Effects";
        public override int Order => 2;

        private bool overlayOpen = true;
        private bool emissionOpen = true;
        private bool rimOpen = true;
        private bool cutoutOpen = false;
        private bool matcapOpen = false;
        private bool gradientOpen = false;
        private bool occlusionOpen = false;
        private bool edgeGlowOpen = true;
        private bool deepColorOpen = false;
        private bool innerGlowOpen = true;
        private bool reflectionOpen = false;
        private bool dissolveOpen = false;
        private bool outlineOpen = false;

        public override void Draw()
        {
            // Full shader effects
            if (material.HasProperty("_UseEdgeGlow")) DrawEdgeGlow();
            if (material.HasProperty("_UseDepth")) DrawDeepColor();
            if (material.HasProperty("_UseInnerGlow")) DrawInnerGlow();
            if (material.HasProperty("_UseReflection")) DrawReflection();
            if (material.HasProperty("_UseDissolve")) DrawDissolve();
            if (material.HasProperty("_UseOutline")) DrawOutline();

            // Shared effects
            if (material.HasProperty("_UseSolidOverlay")) DrawOverlay();
            if (material.HasProperty("_UseEmission")) DrawEmission();
            if (material.HasProperty("_UseRimGlow")) DrawRimGlow();
            if (material.HasProperty("_UseCutout")) DrawCutout();
            if (material.HasProperty("_UseMatcap")) DrawMatcap();
            if (material.HasProperty("_UseGradient")) DrawGradient();
            if (material.HasProperty("_UseOcclusion")) DrawOcclusion();
        }

        void DrawOverlay()
        {
            DrawSectionHeader(ref overlayOpen, "Colour Overlay", true, material.GetFloat("_UseSolidOverlay") > 0.5f, "_UseSolidOverlay");
            if (overlayOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (material.GetFloat("_UseSolidOverlay") > 0.5f)
                {
                    DrawProp("_SolidColor");
                    DrawProp("_SolidStrength");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Colour Overlay to access settings", Theme.GetDisabledLabelStyle());
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawEmission()
        {
            DrawSectionHeader(ref emissionOpen, "Emission", true, material.GetFloat("_UseEmission") > 0.5f, "_UseEmission");
            if (emissionOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (material.GetFloat("_UseEmission") > 0.5f)
                {
                    DrawProp("_EmissionMap");
                    if (material.HasProperty("_EmissionMask")) DrawProp("_EmissionMask");
                    DrawProp("_EmissionColor");
                    DrawProp("_EmissionStrength");
                    if (material.HasProperty("_EmissionUsesPNG")) DrawProp("_EmissionUsesPNG");

                    EditorGUILayout.Space(4);
                    EditorGUILayout.LabelField("Pulse Animation", EditorStyles.boldLabel);
                    DrawProp("_PulseSpeed");
                    DrawProp("_PulseMin");

                    EditorGUILayout.Space(4);
                    EditorGUILayout.LabelField("Flicker Effect", EditorStyles.boldLabel);
                    DrawProp("_FlickerSpeed");
                    DrawProp("_FlickerIntensity");

                    EditorGUILayout.Space(4);
                    EditorGUILayout.LabelField("Scrolling Emission", EditorStyles.boldLabel);
                    DrawProp("_ScrollSpeed");
                    DrawProp("_ScrollDirection");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Emission to access settings", Theme.GetDisabledLabelStyle());
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawRimGlow()
        {
            DrawSectionHeader(ref rimOpen, "Rim Glow", true, material.GetFloat("_UseRimGlow") > 0.5f, "_UseRimGlow");
            if (rimOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (material.GetFloat("_UseRimGlow") > 0.5f)
                {
                    DrawProp("_RimColor");
                    DrawProp("_RimPower");
                    DrawProp("_RimStrength");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Rim Glow to access settings", Theme.GetDisabledLabelStyle());
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawCutout()
        {
            DrawSectionHeader(ref cutoutOpen, "PNG Cutout", true, material.GetFloat("_UseCutout") > 0.5f, "_UseCutout");
            if (cutoutOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (material.GetFloat("_UseCutout") > 0.5f)
                {
                    DrawProp("_AlphaCutoff");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable PNG Cutout to access settings", Theme.GetDisabledLabelStyle());
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawMatcap()
        {
            DrawSectionHeader(ref matcapOpen, "Matcap", true, material.GetFloat("_UseMatcap") > 0.5f, "_UseMatcap");
            if (matcapOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (material.GetFloat("_UseMatcap") > 0.5f)
                {
                    DrawProp("_MatcapTex");
                    DrawProp("_MatcapStrength");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Matcap to access settings", Theme.GetDisabledLabelStyle());
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawGradient()
        {
            DrawSectionHeader(ref gradientOpen, "Gradient", true, material.GetFloat("_UseGradient") > 0.5f, "_UseGradient");
            if (gradientOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (material.GetFloat("_UseGradient") > 0.5f)
                {
                    DrawProp("_GradientTex");
                    DrawProp("_GradientStrength");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Gradient to access settings", Theme.GetDisabledLabelStyle());
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawOcclusion()
        {
            DrawSectionHeader(ref occlusionOpen, "Occlusion", false);
            if (occlusionOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (material.HasProperty("_OcclusionMap")) DrawProp("_OcclusionMap");
                if (material.HasProperty("_OcclusionStrength")) DrawProp("_OcclusionStrength");
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawEdgeGlow()
        {
            DrawSectionHeader(ref edgeGlowOpen, "Edge Glow", true, material.GetFloat("_UseEdgeGlow") > 0.5f, "_UseEdgeGlow");
            if (edgeGlowOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (material.GetFloat("_UseEdgeGlow") > 0.5f)
                {
                    DrawProp("_RimColor");
                    DrawProp("_RimPower");
                    DrawProp("_RimStrength");
                    DrawProp("_EdgeAlphaBoost");
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawDeepColor()
        {
            DrawSectionHeader(ref deepColorOpen, "Deep Color", true, material.GetFloat("_UseDepth") > 0.5f, "_UseDepth");
            if (deepColorOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (material.GetFloat("_UseDepth") > 0.5f)
                {
                    DrawProp("_DepthColor");
                    DrawProp("_DepthStrength");
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawInnerGlow()
        {
            DrawSectionHeader(ref innerGlowOpen, "Inner Glow", true, material.GetFloat("_UseInnerGlow") > 0.5f, "_UseInnerGlow");
            if (innerGlowOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (material.GetFloat("_UseInnerGlow") > 0.5f)
                {
                    DrawProp("_InnerColor");
                    DrawProp("_InnerStrength");
                    DrawProp("_InnerPower");
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawReflection()
        {
            DrawSectionHeader(ref reflectionOpen, "Fake Reflection", true, material.GetFloat("_UseReflection") > 0.5f, "_UseReflection");
            if (reflectionOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (material.GetFloat("_UseReflection") > 0.5f)
                {
                    DrawProp("_ReflectionColor");
                    DrawProp("_ReflectionStrength");
                    DrawProp("_ReflectionPower");
                    DrawProp("_ReflectionMap");
                    DrawProp("_ReflectionBlend");
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawDissolve()
        {
            DrawSectionHeader(ref dissolveOpen, "Dissolve", true, material.GetFloat("_UseDissolve") > 0.5f, "_UseDissolve");
            if (dissolveOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (material.GetFloat("_UseDissolve") > 0.5f)
                {
                    DrawProp("_DissolveAmount");
                    DrawProp("_DissolveEdgeWidth");
                    DrawProp("_DissolveEdgeColor");
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawOutline()
        {
            DrawSectionHeader(ref outlineOpen, "Outline", true, material.GetFloat("_UseOutline") > 0.5f, "_UseOutline");
            if (outlineOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (material.GetFloat("_UseOutline") > 0.5f)
                {
                    DrawProp("_OutlineColor");
                    DrawProp("_OutlineWidth");
                    DrawProp("_OutlineThreshold");
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        protected override string GetTooltip(string propName)
        {
            switch (propName)
            {
                case "_SolidColor": return "Overlay colour applied to the material.";
                case "_SolidStrength": return "Strength of the colour overlay.";
                case "_EmissionMap": return "Texture that defines emission areas.";
                case "_EmissionMask": return "Mask for the emission texture.";
                case "_EmissionColor": return "Colour of the emission.";
                case "_EmissionStrength": return "Brightness of the emission.";
                case "_EmissionUsesPNG": return "Use main PNG alpha as emission mask.";
                case "_PulseSpeed": return "Speed of the pulse animation.";
                case "_PulseMin": return "Minimum brightness during pulse.";
                case "_FlickerSpeed": return "Speed of the flicker effect.";
                case "_FlickerIntensity": return "Intensity of the flicker effect.";
                case "_ScrollSpeed": return "Speed of the scrolling emission.";
                case "_ScrollDirection": return "Direction of the scrolling emission in degrees.";
                case "_RimColor": return "Colour of the rim glow.";
                case "_RimPower": return "How tight the rim glow is.";
                case "_RimStrength": return "Brightness of the rim glow.";
                case "_AlphaCutoff": return "Alpha cutoff for PNG cutout.";
                case "_MatcapTex": return "Matcap texture for fake reflections.";
                case "_MatcapStrength": return "Strength of the matcap effect.";
                case "_GradientTex": return "Gradient texture for vertical colouring.";
                case "_GradientStrength": return "Strength of the gradient effect.";
                case "_OcclusionMap": return "Texture that defines ambient occlusion.";
                case "_OcclusionStrength": return "Strength of the ambient occlusion.";
                case "_EdgeAlphaBoost": return "Additional alpha added to edges for glow effect.";
                case "_DepthColor": return "Colour applied based on viewing angle.";
                case "_DepthStrength": return "Strength of the deep color effect.";
                case "_InnerColor": return "Colour of the inner glow.";
                case "_InnerStrength": return "Brightness of the inner glow.";
                case "_InnerPower": return "Softness of the inner glow.";
                case "_ReflectionColor": return "Colour of the fake reflection.";
                case "_ReflectionStrength": return "Brightness of the fake reflection.";
                case "_ReflectionPower": return "How tight the fake reflection is.";
                case "_ReflectionMap": return "Texture for the fake reflection.";
                case "_ReflectionBlend": return "How much the reflection texture blends with the reflection colour.";
                case "_DissolveAmount": return "How much of the material is dissolved.";
                case "_DissolveEdgeWidth": return "Width of the dissolve edge.";
                case "_DissolveEdgeColor": return "Colour of the dissolve edge.";
                case "_OutlineColor": return "Colour of the outline.";
                case "_OutlineWidth": return "Width of the outline effect.";
                case "_OutlineThreshold": return "Threshold for the outline effect.";
                default: return string.Empty;
            }
        }
    }
}
