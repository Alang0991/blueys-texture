using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using BlueShadeStudio.Core;
using BlueShadeStudio.Modules;

public class BlueShadeStudioGUI : BlueShadeStudioGUIBase
{
    protected override string ShaderTitle => "BlueShade Studio";
    protected override string ShaderSubtitle => "v" + BlueShadeStudioDefine.Version + " | Professional VRChat Shader Suite";
    protected override string[] TabNames => new[] { "Main", "Lighting", "Effects", "Rendering", "Presets", "Optimization" };
    protected override string[] TabIcons => new[] { "d_Shader", "d_LightProbeGroup", "d_DownloadSpinner", "d_Rendering", "d_Presets", "d_UnityEditor.AnimationWindow" };

    protected override List<BaseModule> CreateModules()
    {
        return new List<BaseModule>
        {
            new MainModule(),
            new LightingModule(),
            new EffectsModule(),
            new RenderingModule(),
            new PresetsModule(),
            new OptimizationModule()
        };
    }
}
