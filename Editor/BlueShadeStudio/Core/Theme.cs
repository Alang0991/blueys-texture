using UnityEditor;
using UnityEngine;

namespace BlueShadeStudio.Core
{
    public static class Theme
    {
        // ========================================
        // Colours — Refined dark blue-grey palette
        // ========================================
        public static readonly Color Accent = new Color(0.22f, 0.78f, 1f);
        public static readonly Color AccentHover = new Color(0.30f, 0.85f, 1f);
        public static readonly Color AccentDark = new Color(0.12f, 0.42f, 0.58f);
        public static readonly Color AccentSoft = new Color(0.22f, 0.78f, 1f, 0.10f);
        public static readonly Color AccentMedium = new Color(0.22f, 0.78f, 1f, 0.25f);

        public static readonly Color HeaderOff = new Color(0.16f, 0.16f, 0.18f);
        public static readonly Color HeaderOn = new Color(0.12f, 0.20f, 0.26f);
        public static readonly Color HeaderHover = new Color(0.19f, 0.19f, 0.22f);

        public static readonly Color Body = new Color(0.12f, 0.12f, 0.14f);
        public static readonly Color BodyLight = new Color(0.15f, 0.15f, 0.17f);
        public static readonly Color BodyLighter = new Color(0.18f, 0.18f, 0.20f);
        public static readonly Color BannerBg = new Color(0.05f, 0.08f, 0.11f);
        public static readonly Color SidebarBg = new Color(0.10f, 0.10f, 0.12f);
        public static readonly Color SidebarActive = new Color(0.18f, 0.30f, 0.36f);
        public static readonly Color SearchBg = new Color(0.09f, 0.09f, 0.11f);

        public static readonly Color Divider = new Color(0.26f, 0.26f, 0.28f);
        public static readonly Color DividerLight = new Color(0.20f, 0.20f, 0.22f);
        public static readonly Color Border = new Color(0.20f, 0.20f, 0.23f);
        public static readonly Color BorderLight = new Color(0.30f, 0.30f, 0.33f);

        public static readonly Color TextPrimary = new Color(0.96f, 0.96f, 0.98f);
        public static readonly Color TextSecondary = new Color(0.60f, 0.60f, 0.63f);
        public static readonly Color TextMuted = new Color(0.42f, 0.42f, 0.45f);
        public static readonly Color TextDisabled = new Color(0.35f, 0.35f, 0.38f);

        public static readonly Color Success = new Color(0.28f, 0.90f, 0.45f);
        public static readonly Color SuccessBg = new Color(0.28f, 0.90f, 0.45f, 0.12f);
        public static readonly Color Warning = new Color(1f, 0.72f, 0.20f);
        public static readonly Color WarningBg = new Color(1f, 0.72f, 0.20f, 0.12f);
        public static readonly Color Error = new Color(0.95f, 0.30f, 0.30f);
        public static readonly Color ErrorBg = new Color(0.95f, 0.30f, 0.30f, 0.12f);
        public static readonly Color Info = new Color(0.28f, 0.70f, 0.95f);
        public static readonly Color InfoBg = new Color(0.28f, 0.70f, 0.95f, 0.12f);

        public static readonly Color TagBg = new Color(0.20f, 0.20f, 0.22f, 0.85f);
        public static readonly Color TagText = new Color(0.70f, 0.70f, 0.73f);
        public static readonly Color TexturePreviewBg = new Color(0.08f, 0.08f, 0.10f);

        // ========================================
        // Layout constants
        // ========================================
        public const float SidebarWidth = 120f;
        public const float TabHeight = 36f;
        public const float SectionHeaderHeight = 32f;
        public const float PropertySpacing = 5f;
        public const float SectionSpacing = 8f;
        public const float BannerHeight = 82f;
        public const float SearchHeight = 28f;
        public const float ButtonHeight = 26f;
        public const float SmallButtonHeight = 22f;
        public const float ToggleWidth = 24f;
        public const float BadgeWidth = 48f;
        public const float IconSize = 16f;
        public const float IndentSpace = 28f;

