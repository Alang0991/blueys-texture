using UnityEditor;
using UnityEngine;

public class BlueysTextureGUI : ShaderGUI
{
    bool mainOpen = true;
    bool transparencyOpen = true;
    bool textureOpen = true;
    bool detailOpen = false;
    bool normalOpen = false;
    bool shineOpen = true;
    bool edgeOpen = true;
    bool depthOpen = false;
    bool innerOpen = true;
    bool emissionOpen = false;
    bool reflectionOpen = false;
    bool finalOpen = false;
    bool renderOpen = false;

    readonly Color accent = new Color(0.25f, 0.75f, 1f);
    readonly Color headerOff = new Color(0.16f, 0.16f, 0.16f);
    readonly Color headerOn = new Color(0.12f, 0.22f, 0.26f);
    readonly Color body = new Color(0.13f, 0.13f, 0.13f);

    public override void OnGUI(MaterialEditor editor, MaterialProperty[] props)
    {
        Material mat = editor.target as Material;

        DrawBanner();

        DrawPlainSection(editor, props, ref mainOpen, "Main Texture",
            "_MainTex", "_Color");

        DrawPlainSection(editor, props, ref transparencyOpen, "Transparency",
            "_Alpha");

        DrawToggleSection(editor, props, ref textureOpen, "Texture Enhancement", "_UseTextureBoost",
            "_TextureStrength", "_Contrast", "_Brightness", "_Saturation");

        DrawToggleSection(editor, props, ref detailOpen, "Detail Overlay", "_UseDetail",
            "_DetailTex", "_DetailStrength", "_DetailTiling");

        DrawToggleSection(editor, props, ref normalOpen, "Normal Map", "_UseNormal",
            "_BumpMap", "_BumpStrength");

        DrawToggleSection(editor, props, ref shineOpen, "Wet Shine", "_UseWetShine",
            "_Smoothness", "_SpecularStrength");

        DrawToggleSection(editor, props, ref edgeOpen, "Edge Glow", "_UseEdgeGlow",
            "_RimColor", "_RimPower", "_RimStrength", "_EdgeAlphaBoost");

        DrawToggleSection(editor, props, ref depthOpen, "Deep Color", "_UseDepth",
            "_DepthColor", "_DepthStrength");

        DrawToggleSection(editor, props, ref innerOpen, "Inner Glow", "_UseInnerGlow",
            "_InnerColor", "_InnerStrength", "_InnerPower");

        DrawToggleSection(editor, props, ref emissionOpen, "Emission Texture", "_UseEmission",
            "_EmissionMap", "_EmissionColor", "_EmissionStrength");

        DrawToggleSection(editor, props, ref reflectionOpen, "Fake Reflection", "_UseReflection",
            "_ReflectionColor", "_ReflectionStrength", "_ReflectionPower");

        DrawPlainSection(editor, props, ref finalOpen, "Final Output",
            "_FinalGlowPower");

        DrawRenderSection(ref renderOpen, mat);
    }

    void DrawBanner()
    {
        Rect r = EditorGUILayout.GetControlRect(false, 48);
        EditorGUI.DrawRect(r, new Color(0.05f, 0.08f, 0.10f));

        GUIStyle title = new GUIStyle(EditorStyles.boldLabel);
        title.fontSize = 20;
        title.alignment = TextAnchor.MiddleCenter;
        title.normal.textColor = accent;

        GUI.Label(r, "Blueys Texture", title);

        Rect line = new Rect(r.x, r.yMax - 3, r.width, 3);
        EditorGUI.DrawRect(line, accent);

        EditorGUILayout.Space(8);
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

    bool DrawHeaderStrip(bool open, string title, bool hasToggle, bool enabled, MaterialProperty toggle)
    {
        Rect r = EditorGUILayout.GetControlRect(false, 26);
        EditorGUI.DrawRect(r, hasToggle && enabled ? headerOn : headerOff);

        Rect arrowRect = new Rect(r.x + 7, r.y + 4, 18, 18);
        open = EditorGUI.Foldout(arrowRect, open, GUIContent.none, true);

        Rect titleRect = new Rect(r.x + 26, r.y + 4, r.width - 120, 18);

        GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel);
        titleStyle.normal.textColor = hasToggle && enabled ? accent : new Color(0.82f, 0.82f, 0.82f);

        GUI.Label(titleRect, title, titleStyle);

        if (hasToggle && toggle != null)
        {
            Rect toggleRect = new Rect(r.xMax - 74, r.y + 4, 18, 18);
            bool newEnabled = EditorGUI.Toggle(toggleRect, enabled);

            if (newEnabled != enabled)
            {
                toggle.floatValue = newEnabled ? 1f : 0f;
            }

            Rect statusRect = new Rect(r.xMax - 52, r.y + 4, 45, 18);

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
        box.padding = new RectOffset(10, 10, 8, 8);
        box.margin = new RectOffset(0, 0, 0, 0);

        Color old = GUI.backgroundColor;
        GUI.backgroundColor = body;
        EditorGUILayout.BeginVertical(box);
        GUI.backgroundColor = old;
    }

    void DrawProp(MaterialEditor editor, MaterialProperty[] props, string name)
    {
        MaterialProperty prop = FindProperty(name, props, false);

        if (prop != null)
        {
            editor.ShaderProperty(prop, prop.displayName);
        }
    }

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
}