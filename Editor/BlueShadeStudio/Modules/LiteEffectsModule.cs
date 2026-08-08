using UnityEditor;
using UnityEngine;
using BlueShadeStudio.Core;

namespace BlueShadeStudio.Modules
{
    public class LiteEffectsModule : BaseModule
    {
        private bool overlayOpen = true;
        private bool emissionOpen = true;
        private bool rimOpen = true;
        private bool cutoutOpen = false;
        private bool matcapOpen = false;
        private bool gradientOpen = false;
        private bool occlusionOpen = false;

        protected override string[] ManagedProperties => new[]
        {
            "_UseSolidOverlay", "_SolidColor", "_SolidStrength",
            "_UseEmission", "_EmissionMap", "_EmissionMask", "_EmissionColor",
            "_EmissionStrength", "_EmissionUsesPNG",
            "_PulseSpeed", "_PulseMin", "_FlickerSpeed", "_FlickerIntensity",
            "_ScrollSpeed", "_ScrollDirection",
            "_UseRimGlow", "_RimColor", "_RimPower", "_RimStrength",
            "_UseCutout", "_AlphaCutoff",
            "_UseMatcap", "_MatcapTex", "_MatcapStrength",
            "_UseGradient", "_GradientTex", "_GradientStrength",
            "_UseOcclusion", "_OcclusionMap", "_OcclusionStrength"
        };

        public override void Draw()
        {
            DrawOverlay();
            DrawEmission();
            DrawRimGlow();
            DrawCutout();
            DrawMatcap();
            DrawGradient();
            DrawOcclusion();
        }

