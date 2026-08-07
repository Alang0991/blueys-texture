using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public static class BlueShadeStudioPresetManager
{
    private const string CustomPresetKey = "BlueShadeStudio_CustomPresets_";
    private const string MaxCustomPresets = 20;

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
                Dictionary<string, object> settings = BlueShadeStudioUtils.LoadPresetFromJson(customJson);
                ApplyPresetData(mat, settings);
            }
        }

        EditorUtility.SetDirty(mat);
    }

    public static void ApplyPresetData(Material mat, Dictionary<string, object> settings)
    {
        if (mat == null || settings == null) return;

        foreach (var kvp in settings)
        {
            if (!mat.HasProperty(kvp.Key)) continue;

            switch (kvp.Value)
            {
                case float f:
                    mat.SetFloat(kvp.Key, f);
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
                case int i:
                    mat.SetFloat(kvp.Key, i);
                    break;
                case null:
                    mat.SetTexture(kvp.Key, null);
                    break;
            }
        }
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
            for (int i = 0; i < MaxCustomPresets - 1; i++)
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
}
