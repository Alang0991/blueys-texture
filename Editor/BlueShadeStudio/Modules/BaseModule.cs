using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace BlueShadeStudio.Modules
{
    public abstract class BaseModule
    {
        protected Material material;
        protected MaterialEditor editor;
        protected MaterialProperty[] props;
        protected bool isEnabled = true;
        protected string moduleName;
        protected int order = 0;

        public bool IsEnabled => isEnabled;
        public string ModuleName => moduleName;
        public int Order => order;

        public virtual void Initialize(Material mat, MaterialEditor ed, MaterialProperty[] p)
        {
            material = mat;
            editor = ed;
            props = p;
            isEnabled = true;
        }

        public virtual void Draw() { }

        public virtual void DrawSearchResults(string query) { }

        public virtual bool HasSearchResults(string query)
        {
            if (string.IsNullOrEmpty(query)) return false;
            return true;
        }

        protected void DrawBodyStart()
        {
            GUIStyle box = BlueShadeStudio.Core.Theme.GetBoxStyle();
            Color old = GUI.backgroundColor;
            GUI.backgroundColor = BlueShadeStudio.Core.Theme.Body;
            EditorGUILayout.BeginVertical(box);
            GUI.backgroundColor = old;
        }

        protected void DrawProp(string name)
        {
            MaterialProperty prop = FindProperty(name, props, false);
            if (prop != null)
            {
                GUIContent content = new GUIContent(prop.displayName, GetTooltip(name));
                editor.ShaderProperty(prop, content);
            }
        }

        protected MaterialProperty FindProperty(string name)
        {
            return MaterialEditor.GetMaterialProperty(props, name);
        }

        protected virtual string GetTooltip(string propName)
        {
            return string.Empty;
        }

        protected void DrawSectionHeader(ref bool open, string title, bool hasToggle = false, bool enabled = false, string toggleProperty = null)
        {
            Rect r = EditorGUILayout.GetControlRect(false, BlueShadeStudio.Core.Theme.SectionHeaderHeight);
            EditorGUI.DrawRect(r, hasToggle && enabled ? BlueShadeStudio.Core.Theme.HeaderOn : BlueShadeStudio.Core.Theme.HeaderOff);

            Rect arrowRect = new Rect(r.x + 8, r.y + 5, 18, 18);
            open = EditorGUI.Foldout(arrowRect, open, GUIContent.none, true);

            Rect titleRect = new Rect(r.x + 28, r.y + 5, r.width - 130, 18);
            GUIStyle titleStyle = BlueShadeStudio.Core.Theme.GetSectionHeaderStyle(enabled);
            GUI.Label(titleRect, title, titleStyle);

            if (hasToggle && !string.IsNullOrEmpty(toggleProperty))
            {
                MaterialProperty toggle = FindProperty(toggleProperty);
                if (toggle != null)
                {
                    bool newEnabled = EditorGUI.Toggle(new Rect(r.xMax - 78, r.y + 5, 18, 18), toggle.floatValue > 0.5f);
                    if (newEnabled != (toggle.floatValue > 0.5f))
                    {
                        toggle.floatValue = newEnabled ? 1f : 0f;
                    }

                    GUIStyle statusStyle = new GUIStyle(EditorStyles.miniBoldLabel);
                    statusStyle.alignment = TextAnchor.MiddleRight;
                    statusStyle.normal.textColor = newEnabled ? BlueShadeStudio.Core.Theme.Accent : BlueShadeStudio.Core.Theme.TextSecondary;
                    GUI.Label(new Rect(r.xMax - 56, r.y + 5, 48, 18), newEnabled ? "ON" : "OFF", statusStyle);
                }
            }
        }
    }
}
