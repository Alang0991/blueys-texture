using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class BlueysTextureGUI : ShaderGUI
{
    private int tabIndex = 0;
    private readonly string[] tabs = { "Main", "Lighting", "Effects", "Rendering", "Optimization", "Presets" };

    private bool mainOpen = true;
    private bool transparencyOpen = true;
    private bool textureOpen = true;
    private bool detailOpen = false;
    private bool normalOpen = false;
    private bool shineOpen = true;
    private bool edgeOpen = true;
    private bool depthOpen = false;
    private bool innerOpen = true;
    private bool emissionOpen = false;
    private bool reflectionOpen = false;
    private bool matcapOpen = false;
    private bool gradientOpen = false;
    private bool dissolveOpen = false;
    private bool outlineOpen = false;
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
        { "_Cull", 2f },
        { "_MainTex", null },
        { "_MainTiling", new Vector4(1,1,0,0) },
        { "_MainOffset", new Vector4(0,0,0,0) },
        { "_Color", new Color(1f, 1f, 1f, 1f) },
        { "_Alpha", 1f },
        { "_UseTextureBoost", 0f },
        { "_TextureStrength", 1f },
        { "_Contrast", 1f },
        { "_Brightness", 1f },
        { "_Saturation", 1f },
        { "_HueShift", 0f },
        { "_Gamma", 1f },
        { "_Vibrance", 0f },
        { "_Sharpness", 0f },
        { "_UseDetail", 0f },
        { "_DetailTex", null },
        { "_DetailStrength", 0.2f },
        { "_DetailTiling", 8f },
        { "_DetailOffset", new Vector4(0f, 0f, 0f, 0f) },
        { "_UseNormal", 0f },
        { "_BumpMap", null },
        { "_BumpStrength", 0.4f },
        { "_UseWetShine", 0f },
        { "_Smoothness", 1f },
        { "_SpecularStrength", 0.5f },
        { "_MetallicMap", null },
        { "_MetallicStrength", 0f },
        { "_SmoothnessMap", null },
        { "_SmoothnessStrength", 0f },
        { "_UseEdgeGlow", 0f },
        { "_RimColor", new Color(0.35f, 0.9f, 1f, 1f) },
        { "_RimPower", 3f },
        { "_RimStrength", 2f },
        { "_EdgeAlphaBoost", 0.1f },
        { "_UseDepth", 0f },
        { "_DepthColor", new Color(0f, 0.16f, 0.75f, 1f) },
        { "_DepthStrength", 0.5f },
        { "_UseInnerGlow", 0f },
        { "_InnerColor", new Color(0.15f, 0.75f, 1f, 1f) },
        { "_InnerStrength", 0.6f },
        { "_InnerPower", 2f },
        { "_UseEmission", 0f },
        { "_EmissionMap", null },
        { "_EmissionColor", new Color(0.1f, 0.7f, 1f, 1f) },
        { "_EmissionStrength", 1f },
        { "_PulseSpeed", 0f },
        { "_PulseMin", 0.5f },
        { "_FlickerSpeed", 0f },
        { "_FlickerIntensity", 0f },
        { "_ScrollSpeed", 0f },
        { "_ScrollDirection", 0f },
        { "_UseReflection", 0f },
        { "_ReflectionColor", new Color(0.7f, 0.95f, 1f, 1f) },
        { "_ReflectionStrength", 0.4f },
        { "_ReflectionPower", 4f },
        { "_ReflectionMap", null },
        { "_ReflectionBlend", 0f },
        { "_UseOutline", 0f },
        { "_OutlineColor", new Color(0f, 0f, 0f, 1f) },
        { "_OutlineWidth", 0f },
        { "_OutlineThreshold", 0.1f },
        { "_UseDissolve", 0f },
        { "_DissolveAmount", 0f },
        { "_DissolveEdgeWidth", 0.05f },
        { "_DissolveEdgeColor", new Color(1f, 0.5f, 0f, 1f) },
        { "_UseMatcap", 0f },
        { "_MatcapTex", null },
        { "_MatcapStrength", 0f },
        { "_UseGradient", 0f },
        { "_GradientTex", null },
        { "_GradientStrength", 0f },
        { "_OcclusionMap", null },
        { "_OcclusionStrength", 1f },
        { "_FinalGlowPower", 1f }
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
        GUI.Label(r, "Blueys Texture", title);

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

        DrawPlainSection(editor, props, ref transparencyOpen, "Transparency",
            "_Alpha");

        DrawToggleSection(editor, props, ref textureOpen, "Texture Enhancement", "_UseTextureBoost",
            "_TextureStrength", "_Contrast", "_Brightness", "_Saturation",
            "_HueShift", "_Gamma", "_Vibrance", "_Sharpness");

        DrawToggleSection(editor, props, ref detailOpen, "Detail Overlay", "_UseDetail",
            "_DetailTex", "_DetailStrength", "_DetailTiling", "_DetailOffset");
    }

    void DrawLightingTab(MaterialEditor editor, MaterialProperty[] props, Material mat)
    {
        DrawToggleSection(editor, props, ref normalOpen, "Normal Map", "_UseNormal",
            "_BumpMap", "_BumpStrength");

        DrawToggleSection(editor, props, ref shineOpen, "Wet Shine", "_UseWetShine",
            "_Smoothness", "_SpecularStrength", "_MetallicMap", "_MetallicStrength",
            "_SmoothnessMap", "_SmoothnessStrength");
    }

    void DrawEffectsTab(MaterialEditor editor, MaterialProperty[] props, Material mat)
    {
        DrawToggleSection(editor, props, ref edgeOpen, "Edge Glow", "_UseEdgeGlow",
            "_RimColor", "_RimPower", "_RimStrength", "_EdgeAlphaBoost");

        DrawToggleSection(editor, props, ref depthOpen, "Deep Color", "_UseDepth",
            "_DepthColor", "_DepthStrength");

        DrawToggleSection(editor, props, ref innerOpen, "Inner Glow", "_UseInnerGlow",
            "_InnerColor", "_InnerStrength", "_InnerPower");

        DrawEmissionSection(editor, props, ref emissionOpen, mat);

        DrawToggleSection(editor, props, ref reflectionOpen, "Fake Reflection", "_UseReflection",
            "_ReflectionColor", "_ReflectionStrength", "_ReflectionPower",
            "_ReflectionMap", "_ReflectionBlend");

        DrawToggleSection(editor, props, ref matcapOpen, "Matcap", "_UseMatcap",
            "_MatcapTex", "_MatcapStrength");

        DrawToggleSection(editor, props, ref gradientOpen, "Gradient", "_UseGradient",
            "_GradientTex", "_GradientStrength");

        DrawToggleSection(editor, props, ref dissolveOpen, "Dissolve", "_UseDissolve",
            "_DissolveAmount", "_DissolveEdgeWidth", "_DissolveEdgeColor");

        DrawToggleSection(editor, props, ref outlineOpen, "Outline", "_UseOutline",
            "_OutlineColor", "_OutlineWidth", "_OutlineThreshold");
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
            EditorGUILayout.LabelField("Render Type", mat.GetTag("RenderType", false, "Transparent"));
            EditorGUILayout.LabelField("Shader", mat.shader.name);
            EditorGUILayout.Space(8);

            EditorGUI.BeginChangeCheck();
            int queue = EditorGUILayout.IntField("Custom Render Queue", mat.renderQueue);
            if (EditorGUI.EndChangeCheck())
            {
                mat.renderQueue = queue;
                EditorUtility.SetDirty(mat);
            }

            if (mat.HasProperty("_Cull"))
            {
                EditorGUI.BeginChangeCheck();
                bool doubleSided = mat.GetFloat("_Cull") == 0;
                bool newDoubleSided = EditorGUILayout.Toggle("Double Sided", doubleSided);
                if (EditorGUI.EndChangeCheck())
                {
                    mat.SetFloat("_Cull", newDoubleSided ? 0f : 2f);
                    EditorUtility.SetDirty(mat);
                }
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
                fixes.Add("Assign a Main Image Texture.");
            }

            Texture normal = mat.GetTexture("_BumpMap");
            if (normal != null && !(normal is Texture2D))
            {
                warnings.Add("Normal map is not a 2D texture.");
                fixes.Add("Use a 2D texture for normal maps.");
            }

            if (mat.HasProperty("_UseEmission") && mat.GetFloat("_UseEmission") > 0.5f && mat.GetTexture("_EmissionMap") == null)
            {
                warnings.Add("Emission is enabled but no emission texture is assigned.");
                fixes.Add("Assign an emission texture or disable emission.");
            }

            if (mat.renderQueue != 3000)
            {
                warnings.Add("Render queue is not set to Transparent (3000).");
                fixes.Add("Set render queue to 3000.");
            }

            Texture main = mat.GetTexture("_MainTex");
            bool mainHasMipmaps = false;
            bool mainIsReadable = false;
            if (main != null)
            {
                string path = AssetDatabase.GetAssetPath(main);
                if (!string.IsNullOrEmpty(path))
                {
                    TextureImporter imp = AssetImporter.GetAtPath(path) as TextureImporter;
                    if (imp != null)
                    {
                        mainHasMipmaps = imp.mipmapEnabled;
                        mainIsReadable = imp.isReadable;
                    }
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

            if (main != null && !mainIsReadable)
            {
                warnings.Add("Main texture is not readable.");
                fixes.Add("Enable Read/Write in texture import settings.");
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
                EditorUtility.DisplayDialog("Fix Required", "Please assign a 2D normal map.", "OK");
                break;
            case 2:
                mat.SetFloat("_UseEmission", 0);
                EditorUtility.SetDirty(mat);
                break;
            case 3:
                mat.renderQueue = 3000;
                EditorUtility.SetDirty(mat);
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
                            imp.mipmapEnabled = true;
                            AssetDatabase.ImportAsset(path);
                        }
                    }
                }
                break;
            case 5:
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
            case 6:
                if (main != null)
                {
                    string path = AssetDatabase.GetAssetPath(main);
                    if (!string.IsNullOrEmpty(path))
                    {
                        TextureImporter imp = AssetImporter.GetAtPath(path) as TextureImporter;
                        if (imp != null)
                        {
                            imp.isReadable = true;
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
                "_MainTex", "_DetailTex", "_BumpMap", "_EmissionMap",
                "_ReflectionMap", "_OcclusionMap", "_MetallicMap", "_SmoothnessMap",
                "_MatcapTex", "_GradientTex"
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
            if (mat.IsKeywordEnabled("_USE_TEXTURE_BOOST")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_DETAIL")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_NORMAL")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_WET_SHINE")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_EDGE_GLOW")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_DEPTH")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_INNER_GLOW")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_EMISSION")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_REFLECTION")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_MATCAP")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_GRADIENT")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_DISSOLVE")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_OUTLINE")) keywordCount++;

            EditorGUILayout.LabelField("Active Keywords", keywordCount.ToString());
            EditorGUILayout.LabelField("Shader", mat.shader.name);
            EditorGUILayout.LabelField("Render Queue", mat.renderQueue.ToString());
            EditorGUILayout.LabelField("Render Type", mat.GetTag("RenderType", false, "Unknown"));

            string perfRating = "Low";
            if (keywordCount <= 3) perfRating = "Low (Good for VRChat)";
            else if (keywordCount <= 6) perfRating = "Medium";
            else perfRating = "High (Consider simplifying)";

            EditorGUILayout.LabelField("Performance Rating", perfRating);

            if (keywordCount > 8)
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
            DrawProp(editor, props, "_EmissionColor");
            DrawProp(editor, props, "_EmissionStrength");

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
            case "_MainTex": return "The main image texture for the material.";
            case "_Color": return "Tint colour applied to the main texture.";
            case "_MainTiling": return "Tiling of the main texture UVs.";
            case "_MainOffset": return "Offset of the main texture UVs.";
            case "_Alpha": return "Overall transparency of the material.";
            case "_Contrast": return "Adjusts the difference between light and dark areas.";
            case "_Brightness": return "Brightens or darkens the texture.";
            case "_Saturation": return "Adjusts colour intensity.";
            case "_HueShift": return "Rotates the colour hue of the texture.";
            case "_Gamma": return "Adjusts gamma/brightness curve.";
            case "_Vibrance": return "Intelligently boosts colour saturation.";
            case "_Sharpness": return "Enhances edge detail.";
            case "_TextureStrength": return "Strength of the texture enhancement effect.";
            case "_DetailTex": return "Detail texture overlay.";
            case "_DetailStrength": return "Strength of the detail overlay.";
            case "_DetailTiling": return "Tiling of the detail texture.";
            case "_DetailOffset": return "Offset of the detail texture UVs.";
            case "_BumpMap": return "Normal map for surface detail.";
            case "_BumpStrength": return "Strength of the normal map effect.";
            case "_Smoothness": return "Smoothness of the surface.";
            case "_SpecularStrength": return "Strength of the specular highlight.";
            case "_MetallicMap": return "Texture that defines metallic areas.";
            case "_MetallicStrength": return "Strength of the metallic map effect.";
            case "_SmoothnessMap": return "Texture that defines smoothness areas.";
            case "_SmoothnessStrength": return "Strength of the smoothness map effect.";
            case "_RimColor": return "Colour of the edge glow.";
            case "_RimPower": return "How tight the edge glow is.";
            case "_RimStrength": return "Brightness of the edge glow.";
            case "_EdgeAlphaBoost": return "Additional alpha added to edges for glow effect.";
            case "_DepthColor": return "Colour applied based on viewing angle.";
            case "_DepthStrength": return "Strength of the deep color effect.";
            case "_InnerColor": return "Colour of the inner glow.";
            case "_InnerStrength": return "Brightness of the inner glow.";
            case "_InnerPower": return "Softness of the inner glow.";
            case "_EmissionMap": return "Texture that defines emission areas.";
            case "_EmissionColor": return "Colour of the emission.";
            case "_EmissionStrength": return "Brightness of the emission.";
            case "_PulseSpeed": return "Speed of the pulse animation.";
            case "_PulseMin": return "Minimum brightness during pulse.";
            case "_FlickerSpeed": return "Speed of the flicker effect.";
            case "_FlickerIntensity": return "Intensity of the flicker effect.";
            case "_ScrollSpeed": return "Speed of the scrolling emission.";
            case "_ScrollDirection": return "Direction of the scrolling emission in degrees.";
            case "_ReflectionColor": return "Colour of the fake reflection.";
            case "_ReflectionStrength": return "Brightness of the fake reflection.";
            case "_ReflectionPower": return "How tight the fake reflection is.";
            case "_ReflectionMap": return "Texture for the fake reflection.";
            case "_ReflectionBlend": return "How much the reflection texture blends with the reflection colour.";
            case "_OutlineColor": return "Colour of the outline.";
            case "_OutlineWidth": return "Width of the outline effect.";
            case "_OutlineThreshold": return "Threshold for the outline effect.";
            case "_DissolveAmount": return "How much of the material is dissolved.";
            case "_DissolveEdgeWidth": return "Width of the dissolve edge.";
            case "_DissolveEdgeColor": return "Colour of the dissolve edge.";
            case "_MatcapTex": return "Matcap texture for fake reflections.";
            case "_MatcapStrength": return "Strength of the matcap effect.";
            case "_GradientTex": return "Gradient texture for vertical colouring.";
            case "_GradientStrength": return "Strength of the gradient effect.";
            case "_OcclusionMap": return "Texture that defines ambient occlusion.";
            case "_OcclusionStrength": return "Strength of the ambient occlusion.";
            case "_FinalGlowPower": return "Final multiplier for all glow/emission output.";
            default: return "";
        }
    }

    void LoadSectionStates(Material mat)
    {
        string key = "BlueysTexture_" + mat.GetInstanceID() + "_";
        mainOpen = EditorPrefs.GetBool(key + "mainOpen", true);
        transparencyOpen = EditorPrefs.GetBool(key + "transparencyOpen", true);
        textureOpen = EditorPrefs.GetBool(key + "textureOpen", true);
        detailOpen = EditorPrefs.GetBool(key + "detailOpen", false);
        normalOpen = EditorPrefs.GetBool(key + "normalOpen", false);
        shineOpen = EditorPrefs.GetBool(key + "shineOpen", true);
        edgeOpen = EditorPrefs.GetBool(key + "edgeOpen", true);
        depthOpen = EditorPrefs.GetBool(key + "depthOpen", false);
        innerOpen = EditorPrefs.GetBool(key + "innerOpen", true);
        emissionOpen = EditorPrefs.GetBool(key + "emissionOpen", false);
        reflectionOpen = EditorPrefs.GetBool(key + "reflectionOpen", false);
        matcapOpen = EditorPrefs.GetBool(key + "matcapOpen", false);
        gradientOpen = EditorPrefs.GetBool(key + "gradientOpen", false);
        dissolveOpen = EditorPrefs.GetBool(key + "dissolveOpen", false);
        outlineOpen = EditorPrefs.GetBool(key + "outlineOpen", false);
        renderOpen = EditorPrefs.GetBool(key + "renderOpen", true);
        perfOpen = EditorPrefs.GetBool(key + "perfOpen", true);
    }

    void SaveSectionStates(Material mat)
    {
        string key = "BlueysTexture_" + mat.GetInstanceID() + "_";
        EditorPrefs.SetBool(key + "mainOpen", mainOpen);
        EditorPrefs.SetBool(key + "transparencyOpen", transparencyOpen);
        EditorPrefs.SetBool(key + "textureOpen", textureOpen);
        EditorPrefs.SetBool(key + "detailOpen", detailOpen);
        EditorPrefs.SetBool(key + "normalOpen", normalOpen);
        EditorPrefs.SetBool(key + "shineOpen", shineOpen);
        EditorPrefs.SetBool(key + "edgeOpen", edgeOpen);
        EditorPrefs.SetBool(key + "depthOpen", depthOpen);
        EditorPrefs.SetBool(key + "innerOpen", innerOpen);
        EditorPrefs.SetBool(key + "emissionOpen", emissionOpen);
        EditorPrefs.SetBool(key + "reflectionOpen", reflectionOpen);
        EditorPrefs.SetBool(key + "matcapOpen", matcapOpen);
        EditorPrefs.SetBool(key + "gradientOpen", gradientOpen);
        EditorPrefs.SetBool(key + "dissolveOpen", dissolveOpen);
        EditorPrefs.SetBool(key + "outlineOpen", outlineOpen);
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

        EditorPrefs.SetString("Blueys_CopiedSettings", BlueysTextureUtils.SavePresetToJson(settings));
        EditorUtility.DisplayDialog("Copied", "Material settings copied to clipboard.", "OK");
    }

    void PasteMaterialSettings(Material mat)
    {
        string json = EditorPrefs.GetString("Blueys_CopiedSettings", "");
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
