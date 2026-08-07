using UnityEngine;

public static class BlueShadeStudioDefine
{
    public const string PackageName = "com.blueshade.studio";
    public const string ShaderName = "BlueShade/Studio";
    public const string SimpleShaderName = "BlueShade/Studio Lite";
    public const string Version = "1.1.1";

    public static readonly Color AccentColor = new Color(0.25f, 0.75f, 1f);
    public static readonly Color HeaderOffColor = new Color(0.16f, 0.16f, 0.16f);
    public static readonly Color HeaderOnColor = new Color(0.12f, 0.22f, 0.26f);
    public static readonly Color BodyColor = new Color(0.13f, 0.13f, 0.13f);
    public static readonly Color BannerBgColor = new Color(0.04f, 0.07f, 0.09f);

    public const string SectionPrefix = "BlueShadeStudio_";
    public const string SimpleSectionPrefix = "BlueShadeStudio_";

    public static readonly string[] MainKeywords =
    {
        "_USE_TEXTURE_BOOST",
        "_USE_DETAIL",
        "_USE_NORMAL",
        "_USE_WET_SHINE",
        "_USE_EDGE_GLOW",
        "_USE_DEPTH",
        "_USE_INNER_GLOW",
        "_USE_EMISSION",
        "_USE_REFLECTION",
        "_USE_OUTLINE",
        "_USE_DISSOLVE",
        "_USE_MATCAP",
        "_USE_GRADIENT"
    };

    public static readonly string[] SimpleKeywords =
    {
        "_USE_SOLID_OVERLAY",
        "_USE_EMISSION",
        "_USE_RIM_GLOW",
        "_USE_CUTOUT",
        "_USE_MATCAP",
        "_USE_GRADIENT"
    };
}
