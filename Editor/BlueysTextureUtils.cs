using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public static class BlueysTextureUtils
{
    public static string FormatVRAM(Texture tex)
    {
        if (tex == null) return "N/A";
        long bytes = (long)tex.width * tex.height * 4;
        if (bytes > 1048576) return (bytes / 1048576f).ToString("F1") + " MB";
        if (bytes > 1024) return (bytes / 1024f).ToString("F1") + " KB";
        return bytes + " B";
    }

    public static string GetTextureInfo(Texture tex)
    {
        if (tex == null) return "None assigned";
        string path = AssetDatabase.GetAssetPath(tex);
        if (string.IsNullOrEmpty(path)) return "Unknown source";

        TextureImporter imp = AssetImporter.GetAtPath(path) as TextureImporter;
        if (imp == null) return "Unknown format";

        string info = $"{tex.width}x{tex.height} | {imp.textureCompression} | Mipmaps: {(imp.mipmapEnabled ? "On" : "Off")} | sRGB: {(imp.sRGBTexture ? "Yes" : "No")}";
        return info;
    }

    public static bool IsValidTexture(Texture tex, out string warning)
    {
        warning = "";
        if (tex == null) return true;

        string path = AssetDatabase.GetAssetPath(tex);
        if (string.IsNullOrEmpty(path))
        {
            warning = "Texture is not in the project.";
            return false;
        }

        TextureImporter imp = AssetImporter.GetAtPath(path) as TextureImporter;
        if (imp == null)
        {
            warning = "Could not inspect texture.";
            return false;
        }

        if (!imp.mipmapEnabled)
        {
            warning = "Mipmaps are disabled. Enable for better quality.";
            return false;
        }

        if (imp.wrapMode != TextureWrapMode.Repeat)
        {
            warning = "Wrap mode is not Repeat. Set to Repeat for tiling.";
            return false;
        }

        return true;
    }

    public static void ApplyFixTexture(Texture tex)
    {
        if (tex == null) return;
        string path = AssetDatabase.GetAssetPath(tex);
        if (string.IsNullOrEmpty(path)) return;

        TextureImporter imp = AssetImporter.GetAtPath(path) as TextureImporter;
        if (imp == null) return;

        imp.mipmapEnabled = true;
        imp.wrapMode = TextureWrapMode.Repeat;
        imp.isReadable = false;
        AssetDatabase.ImportAsset(path);
    }

    public static string GetPerformanceRating(int keywordCount)
    {
        if (keywordCount <= 3) return "Low (Good for VRChat)";
        if (keywordCount <= 6) return "Medium";
        return "High (Consider simplifying)";
    }

    public static string GetPerformanceColor(int keywordCount)
    {
        if (keywordCount <= 3) return "#44ff44";
        if (keywordCount <= 6) return "#ffaa00";
        return "#ff4444";
    }

    public static bool MatchesSearch(string text, string query)
    {
        if (string.IsNullOrEmpty(query)) return true;
        return text.ToLower().Contains(query.ToLower());
    }

    public static void SetMaterialKeyword(Material mat, string keyword, bool enabled)
    {
        if (enabled)
            mat.EnableKeyword(keyword);
        else
            mat.DisableKeyword(keyword);
    }

    public static bool IsKeywordEnabled(Material mat, string keyword)
    {
        return mat.IsKeywordEnabled(keyword);
    }

    public static string SavePresetToJson(Dictionary<string, object> settings)
    {
        return JsonUtility.ToJson(new PresetWrapper(settings));
    }

    public static Dictionary<string, object> LoadPresetFromJson(string json)
    {
        PresetWrapper wrapper = JsonUtility.FromJson<PresetWrapper>(json);
        return wrapper != null ? wrapper.settings : new Dictionary<string, object>();
    }

    [System.Serializable]
    private class PresetWrapper
    {
        public Dictionary<string, object> settings;

        public PresetWrapper(Dictionary<string, object> s)
        {
            settings = s;
        }
    }
}
