using UnityEditor;
using UnityEngine;

namespace BlueShadeStudio.Core
{
    public static class State
    {
        public static bool GetBool(string key, bool defaultValue = false)
        {
            return EditorPrefs.GetBool(key, defaultValue);
        }

        public static void SetBool(string key, bool value)
        {
            EditorPrefs.SetBool(key, value);
        }

        public static float GetFloat(string key, float defaultValue = 0f)
        {
            return EditorPrefs.GetFloat(key, defaultValue);
        }

        public static void SetFloat(string key, float value)
        {
            EditorPrefs.SetFloat(key, value);
        }

        public static int GetInt(string key, int defaultValue = 0)
        {
            return EditorPrefs.GetInt(key, defaultValue);
        }

        public static void SetInt(string key, int value)
        {
            EditorPrefs.SetInt(key, value);
        }

        public static string GetString(string key, string defaultValue = "")
        {
            return EditorPrefs.GetString(key, defaultValue);
        }

        public static void SetString(string key, string value)
        {
            EditorPrefs.SetString(key, value);
        }

        public static bool HasKey(string key)
        {
            return EditorPrefs.HasKey(key);
        }

        public static void DeleteKey(string key)
        {
            EditorPrefs.DeleteKey(key);
        }

        public static void DeleteKeysStartingWith(string prefix)
        {
            if (string.IsNullOrEmpty(prefix)) return;
            var keys = new System.Collections.Generic.List<string>();
            foreach (string key in EditorPrefs.GetString("BlueShadeStudio_Keys", "").Split(','))
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith(prefix))
                {
                    keys.Add(key);
                }
            }
            foreach (string key in keys)
            {
                EditorPrefs.DeleteKey(key);
            }
        }

        public static void RegisterKey(string key)
        {
            string existing = EditorPrefs.GetString("BlueShadeStudio_Keys", "");
            if (!existing.Contains(key))
            {
                EditorPrefs.SetString("BlueShadeStudio_Keys", existing + (string.IsNullOrEmpty(existing) ? "" : ",") + key);
            }
        }
    }
}