        // ========================================
        // Cached GUIStyles (created once, reused)
        // ========================================
        public static readonly GUIStyle TitleStyle;
        public static readonly GUIStyle SubtitleStyle;
        public static readonly GUIStyle TabStyle;
        public static readonly GUIStyle TabActiveStyle;
        public static readonly GUIStyle SearchStyle;
        public static readonly GUIStyle SectionHeaderStyle;
        public static readonly GUIStyle SectionHeaderEnabledStyle;
        public static readonly GUIStyle PropertyLabelStyle;
        public static readonly GUIStyle DisabledLabelStyle;
        public static readonly GUIStyle BoxStyle;
        public static readonly GUIStyle ButtonStyle;
        public static readonly GUIStyle ButtonHoverStyle;
        public static readonly GUIStyle SmallButtonStyle;
        public static readonly GUIStyle ToggleStyle;
        public static readonly GUIStyle BadgeStyle;
        public static readonly GUIStyle SubLabelStyle;
        public static readonly GUIStyle HelpBoxStyle;
        public static readonly GUIStyle CenterLabelStyle;
        public static readonly GUIStyle RatingStyle;
        public static readonly GUIStyle TexturePreviewStyle;
        public static readonly GUIStyle TagStyle;
        public static readonly GUIStyle RichTextStyle;

        // Icon cache
        private static GUIContent _searchIcon;
        private static GUIContent _clearIcon;
        private static GUIContent _infoIcon;
        private static GUIContent _warningIcon;
        private static GUIContent _errorIcon;
        private static GUIContent _textureIcon;
        private static GUIContent _circleIcon;