        void DrawOverlay()
        {
            bool enabled = material.GetFloat("_UseSolidOverlay") > 0.5f;
            DrawSectionHeader(ref overlayOpen, "Colour Overlay", true, enabled, "_UseSolidOverlay");
            if (overlayOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (enabled)
                {
                    DrawProp("_SolidColor");
                    DrawProp("_SolidStrength");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Colour Overlay to access settings.", Theme.DisabledLabelStyle);
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
                    DrawProp("_EmissionMask");
                    DrawProp("_EmissionColor");
                    DrawProp("_EmissionStrength");
                    DrawProp("_EmissionUsesPNG");

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

        void DrawRimGlow()
        {
            bool enabled = material.GetFloat("_UseRimGlow") > 0.5f;
            DrawSectionHeader(ref rimOpen, "Rim Glow", true, enabled, "_UseRimGlow");
            if (rimOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (enabled)
                {
                    DrawProp("_RimColor");
                    DrawProp("_RimPower");
                    DrawProp("_RimStrength");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Rim Glow to access settings.", Theme.DisabledLabelStyle);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        void DrawCutout()
        {
            bool enabled = material.GetFloat("_UseCutout") > 0.5f;
            DrawSectionHeader(ref cutoutOpen, "PNG Cutout", true, enabled, "_UseCutout");
            if (cutoutOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (enabled)
                {
                    DrawProp("_AlphaCutoff");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable PNG Cutout to access settings.", Theme.DisabledLabelStyle);
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

        void DrawOcclusion()
        {
            bool enabled = material.GetFloat("_UseOcclusion") > 0.5f;
            DrawSectionHeader(ref occlusionOpen, "Ambient Occlusion", true, enabled, "_UseOcclusion");
            if (occlusionOpen)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                if (enabled)
                {
                    DrawProp("_OcclusionMap");
                    if (material.HasProperty("_OcclusionMap"))
                    {
                        MaterialProperty texProp = FindProperty("_OcclusionMap");
                        if (texProp != null && texProp.textureValue != null)
                            DrawTextureWarning("_OcclusionMap");
                    }
                    DrawProp("_OcclusionStrength");
                }
                else
                {
                    EditorGUILayout.LabelField("Enable Ambient Occlusion to access settings.", Theme.DisabledLabelStyle);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(Theme.SectionSpacing);
        }

        public override void LoadSectionStates(string prefix)
        {
            overlayOpen = State.GetBool(prefix + "liteOverlayOpen", true);
            emissionOpen = State.GetBool(prefix + "liteEmissionOpen", true);
            rimOpen = State.GetBool(prefix + "liteRimGlowOpen", true);
            cutoutOpen = State.GetBool(prefix + "liteCutoutOpen", false);
            matcapOpen = State.GetBool(prefix + "liteMatcapOpen", false);
            gradientOpen = State.GetBool(prefix + "liteGradientOpen", false);
            occlusionOpen = State.GetBool(prefix + "liteOcclusionOpen", false);
        }

        public override void SaveSectionStates(string prefix)
        {
            State.SetBool(prefix + "liteOverlayOpen", overlayOpen);
            State.SetBool(prefix + "liteEmissionOpen", emissionOpen);
            State.SetBool(prefix + "liteRimGlowOpen", rimOpen);
            State.SetBool(prefix + "liteCutoutOpen", cutoutOpen);
            State.SetBool(prefix + "liteMatcapOpen", matcapOpen);
            State.SetBool(prefix + "liteGradientOpen", gradientOpen);
            State.SetBool(prefix + "liteOcclusionOpen", occlusionOpen);
        }

        public override void ResetValues()
        {
            SetDefault("_UseSolidOverlay", 0f);
            SetDefault("_SolidStrength", 0f);
            SetDefault("_UseEmission", 0f);
            SetDefault("_EmissionStrength", 1f);
            SetDefault("_EmissionUsesPNG", 1f);
            SetDefault("_PulseSpeed", 0f);
            SetDefault("_PulseMin", 0.5f);
            SetDefault("_FlickerSpeed", 0f);
            SetDefault("_FlickerIntensity", 0f);
            SetDefault("_ScrollSpeed", 0f);
            SetDefault("_UseRimGlow", 1f);
            SetDefault("_RimStrength", 1f);
            SetDefault("_RimPower", 3f);
            SetDefault("_UseCutout", 0f);
            SetDefault("_AlphaCutoff", 0.05f);
            SetDefault("_UseMatcap", 0f);
            SetDefault("_MatcapStrength", 0f);
            SetDefault("_UseGradient", 0f);
            SetDefault("_GradientStrength", 0f);
            SetDefault("_UseOcclusion", 0f);
            SetDefault("_OcclusionStrength", 1f);
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
                case "_SolidColor": return "Overlay colour applied to the material.";
                case "_SolidStrength": return "Strength of the colour overlay.";
                case "_EmissionMap": return "Texture that defines emission areas.";
                case "_EmissionMask": return "Mask for the emission texture.";
                case "_EmissionColor": return "Colour of the emission.";
                case "_EmissionStrength": return "Brightness of the emission.";
                case "_EmissionUsesPNG": return "Blend emission with main PNG texture colours.";
                case "_PulseSpeed": return "Speed of the pulse animation.";
                case "_PulseMin": return "Minimum brightness during pulse.";
                case "_FlickerSpeed": return "Speed of the flicker effect.";
                case "_FlickerIntensity": return "Intensity of the flicker effect.";
                case "_ScrollSpeed": return "Speed of the scrolling emission.";
                case "_ScrollDirection": return "Direction of the scrolling emission in degrees.";
                case "_RimColor": return "Colour of the rim glow.";
                case "_RimPower": return "How tight the rim glow is.";
                case "_RimStrength": return "Brightness of the rim glow.";
                case "_AlphaCutoff": return "Alpha cutoff for PNG cutout transparency.";
                case "_MatcapTex": return "Matcap texture for fake reflections.";
                case "_MatcapStrength": return "Strength of the matcap effect.";
                case "_GradientTex": return "Gradient texture for vertical colouring.";
                case "_GradientStrength": return "Strength of the gradient effect.";
                case "_OcclusionMap": return "Texture that defines ambient occlusion.";
                case "_OcclusionStrength": return "Strength of the ambient occlusion.";
                default: return string.Empty;
            }
        }
    }
}
