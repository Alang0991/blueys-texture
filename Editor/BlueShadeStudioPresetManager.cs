using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Text;

public static class BlueShadeStudioPresetManager
{
    private const string CustomPresetKey = "BlueShadeStudio_CustomPresets_";
    private const int MaxCustomPresets = 20;

    [Serializable]
    private class PresetData
    {
        public List<PresetEntry> entries = new List<PresetEntry>();
    }

    [Serializable]
    private class PresetEntry
    {
        public string name;
        public string type;
        public string value;
    }

    public static void ApplyPreset(Material mat, string presetName)
    {
        if (mat == null) return;

        if (BlueShadeStudioPresets.Presets.TryGetValue(presetName, out MaterialPreset preset))
        {
            ApplyPresetData(mat, preset.settings);
        }
        else
        {
            string customJson = EditorPrefs.GetString(CustomPresetKey + presetName, "");
            if (!string.IsNullOrEmpty(customJson))
            {
                Dictionary<string, object> settings = LoadPresetFromJson(customJson);
                ApplyPresetData(mat, settings);
            }
        }

        EditorUtility.SetDirty(mat);
    }

    public static void ApplyPresetData(Material mat, Dictionary<string, object> settings)
    {
        if (mat == null || settings == null) return;

        Texture mainTex = mat.GetTexture("_MainTex");
        Color mainColor = mat.GetColor("_Color");

        foreach (var kvp in settings)
        {
            if (!mat.HasProperty(kvp.Key)) continue;

            switch (kvp.Key)
            {
                case "_MainTex":
                case "_Color":
                    continue;
            }

            switch (kvp.Value)
            {
                case float f:
                    mat.SetFloat(kvp.Key, f);
                    break;
                case int i:
                    mat.SetFloat(kvp.Key, i);
                    break;
                case Color c:
                    mat.SetColor(kvp.Key, c);
                    break;
                case Vector4 v:
                    mat.SetVector(kvp.Key, v);
                    break;
                case Texture t:
                    mat.SetTexture(kvp.Key, t);
                    break;
                case null:
                    mat.SetTexture(kvp.Key, null);
                    break;
            }
        }

        if (mainTex != null)
            mat.SetTexture("_MainTex", mainTex);
        mat.SetColor("_Color", mainColor);

        BlueShadeStudioUtils.SyncMaterialKeywords(mat);
    }

    public static string[] GetBuiltinPresetNames()
    {
        List<string> names = new List<string>();
        foreach (var kvp in BlueShadeStudioPresets.Presets)
        {
            names.Add(kvp.Key);
        }
        return names.ToArray();
    }

    public static string[] GetAllPresetNames()
    {
        List<string> names = new List<string>(GetBuiltinPresetNames());

        for (int i = 0; i < MaxCustomPresets; i++)
        {
            string key = CustomPresetKey + i;
            if (EditorPrefs.HasKey(key))
            {
                string name = EditorPrefs.GetString(key + "_name", "Custom " + i);
                names.Add(name);
            }
        }

        return names.ToArray();
    }

