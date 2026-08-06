using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class BlueysTextureSimpleGUI : ShaderGUI
{
    private int tabIndex = 0;
    private readonly string[] tabs = { "Main", "Lighting", "Effects", "Rendering", "Optimization", "Presets" };

    private bool mainOpen = true;
    private bool lookOpen = true;
    private bool overlayOpen = true;
    private bool emissionOpen = true;
    private bool rimOpen = true;
    private bool cutoutOpen = false;
    private bool matcapOpen = false;
    private bool gradientOpen = false;
    private bool occlusionOpen = false;
    private bool renderOpen = true;
    private bool perfOpen = true;

    private Vector2 scrollPos;
    private bool showValidation = false;
    private bool showTextureInfo = false;

    Material cachedMat;

    readonly Color accent = new Color(0.25f, 0.75f, 1f);
    readonly Color headerOff = new Color(0.16f, 0.16f, 0.16f);
    readonly Color headerOn = new Color(0.12f, 0.22f, 0.26f);
    readonly Color body = new Color(0.13f, 0.13f, 0.13f);
    readonly Color bannerBg = new Color(0.04f, 0.07f, 0.09f);

    static readonly Dictionary<string, object> defaultValues = new Dictionary<string, object>()
    {
        { "_MainTex", null },
        { "_MainTiling", new Vector4(1,1,0,0) },
        { "_MainOffset", new Vector4(0,0,0,0) },
        { "_Color", new Color(1f, 1f, 1f, 1f) },
        { "_Brightness", 1f },
        { "_Contrast", 1f },
        { "_Saturation", 1f },
        { "_HueShift", 0f },
        { "_Gamma", 1f },
        { "_Vibrance", 0f },
        { "_Sharpness", 0f },
        { "_Smoothness", 0.5f },
        { "_Metallic", 0f },
        { "_MetallicMap", null },
        { "_MetallicStrength", 0f },
        { "_SmoothnessMap", null },
        { "_SmoothnessStrength", 0f },
        { "_UseSolidOverlay", 0f },
        { "_SolidColor", new Color(1f, 1f, 1f, 1f) },
        { "_SolidStrength", 0f },
        { "_UseEmission", 0f },
        { "_EmissionMap", null },
        { "_EmissionMask", null },
        { "_EmissionColor", new Color(0.2f, 0.7f, 1f, 1f) },
        { "_EmissionStrength", 1f },
        { "_EmissionUsesPNG", 1f },
        { "_PulseSpeed", 0f },
        { "_PulseMin", 0.5f },
        { "_FlickerSpeed", 0f },
        { "_FlickerIntensity", 0f },
        { "_ScrollSpeed", 0f },
        { "_ScrollDirection", 0f },
        { "_UseRimGlow", 1f },
        { "_RimColor", new Color(0.35f, 0.8f, 1f, 1f) },
        { "_RimPower", 3f },
        { "_RimStrength", 1f },
        { "_UseCutout", 0f },
        { "_AlphaCutoff", 0.05f },
        { "_UseMatcap", 0f },
        { "_MatcapTex", null },
        { "_MatcapStrength", 0f },
        { "_UseGradient", 0f },
        { "_GradientTex", null },
        { "_GradientStrength", 0f },
        { "_OcclusionMap", null },
        { "_OcclusionStrength", 1f }
    };

    public override void OnGUI(MaterialEditor editor, MaterialProperty[] props)
    {
        Material mat = editor.target as Material;

        if (cachedMat != mat)
        {
            LoadSectionStates(mat);
            cachedMat = mat;
        }

        DrawBanner();
        DrawTabBar();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        if (showValidation) DrawValidation(editor, mat);
        else if (showTextureInfo) DrawTextureInfo(mat);
        else DrawTabContent(editor, props, mat);

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space(4);
        DrawUtilityButtons(mat);

        SaveSectionStates(mat);
    }

    void DrawBanner()
    {
        Rect r = EditorGUILayout.GetControlRect(false, 64);
        EditorGUI.DrawRect(r, bannerBg);

        GUIStyle title = new GUIStyle(EditorStyles.boldLabel);
        title.fontSize = 20;
        title.alignment = TextAnchor.MiddleCenter;
        title.normal.textColor = accent;
        GUI.Label(r, "Blueys Texture Simple", title);

        Rect line = new Rect(r.x, r.yMax - 3, r.width, 3);
        EditorGUI.DrawRect(line, accent);

        GUIStyle ver = new GUIStyle(EditorStyles.miniLabel);
        ver.alignment = TextAnchor.MiddleCenter;
        ver.normal.textColor = new Color(0.5f, 0.5f, 0.5f);
        GUI.Label(new Rect(r.x, r.yMax - 18, r.width, 18), "v1.1.1 | Professional VRChat Texture Shader", ver);

        EditorGUILayout.Space(6);
    }

    void DrawTabBar()
    {
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < tabs.Length; i++)
        {
            bool active = tabIndex == i;
            Color bg = active ? headerOn : headerOff;
            Color textCol = active ? accent : new Color(0.82f, 0.82f, 0.82f);

            Rect tabRect = EditorGUILayout.GetControlRect(GUILayout.Height(28), GUILayout.Width(80));
            EditorGUI.DrawRect(tabRect, bg);

            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = textCol;
            style.fontSize = 11;
            GUI.Label(tabRect, tabs[i], style);

            if (active)
            {
                Rect underline = new Rect(tabRect.x, tabRect.yMax - 2, tabRect.width, 2);
                EditorGUI.DrawRect(underline, accent);
            }

            if (Event.current.type == EventType.MouseDown && tabRect.Contains(Event.current.mousePosition))
            {
                tabIndex = i;
                Event.current.Use();
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(4);
    }

    void DrawTabContent(MaterialEditor editor, MaterialProperty[] props, Material mat)
    {
        switch (tabIndex)
        {
            case 0: DrawMainTab(editor, props, mat); break;
            case 1: DrawLightingTab(editor, props, mat); break;
            case 2: DrawEffectsTab(editor, props, mat); break;
            case 3: DrawRenderingTab(editor, props, mat); break;
            case 4: DrawOptimizationTab(mat); break;
            case 5: DrawPresetsTab(mat); break;
        }
    }

    void DrawMainTab(MaterialEditor editor, MaterialProperty[] props, Material mat)
    {
        DrawPlainSection(editor, props, ref mainOpen, "Main Texture",
            "_MainTex", "_Color", "_MainTiling", "_MainOffset");

        DrawPlainSection(editor, props, ref lookOpen, "Texture Look",
            "_Brightness", "_Contrast", "_Saturation", "_HueShift", "_Gamma", "_Vibrance", "_Sharpness");
    }

    void DrawLightingTab(MaterialEditor editor, MaterialProperty[] props, Material mat)
    {
        DrawToggleSection(editor, props, ref overlayOpen, "Smoothness & Metallic", "_UseWetShine",
            "_Smoothness", "_SpecularStrength", "_MetallicMap", "_MetallicStrength",
            "_SmoothnessMap", "_SmoothnessStrength");
    }

    void DrawEffectsTab(MaterialEditor editor, MaterialProperty[] props, Material mat)
    {
        DrawToggleSection(editor, props, ref overlayOpen, "Colour Overlay", "_UseSolidOverlay",
            "_SolidColor", "_SolidStrength");

        DrawEmissionSection(editor, props, ref emissionOpen, mat);

        DrawToggleSection(editor, props, ref rimOpen, "Rim Glow", "_UseRimGlow",
            "_RimColor", "_RimPower", "_RimStrength");

        DrawToggleSection(editor, props, ref cutoutOpen, "PNG Cutout", "_UseCutout",
            "_AlphaCutoff");

        DrawToggleSection(editor, props, ref matcapOpen, "Matcap", "_UseMatcap",
            "_MatcapTex", "_MatcapStrength");

        DrawToggleSection(editor, props, ref gradientOpen, "Gradient", "_UseGradient",
            "_GradientTex", "_GradientStrength");

        DrawToggleSection(editor, props, ref occlusionOpen, "Occlusion", "_UseOcclusion",
            "_OcclusionMap", "_OcclusionStrength");
    }

    void DrawRenderingTab(MaterialEditor editor, MaterialProperty[] props, Material mat)
    {
        EditorGUILayout.BeginVertical();
        bool open = true;
        open = DrawHeaderStrip(open, "Rendering Settings", false, false, null);

        if (open)
        {
            DrawBodyStart();
            EditorGUI.indentLevel++;

            EditorGUILayout.LabelField("Render Queue", mat.renderQueue.ToString());
            EditorGUILayout.LabelField("Render Type", mat.GetTag("RenderType", false, "Opaque"));
            EditorGUILayout.LabelField("Shader", mat.shader.name);
            EditorGUILayout.Space(8);

            EditorGUI.BeginChangeCheck();
            int queue = EditorGUILayout.IntField("Custom Render Queue", mat.renderQueue);
            if (EditorGUI.EndChangeCheck())
            {
                mat.renderQueue = queue;
                EditorUtility.SetDirty(mat);
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(5);
    }

    void DrawOptimizationTab(Material mat)
    {
        DrawPerformance(mat);
    }

    void DrawPresetsTab(Material mat)
    {
        DrawPresetSection(mat);
        DrawCopyPaste(mat);
    }

    void DrawValidation(MaterialEditor editor, Material mat)
    {
        EditorGUILayout.BeginVertical();
        bool valOpen = true;
        valOpen = DrawHeaderStrip(valOpen, "Material Validator", false, false, null);

        if (valOpen)
        {
            DrawBodyStart();

            List<string> warnings = new List<string>();
            List<string> fixes = new List<string>();

            if (mat.GetTexture("_MainTex") == null)
            {
                warnings.Add("Main texture is missing.");
                fixes.Add("Assign a Main PNG Texture.");
            }

            if (mat.GetFloat("_UseEmission") > 0.5f && mat.GetTexture("_EmissionMap") == null)
            {
                warnings.Add("Emission is enabled but no emission texture is assigned.");
                fixes.Add("Assign an emission texture or disable emission.");
            }

            if (mat.renderQueue != 2000)
            {
                warnings.Add("Render queue is not set to Geometry (2000).");
                fixes.Add("Set render queue to 2000.");
            }

            Texture main = mat.GetTexture("_MainTex");
            bool mainHasMipmaps = false;
            if (main != null)
            {
                string path = AssetDatabase.GetAssetPath(main);
                if (!string.IsNullOrEmpty(path))
                {
                    TextureImporter imp = AssetImporter.GetAtPath(path) as TextureImporter;
                    if (imp != null) mainHasMipmaps = imp.mipmapEnabled;
                }
            }

            if (main != null && !mainHasMipmaps)
            {
                warnings.Add("Mipmaps are disabled on main texture.");
                fixes.Add("Enable mipmaps in texture import settings.");
            }

            if (main != null && main.wrapMode != TextureWrapMode.Repeat)
            {
                warnings.Add("Main texture wrap mode is not Repeat.");
                fixes.Add("Set wrap mode to Repeat in texture import settings.");
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
                    if (GUILayout.Button("Fix: " + fixes[i]))
                    {
                        ApplyFix(mat, i, main);
                    }
                }
            }

            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(5);
    }

    void ApplyFix(Material mat, int index, Texture main)
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
            case 2:
                mat.renderQueue = 2000;
                EditorUtility.SetDirty(mat);
                break;
            case 3:
                if (main != null)
                {
                    string path = AssetDatabase.GetAssetPath(main);
                    if (!string.IsNullOrEmpty(path))
                    {
                        TextureImporter imp = AssetImporter.GetAtPath(path) as TextureImporter;
                        if (imp != null)
                        {
                            imp.mipmapEnabled = true;
                            AssetDatabase.ImportAsset(path);
                        }
                    }
                }
                break;
            case 4:
                if (main != null)
                {
                    string path = AssetDatabase.GetAssetPath(main);
                    if (!string.IsNullOrEmpty(path))
                    {
                        TextureImporter imp = AssetImporter.GetAtPath(path) as TextureImporter;
                        if (imp != null)
                        {
                            imp.wrapMode = TextureWrapMode.Repeat;
                            AssetDatabase.ImportAsset(path);
                        }
                    }
                }
                break;
        }
    }

    void DrawTextureInfo(Material mat)
    {
        EditorGUILayout.BeginVertical();
        bool infoOpen = true;
        infoOpen = DrawHeaderStrip(infoOpen, "Texture Information", false, false, null);

        if (infoOpen)
        {
            DrawBodyStart();

            string[] texProps = new string[]
            {
                "_MainTex", "_EmissionMap", "_EmissionMask", "_MetallicMap", "_SmoothnessMap",
                "_MatcapTex", "_GradientTex", "_OcclusionMap"
            };

            foreach (string prop in texProps)
            {
                if (!mat.HasProperty(prop)) continue;
                Texture tex = mat.GetTexture(prop);
                if (tex == null) continue;

                EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(prop), tex.name);
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("Resolution", tex.width + " x " + tex.height);
                EditorGUILayout.LabelField("Type", tex.GetType().Name);

                string path = AssetDatabase.GetAssetPath(tex);
                if (!string.IsNullOrEmpty(path))
                {
                    TextureImporter imp = AssetImporter.GetAtPath(path) as TextureImporter;
                    if (imp != null)
                    {
                        EditorGUILayout.LabelField("Format", imp.textureCompression.ToString());
                        EditorGUILayout.LabelField("Mipmaps", imp.mipmapEnabled ? "Enabled" : "Disabled");
                        EditorGUILayout.LabelField("sRGB", imp.sRGBTexture ? "Yes" : "No");
                    }
                }

                EditorGUILayout.LabelField("VRAM", BlueysTextureUtils.FormatVRAM(tex));
                EditorGUI.indentLevel--;
                EditorGUILayout.Space(4);
            }

            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(5);
    }

    void DrawPerformance(Material mat)
    {
        EditorGUILayout.BeginVertical();
        bool perfOpen = true;
        perfOpen = DrawHeaderStrip(perfOpen, "Performance Information", false, false, null);

        if (perfOpen)
        {
            DrawBodyStart();

            int keywordCount = 0;
            if (mat.IsKeywordEnabled("_USE_SOLID_OVERLAY")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_EMISSION")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_RIM_GLOW")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_CUTOUT")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_MATCAP")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_GRADIENT")) keywordCount++;

            EditorGUILayout.LabelField("Active Keywords", keywordCount.ToString());
            EditorGUILayout.LabelField("Shader", mat.shader.name);
            EditorGUILayout.LabelField("Render Queue", mat.renderQueue.ToString());
            EditorGUILayout.LabelField("Render Type", mat.GetTag("RenderType", false, "Unknown"));

            string perfRating = "Low (Good for VRChat)";
            if (keywordCount > 3) perfRating = "Medium";
            if (keywordCount > 5) perfRating = "High (Consider simplifying)";

            EditorGUILayout.LabelField("Performance Rating", perfRating);

            if (keywordCount > 5)
            {
                EditorGUILayout.HelpBox("This material has many active features. Consider disabling unused features for better VRChat performance.", MessageType.Warning);
            }

            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(5);
    }

    void DrawPresetSection(Material mat)
    {
        EditorGUILayout.BeginVertical();
        bool presetOpen = true;
        presetOpen = DrawHeaderStrip(presetOpen, "Presets", false, false, null);

        if (presetOpen)
        {
            DrawBodyStart();

            EditorGUILayout.BeginHorizontal();
            string[] presets = new string[]
            {
                "Wet Fur", "Plastic", "Rubber", "Latex", "Metal", "Skin", "Toon", "Glow", "Matte", "Fabric"
            };

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
                            BlueysTexturePresets.ApplyPreset(mat, presets[idx]);
                        }
                    }
                }
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(6);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Save Custom Preset", GUILayout.Height(22)))
            {
                string name = EditorUtility.DisplayDialogComplex("Save Preset", "Enter preset name:", "Save", "Cancel", "") == 0 ? "Custom" : "";
                if (!string.IsNullOrEmpty(name))
                {
                    BlueysTexturePresets.SaveCustomPreset(mat, name);
                }
            }
            if (GUILayout.Button("Reset Material", GUILayout.Height(22)))
            {
                if (EditorUtility.DisplayDialog("Reset Material", "Reset all material properties to defaults?", "Yes", "Cancel"))
                {
                    ResetMaterial(mat);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(5);
    }

    void DrawUtilityButtons(Material mat)
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Validate", GUILayout.Height(22)))
        {
            showValidation = true;
            showTextureInfo = false;
        }
        if (GUILayout.Button("Textures", GUILayout.Height(22)))
        {
            showTextureInfo = true;
            showValidation = false;
        }
        if (GUILayout.Button("Back to Inspector", GUILayout.Height(22)))
        {
            showValidation = false;
            showTextureInfo = false;
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(4);
    }

    void DrawEmissionSection(MaterialEditor editor, MaterialProperty[] props, ref bool open, Material mat)
    {
        EditorGUILayout.BeginVertical();
        open = DrawHeaderStrip(open, "Emission", true, mat.GetFloat("_UseEmission") > 0.5f, FindProperty("_UseEmission", props, false));

        if (open)
        {
            DrawBodyStart();
            EditorGUI.indentLevel++;

            DrawProp(editor, props, "_EmissionMap");
            DrawProp(editor, props, "_EmissionMask");
            DrawProp(editor, props, "_EmissionColor");
            DrawProp(editor, props, "_EmissionStrength");
            DrawProp(editor, props, "_EmissionUsesPNG");

            if (mat.GetFloat("_UseEmission") > 0.5f)
            {
                EditorGUILayout.Space(4);
                EditorGUILayout.LabelField("Pulse Animation", EditorStyles.boldLabel);
                DrawProp(editor, props, "_PulseSpeed");
                DrawProp(editor, props, "_PulseMin");

                EditorGUILayout.Space(4);
                EditorGUILayout.LabelField("Flicker Effect", EditorStyles.boldLabel);
                DrawProp(editor, props, "_FlickerSpeed");
                DrawProp(editor, props, "_FlickerIntensity");

                EditorGUILayout.Space(4);
                EditorGUILayout.LabelField("Scrolling Emission", EditorStyles.boldLabel);
                DrawProp(editor, props, "_ScrollSpeed");
                DrawProp(editor, props, "_ScrollDirection");
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(5);
    }

    bool DrawHeaderStrip(bool open, string title, bool hasToggle, bool enabled, MaterialProperty toggle)
    {
        Rect r = EditorGUILayout.GetControlRect(false, 28);
        EditorGUI.DrawRect(r, hasToggle && enabled ? headerOn : headerOff);

        Rect arrowRect = new Rect(r.x + 8, r.y + 5, 18, 18);
        open = EditorGUI.Foldout(arrowRect, open, GUIContent.none, true);

        Rect titleRect = new Rect(r.x + 28, r.y + 5, r.width - 130, 18);

        GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel);
        titleStyle.normal.textColor = hasToggle && enabled ? accent : new Color(0.82f, 0.82f, 0.82f);

        GUI.Label(titleRect, title, titleStyle);

        if (hasToggle && toggle != null)
        {
            Rect toggleRect = new Rect(r.xMax - 78, r.y + 5, 18, 18);
            bool newEnabled = EditorGUI.Toggle(toggleRect, enabled);

            if (newEnabled != enabled)
            {
                toggle.floatValue = newEnabled ? 1f : 0f;
            }

            Rect statusRect = new Rect(r.xMax - 56, r.y + 5, 48, 18);

            GUIStyle statusStyle = new GUIStyle(EditorStyles.miniBoldLabel);
            statusStyle.alignment = TextAnchor.MiddleRight;
            statusStyle.normal.textColor = newEnabled ? accent : Color.gray;

            GUI.Label(statusRect, newEnabled ? "ON" : "OFF", statusStyle);
        }

        return open;
    }

    void DrawBodyStart()
    {
        GUIStyle box = new GUIStyle("box");
        box.padding = new RectOffset(12, 12, 10, 10);
        box.margin = new RectOffset(0, 0, 0, 0);

        Color old = GUI.backgroundColor;
        GUI.backgroundColor = body;
        EditorGUILayout.BeginVertical(box);
        GUI.backgroundColor = old;
    }

    void DrawPlainSection(MaterialEditor editor, MaterialProperty[] props, ref bool open, string title, params string[] propertyNames)
    {
        EditorGUILayout.BeginVertical();
        open = DrawHeaderStrip(open, title, false, false, null);

        if (open)
        {
            DrawBodyStart();
            EditorGUI.indentLevel++;
            foreach (string propertyName in propertyNames)
            {
                DrawProp(editor, props, propertyName);
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(5);
    }

    void DrawToggleSection(MaterialEditor editor, MaterialProperty[] props, ref bool open, string title, string toggleName, params string[] propertyNames)
    {
        MaterialProperty toggle = FindProperty(toggleName, props, false);
        bool enabled = toggle != null && toggle.floatValue > 0.5f;

        EditorGUILayout.BeginVertical();
        open = DrawHeaderStrip(open, title, true, enabled, toggle);

        if (open)
        {
            DrawBodyStart();
            EditorGUI.indentLevel++;

            if (enabled)
            {
                foreach (string propertyName in propertyNames)
                {
                    DrawProp(editor, props, propertyName);
                }
            }
            else
            {
                GUIStyle offStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
                offStyle.normal.textColor = new Color(0.6f, 0.6f, 0.6f);
                EditorGUILayout.LabelField("Disabled - tick the box to enable settings", offStyle);
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(5);
    }

    void DrawProp(MaterialEditor editor, MaterialProperty[] props, string name)
    {
        MaterialProperty prop = FindProperty(name, props, false);
        if (prop != null)
        {
            GUIContent content = new GUIContent(prop.displayName, GetTooltip(name));
            editor.ShaderProperty(prop, content);
        }
    }

    string GetTooltip(string propName)
    {
        switch (propName)
        {
            case "_MainTex": return "The main PNG texture for the material.";
            case "_Color": return "Tint colour applied to the main texture.";
            case "_MainTiling": return "Tiling of the main texture UVs.";
            case "_MainOffset": return "Offset of the main texture UVs.";
            case "_Brightness": return "Brightens or darkens the texture.";
            case "_Contrast": return "Adjusts the difference between light and dark areas.";
            case "_Saturation": return "Adjusts colour intensity.";
            case "_HueShift": return "Rotates the colour hue of the texture.";
            case "_Gamma": return "Adjusts gamma/brightness curve.";
            case "_Vibrance": return "Intelligently boosts colour saturation.";
            case "_Sharpness": return "Enhances edge detail.";
            case "_Smoothness": return "Smoothness of the surface.";
            case "_Metallic": return "Metallic value of the surface.";
            case "_MetallicMap": return "Texture that defines metallic areas.";
            case "_MetallicStrength": return "Strength of the metallic map effect.";
            case "_SmoothnessMap": return "Texture that defines smoothness areas.";
            case "_SmoothnessStrength": return "Strength of the smoothness map effect.";
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
            default: return "";
        }
    }

    void LoadSectionStates(Material mat)
    {
        string key = "BlueysTextureSimple_" + mat.GetInstanceID() + "_";
        mainOpen = EditorPrefs.GetBool(key + "mainOpen", true);
        lookOpen = EditorPrefs.GetBool(key + "lookOpen", true);
        overlayOpen = EditorPrefs.GetBool(key + "overlayOpen", true);
        emissionOpen = EditorPrefs.GetBool(key + "emissionOpen", true);
        rimOpen = EditorPrefs.GetBool(key + "rimOpen", true);
        cutoutOpen = EditorPrefs.GetBool(key + "cutoutOpen", false);
        matcapOpen = EditorPrefs.GetBool(key + "matcapOpen", false);
        gradientOpen = EditorPrefs.GetBool(key + "gradientOpen", false);
        occlusionOpen = EditorPrefs.GetBool(key + "occlusionOpen", false);
        renderOpen = EditorPrefs.GetBool(key + "renderOpen", true);
        perfOpen = EditorPrefs.GetBool(key + "perfOpen", true);
    }

    void SaveSectionStates(Material mat)
    {
        string key = "BlueysTextureSimple_" + mat.GetInstanceID() + "_";
        EditorPrefs.SetBool(key + "mainOpen", mainOpen);
        EditorPrefs.SetBool(key + "lookOpen", lookOpen);
        EditorPrefs.SetBool(key + "overlayOpen", overlayOpen);
        EditorPrefs.SetBool(key + "emissionOpen", emissionOpen);
        EditorPrefs.SetBool(key + "rimOpen", rimOpen);
        EditorPrefs.SetBool(key + "cutoutOpen", cutoutOpen);
        EditorPrefs.SetBool(key + "matcapOpen", matcapOpen);
        EditorPrefs.SetBool(key + "gradientOpen", gradientOpen);
        EditorPrefs.SetBool(key + "occlusionOpen", occlusionOpen);
        EditorPrefs.SetBool(key + "renderOpen", renderOpen);
        EditorPrefs.SetBool(key + "perfOpen", perfOpen);
    }

    void CopyMaterialSettings(Material mat)
    {
        Dictionary<string, object> settings = new Dictionary<string, object>();
        Shader shader = mat.shader;
        int propCount = shader.GetPropertyCount();
        for (int i = 0; i < propCount; i++)
        {
            string name = shader.GetPropertyName(i);
            var type = shader.GetPropertyType(name);
            switch (type)
            {
                case UnityEngine.Rendering.ShaderPropertyType.Color:
                    settings[name] = mat.GetColor(name);
                    break;
                case UnityEngine.Rendering.ShaderPropertyType.Float:
                case UnityEngine.Rendering.ShaderPropertyType.Range:
                    settings[name] = mat.GetFloat(name);
                    break;
                case UnityEngine.Rendering.ShaderPropertyType.Vector:
                    settings[name] = mat.GetVector(name);
                    break;
                case UnityEngine.Rendering.ShaderPropertyType.Texture:
                    settings[name] = mat.GetTexture(name);
                    break;
            }
        }

        EditorPrefs.SetString("BlueysSimple_CopiedSettings", BlueysTextureUtils.SavePresetToJson(settings));
        EditorUtility.DisplayDialog("Copied", "Material settings copied to clipboard.", "OK");
    }

    void PasteMaterialSettings(Material mat)
    {
        string json = EditorPrefs.GetString("BlueysSimple_CopiedSettings", "");
        if (string.IsNullOrEmpty(json))
        {
            EditorUtility.DisplayDialog("Paste Failed", "No settings in clipboard. Copy a material first.", "OK");
            return;
        }

        Dictionary<string, object> settings = BlueysTextureUtils.LoadPresetFromJson(json);
        if (settings != null)
        {
            BlueysTexturePresets.ApplyPresetData(mat, settings);
            EditorUtility.SetDirty(mat);
            EditorUtility.DisplayDialog("Pasted", "Material settings applied.", "OK");
        }
    }

    void ResetMaterial(Material mat)
    {
        Shader shader = mat.shader;
        int propCount = shader.GetPropertyCount();
        for (int i = 0; i < propCount; i++)
        {
            string name = shader.GetPropertyName(i);
            if (!defaultValues.ContainsKey(name)) continue;

            var type = shader.GetPropertyType(name);
            switch (type)
            {
                case UnityEngine.Rendering.ShaderPropertyType.Color:
                    if (defaultValues[name] is Color col) mat.SetColor(name, col);
                    break;
                case UnityEngine.Rendering.ShaderPropertyType.Float:
                case UnityEngine.Rendering.ShaderPropertyType.Range:
                    if (defaultValues[name] is float f) mat.SetFloat(name, f);
                    break;
                case UnityEngine.Rendering.ShaderPropertyType.Vector:
                    if (defaultValues[name] is Vector4 v) mat.SetVector(name, v);
                    break;
                case UnityEngine.Rendering.ShaderPropertyType.Texture:
                    mat.SetTexture(name, null);
                    break;
            }
        }
        EditorUtility.SetDirty(mat);
    }
}
