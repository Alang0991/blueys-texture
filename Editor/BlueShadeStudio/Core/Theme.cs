using UnityEditor;
using UnityEngine;

namespace BlueShadeStudio.Core
{
    public static class Theme
    {
        public static readonly Color Accent = new Color(0.25f, 0.75f, 1f);
        public static readonly Color AccentDark = new Color(0.15f, 0.45f, 0.6f);
        public static readonly Color HeaderOff = new Color(0.16f, 0.16f, 0.16f);
        public static readonly Color HeaderOn = new Color(0.12f, 0.22f, 0.26f);
        public static readonly Color Body = new Color(0.13f, 0.13f, 0.13f);
        public static readonly Color BannerBg = new Color(0.04f, 0.07f, 0.09f);
        public static readonly Color SidebarBg = new Color(0.10f, 0.10f, 0.10f);
        public static readonly Color SidebarActive = new Color(0.18f, 0.28f, 0.32f);
        public static readonly Color SearchBg = new Color(0.08f, 0.08f, 0.08f);
        public static readonly Color Divider = new Color(0.3f, 0.3f, 0.3f);
        public static readonly Color TextPrimary = new Color(0.95f, 0.95f, 0.95f);
        public static readonly Color TextSecondary = new Color(0.6f, 0.6f, 0.6f);
        public static readonly Color Success = new Color(0.3f, 0.9f, 0.4f);
        public static readonly Color Warning = new Color(1f, 0.7f, 0.2f);
        public static readonly Color Error = new Color(0.9f, 0.3f, 0.3f);
        public static readonly Color Info = new Color(0.3f, 0.6f, 0.9f);

        public static readonly float SidebarWidth = 100f;
        public static readonly float TabHeight = 32f;
        public static readonly float SectionHeaderHeight = 28f;
        public static readonly float PropertySpacing = 4f;
        public static readonly float SectionSpacing = 6f;
        public static readonly float BannerHeight = 72f;

        public static GUIStyle GetTitleStyle(float fontSize = 20)
        {
            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
            style.fontSize = (int)fontSize;
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Accent;
            style.fontStyle = FontStyle.Bold;
            return style;
        }

        public static GUIStyle GetSubtitleStyle()
        {
            GUIStyle style = new GUIStyle(EditorStyles.miniLabel);
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = TextSecondary;
            style.fontSize = 10;
            return style;
        }

        public static GUIStyle GetTabStyle(bool active)
        {
            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 11;
            style.normal.textColor = active ? Accent : TextSecondary;
            style.fontStyle = active ? FontStyle.Bold : FontStyle.Normal;
            return style;
        }

        public static GUIStyle GetSearchStyle()
        {
            GUIStyle style = new GUIStyle(EditorStyles.toolbarSearchField);
            style.fontSize = 11;
            style.alignment = TextAnchor.MiddleLeft;
            style.padding = new RectOffset(8, 8, 4, 4);
            return style;
        }

        public static GUIStyle GetSectionHeaderStyle(bool enabled)
        {
            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
            style.fontSize = 12;
            style.normal.textColor = enabled ? TextPrimary : TextSecondary;
            style.fontStyle = enabled ? FontStyle.Bold : FontStyle.Normal;
            return style;
        }

        public static GUIStyle GetPropertyLabelStyle()
        {
            GUIStyle style = new GUIStyle(EditorStyles.label);
            style.fontSize = 11;
            style.normal.textColor = TextPrimary;
            return style;
        }

        public static GUIStyle GetDisabledLabelStyle()
        {
            GUIStyle style = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
            style.normal.textColor = TextSecondary;
            style.fontSize = 10;
            return style;
        }

        public static GUIStyle GetBoxStyle()
        {
            GUIStyle box = new GUIStyle("box");
            box.padding = new RectOffset(12, 12, 10, 10);
            box.margin = new RectOffset(0, 0, 0, 0);
            return box;
        }

        public static GUIStyle GetButtonStyle()
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.fontSize = 11;
            style.padding = new RectOffset(8, 8, 6, 6);
            style.margin = new RectOffset(2, 2, 2, 2);
            return style;
        }
    }
}
