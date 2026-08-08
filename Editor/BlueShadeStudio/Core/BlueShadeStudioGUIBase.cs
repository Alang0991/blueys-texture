using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using BlueShadeStudio.Core;
using BlueShadeStudio.Modules;

public abstract class BlueShadeStudioGUIBase : ShaderGUI
{
    protected int tabIndex = 0;
    protected string searchQuery = "";
    protected Vector2 scrollPos;

    protected Material cachedMat;
    protected List<BaseModule> modules;

    protected MaterialEditor cachedEditor;
    protected MaterialProperty[] cachedProps;

    private static readonly Color SidebarHover = new Color(0.14f, 0.14f, 0.16f);
    private int hoveredTab = -1;
    private bool wasScrollActive;

    // ========================================
    // Abstract members subclasses must implement
    // ========================================
    protected abstract string ShaderTitle { get; }
    protected abstract string ShaderSubtitle { get; }
    protected abstract string[] TabNames { get; }
    protected virtual string[] TabIcons => null;
    protected abstract List<BaseModule> CreateModules();

    // ========================================
    // Main entry point
    // ========================================
    public override void OnGUI(MaterialEditor editor, MaterialProperty[] props)
    {
        Material mat = editor.target as Material;
        EnsureModules(mat, editor, props);

        DrawBanner();
        DrawSidebar();
        DrawSearchBar();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        if (!string.IsNullOrEmpty(searchQuery))
        {
            DrawSearchResults(mat);
        }
        else
        {
            DrawActiveTab();
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space(6);
        DrawUtilityButtons();
        SaveSectionStatesIfNeeded(mat);
    }

    // ========================================
    // Section state persistence (only writes when a foldout changed)
    // ========================================
    void SaveSectionStatesIfNeeded(Material mat)
    {
        bool needSave = false;
        foreach (var module in modules)
        {
            if (module.HasUnsavedState) { needSave = true; break; }
        }
        if (!needSave) return;

        SaveSectionStates(mat);
        foreach (var module in modules)
        {
            module.ClearUnsavedState();
        }
    }

    // ========================================
    // Module management
    // ========================================
    void EnsureModules(Material mat, MaterialEditor editor, MaterialProperty[] props)
    {
        if (cachedMat != mat || modules == null)
        {
            BlueShadeStudioUtils.SyncMaterialKeywords(mat);

            modules = CreateModules();
            foreach (var module in modules)
            {
                module.Initialize(mat, editor, props);
            }
            cachedMat = mat;
            cachedEditor = editor;
            cachedProps = props;
            scrollPos = Vector2.zero;
            LoadSectionStates(mat);
        }
        else if (cachedEditor != editor || cachedProps != props)
        {
            cachedEditor = editor;
            cachedProps = props;
            foreach (var module in modules)
            {
                module.UpdateContext(editor, props);
            }
        }
    }

    // ========================================
    // Banner
    // ========================================
    void DrawBanner()
    {
        Rect r = EditorGUILayout.GetControlRect(false, Theme.BannerHeight);
        EditorGUI.DrawRect(r, Theme.BannerBg);

        // Subtle top accent line
        EditorGUI.DrawRect(new Rect(r.x, r.y, r.width, 2), Theme.Accent);

        // Title
        GUI.Label(r, ShaderTitle, Theme.TitleStyle);

        // Subtitle bar
        Rect subtitleRect = new Rect(r.x, r.yMax - 22, r.width, 18);
        EditorGUI.DrawRect(subtitleRect, Theme.Body);
        GUI.Label(subtitleRect, ShaderSubtitle, Theme.SubtitleStyle);

        EditorGUILayout.Space(8);
    }

    // ========================================
    // Sidebar tabs
    // ========================================
    void DrawSidebar()
    {
        string[] tabs = TabNames;
        string[] icons = TabIcons;
        EditorGUILayout.BeginHorizontal();

        for (int i = 0; i < tabs.Length; i++)
        {
            bool active = tabIndex == i;
            Rect tabRect = EditorGUILayout.GetControlRect(GUILayout.Height(Theme.TabHeight), GUILayout.Width(Theme.SidebarWidth));

            bool hovered = Event.current.type == EventType.MouseMove && tabRect.Contains(Event.current.mousePosition);

            // Background
            if (active)
            {
                EditorGUI.DrawRect(tabRect, Theme.SidebarActive);
                EditorGUI.DrawRect(new Rect(tabRect.x, tabRect.yMax - 2, tabRect.width, 2), Theme.Accent);
            }
            else if (hovered)
            {
                EditorGUI.DrawRect(tabRect, SidebarHover);
            }
            else
            {
                EditorGUI.DrawRect(tabRect, Theme.SidebarBg);
            }

            // Icon
            if (icons != null && i < icons.Length && !string.IsNullOrEmpty(icons[i]))
            {
                Texture iconTex = EditorGUIUtility.IconContent(icons[i])?.image;
                if (iconTex != null)
                {
                    Rect iconRect = new Rect(tabRect.x + 8, tabRect.y + 6, 16, 16);
                    GUI.DrawTexture(iconRect, iconTex, ScaleMode.ScaleToFit, false);
                }
            }

            // Label
            Rect labelRect = new Rect(tabRect.x + 28, tabRect.y + 6, tabRect.width - 32, 20);
            Color oldColor = GUI.color;
            GUI.color = active ? Theme.Accent : (hovered ? Theme.TextSecondary : Theme.TextMuted);
            FontStyle oldStyle = Theme.TabStyle.fontStyle;
            Theme.TabStyle.fontStyle = active ? FontStyle.Bold : FontStyle.Normal;
            GUI.Label(labelRect, tabs[i], active ? Theme.TabActiveStyle : Theme.TabStyle);
            Theme.TabStyle.fontStyle = oldStyle;
            GUI.color = oldColor;

            // Click handling
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && tabRect.Contains(Event.current.mousePosition))
            {
                tabIndex = i;
                Event.current.Use();
            }
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(6);
    }

    // ========================================
    // Search bar
    // ========================================
    void DrawSearchBar()
    {
        Rect r = EditorGUILayout.GetControlRect(false, Theme.SearchHeight + 4);
        r.y += 2;
        r.height = Theme.SearchHeight;

        EditorGUI.DrawRect(r, Theme.SearchBg);
        EditorGUI.DrawRect(new Rect(r.x, r.y, 2, r.height), Theme.Accent);
        EditorGUI.DrawRect(new Rect(r.xMax - 1, r.y, 1, r.height), Theme.Border);

        // Search icon
        Rect iconRect = new Rect(r.x + 6, r.y + 5, 16, 16);
        if (Theme.SearchIcon != null && Theme.SearchIcon.image != null)
            GUI.DrawTexture(iconRect, Theme.SearchIcon.image, ScaleMode.ScaleToFit);

        Rect textRect = new Rect(r.x + 26, r.y + 2, r.width - 50, r.height - 4);
        searchQuery = EditorGUI.TextField(textRect, searchQuery, Theme.SearchStyle);

        if (!string.IsNullOrEmpty(searchQuery))
        {
            Rect clearRect = new Rect(r.xMax - 20, r.y + 5, 16, 16);
            if (GUI.Button(clearRect, Theme.ClearIcon, EditorStyles.miniButton))
            {
                searchQuery = "";
                GUI.FocusControl(null);
            }
        }

        EditorGUILayout.Space(6);
    }

    // ========================================
    // Tab content
    // ========================================
    void DrawActiveTab()
    {
        if (modules != null && tabIndex < modules.Count)
        {
            modules[tabIndex]?.Draw();
        }
    }

    // ========================================
    // Search results
    // ========================================
    void DrawSearchResults(Material mat)
    {
        Rect headerRect = EditorGUILayout.GetControlRect(false, Theme.SectionHeaderHeight);
        EditorGUI.DrawRect(headerRect, Theme.HeaderOn);

        GUI.Label(new Rect(headerRect.x + 28, headerRect.y + 5, headerRect.width - 32, 18),
            $"Results for \"{searchQuery}\"", Theme.SectionHeaderEnabledStyle);

        EditorGUILayout.Space(4);

        bool anyResults = false;
        foreach (var module in modules)
        {
            if (module.HasSearchResults(searchQuery))
            {
                anyResults = true;
                module.DrawSearchResults(searchQuery);
            }
        }

        if (!anyResults)
        {
            EditorGUILayout.Space(12);
            Rect helpRect = EditorGUILayout.GetControlRect(false, 40);
            EditorGUI.HelpBox(helpRect, "No properties match your search. Try using broader terms or check the spelling.", MessageType.Info);
        }

        EditorGUILayout.Space(Theme.SectionSpacing);
    }

    // ========================================
    // Utility buttons
    // ========================================
    void DrawUtilityButtons()
    {
        EditorGUILayout.BeginHorizontal();

        int optTab = TabNames.Length - 1;

        if (GUILayout.Button("Validate Material", Theme.ButtonStyle, GUILayout.Height(Theme.SmallButtonHeight)))
        {
            tabIndex = optTab;
        }

        if (GUILayout.Button("Texture Info", Theme.ButtonStyle, GUILayout.Height(Theme.SmallButtonHeight)))
        {
            tabIndex = optTab;
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Reset All", Theme.ButtonStyle, GUILayout.Height(Theme.SmallButtonHeight)))
        {
            if (EditorUtility.DisplayDialog("Reset Material", "Reset all material properties to their defaults?", "Yes", "Cancel"))
            {
                Undo.RecordObject(cachedMat, "Reset Material");
                foreach (var module in modules)
                    module.ResetValues();
                BlueShadeStudioUtils.SyncMaterialKeywords(cachedMat);
                EditorUtility.SetDirty(cachedMat);
            }
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(4);
    }

    // ========================================
    // Section state persistence
    // ========================================
    void LoadSectionStates(Material mat)
    {
        string prefix = mat.shader.name + "_" + mat.GetInstanceID() + "_";
        foreach (var module in modules)
        {
            module.LoadSectionStates(prefix);
            module.ClearUnsavedState();
        }
    }

    void SaveSectionStates(Material mat)
    {
        string prefix = mat.shader.name + "_" + mat.GetInstanceID() + "_";
        foreach (var module in modules)
        {
            module.SaveSectionStates(prefix);
        }
    }
}
