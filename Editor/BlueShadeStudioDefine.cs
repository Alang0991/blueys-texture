using UnityEngine;

public static class BlueShadeStudioDefine
{
    public const string PackageName = "com.blueys.texture";
    public const string ShaderName = "Blueys/BlueShade";
    public const string SimpleShaderName = "Blueys/BlueShade Lite";
    public const string Version = "1.2.2";

    public static readonly Color AccentColor = new Color(0.25f, 0.82f, 1f);
    public static readonly Color HeaderOffColor = new Color(0.17f, 0.17f, 0.19f);
    public static readonly Color HeaderOnColor = new Color(0.13f, 0.22f, 0.27f);
    public static readonly Color BodyColor = new Color(0.13f, 0.13f, 0.15f);
    public static readonly Color BannerBgColor = new Color(0.06f, 0.09f, 0.12f);

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
        "_USE_GRADIENT",
        "_USE_OCCLUSION"
    };

    public static readonly string[] SimpleKeywords =
    {
        "_USE_SOLID_OVERLAY",
        "_USE_EMISSION",
        "_USE_METALLIC_MAP",
        "_USE_SMOOTHNESS_MAP",
        "_USE_RIM_GLOW",
        "_USE_CUTOUT",
        "_USE_MATCAP",
        "_USE_GRADIENT",
        "_USE_OCCLUSION"
    };

    // Every [Toggle] property across both shaders. Used to sync material keywords
    // with the float property values so toggles actually drive the correct variant.
    public static readonly string[] ToggleProperties =
    {
        "_UseTextureBoost",
        "_UseDetail",
        "_UseNormal",
        "_UseWetShine",
        "_UseEdgeGlow",
        "_UseDepth",
        "_UseInnerGlow",
        "_UseEmission",
        "_UseReflection",
        "_UseOutline",
        "_UseDissolve",
        "_UseMatcap",
        "_UseGradient",
        "_UseOcclusion",
        "_UseMetallicMap",
        "_UseSmoothnessMap",
        "_UseSolidOverlay",
        "_UseRimGlow",
        "_UseCutout"
    };
}
