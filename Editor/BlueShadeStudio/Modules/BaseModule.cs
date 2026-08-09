using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using BlueShadeStudio.Core;

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

        private bool _unsavedState;
        private Dictionary<string, GUIContent> _contentCache;
        private Dictionary<string, Texture> _texturePreviewCache;

        public bool IsEnabled => isEnabled;
        public string ModuleName => moduleName;
        public int Order => order;
        public bool HasUnsavedState => _unsavedState;

        public virtual void Initialize(Material mat, MaterialEditor ed, MaterialProperty[] p)
        {
            material = mat;
            editor = ed;
            props = p;
            isEnabled = true;
        }

        public virtual void UpdateContext(MaterialEditor ed, MaterialProperty[] p)
        {
            editor = ed;
            props = p;
        }

        public void ClearUnsavedState()
        {
            _unsavedState = false;
        }

        protected void MarkStateDirty()
        {
            _unsavedState = true;
        }

        public virtual void Draw() { }

        public virtual void DrawSearchResults(string query)
        {
            if (string.IsNullOrEmpty(query)) return;

            var matching = new List<MaterialProperty>();
            foreach (string name in ManagedProperties)
            {
                MaterialProperty prop = FindProperty(name);
                if (prop == null) continue;
                if (Search.Matches(prop.displayName, query) || Search.Matches(name, query))
                {
                    matching.Add(prop);
                }
            }

            if (matching.Count > 0)
            {
                DrawBodyStart();
                EditorGUI.indentLevel++;
                foreach (var prop in matching)
                {
                    DrawProperty(prop);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
        }

        public virtual bool HasSearchResults(string query)
        {
            if (string.IsNullOrEmpty(query)) return false;
            foreach (string name in ManagedProperties)
            {
                MaterialProperty prop = FindProperty(name);
                if (prop == null) continue;
                if (Search.Matches(prop.displayName, query) || Search.Matches(name, query))
                    return true;
            }
            return false;
        }

        protected virtual string[] ManagedProperties => new string[0];

        public virtual void LoadSectionStates(string prefix) { }
        public virtual void SaveSectionStates(string prefix) { }
        public virtual void ResetValues() { }

        // ========================================
        // Drawing helpers
        // ========================================
        protected GUIContent GetContent(MaterialProperty prop)
        {
            if (prop == null) return GUIContent.none;
            if (_contentCache == null) _contentCache = new Dictionary<string, GUIContent>();
            if (!_contentCache.TryGetValue(prop.name, out GUIContent c))
            {
                string tip = GetTooltip(prop.name);
                c = new GUIContent(prop.displayName, tip);
                _contentCache[prop.name] = c;
            }
            return c;
        }

        protected GUIContent GetContent(string name)
        {
            MaterialProperty prop = FindProperty(name);
            return prop != null ? GetContent(prop) : new GUIContent(name, GetTooltip(name));
        }

        protected void ClearContentCache()
        {
            _contentCache?.Clear();
        }

        protected void DrawBodyStart()
        {
            Color old = GUI.backgroundColor;
            GUI.backgroundColor = Theme.BodyLight;
            EditorGUILayout.BeginVertical(Theme.BoxStyle);
            GUI.backgroundColor = old;
        }

        protected void DrawProp(string name)
        {
            MaterialProperty prop = FindProperty(name);
            if (prop != null)
            {
                DrawProperty(prop);
            }
        }

        protected void DrawProp(string name, string labelOverride)
        {
            MaterialProperty prop = FindProperty(name);
            if (prop != null)
            {
                string tip = GetTooltip(prop.name);
                var content = new GUIContent(labelOverride, tip);
                DrawProperty(prop, content);
            }
        }

        protected void DrawProperty(MaterialProperty prop, GUIContent content = null)
        {
            if (prop == null) return;
            if (content == null) content = GetContent(prop);

            if (prop.type == MaterialProperty.PropType.Texture)
            {
                DrawTextureProperty(prop, content);
            }
            else
            {
                editor.ShaderProperty(prop, content);
            }
        }

        protected void DrawTextureProperty(MaterialProperty prop, GUIContent content)
        {
            if (prop == null) return;

            EditorGUI.BeginChangeCheck();

            Texture oldTex = prop.textureValue;
            editor.TextureProperty(prop, content.text, false);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(material, content.text);
                EditorUtility.SetDirty(material);
                SceneView.RepaintAll();
                editor.Repaint();
            }
        }

        protected void DrawTexturePreview(Texture tex)
        {
            if (tex == null) return;

            EditorGUILayout.Space(2);
            Rect previewRect = EditorGUILayout.GetControlRect(false, 64);
            previewRect.x += Theme.IndentSpace;
            previewRect.width -= Theme.IndentSpace;

            // Preview background
            EditorGUI.DrawRect(previewRect, Theme.TexturePreviewBg);
            EditorGUI.DrawRect(new Rect(previewRect.x, previewRect.y, previewRect.width, 1), Theme.Border);
            EditorGUI.DrawRect(new Rect(previewRect.x, previewRect.yMax - 1, previewRect.width, 1), Theme.Border);

            // Draw texture
            if (tex != null)
            {
                Rect texRect = new Rect(previewRect.x + 4, previewRect.y + 4, previewRect.width - 8, previewRect.height - 8);
                GUI.DrawTexture(texRect, tex, ScaleMode.ScaleToFit, true);
            }

            // Texture info tag
            string info = tex.width + "x" + tex.height;
            Rect tagRect = new Rect(previewRect.xMax - 60, previewRect.y + 4, 56, 16);
            GUI.Box(tagRect, info, Theme.TagStyle);
        }

        protected MaterialProperty FindProperty(string name)
        {
            if (props == null) return null;
            foreach (var prop in props)
            {
                if (prop != null && prop.name == name)
                    return prop;
            }
            return null;
        }

        protected virtual string GetTooltip(string propName)
        {
            return string.Empty;
        }

        protected void DrawSectionHeader(ref bool open, string title, bool hasToggle = false, bool enabled = false, string toggleProperty = null)
        {
            Rect r = EditorGUILayout.GetControlRect(false, Theme.SectionHeaderHeight);
            EditorGUI.DrawRect(r, hasToggle && enabled ? Theme.HeaderOn : Theme.HeaderOff);

            // Subtle left accent for enabled sections
            if (hasToggle && enabled)
            {
                EditorGUI.DrawRect(new Rect(r.x, r.y, 3, r.height), Theme.Accent);
            }

            bool prevOpen = open;
            Rect foldoutRect = new Rect(r.x + 6, r.y + 4, 20, 20);
            open = EditorGUI.Foldout(foldoutRect, open, GUIContent.none, true);
            if (open != prevOpen) MarkStateDirty();

            Rect titleRect = new Rect(r.x + 28, r.y + 4, r.width - 130, 20);
            GUIStyle titleStyle = hasToggle && enabled
                ? Theme.SectionHeaderEnabledStyle
                : Theme.SectionHeaderStyle;
            GUI.Label(titleRect, title, titleStyle);

            if (hasToggle && !string.IsNullOrEmpty(toggleProperty))
            {
                MaterialProperty toggle = FindProperty(toggleProperty);
                if (toggle != null)
                {
                    Rect toggleRect = new Rect(r.xMax - 76, r.y + 3, 22, 20);

                    EditorGUI.BeginChangeCheck();
                    bool newEnabled = EditorGUI.Toggle(toggleRect, enabled, Theme.ToggleStyle);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(material, newEnabled ? "Enable " + title : "Disable " + title);
                        material.SetFloat(toggleProperty, newEnabled ? 1f : 0f);
                        string keyword = BlueShadeStudioUtils.TogglePropertyToKeyword(toggleProperty);
                        if (newEnabled)
                            material.EnableKeyword(keyword);
                        else
                            material.DisableKeyword(keyword);
                        EditorUtility.SetDirty(material);
                    }

                    GUIStyle badgeStyle = Theme.BadgeStyle;
                    Color oldCol = badgeStyle.normal.textColor;
                    badgeStyle.normal.textColor = newEnabled ? Theme.Success : Theme.TextMuted;
                    GUI.Label(new Rect(r.xMax - 50, r.y + 2, 50, 20), newEnabled ? "ON" : "OFF", badgeStyle);
                    badgeStyle.normal.textColor = oldCol;
                }
            }
        }

        protected void DrawSubHeader(string title)
        {
            EditorGUILayout.Space(2);
            EditorGUILayout.LabelField(title, Theme.SubLabelStyle);
        }

        protected void DrawDivider()
        {
            Rect r = EditorGUILayout.GetControlRect(false, 1);
            EditorGUI.DrawRect(r, Theme.Divider);
            EditorGUILayout.Space(2);
        }

        protected void DrawTextureWarning(string texPropName)
        {
            MaterialProperty texProp = FindProperty(texPropName);
            if (texProp == null) return;
            Texture tex = texProp.textureValue;
            if (tex == null) return;

            string warning = BlueShadeStudioUtils.GetTextureWarning(tex);
            if (!string.IsNullOrEmpty(warning))
            {
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
            }
        }

        protected void DrawSeparator()
        {
            EditorGUILayout.Space(Theme.PropertySpacing);
            Rect r = EditorGUILayout.GetControlRect(false, 1);
            EditorGUI.DrawRect(r, Theme.DividerLight);
            EditorGUILayout.Space(Theme.PropertySpacing);
        }
    }
}
