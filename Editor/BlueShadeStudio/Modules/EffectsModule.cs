using UnityEditor;
using UnityEngine;
using BlueShadeStudio.Core;

namespace BlueShadeStudio.Modules
{
    public class EffectsModule : BaseModule
    {
        private bool edgeGlowOpen = true;
        private bool deepColorOpen = false;
        private bool innerGlowOpen = true;
        private bool reflectionOpen = false;
        private bool dissolveOpen = false;
        private bool outlineOpen = false;
        private bool emissionOpen = true;
        private bool matcapOpen = false;
        private bool gradientOpen = false;

        protected override string[] ManagedProperties => new[]
        {
            "_UseEdgeGlow", "_RimColor", "_RimPower", "_RimStrength", "_EdgeAlphaBoost",
            "_UseDepth", "_DepthColor", "_DepthStrength",
            "_UseInnerGlow", "_InnerColor", "_InnerStrength", "_InnerPower",
            "_UseReflection", "_ReflectionColor", "_ReflectionStrength", "_ReflectionPower",
            "_ReflectionMap", "_ReflectionBlend",
            "_UseDissolve", "_DissolveAmount", "_DissolveEdgeWidth", "_DissolveEdgeColor",
            "_UseOutline", "_OutlineColor", "_OutlineWidth", "_OutlineThreshold",
            "_UseEmission", "_EmissionMap", "_EmissionColor", "_EmissionStrength",
            "_PulseSpeed", "_PulseMin", "_FlickerSpeed", "_FlickerIntensity",
            "_ScrollSpeed", "_ScrollDirection",
            "_UseMatcap", "_MatcapTex", "_MatcapStrength",
            "_UseGradient", "_GradientTex", "_GradientStrength",
            "_FinalGlowPower"
        };

        public override void Draw()
        {
            if (material.HasProperty("_UseEdgeGlow")) DrawEdgeGlow();
            if (material.HasProperty("_UseDepth")) DrawDeepColor();
            if (material.HasProperty("_UseInnerGlow")) DrawInnerGlow();
            if (material.HasProperty("_UseReflection")) DrawReflection();
            if (material.HasProperty("_UseDissolve")) DrawDissolve();
            if (material.HasProperty("_UseOutline")) DrawOutline();

            if (material.HasProperty("_UseEmission")) DrawEmission();
            if (material.HasProperty("_UseMatcap")) DrawMatcap();
            if (material.HasProperty("_UseGradient")) DrawGradient();

            DrawFinalGlow();
        }