        static Theme()
        {
            // Title
            TitleStyle = new GUIStyle(EditorStyles.boldLabel);
            TitleStyle.fontSize = 22;
            TitleStyle.alignment = TextAnchor.MiddleCenter;
            TitleStyle.normal.textColor = Accent;
            TitleStyle.fontStyle = FontStyle.Bold;
            TitleStyle.margin = new RectOffset(0, 0, 2, 0);

            // Subtitle
            SubtitleStyle = new GUIStyle(EditorStyles.miniLabel);
            SubtitleStyle.alignment = TextAnchor.MiddleCenter;
            SubtitleStyle.normal.textColor = TextSecondary;
            SubtitleStyle.fontSize = 10;
            SubtitleStyle.margin = new RectOffset(0, 0, 0, 4);

            // Tab (inactive)
            TabStyle = new GUIStyle(EditorStyles.boldLabel);
            TabStyle.alignment = TextAnchor.MiddleCenter;
            TabStyle.fontSize = 11;
            TabStyle.normal.textColor = TextSecondary;
            TabStyle.fontStyle = FontStyle.Normal;
            TabStyle.padding = new RectOffset(8, 8, 5, 5);
            TabStyle.margin = new RectOffset(2, 2, 2, 2);

            // Tab (active)
            TabActiveStyle = new GUIStyle(TabStyle);
            TabActiveStyle.fontStyle = FontStyle.Bold;
            TabActiveStyle.normal.textColor = Accent;

            // Search
            SearchStyle = new GUIStyle(EditorStyles.toolbarSearchField);
            SearchStyle.fontSize = 11;
            SearchStyle.alignment = TextAnchor.MiddleLeft;
            SearchStyle.padding = new RectOffset(26, 26, 4, 4);
            SearchStyle.normal.textColor = TextPrimary;
            SearchStyle.focused = SearchStyle.normal;
            SearchStyle.margin = new RectOffset(0, 0, 0, 0);

            // Section header (disabled)
            SectionHeaderStyle = new GUIStyle(EditorStyles.boldLabel);
            SectionHeaderStyle.fontSize = 12;
            SectionHeaderStyle.normal.textColor = TextSecondary;
            SectionHeaderStyle.fontStyle = FontStyle.Normal;
            SectionHeaderStyle.alignment = TextAnchor.MiddleLeft;
            SectionHeaderStyle.padding = new RectOffset(8, 8, 4, 4);
            SectionHeaderStyle.margin = new RectOffset(0, 0, 0, 0);

            // Section header (enabled)
            SectionHeaderEnabledStyle = new GUIStyle(SectionHeaderStyle);
            SectionHeaderEnabledStyle.fontStyle = FontStyle.Bold;
            SectionHeaderEnabledStyle.normal.textColor = Accent;

            // Property label
            PropertyLabelStyle = new GUIStyle(EditorStyles.label);
            PropertyLabelStyle.fontSize = 11;
            PropertyLabelStyle.normal.textColor = TextPrimary;
            PropertyLabelStyle.wordWrap = true;
            PropertyLabelStyle.margin = new RectOffset(0, 0, 2, 2);

            // Disabled label
            DisabledLabelStyle = new GUIStyle(EditorStyles.miniLabel);
            DisabledLabelStyle.normal.textColor = TextDisabled;
            DisabledLabelStyle.fontSize = 10;
            DisabledLabelStyle.alignment = TextAnchor.MiddleLeft;
            DisabledLabelStyle.wordWrap = true;
            DisabledLabelStyle.margin = new RectOffset(0, 0, 2, 2);

            // Box
            BoxStyle = new GUIStyle("box");
            BoxStyle.padding = new RectOffset(12, 12, 10, 10);
            BoxStyle.margin = new RectOffset(0, 0, 0, 0);
            BoxStyle.border = new RectOffset(1, 1, 1, 1);

            // Button
            ButtonStyle = new GUIStyle(GUI.skin.button);
            ButtonStyle.fontSize = 11;
            ButtonStyle.padding = new RectOffset(14, 14, 7, 7);
            ButtonStyle.margin = new RectOffset(3, 3, 3, 3);
            ButtonStyle.alignment = TextAnchor.MiddleCenter;
            ButtonStyle.fontStyle = FontStyle.Bold;
            ButtonStyle.normal.textColor = TextPrimary;
            ButtonStyle.hover.textColor = Accent;
            ButtonStyle.active.textColor = Accent;
            ButtonStyle.focused.textColor = Accent;
            ButtonStyle.border = new RectOffset(8, 8, 4, 4);

            // Button hover (for custom drawing)
            ButtonHoverStyle = new GUIStyle(ButtonStyle);
            ButtonHoverStyle.normal.textColor = Accent;

            // Small button
            SmallButtonStyle = new GUIStyle(ButtonStyle);
            SmallButtonStyle.fontSize = 10;
            SmallButtonStyle.padding = new RectOffset(8, 8, 3, 3);
            SmallButtonStyle.margin = new RectOffset(2, 2, 2, 2);
            SmallButtonStyle.border = new RectOffset(6, 6, 3, 3);

            // Toggle
            ToggleStyle = new GUIStyle(EditorStyles.toggle);
            ToggleStyle.fontSize = 11;
            ToggleStyle.normal.textColor = TextPrimary;
            ToggleStyle.onNormal.textColor = Accent;
            ToggleStyle.onHover.textColor = Accent;
            ToggleStyle.hover.textColor = Accent;
            ToggleStyle.margin = new RectOffset(0, 0, 2, 2);

            // Badge
            BadgeStyle = new GUIStyle(EditorStyles.miniBoldLabel);
            BadgeStyle.alignment = TextAnchor.MiddleCenter;
            BadgeStyle.fontSize = 9;
            BadgeStyle.fontStyle = FontStyle.Bold;
            BadgeStyle.padding = new RectOffset(4, 4, 2, 2);

            // Sub-label
            SubLabelStyle = new GUIStyle(EditorStyles.boldLabel);
            SubLabelStyle.fontSize = 10;
            SubLabelStyle.fontStyle = FontStyle.Bold;
            SubLabelStyle.normal.textColor = TextSecondary;
            SubLabelStyle.alignment = TextAnchor.UpperLeft;
            SubLabelStyle.margin = new RectOffset(0, 0, 4, 2);

            // Help box
            HelpBoxStyle = new GUIStyle(EditorStyles.helpBox);
            HelpBoxStyle.alignment = TextAnchor.UpperLeft;
            HelpBoxStyle.margin = new RectOffset(0, 0, 0, 0);
            HelpBoxStyle.padding = new RectOffset(8, 8, 6, 6);

            // Center label
            CenterLabelStyle = new GUIStyle(EditorStyles.label);
            CenterLabelStyle.alignment = TextAnchor.MiddleCenter;

            // Rating badge
            RatingStyle = new GUIStyle(EditorStyles.boldLabel);
            RatingStyle.alignment = TextAnchor.MiddleCenter;
            RatingStyle.fontSize = 11;
            RatingStyle.fontStyle = FontStyle.Bold;
            RatingStyle.padding = new RectOffset(4, 4, 2, 2);

            // Texture preview
            TexturePreviewStyle = new GUIStyle("box");
            TexturePreviewStyle.border = new RectOffset(1, 1, 1, 1);
            TexturePreviewStyle.margin = new RectOffset(0, 0, 0, 0);
            TexturePreviewStyle.padding = new RectOffset(2, 2, 2, 2);

            // Tag
            TagStyle = new GUIStyle(EditorStyles.miniBoldLabel);
            TagStyle.alignment = TextAnchor.MiddleCenter;
            TagStyle.fontSize = 8;
            TagStyle.fontStyle = FontStyle.Bold;
            TagStyle.normal.textColor = TagText;
            TagStyle.padding = new RectOffset(3, 3, 1, 1);

            // Rich text
            RichTextStyle = new GUIStyle(EditorStyles.label);
            RichTextStyle.richText = true;
            RichTextStyle.wordWrap = true;
            RichTextStyle.fontSize = 11;

            // Icons
            _searchIcon = EditorGUIUtility.IconContent("d_searchfield") ?? EditorGUIUtility.IconContent("Search");
            _clearIcon = EditorGUIUtility.IconContent("d_search_box_close") ?? EditorGUIUtility.IconContent("CrossReference");
            _infoIcon = EditorGUIUtility.IconContent("d_console.infoicon.sml");
            _warningIcon = EditorGUIUtility.IconContent("d_console.warnicon.sml");
            _errorIcon = EditorGUIUtility.IconContent("d_console.erroricon.sml");
            _textureIcon = EditorGUIUtility.IconContent("d_texturethumb");
            _circleIcon = EditorGUIUtility.IconContent("d_circle");
        }

