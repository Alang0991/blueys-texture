using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class BlueysTextureGUI : ShaderGUI
{
    bool mainOpen = true;
    bool transparencyOpen = true;
    bool textureOpen = true;
    bool toonOpen = false;
    bool rimOpen = false;
    bool normalOpen = false;
    bool materialOpen = false;
    bool emissionOpen = false;
    bool detailOpen = false;
    bool matcapOpen = false;
    bool gradientOpen = false;
    bool finalOpen = false;
    bool renderOpen = false;

    string searchQuery = "";
    Vector2 scrollPos;
    bool showPerformance = false;
    bool showValidation = false;
    bool showTextureInfo = false;

    Material cachedMat;

    readonly Color accent = new Color(0.25f, 0.75f, 1f);
    readonly Color headerOff = new Color(0.16f, 0.16f, 0.16f);
    readonly Color headerOn = new Color(0.12f, 0.22f, 0.26f);
    readonly Color body = new Color(0.13f, 0.13f, 0.13f);

    static Dictionary<string, bool> sectionStates = new Dictionary<string, bool>();

    static readonly Dictionary<string, object> defaultValues = new Dictionary<string, object>()
    {
        { "_Cull", 2f },
        { "_Color", new Color(1f, 1f, 1f, 1f) },
        { "_MainTexTiling", new Vector4(1f, 1f, 0f, 0f) },
        { "_MainTexOffset", new Vector4(0f, 0f, 0f, 0f) },
        { "_Alpha", 1f },
        { "_Brightness", 1f },
        { "_Contrast", 1f },
        { "_Saturation", 1f },
        { "_HueShift", 0f },
        { "_Gamma", 1f },
        { "_Vibrance", 0f },
        { "_Sharpness", 0f },
        { "_UseToonLighting", 0f },
        { "_UseCelShading", 0f },
        { "_ShadowColor", new Color(0.1f, 0.1f, 0.2f, 1f) },
        { "_ShadowStrength", 0.5f },
        { "_ShadowThreshold", 0.5f },
        { "_ShadowSmoothness", 0.1f },
        { "_ToonLightDir", new Vector4(0f, 1f, 0f, 0f) },
        { "_UseShadeRamp", 0f },
        { "_UseRimLight", 0f },
        { "_RimColor", new Color(0.35f, 0.9f, 1f, 1f) },
        { "_RimPower", 3f },
        { "_RimStrength", 2f },
        { "_RimWidth", 0.5f },
        { "_UseFresnel", 0f },
        { "_FresnelColor", new Color(0.35f, 0.9f, 1f, 1f) },
        { "_FresnelPower", 3f },
        { "_FresnelStrength", 2f },
        { "_UseMatcap", 0f },
        { "_MatcapStrength", 0f },
        { "_UseFakeLight", 0f },
        { "_FakeLightColor", new Color(1f, 1f, 1f, 1f) },
        { "_FakeLightIntensity", 1f },
        { "_FakeLightDir", new Vector4(0f, 1f, 0f, 0f) },
        { "_UseDetail", 0f },
        { "_DetailStrength", 0.2f },
        { "_DetailTexTiling", new Vector4(8f, 8f, 0f, 0f) },
        { "_DetailTexOffset", new Vector4(0f, 0f, 0f, 0f) },
        { "_DetailBlend", 1f },
        { "_UseNormal", 0f },
        { "_BumpStrength", 0.4f },
        { "_UseParallax", 0f },
        { "_ParallaxStrength", 0.02f },
        { "_Smoothness", 1f },
        { "_MetallicStrength", 0f },
        { "_SmoothnessStrength", 0f },
        { "_OcclusionStrength", 1f },
        { "_UseEmission", 0f },
        { "_EmissionColor", new Color(0.1f, 0.7f, 1f, 1f) },
        { "_EmissionStrength", 1f },
        { "_EmissionPulseSpeed", 0f },
        { "_EmissionPulseMin", 0.5f },
        { "_EmissionFlickerSpeed", 0f },
        { "_EmissionFlickerIntensity", 0f },
        { "_EmissionScrollSpeed", 0f },
        { "_EmissionScrollDirection", 0f },
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
        DrawToolbar(mat);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        if (showValidation) DrawValidation(editor, mat);
        else if (showTextureInfo) DrawTextureInfo(mat);
        else if (showPerformance) DrawPerformance(mat);
        else DrawMainSections(editor, props, mat);

        EditorGUILayout.EndScrollView();

        SaveSectionStates(mat);
    }

    void DrawBanner()
    {
        Rect r = EditorGUILayout.GetControlRect(false, 56);
        EditorGUI.DrawRect(r, new Color(0.04f, 0.07f, 0.09f));

        GUIStyle title = new GUIStyle(EditorStyles.boldLabel);
        title.fontSize = 22;
        title.alignment = TextAnchor.MiddleCenter;
        title.normal.textColor = accent;

        GUI.Label(r, "Blueys Texture", title);

        Rect line = new Rect(r.x, r.yMax - 3, r.width, 3);
        EditorGUI.DrawRect(line, accent);

        GUIStyle ver = new GUIStyle(EditorStyles.miniLabel);
        ver.alignment = TextAnchor.MiddleCenter;
        ver.normal.textColor = new Color(0.5f, 0.5f, 0.5f);
        GUI.Label(new Rect(r.x, r.yMax - 18, r.width, 18), "v1.1.0", ver);

        EditorGUILayout.Space(6);
    }

    void DrawToolbar(Material mat)
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        Rect searchRect = EditorGUILayout.GetControlRect(GUILayout.Width(180));
        searchRect.y += 2;
        searchRect.height = 18;
        searchQuery = EditorGUI.TextField(searchRect, searchQuery, EditorStyles.toolbarSearchField);

        if (GUILayout.Button("Validate", EditorStyles.toolbarButton, GUILayout.Width(70)))
        {
            showValidation = true;
            showTextureInfo = false;
            showPerformance = false;
        }

        if (GUILayout.Button("Textures", EditorStyles.toolbarButton, GUILayout.Width(70)))
        {
            showTextureInfo = true;
            showValidation = false;
            showPerformance = false;
        }

        if (GUILayout.Button("Performance", EditorStyles.toolbarButton, GUILayout.Width(90)))
        {
            showPerformance = !showPerformance;
            showValidation = false;
            showTextureInfo = false;
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Reset All", EditorStyles.toolbarButton, GUILayout.Width(70)))
        {
            if (EditorUtility.DisplayDialog("Reset Material", "Reset all material properties to defaults?", "Yes", "Cancel"))
            {
                ResetMaterial(mat);
            }
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(4);
    }

    void DrawMainSections(MaterialEditor editor, MaterialProperty[] props, Material mat)
    {
        DrawPlainSection(editor, props, ref mainOpen, "Main Texture",
            "_MainTex", "_Color", "_MainTexTiling", "_MainTexOffset");

        DrawPlainSection(editor, props, ref transparencyOpen, "Transparency",
            "_Alpha");

        DrawPlainSection(editor, props, ref textureOpen, "Texture Look",
            "_Brightness", "_Contrast", "_Saturation", "_HueShift", "_Gamma", "_Vibrance", "_Sharpness");

        DrawShadingSection(editor, props, ref toonOpen, mat);

        DrawRimLightSection(editor, props, ref rimOpen, mat);

        DrawToggleSection(editor, props, ref normalOpen, "Normal Map", "_UseNormal",
            "_BumpMap", "_BumpStrength");

        DrawPlainSection(editor, props, ref materialOpen, "Material",
            "_Smoothness", "_MetallicStrength", "_MetallicMap",
            "_SmoothnessStrength", "_SmoothnessMap");

        DrawEmissionSection(editor, props, ref emissionOpen, mat);

        DrawToggleSection(editor, props, ref detailOpen, "Detail Overlay", "_UseDetail",
            "_DetailTex", "_DetailStrength", "_DetailTexTiling", "_DetailTexOffset");

        DrawToggleSection(editor, props, ref matcapOpen, "Matcap", "_UseMatcap",
            "_MatcapTex", "_MatcapStrength");

        DrawToggleSection(editor, props, ref gradientOpen, "Gradient", "_UseGradient",
            "_GradientTex", "_GradientStrength");

        DrawPlainSection(editor, props, ref finalOpen, "Occlusion & Final Output",
            "_OcclusionMap", "_OcclusionStrength", "_FinalGlowPower");

        DrawRenderSection(ref renderOpen, mat);
        DrawPresetSection(mat);
        DrawCopyPaste(mat);
    }

    void DrawValidation(MaterialEditor editor, Material mat)
    {
        bool valOpen = true;
        EditorGUILayout.BeginVertical();
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
        bool infoOpen = true;
        EditorGUILayout.BeginVertical();
        infoOpen = DrawHeaderStrip(infoOpen, "Texture Information", false, false, null);

        if (infoOpen)
        {
            DrawBodyStart();

            string[] texProps = new string[]
            {
                "_MainTex", "_DetailTex", "_BumpMap", "_EmissionMap", "_EmissionMask",
                "_OcclusionMap", "_MetallicMap", "_SmoothnessMap",
                "_MatcapTex", "_GradientTex", "_RimMask"
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

                EditorGUILayout.LabelField("VRAM", FormatVRAM(tex));
                EditorGUI.indentLevel--;
                EditorGUILayout.Space(4);
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(5);
    }

    string FormatVRAM(Texture tex)
    {
        long bytes = (long)tex.width * tex.height * 4;
        if (bytes > 1048576) return (bytes / 1048576f).ToString("F1") + " MB";
        if (bytes > 1024) return (bytes / 1024f).ToString("F1") + " KB";
        return bytes + " B";
    }

    void DrawPerformance(Material mat)
    {
        bool perfOpen = true;
        EditorGUILayout.BeginVertical();
        perfOpen = DrawHeaderStrip(perfOpen, "Performance Information", false, false, null);

        if (perfOpen)
        {
            DrawBodyStart();

            int keywordCount = 0;
            if (mat.IsKeywordEnabled("_USE_TOON_LIGHTING")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_DETAIL")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_NORMAL")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_RIM_LIGHT")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_FRESNEL")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_MATCAP")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_FAKE_LIGHT")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_EMISSION")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_OUTLINE")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_DISSOLVE")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_GRADIENT")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_DECAL")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_STENCIL")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_AUDIOLINK")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_IRIDESCENT")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_GLITTER")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_PARALLAX")) keywordCount++;
            if (mat.IsKeywordEnabled("_USE_SHADE_RAMP")) keywordCount++;

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
        bool presetOpen = true;
        EditorGUILayout.BeginVertical();
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
                            ApplyPreset(mat, presets[idx]);
                        }
                    }
                }
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(5);
    }

    void ApplyPreset(Material mat, string preset)
    {
        switch (preset)
        {
            case "Wet Fur":
                mat.SetFloat("_Smoothness", 0.9f);
                mat.SetFloat("_MetallicStrength", 0.1f);
                mat.SetFloat("_Brightness", 1.1f);
                mat.SetFloat("_Contrast", 1.2f);
                mat.SetFloat("_Saturation", 0.9f);
                mat.SetFloat("_Vibrance", 0.3f);
                mat.SetFloat("_UseRimLight", 1);
                mat.SetFloat("_RimStrength", 1.5f);
                break;
            case "Plastic":
                mat.SetFloat("_Smoothness", 0.7f);
                mat.SetFloat("_MetallicStrength", 0f);
                mat.SetFloat("_Brightness", 1.0f);
                mat.SetFloat("_Contrast", 1.1f);
                mat.SetFloat("_Saturation", 1.2f);
                mat.SetFloat("_Vibrance", 0.1f);
                break;
            case "Rubber":
                mat.SetFloat("_Smoothness", 0.3f);
                mat.SetFloat("_MetallicStrength", 0f);
                mat.SetFloat("_Brightness", 1.0f);
                mat.SetFloat("_Contrast", 1.0f);
                break;
            case "Latex":
                mat.SetFloat("_Smoothness", 0.95f);
                mat.SetFloat("_MetallicStrength", 0f);
                mat.SetFloat("_Brightness", 1.1f);
                mat.SetFloat("_Contrast", 1.1f);
                mat.SetFloat("_Vibrance", 0.2f);
                mat.SetFloat("_UseRimLight", 1);
                mat.SetFloat("_RimStrength", 1.5f);
                break;
            case "Metal":
                mat.SetFloat("_Smoothness", 0.9f);
                mat.SetFloat("_MetallicStrength", 1.0f);
                mat.SetFloat("_Brightness", 1.0f);
                mat.SetFloat("_Contrast", 1.1f);
                mat.SetFloat("_Saturation", 0.7f);
                break;
            case "Skin":
                mat.SetFloat("_Smoothness", 0.4f);
                mat.SetFloat("_MetallicStrength", 0f);
                mat.SetFloat("_Brightness", 1.05f);
                mat.SetFloat("_Contrast", 1.05f);
                mat.SetFloat("_Saturation", 0.9f);
                mat.SetFloat("_Vibrance", 0.1f);
                break;
            case "Toon":
                mat.SetFloat("_UseToonLighting", 1);
                mat.SetFloat("_UseCelShading", 1);
                mat.SetFloat("_Contrast", 1.8f);
                mat.SetFloat("_Brightness", 1.0f);
                mat.SetFloat("_Saturation", 1.5f);
                mat.SetFloat("_Sharpness", 1.0f);
                mat.SetFloat("_Smoothness", 0f);
                break;
            case "Glow":
                mat.SetFloat("_UseEmission", 1);
                mat.SetFloat("_EmissionStrength", 2.0f);
                mat.SetFloat("_FinalGlowPower", 1.5f);
                mat.SetFloat("_UseRimLight", 1);
                mat.SetFloat("_RimStrength", 2.0f);
                mat.SetFloat("_Brightness", 1.2f);
                break;
            case "Matte":
                mat.SetFloat("_Smoothness", 0f);
                mat.SetFloat("_MetallicStrength", 0f);
                mat.SetFloat("_Brightness", 1.0f);
                mat.SetFloat("_Contrast", 1.0f);
                break;
            case "Fabric":
                mat.SetFloat("_Smoothness", 0.2f);
                mat.SetFloat("_MetallicStrength", 0f);
                mat.SetFloat("_Brightness", 1.0f);
                mat.SetFloat("_Contrast", 1.1f);
                mat.SetFloat("_Saturation", 0.9f);
                mat.SetFloat("_UseDetail", 1);
                mat.SetFloat("_DetailStrength", 0.3f);
                break;
        }

        EditorUtility.SetDirty(mat);
    }

    void DrawCopyPaste(Material mat)
    {
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Copy Settings", GUILayout.Height(22)))
        {
            CopyMaterialSettings(mat);
        }

        if (GUILayout.Button("Paste Settings", GUILayout.Height(22)))
        {
            PasteMaterialSettings(mat);
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(4);
    }

    Dictionary<string, object> copiedSettings;

    void CopyMaterialSettings(Material mat)
    {
        copiedSettings = new Dictionary<string, object>();
        Shader shader = mat.shader;
        int propCount = ShaderUtil.GetPropertyCount(shader);
        for (int i = 0; i < propCount; i++)
        {
            string name = ShaderUtil.GetPropertyName(shader, i);
            ShaderUtil.ShaderPropertyType type = ShaderUtil.GetPropertyType(shader, i);
            switch (type)
            {
                case ShaderUtil.ShaderPropertyType.Color:
                    copiedSettings[name] = mat.GetColor(name);
                    break;
                case ShaderUtil.ShaderPropertyType.Float:
                case ShaderUtil.ShaderPropertyType.Range:
                    copiedSettings[name] = mat.GetFloat(name);
                    break;
                case ShaderUtil.ShaderPropertyType.Vector:
                    copiedSettings[name] = mat.GetVector(name);
                    break;
                case ShaderUtil.ShaderPropertyType.TexEnv:
                    copiedSettings[name] = mat.GetTexture(name);
                    break;
            }
        }
        EditorUtility.DisplayDialog("Copied", "Material settings copied to clipboard.", "OK");
    }

    void PasteMaterialSettings(Material mat)
    {
        if (copiedSettings == null)
        {
            EditorUtility.DisplayDialog("Paste Failed", "No settings in clipboard. Copy a material first.", "OK");
            return;
        }

        foreach (var kvp in copiedSettings)
        {
            if (mat.HasProperty(kvp.Key))
            {
                switch (kvp.Value)
                {
                    case Color c:
                        mat.SetColor(kvp.Key, c);
                        break;
                    case float f:
                        mat.SetFloat(kvp.Key, f);
                        break;
                    case Vector4 v:
                        mat.SetVector(kvp.Key, v);
                        break;
                    case Texture t:
                        mat.SetTexture(kvp.Key, t);
                        break;
                }
            }
        }

        EditorUtility.SetDirty(mat);
        EditorUtility.DisplayDialog("Pasted", "Material settings applied.", "OK");
    }

    void ResetMaterial(Material mat)
    {
        Shader shader = mat.shader;
        int propCount = ShaderUtil.GetPropertyCount(shader);
        for (int i = 0; i < propCount; i++)
        {
            string name = ShaderUtil.GetPropertyName(shader, i);
            ShaderUtil.ShaderPropertyType type = ShaderUtil.GetPropertyType(shader, i);
            if (type == ShaderUtil.ShaderPropertyType.Color)
            {
                if (defaultValues.ContainsKey(name) && defaultValues[name] is Color col)
                    mat.SetColor(name, col);
            }
            else if (type == ShaderUtil.ShaderPropertyType.Float || type == ShaderUtil.ShaderPropertyType.Range)
            {
                if (defaultValues.ContainsKey(name) && defaultValues[name] is float f)
                    mat.SetFloat(name, f);
            }
            else if (type == ShaderUtil.ShaderPropertyType.Vector)
            {
                if (defaultValues.ContainsKey(name) && defaultValues[name] is Vector4 v)
                    mat.SetVector(name, v);
            }
        }
        EditorUtility.SetDirty(mat);
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

            if (mat.GetFloat("_UseEmission") > 0.5f)
            {
                EditorGUILayout.LabelField("Pulse Animation");
                DrawProp(editor, props, "_EmissionPulseSpeed");
                DrawProp(editor, props, "_EmissionPulseMin");

                EditorGUILayout.LabelField("Flicker Effect");
                DrawProp(editor, props, "_EmissionFlickerSpeed");
                DrawProp(editor, props, "_EmissionFlickerIntensity");

                EditorGUILayout.LabelField("Scrolling Emission");
                DrawProp(editor, props, "_EmissionScrollSpeed");
                DrawProp(editor, props, "_EmissionScrollDirection");
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(5);
    }

    void DrawShadingSection(MaterialEditor editor, MaterialProperty[] props, ref bool open, Material mat)
    {
        EditorGUILayout.BeginVertical();
        open = DrawHeaderStrip(open, "Shading", true, mat.GetFloat("_UseToonLighting") > 0.5f, FindProperty("_UseToonLighting", props, false));

        if (open)
        {
            DrawBodyStart();
            EditorGUI.indentLevel++;

            DrawProp(editor, props, "_UseCelShading");
            DrawProp(editor, props, "_ShadowColor");
            DrawProp(editor, props, "_ShadowStrength");
            DrawProp(editor, props, "_ShadowThreshold");
            DrawProp(editor, props, "_ShadowSmoothness");
            DrawProp(editor, props, "_ToonLightDir");

            DrawToggleSection(editor, props, ref shadeRampOpen, "Shade Ramp", "_UseShadeRamp",
                "_ShadeRampTex");

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(5);
    }

    void DrawRimLightSection(MaterialEditor editor, MaterialProperty[] props, ref bool open, Material mat)
    {
        EditorGUILayout.BeginVertical();
        open = DrawHeaderStrip(open, "Rim Light", true, mat.GetFloat("_UseRimLight") > 0.5f, FindProperty("_UseRimLight", props, false));

        if (open)
        {
            DrawBodyStart();
            EditorGUI.indentLevel++;

            DrawProp(editor, props, "_RimColor");
            DrawProp(editor, props, "_RimPower");
            DrawProp(editor, props, "_RimStrength");
            DrawProp(editor, props, "_RimWidth");
            DrawProp(editor, props, "_RimMask");

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(5);
    }

    bool shadeRampOpen = false;

    void DrawRenderSection(ref bool open, Material mat)
    {
        EditorGUILayout.BeginVertical();
        open = DrawHeaderStrip(open, "Rendering", false, false, null);

        if (open && mat != null)
        {
            DrawBodyStart();

            EditorGUI.BeginChangeCheck();

            int queue = EditorGUILayout.IntField("Render Queue", mat.renderQueue);

            bool doubleSided = mat.HasProperty("_Cull") && mat.GetFloat("_Cull") == 0;
            bool newDoubleSided = EditorGUILayout.Toggle("Double Sided", doubleSided);

            if (EditorGUI.EndChangeCheck())
            {
                mat.renderQueue = queue;

                if (mat.HasProperty("_Cull"))
                {
                    mat.SetFloat("_Cull", newDoubleSided ? 0 : 2);
                }

                EditorUtility.SetDirty(mat);
            }

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
            case "_Alpha": return "Overall transparency of the material.";
            case "_Contrast": return "Adjusts the difference between light and dark areas.";
            case "_Brightness": return "Brightens or darkens the texture.";
            case "_Saturation": return "Adjusts colour intensity.";
            case "_HueShift": return "Rotates the colour hue of the texture.";
            case "_Gamma": return "Adjusts gamma/brightness curve.";
            case "_Vibrance": return "Intelligently boosts colour saturation.";
            case "_Sharpness": return "Enhances edge detail.";
            case "_ShadowColor": return "Colour tinted onto shadowed areas in toon mode.";
            case "_ShadowStrength": return "How strongly shadows affect the surface colour.";
            case "_ShadowThreshold": return "Angle threshold for shadow transition.";
            case "_ShadowSmoothness": return "Smoothness of the shadow transition edge.";
            case "_ToonLightDir": return "World-space direction the toon light comes from.";
            case "_BumpMap": return "Normal map for surface detail.";
            case "_BumpStrength": return "Strength of the normal map effect.";
            case "_EmissionMap": return "Texture that defines emission areas.";
            case "_EmissionMask": return "Mask texture for emission areas.";
            case "_EmissionColor": return "Colour of the emission.";
            case "_EmissionStrength": return "Brightness of the emission.";
            case "_EmissionPulseSpeed": return "Speed of the emission pulse animation.";
            case "_EmissionPulseMin": return "Minimum brightness during pulse animation.";
            case "_EmissionFlickerSpeed": return "Speed of the emission flicker effect.";
            case "_EmissionFlickerIntensity": return "Intensity of the emission flicker effect.";
            case "_EmissionScrollSpeed": return "Speed of the scrolling emission effect.";
            case "_EmissionScrollDirection": return "Direction of the scrolling emission effect.";
            case "_RimPower": return "How tight the edge glow is.";
            case "_RimStrength": return "Brightness of the edge glow.";
            case "_RimMask": return "Mask texture for rim light areas.";
            case "_OutlineWidth": return "Width of the outline effect.";
            case "_DissolveAmount": return "How much of the material is dissolved.";
            default: return "";
        }
    }

    void LoadSectionStates(Material mat)
    {
        string key = "BlueysTexture_" + mat.GetInstanceID() + "_";
        mainOpen = EditorPrefs.GetBool(key + "mainOpen", true);
        transparencyOpen = EditorPrefs.GetBool(key + "transparencyOpen", true);
        textureOpen = EditorPrefs.GetBool(key + "textureOpen", true);
        toonOpen = EditorPrefs.GetBool(key + "toonOpen", false);
        rimOpen = EditorPrefs.GetBool(key + "rimOpen", false);
        normalOpen = EditorPrefs.GetBool(key + "normalOpen", false);
        materialOpen = EditorPrefs.GetBool(key + "materialOpen", false);
        emissionOpen = EditorPrefs.GetBool(key + "emissionOpen", false);
        detailOpen = EditorPrefs.GetBool(key + "detailOpen", false);
        matcapOpen = EditorPrefs.GetBool(key + "matcapOpen", false);
        gradientOpen = EditorPrefs.GetBool(key + "gradientOpen", false);
        shadeRampOpen = EditorPrefs.GetBool(key + "shadeRampOpen", false);
        finalOpen = EditorPrefs.GetBool(key + "finalOpen", false);
        renderOpen = EditorPrefs.GetBool(key + "renderOpen", false);
    }

    void SaveSectionStates(Material mat)
    {
        string key = "BlueysTexture_" + mat.GetInstanceID() + "_";
        EditorPrefs.SetBool(key + "mainOpen", mainOpen);
        EditorPrefs.SetBool(key + "transparencyOpen", transparencyOpen);
        EditorPrefs.SetBool(key + "textureOpen", textureOpen);
        EditorPrefs.SetBool(key + "toonOpen", toonOpen);
        EditorPrefs.SetBool(key + "rimOpen", rimOpen);
        EditorPrefs.SetBool(key + "normalOpen", normalOpen);
        EditorPrefs.SetBool(key + "materialOpen", materialOpen);
        EditorPrefs.SetBool(key + "emissionOpen", emissionOpen);
        EditorPrefs.SetBool(key + "detailOpen", detailOpen);
        EditorPrefs.SetBool(key + "matcapOpen", matcapOpen);
        EditorPrefs.SetBool(key + "gradientOpen", gradientOpen);
        EditorPrefs.SetBool(key + "shadeRampOpen", shadeRampOpen);
        EditorPrefs.SetBool(key + "finalOpen", finalOpen);
        EditorPrefs.SetBool(key + "renderOpen", renderOpen);
    }
}
