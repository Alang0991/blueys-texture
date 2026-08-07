using UnityEditor;
using UnityEngine;
using BlueShadeStudio.Core;

namespace BlueShadeStudio.Modules
{
    public class LiteEffectsModule : BaseModule
    {
        public override string ModuleName => "Effects";
        public override int Order => 3;

        private bool overlayOpen = true;
        private bool emissionOpen = true;
        private bool rimOpen = true;
        private bool cutoutOpen = false;
        private bool matcapOpen = false;
        private bool gradientOpen = false;
        private bool occlusionOpen = false;

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
                default: return string.Empty;
            }
        }
    }
}