        // ========================================
        // Style getter methods (backward compatibility)
        // ========================================
        public static GUIStyle GetTitleStyle(float fontSize = 22)
        {
            GUIStyle style = new GUIStyle(TitleStyle);
            style.fontSize = (int)fontSize;
            return style;
        }

        public static GUIStyle GetSubtitleStyle() => SubtitleStyle;
        public static GUIStyle GetTabStyle(bool active) => active ? TabActiveStyle : TabStyle;
        public static GUIStyle GetSearchStyle() => SearchStyle;
        public static GUIStyle GetSectionHeaderStyle(bool enabled) => enabled ? SectionHeaderEnabledStyle : SectionHeaderStyle;
        public static GUIStyle GetPropertyLabelStyle() => PropertyLabelStyle;
        public static GUIStyle GetDisabledLabelStyle() => DisabledLabelStyle;
        public static GUIStyle GetBoxStyle() => BoxStyle;
        public static GUIStyle GetButtonStyle() => ButtonStyle;
        public static GUIStyle GetSmallButtonStyle() => SmallButtonStyle;
        public static GUIStyle GetBadgeStyle() => BadgeStyle;
        public static GUIStyle GetSubLabelStyle() => SubLabelStyle;
        public static GUIStyle GetCenterLabelStyle() => CenterLabelStyle;
        public static GUIStyle GetTexturePreviewStyle() => TexturePreviewStyle;
        public static GUIStyle GetTagStyle() => TagStyle;
        public static GUIStyle GetRichTextStyle() => RichTextStyle;

        // Icon getters
        public static GUIContent SearchIcon => _searchIcon;
        public static GUIContent ClearIcon => _clearIcon;
        public static GUIContent InfoIcon => _infoIcon;
        public static GUIContent WarningIcon => _warningIcon;
        public static GUIContent ErrorIcon => _errorIcon;
        public static GUIContent TextureIcon => _textureIcon;
        public static GUIContent CircleIcon => _circleIcon;
    }
}