        void DrawEdgeGlow()
        {
            bool enabled = material.GetFloat("_UseEdgeGlow") > 0.5f;
            DrawSectionHeader(ref edgeGlowOpen, "Edge Glow", true, enabled, "_UseEdgeGlow");
            if (edgeGlowOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (enabled)
                {
                    DrawProp("_RimColor");
                    DrawProp("_RimPower");
                    DrawProp("_RimStrength");
                    DrawProp("_EdgeAlphaBoost");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Edge Glow to access settings.", Theme.DisabledLabelStyle);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawDeepColor()
        {
            bool enabled = material.GetFloat("_UseDepth") > 0.5f;
            DrawSectionHeader(ref deepColorOpen, "Deep Color", true, enabled, "_UseDepth");
            if (deepColorOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (enabled)
                {
                    DrawProp("_DepthColor");
                    DrawProp("_DepthStrength");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Deep Color to access settings.", Theme.DisabledLabelStyle);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawInnerGlow()
        {
            bool enabled = material.GetFloat("_UseInnerGlow") > 0.5f;
            DrawSectionHeader(ref innerGlowOpen, "Inner Glow", true, enabled, "_UseInnerGlow");
            if (innerGlowOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (enabled)
                {
                    DrawProp("_InnerColor");
                    DrawProp("_InnerStrength");
                    DrawProp("_InnerPower");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Inner Glow to access settings.", Theme.DisabledLabelStyle);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawReflection()
        {
            bool enabled = material.GetFloat("_UseReflection") > 0.5f;
            DrawSectionHeader(ref reflectionOpen, "Fake Reflection", true, enabled, "_UseReflection");
            if (reflectionOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (enabled)
                {
                    DrawProp("_ReflectionColor");
                    DrawProp("_ReflectionStrength");
                    DrawProp("_ReflectionPower");
                    DrawProp("_ReflectionMap");
                    DrawProp("_ReflectionBlend");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Fake Reflection to access settings.", Theme.DisabledLabelStyle);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawDissolve()
        {
            bool enabled = material.GetFloat("_UseDissolve") > 0.5f;
            DrawSectionHeader(ref dissolveOpen, "Dissolve", true, enabled, "_UseDissolve");
            if (dissolveOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (enabled)
                {
                    DrawProp("_DissolveAmount");
                    DrawProp("_DissolveEdgeWidth");
                    DrawProp("_DissolveEdgeColor");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Dissolve to access settings.", Theme.DisabledLabelStyle);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawOutline()
        {
            bool enabled = material.GetFloat("_UseOutline") > 0.5f;
            DrawSectionHeader(ref outlineOpen, "Outline", true, enabled, "_UseOutline");
            if (outlineOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (enabled)
                {
                    DrawProp("_OutlineColor");
                    DrawProp("_OutlineWidth");
                    DrawProp("_OutlineThreshold");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Outline to access settings.", Theme.DisabledLabelStyle);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawEmission()
        {
            bool enabled = material.GetFloat("_UseEmission") > 0.5f;
            DrawSectionHeader(ref emissionOpen, "Emission", true, enabled, "_UseEmission");
            if (emissionOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (enabled)
                {
                    DrawProp("_EmissionMap");
                    if (material.HasProperty("_EmissionMask")) DrawProp("_EmissionMask");
                    DrawProp("_EmissionColor");
                    DrawProp("_EmissionStrength");

                    DrawSubHeader("Pulse Animation");
                    DrawProp("_PulseSpeed");
                    DrawProp("_PulseMin");

                    DrawSubHeader("Flicker Effect");
                    DrawProp("_FlickerSpeed");
                    DrawProp("_FlickerIntensity");

                    DrawSubHeader("Scrolling Emission");
                    DrawProp("_ScrollSpeed");
                    DrawProp("_ScrollDirection");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Emission to access settings.", Theme.DisabledLabelStyle);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawMatcap()
        {
            bool enabled = material.GetFloat("_UseMatcap") > 0.5f;
            DrawSectionHeader(ref matcapOpen, "Matcap", true, enabled, "_UseMatcap");
            if (matcapOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (enabled)
                {
                    DrawProp("_MatcapTex");
                    DrawProp("_MatcapStrength");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Matcap to access settings.", Theme.DisabledLabelStyle);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawGradient()
        {
            bool enabled = material.GetFloat("_UseGradient") > 0.5f;
            DrawSectionHeader(ref gradientOpen, "Gradient", true, enabled, "_UseGradient");
            if (gradientOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (enabled)
                {
                    DrawProp("_GradientTex");
                    DrawProp("_GradientStrength");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Gradient to access settings.", Theme.DisabledLabelStyle);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawFinalGlow()
        {
            DrawSectionHeader(ref finalGlowOpen, "Final Glow", false);
            if (finalGlowOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                DrawProp("_FinalGlowPower");
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        private bool finalGlowOpen = true;

        public override void LoadSectionStates(string prefix)
        {
            edgeGlowOpen = State.GetBool(prefix + "effectsEdgeGlowOpen", true);
            deepColorOpen = State.GetBool(prefix + "effectsDeepColorOpen", false);
            innerGlowOpen = State.GetBool(prefix + "effectsInnerGlowOpen", true);
            reflectionOpen = State.GetBool(prefix + "effectsReflectionOpen", false);
            dissolveOpen = State.GetBool(prefix + "effectsDissolveOpen", false);
            outlineOpen = State.GetBool(prefix + "effectsOutlineOpen", false);
            emissionOpen = State.GetBool(prefix + "effectsEmissionOpen", true);
            matcapOpen = State.GetBool(prefix + "effectsMatcapOpen", false);
            gradientOpen = State.GetBool(prefix + "effectsGradientOpen", false);
            finalGlowOpen = State.GetBool(prefix + "effectsFinalGlowOpen", true);
        }

        public override void SaveSectionStates(string prefix)
        {
            State.SetBool(prefix + "effectsEdgeGlowOpen", edgeGlowOpen);
            State.SetBool(prefix + "effectsDeepColorOpen", deepColorOpen);
            State.SetBool(prefix + "effectsInnerGlowOpen", innerGlowOpen);
            State.SetBool(prefix + "effectsReflectionOpen", reflectionOpen);
            State.SetBool(prefix + "effectsDissolveOpen", dissolveOpen);
            State.SetBool(prefix + "effectsOutlineOpen", outlineOpen);
            State.SetBool(prefix + "effectsEmissionOpen", emissionOpen);
            State.SetBool(prefix + "effectsMatcapOpen", matcapOpen);
            State.SetBool(prefix + "effectsGradientOpen", gradientOpen);
            State.SetBool(prefix + "effectsFinalGlowOpen", finalGlowOpen);
        }

        public override void ResetValues()
        {
            SetDefault("_UseEdgeGlow", 0f);
            SetDefault("_RimStrength", 2f);
            SetDefault("_RimPower", 3f);
            SetDefault("_EdgeAlphaBoost", 0.1f);
            SetDefault("_UseDepth", 0f);
            SetDefault("_DepthStrength", 0.5f);
            SetDefault("_UseInnerGlow", 0f);
            SetDefault("_InnerStrength", 0.6f);
            SetDefault("_InnerPower", 2f);
            SetDefault("_UseReflection", 0f);
            SetDefault("_ReflectionStrength", 0.4f);
            SetDefault("_ReflectionPower", 4f);
            SetDefault("_ReflectionBlend", 0f);
            SetDefault("_UseDissolve", 0f);
            SetDefault("_DissolveAmount", 0f);
            SetDefault("_DissolveEdgeWidth", 0.05f);
            SetDefault("_UseOutline", 0f);
            SetDefault("_OutlineWidth", 0f);
            SetDefault("_OutlineThreshold", 0.1f);
            SetDefault("_UseEmission", 0f);
            SetDefault("_EmissionStrength", 1f);
            SetDefault("_PulseSpeed", 0f);
            SetDefault("_PulseMin", 0.5f);
            SetDefault("_FlickerSpeed", 0f);
            SetDefault("_FlickerIntensity", 0f);
            SetDefault("_ScrollSpeed", 0f);
            SetDefault("_UseMatcap", 0f);
            SetDefault("_MatcapStrength", 0f);
            SetDefault("_UseGradient", 0f);
            SetDefault("_GradientStrength", 0f);
            SetDefault("_FinalGlowPower", 1f);
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
                case "_RimColor": return "Colour of the edge glow effect.";
                case "_RimPower": return "How tight the edge glow is (lower = wider).";
                case "_RimStrength": return "Brightness of the edge glow.";
                case "_EdgeAlphaBoost": return "Additional alpha at edges for glow visibility.";
                case "_DepthColor": return "Colour applied to surfaces viewed at a grazing angle.";
                case "_DepthStrength": return "Strength of the deep color effect.";
                case "_InnerColor": return "Colour of the inner glow effect.";
                case "_InnerStrength": return "Brightness of the inner glow.";
                case "_InnerPower": return "Softness of the inner glow edge.";
                case "_ReflectionColor": return "Colour of the fake reflection.";
                case "_ReflectionStrength": return "Brightness of the fake reflection.";
                case "_ReflectionPower": return "How tight the reflection is (Fresnel power).";
                case "_ReflectionMap": return "Texture used for reflection sampling.";
                case "_ReflectionBlend": return "Blend between reflection colour and texture.";
                case "_DissolveAmount": return "How much of the material is dissolved (0 = intact, 1 = fully dissolved).";
                case "_DissolveEdgeWidth": return "Width of the dissolve edge glow.";
                case "_DissolveEdgeColor": return "Colour of the dissolve edge.";
                case "_OutlineColor": return "Colour of the outline.";
                case "_OutlineWidth": return "Width of the outline effect.";
                case "_OutlineThreshold": return "Threshold for the outline alpha cutoff.";
                case "_EmissionMap": return "Texture that defines emission areas.";
                case "_EmissionColor": return "Colour of the emission.";
                case "_EmissionStrength": return "Brightness of the emission.";
                case "_PulseSpeed": return "Speed of the pulse animation.";
                case "_PulseMin": return "Minimum brightness during pulse.";
                case "_FlickerSpeed": return "Speed of the flicker effect.";
                case "_FlickerIntensity": return "Intensity of the flicker effect.";
                case "_ScrollSpeed": return "Speed of the scrolling emission.";
                case "_ScrollDirection": return "Direction of the scrolling emission in degrees.";
                case "_EmissionMask": return "Mask for the emission texture.";
                case "_MatcapTex": return "Matcap texture for fake reflections.";
                case "_MatcapStrength": return "Strength of the matcap effect.";
                case "_GradientTex": return "Gradient texture for vertical colouring.";
                case "_GradientStrength": return "Strength of the gradient effect.";
                case "_FinalGlowPower": return "Global multiplier for all emission and glow effects.";
                default: return string.Empty;
            }
        }
    }
}
