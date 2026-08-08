using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using BlueShadeStudio.Core;
using BlueShadeStudio.Modules;

public class BlueShadeStudioLiteGUI : BlueShadeStudioGUIBase
{
    protected override string ShaderTitle => "BlueShade Studio Lite";
    protected override string ShaderSubtitle => "v" + BlueShadeStudioDefine.Version + " | Lightweight VRChat Shader";
    protected override string[] TabNames => new[] { "Main", "Look", "Lighting", "Effects", "Presets", "Optimization" };
    protected override string[] TabIcons => new[] { "d_Shader", "d_Inspector", "d_LightProbeGroup", "d_DownloadSpinner", "d_Presets", "d_UnityEditor.AnimationWindow" };

    protected override List<BaseModule> CreateModules()
    {
        return new List<BaseModule>
        {
            new LiteMainModule(),
            new LiteLookModule(),
            new LiteLightingModule(),
            new LiteEffectsModule(),
            new PresetsModule(),
            new OptimizationModule()
        };
    }
}