    public static void SaveCustomPreset(Material mat, string presetName)
    {
        if (mat == null || string.IsNullOrEmpty(presetName)) return;

        Dictionary<string, object> settings = new Dictionary<string, object>();
        Shader shader = mat.shader;
        int propCount = shader.GetPropertyCount();

        for (int i = 0; i < propCount; i++)
        {
            string name = shader.GetPropertyName(i);
            UnityEngine.Rendering.ShaderPropertyType type = shader.GetPropertyType(name);

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

        string json = BlueShadeStudioUtils.SavePresetToJson(settings);

        int slot = FindEmptySlot();
        if (slot < 0)
        {
            slot = 0;
            for (int i = 0; i < MaxCustomPresets; i++)
            {
                EditorPrefs.DeleteKey(CustomPresetKey + i);
                EditorPrefs.DeleteKey(CustomPresetKey + i + "_name");
            }
        }

        EditorPrefs.SetString(CustomPresetKey + slot, json);
        EditorPrefs.SetString(CustomPresetKey + slot + "_name", presetName);
    }

    private static int FindEmptySlot()
    {
        for (int i = 0; i < MaxCustomPresets; i++)
        {
            if (!EditorPrefs.HasKey(CustomPresetKey + i)) return i;
        }
        return -1;
    }

    public static string[] GetCustomPresetNames()
    {
        List<string> names = new List<string>();
        for (int i = 0; i < MaxCustomPresets; i++)
        {
            if (EditorPrefs.HasKey(CustomPresetKey + i))
            {
                names.Add(EditorPrefs.GetString(CustomPresetKey + i + "_name", "Custom " + i));
            }
        }
        return names.ToArray();
    }

    public static void ResetMaterial(Material mat)
    {
        if (mat == null) return;

        Material defaultMat = new Material(mat.shader);
        mat.CopyPropertiesFromMaterial(defaultMat);

        // Keywords are not copied by CopyPropertiesFromMaterial, so re-sync them
        // from the (now default) toggle float values.
        BlueShadeStudioUtils.SyncMaterialKeywords(mat);

        Object.DestroyImmediate(defaultMat);
        EditorUtility.SetDirty(mat);
    }

    private static Dictionary<string, object> clipboard = null;

    public static void CopyMaterialSettings(Material mat)
    {
        if (mat == null) return;

        clipboard = new Dictionary<string, object>();
        Shader shader = mat.shader;
        int propCount = shader.GetPropertyCount();

        for (int i = 0; i < propCount; i++)
        {
            string name = shader.GetPropertyName(i);
            UnityEngine.Rendering.ShaderPropertyType type = shader.GetPropertyType(name);

            switch (type)
            {
                case UnityEngine.Rendering.ShaderPropertyType.Color:
                    clipboard[name] = mat.GetColor(name);
                    break;
                case UnityEngine.Rendering.ShaderPropertyType.Float:
                case UnityEngine.Rendering.ShaderPropertyType.Range:
                    clipboard[name] = mat.GetFloat(name);
                    break;
                case UnityEngine.Rendering.ShaderPropertyType.Vector:
                    clipboard[name] = mat.GetVector(name);
                    break;
                case UnityEngine.Rendering.ShaderPropertyType.Texture:
                    clipboard[name] = mat.GetTexture(name);
                    break;
            }
        }
    }

    public static void PasteMaterialSettings(Material mat)
    {
        if (mat == null || clipboard == null) return;

        ApplyPresetData(mat, clipboard);
        EditorUtility.SetDirty(mat);
    }

    // ========================================
    // JSON Serialization (Dictionary-compatible)
    // ========================================
    public static string SavePresetToJson(Dictionary<string, object> settings)
    {
        PresetData data = new PresetData();
        foreach (var kvp in settings)
        {
            data.entries.Add(new PresetEntry
            {
                name = kvp.Key,
                type = ValueTypeName(kvp.Value),
                value = ValueToString(kvp.Value)
            });
        }
        return JsonUtility.ToJson(data);
    }

    public static Dictionary<string, object> LoadPresetFromJson(string json)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        if (string.IsNullOrEmpty(json)) return dict;

        PresetData data = JsonUtility.FromJson<PresetData>(json);
        if (data?.entries == null) return dict;

        foreach (var entry in data.entries)
        {
            dict[entry.name] = StringToValue(entry.type, entry.value);
        }
        return dict;
    }

    private static string ValueTypeName(object value)
    {
        switch (value)
        {
            case Color _: return "color";
            case float _: return "float";
            case int _: return "int";
            case Vector4 _: return "vector";
            case Texture _: return "texture";
            default: return "null";
        }
    }

    private static string ValueToString(object value)
    {
        switch (value)
        {
            case Color c: return $"{c.r},{c.g},{c.b},{c.a}";
            case float f: return f.ToString(System.Globalization.CultureInfo.InvariantCulture);
            case int i: return i.ToString();
            case Vector4 v: return $"{v.x},{v.y},{v.z},{v.w}";
            case Texture t: return t != null ? AssetDatabase.GetAssetPath(t) : "";
            case null: return "";
            default: return "";
        }
    }

    private static object StringToValue(string type, string value)
    {
        if (string.IsNullOrEmpty(value)) return null;

        switch (type)
        {
            case "color":
                string[] c = value.Split(',');
                if (c.Length >= 4) return new Color(
                    ParseFloat(c[0]), ParseFloat(c[1]),
                    ParseFloat(c[2]), ParseFloat(c[3]));
                break;
            case "float":
                return ParseFloat(value);
            case "int":
                return int.Parse(value);
            case "vector":
                string[] v = value.Split(',');
                if (v.Length >= 4) return new Vector4(
                    ParseFloat(v[0]), ParseFloat(v[1]),
                    ParseFloat(v[2]), ParseFloat(v[3]));
                break;
            case "texture":
                return AssetDatabase.LoadAssetAtPath<Texture>(value);
        }
        return null;
    }

    private static float ParseFloat(string s)
    {
        return float.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
    }
}
