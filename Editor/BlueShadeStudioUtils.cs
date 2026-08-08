using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

public static class BlueShadeStudioUtils
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

    public static string GetTextureWarning(Texture tex)
    {
        if (tex == null) return null;

        string path = AssetDatabase.GetAssetPath(tex);
        if (string.IsNullOrEmpty(path))
            return "Texture is not from the project.\n\nThis may cause issues with texture quality.";

        TextureImporter imp = AssetImporter.GetAtPath(path) as TextureImporter;
        if (imp == null) return null;

        List<string> issues = new List<string>();

        if (!imp.mipmapEnabled)
            issues.Add("Mipmaps are disabled — enable for better quality at distance.");

        if (imp.wrapMode != TextureWrapMode.Repeat)
            issues.Add("Wrap mode is not Repeat — set to Repeat for proper tiling.");

        if (issues.Count == 0) return null;

        return string.Join("\n\n", issues);
    }

    public static string GetPerformanceRating(int keywordCount)
    {
        if (keywordCount <= 3) return "Low (Good for VRChat)";
        if (keywordCount <= 6) return "Medium";
        return "High (Consider simplifying)";
    }

    public static Color GetPerformanceColor(int keywordCount)
    {
        if (keywordCount <= 3) return new Color(0.3f, 1f, 0.3f);
        if (keywordCount <= 6) return new Color(1f, 0.7f, 0.2f);
        return new Color(1f, 0.3f, 0.3f);
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

    public static string TogglePropertyToKeyword(string propertyName)
    {
        if (string.IsNullOrEmpty(propertyName)) return propertyName;
        var sb = new StringBuilder(propertyName.Length + 2);
        for (int i = 0; i < propertyName.Length; i++)
        {
            char c = propertyName[i];
            if (char.IsUpper(c) && i > 0)
            {
                char prev = propertyName[i - 1];
                if (prev != '_' && !char.IsUpper(prev) && !char.IsDigit(prev))
                    sb.Append('_');
            }
            sb.Append(char.ToUpperInvariant(c));
        }
        return sb.ToString();
    }

    public static void SyncMaterialKeywords(Material mat)
    {
        if (mat == null || mat.shader == null) return;
        foreach (string prop in BlueShadeStudioDefine.ToggleProperties)
        {
            if (!mat.HasProperty(prop)) continue;
            string keyword = TogglePropertyToKeyword(prop);
            bool on = mat.GetFloat(prop) > 0.5f;
            if (on)
                mat.EnableKeyword(keyword);
            else
                mat.DisableKeyword(keyword);
        }
    }
}
