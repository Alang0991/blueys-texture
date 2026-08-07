using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using BlueShadeStudio.Core;
using BlueShadeStudio.Modules;

public class BlueShadeStudioLiteGUI : ShaderGUI
{
    private int tabIndex = 0;
    private readonly string[] tabs = { "Main", "Look", "Lighting", "Effects", "Presets" };

    private string searchQuery = "";
    private Vector2 scrollPos;

    private Material cachedMat;
    private List<BaseModule> modules;

    private readonly Color accent = Theme.Accent;
    private readonly Color headerOff = Theme.HeaderOff;
    private readonly Color headerOn = Theme.HeaderOn;
    private readonly Color body = Theme.Body;
    private readonly Color bannerBg = Theme.BannerBg;
    private readonly Color sidebarBg = Theme.SidebarBg;
    private readonly Color sidebarActive = Theme.SidebarActive;

    public override void OnGUI(MaterialEditor editor, MaterialProperty[] props)
    {
        Material mat = editor.target as Material;

        if (cachedMat != mat || modules == null)
        {
            InitializeModules(mat);
            cachedMat = mat;
        }

        DrawBanner();
        DrawSidebar();
        DrawSearchBar();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        if (!string.IsNullOrEmpty(searchQuery))
        {
            DrawSearchResults(editor, props, mat);
        }
        else
        {
            DrawActiveTab(editor, props, mat);
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space(4);
        DrawUtilityButtons(mat);

        SaveSectionStates(mat);
    }

    void InitializeModules(Material mat)
    {
        modules = new List<BaseModule>
        {
            new LiteMainModule(),
            new LiteLookModule(),
            new LiteLightingModule(),
            new LiteEffectsModule(),
            new PresetsModule()
        };

        foreach (var module in modules)
        {
            module.Initialize(mat, null, null);
        }

        LoadSectionStates(mat);
    }

    void DrawBanner()
    {
        Rect r = EditorGUILayout.GetControlRect(false, Theme.BannerHeight);
        EditorGUI.DrawRect(r, bannerBg);

        GUIStyle title = Theme.GetTitleStyle(20);
        GUI.Label(r, "BlueShade Studio Lite", title);

        Rect line = new Rect(r.x, r.yMax - 3, r.width, 3);
        EditorGUI.DrawRect(line, accent);

        GUIStyle ver = Theme.GetSubtitleStyle();
        GUI.Label(new Rect(r.x, r.yMax - 18, r.width, 18), "v1.2.0 | Lightweight VRChat Shader", ver);

        EditorGUILayout.Space(6);
    }

    void DrawSidebar()
    {
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < tabs.Length; i++)
        {
            bool active = tabIndex == i;
            Color bg = active ? sidebarActive : sidebarBg;
            Color textCol = active ? accent : Theme.TextSecondary;

            Rect tabRect = EditorGUILayout.GetControlRect(GUILayout.Height(Theme.TabHeight), GUILayout.Width(Theme.SidebarWidth));
            EditorGUI.DrawRect(tabRect, bg);

            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 10;
            style.normal.textColor = textCol;
            style.fontStyle = active ? FontStyle.Bold : FontStyle.Normal;
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

    void DrawSearchBar()
    {
        EditorGUILayout.BeginHorizontal();
        Rect searchRect = EditorGUILayout.GetControlRect(GUILayout.Height(24));
        searchRect.y += 2;
        searchRect.height = 20;

        EditorGUI.DrawRect(searchRect, Theme.SearchBg);
        searchQuery = EditorGUI.TextField(searchRect, searchQuery, Theme.GetSearchStyle());

        if (!string.IsNullOrEmpty(searchQuery))
        {
            Rect clearRect = new Rect(searchRect.xMax - 20, searchRect.y + 3, 16, 16);
            if (GUI.Button(clearRect, "×", EditorStyles.miniButton))
            {
                searchQuery = "";
                GUI.FocusControl(null);
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(4);
    }

    void DrawActiveTab(MaterialEditor editor, MaterialProperty[] props, Material mat)
    {
        switch (tabIndex)
        {
            case 0: DrawModule(modules[0]); break;
            case 1: DrawModule(modules[1]); break;
            case 2: DrawModule(modules[2]); break;
            case 3: DrawModule(modules[3]); break;
            case 4: DrawModule(modules[4]); break;
        }
    }

    void DrawModule(BaseModule module)
    {
        if (module != null)
        {
            module.Draw();
        }
    }

    void DrawSearchResults(MaterialEditor editor, MaterialProperty[] props, Material mat)
    {
        EditorGUILayout.BeginVertical();
        bool searchOpen = true;
        searchOpen = DrawHeaderStrip(searchOpen, "Search Results", false, false, null);

        if (searchOpen)
        {
            DrawBodyStart();
            EditorGUI.indentLevel++;

            foreach (var module in modules)
            {
                module.DrawSearchResults(searchQuery);
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(Theme.SectionSpacing);
    }

    void DrawUtilityButtons(Material mat)
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Validate", Theme.GetButtonStyle(), GUILayout.Height(22)))
        {
            tabIndex = 5;
        }
        if (GUILayout.Button("Textures", Theme.GetButtonStyle(), GUILayout.Height(22)))
        {
            tabIndex = 5;
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(4);
    }

    bool DrawHeaderStrip(bool open, string title, bool hasToggle, bool enabled, MaterialProperty toggle)
    {
        Rect r = EditorGUILayout.GetControlRect(false, Theme.SectionHeaderHeight);
        EditorGUI.DrawRect(r, hasToggle && enabled ? headerOn : headerOff);

        Rect arrowRect = new Rect(r.x + 8, r.y + 5, 18, 18);
        open = EditorGUI.Foldout(arrowRect, open, GUIContent.none, true);

        Rect titleRect = new Rect(r.x + 28, r.y + 5, r.width - 130, 18);
        GUIStyle titleStyle = Theme.GetSectionHeaderStyle(enabled);
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
            statusStyle.normal.textColor = newEnabled ? accent : Theme.TextSecondary;
            GUI.Label(statusRect, newEnabled ? "ON" : "OFF", statusStyle);
        }

        return open;
    }

    void DrawBodyStart()
    {
        GUIStyle box = Theme.GetBoxStyle();
        Color old = GUI.backgroundColor;
        GUI.backgroundColor = body;
        EditorGUILayout.BeginVertical(box);
        GUI.backgroundColor = old;
    }

    void LoadSectionStates(Material mat)
    {
        string key = "BlueShadeStudioLite_" + mat.GetInstanceID() + "_";
    }

    void SaveSectionStates(Material mat)
    {
        // Module states are saved within each module
    }
}
