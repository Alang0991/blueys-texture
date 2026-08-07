# BlueShadeStudio v1.1.0 Feature Roadmap

> **Status:** Planning only. No implementation yet.
> **Base:** v1.0.3 stable foundation
> **Goal:** Expand feature set while preserving existing _MainTex UV compatibility

---

## 1. Architecture Principles

### 1.1 Non-Negotiable Constraints

- **_MainTex UV workflow is immutable.** All new features must build on top of the existing UV system.
- **Existing materials must continue working.** No breaking changes to property names, UV channels, or shader passes.
- **Keyword-based toggles only.** No runtime branches for disabled features.
- **Surface shader only.** No vertex/fragment shader rewrites unless absolutely necessary.
- **PC VRChat target.** No mobile/Quest compatibility in this release.

### 1.2 Current Asset Map

| Asset | Role | Protected? |
|-------|------|------------|
| `_MainTex` / `IN.uv_MainTex` | Primary diffuse/albedo | **Yes - never change** |
| `_Color` | Main tint | Yes |
| `_MainTexTiling` / `_MainTexOffset` | UV transform | Yes |
| `_Alpha` | Transparency | Yes |
| Surface Output | StandardSpecular / Standard | Yes |
| Existing keywords | Feature toggles | Extend only |

### 1.3 Keyword Convention

New keywords follow the existing pattern:

```
_USE_<FEATURE_NAME>  -> on
_USE_<FEATURE_NAME>_OFF -> off
```

All new `#pragma multi_compile` blocks must have both variants.

---

## 2. Feature Architecture

### 2.1 Toon Shading

**Current state:** `_USE_TOON_LIGHTING` and `_USE_CEL_SHADING` exist but are functionally duplicates in the current shader. The toon block runs texture adjustments identically to the non-toon path.

**Proposed architecture:**

```
Properties:
  [Toggle] _USE_TOON_LIGHTING
  [Toggle] _USE_CEL_SHADING
  _ShadowColor
  _ShadowStrength
  _ShadowThreshold
  _ShadowSmoothness
  [Toggle] _USE_SHADE_RAMP
  _ShadeRampTex
  _ShadowMap (NEW - baked shadow mask)
  _ShadowMapStrength (NEW)

Keywords:
  _USE_TOON_LIGHTING
  _USE_CEL_SHADING
  _USE_SHADE_RAMP
  _USE_SHADOW_MAP (NEW)
```

**Behaviour:**
- When `_USE_TOON_LIGHTING` is on, lighting calculations use stepped/smooth shadow instead of PBR response.
- `_USE_CEL_SHADING` hardens the step (no smoothstep falloff).
- `_USE_SHADE_RAMP` samples a 1D ramp texture instead of procedural shadow.
- `_USE_SHADOW_MAP` reads a pre-baked shadow mask for static toon shading (VRChat optimisation).
- When toon is off, the shader behaves exactly as v1.0.3.

**VRChat note:** Toon shading on avatars requires `fullforwardshadows` or the avatar will look flat in shadowed areas. This should be a render queue or tag option, not forced.

---

### 2.2 MatCap

**Current state:** `_USE_MATCAP` exists with basic normal-based UV sampling.

**Proposed architecture:**

```
Properties:
  [Toggle] _USE_MATCAP
  _MatcapTex
  _MatcapStrength
  _MatcapMask (NEW - mask texture for matcap areas)
  _MatcapBlend (NEW - blend mode: multiply/add/overlay)
  _MatcapTiling (NEW)

Keywords:
  _USE_MATCAP
```

**Behaviour:**
- Keep existing normal-based UV: `o.Normal.xy * 0.5 + 0.5`
- Add optional `_MatcapMask` to restrict matcap to specific areas (e.g., eyes, metal parts).
- Blend modes: 0 = multiply, 1 = add, 2 = overlay.
- `_MatcapTiling` allows tiling for pattern matcaps.
- Disabled matcap adds zero cost (keyword).

---

### 2.3 Outline

**Current state:** `_USE_OUTLINE` exists with width/threshold but no implementation in the simple shader, and no camera-facing expansion in the full shader.

**Proposed architecture:**

```
Properties:
  [Toggle] _USE_OUTLINE
  _OutlineColor
  _OutlineWidth
  _OutlineThreshold
  _OutlineMask (NEW)
  _OutlineTransparency (NEW)
  _OutlineDistanceScale (NEW)
  _OutlineCameraFacing (NEW - toggle for screen-space expansion)

Keywords:
  _USE_OUTLINE
```

**Implementation note:** True outline requires a second pass or vertex expansion. For v1.1.0, the preferred approach is:

- **Option A (recommended):** Add outline as a second render pass in the shader. This keeps the main surface shader untouched.
- **Option B:** Vertex-based expansion in the surface shader using `_OutlineWidth` and view direction. Simpler but less accurate.

For v1.1.0, implement Option B first as it is single-pass and cheaper. The outline is drawn by expanding vertices along their normals in the vertex modifier, then drawing a second pass behind the main pass.

**Critical:** The outline pass must use the same `_MainTex` UV as the main pass so textures align.

---

### 2.4 Advanced Emission

**Current state:** Emission has pulse, flicker, and scroll. It uses `_EmissionMap`, `_EmissionMask`, `_EmissionColor`, `_EmissionStrength`.

**Proposed architecture:**

```
Properties:
  [Toggle] _USE_EMISSION
  _EmissionMap
  _EmissionMask
  _EmissionColor
  _EmissionStrength
  _EmissionPulseSpeed
  _EmissionPulseMin
  _EmissionFlickerSpeed
  _EmissionFlickerIntensity
  _EmissionScrollSpeed
  _EmissionScrollDirection
  _EmissionUsesPNG (simple only)
  _EmissionBloomPower (NEW - HDR bloom intensity)
  _EmissionRamp (NEW - 1D ramp for falloff control)
  _EmissionRampStrength (NEW)
  _EmissionParallax (NEW - parallax UV for emission map)
  _EmissionParallaxStrength (NEW)

Keywords:
  _USE_EMISSION
```

**Behaviour:**
- Keep all existing emission features unchanged.
- `_EmissionRamp` allows artists to control emission falloff with a curve instead of hard linear.
- `_EmissionBloomPower` multiplies emission for HDR bloom in VRChat (PC only).
- `_EmissionParallax` shifts emission UV based on view direction for depth.
- Disabled emission adds zero cost (keyword).

---

### 2.5 Better Lighting

**Current state:** Fake light, rim light, fresnel, and toon lighting exist but are fragmented.

**Proposed architecture:**

```
Properties:
  [Toggle] _USE_FAKE_LIGHT
  _FakeLightColor
  _FakeLightIntensity
  _FakeLightDir
  _FakeLightSmoothness (NEW)

  [Toggle] _USE_RIM_LIGHT
  _RimColor
  _RimPower
  _RimStrength
  _RimWidth
  _RimMask (NEW)

  [Toggle] _USE_FRESNEL
  _FresnelColor
  _FresnelPower
  _FresnelStrength

  [Toggle] _USE_AMBIENT_OCCLUSION
  _OcclusionMap
  _OcclusionStrength

Keywords:
  _USE_FAKE_LIGHT
  _USE_RIM_LIGHT
  _USE_FRESNEL
```

**Behaviour:**
- `_USE_FAKE_LIGHT` adds a directional light term independent of scene lights. Useful for VRChat avatars in dark worlds.
- `_FakeLightSmoothness` controls the transition between lit and unlit areas.
- `_RimMask` restricts rim light to specific texture areas.
- Keep all existing behaviour identical to v1.0.3 when these are off.

---

### 2.6 Texture Layers

**Current state:** Detail overlay exists (`_USE_DETAIL`) with tiling, offset, UV select, and blend.

**Proposed architecture:**

```
Properties:
  [Toggle] _USE_DETAIL
  _DetailTex
  _DetailStrength
  _DetailTexTiling
  _DetailTexOffset
  _DetailTexUVSelect
  _DetailBlend

  [Toggle] _USE_DETAIL_2 (NEW)
  _Detail2Tex (NEW)
  _Detail2Strength (NEW)
  _Detail2Tiling (NEW)
  _Detail2Offset (NEW)
  _Detail2Mask (NEW - mask from main texture)

  [Toggle] _USE_DETAIL_3 (NEW)
  _Detail3Tex (NEW)
  _Detail3Strength (NEW)
  _Detail3Tiling (NEW)
  _Detail3Offset (NEW)
  _Detail3Mask (NEW)

Keywords:
  _USE_DETAIL
  _USE_DETAIL_2
  _USE_DETAIL_3
```

**Behaviour:**
- Each layer blends multiplicatively over the previous layer.
- `_DetailXMask` uses the main texture alpha or luminance to mask where the detail appears.
- UV selection chooses between main UV and detail UV for each layer.
- When disabled, the layer adds zero cost (keyword).
- Maximum 3 detail layers to keep shader complexity bounded.

**Critical:** Detail layers must use `IN.uv_MainTex` as the base UV. The existing `_DetailTexUVSelect` must remain functional.

---

### 2.7 VRChat Avatar Optimisation

**Current state:** No VRChat-specific features.

**Proposed architecture:**

```
Properties:
  [Toggle] _VRC_USE_STENCIL
  _VRC_StencilRef
  _VRC_StencilComp
  _VRC_StencilPass
  _VRC_StencilFail

  [Toggle] _VRC_USE_AUDIOLINK
  _VRC_AudioLinkBand
  _VRC_AudioLinkStrength

  [Toggle] _VRC_USE_PROXIMITY
  _VRC_ProximityRange
  _VRC_ProximityColor
  _VRC_ProximityStrength

  _VRC_RenderQueue (NEW - VRChat queue override)
  _VRC_QueueValue (NEW)

Keywords:
  _USE_STENCIL (existing)
  _USE_AUDIOLINK (existing)
  _VRC_USE_PROXIMITY (NEW)
```

**Behaviour:**
- Stencil support is already partially in the shader but not exposed in the simple shader.
- AudioLink is already present.
- `_VRC_USE_PROXIMITY` uses world position distance to change color/emission for proximity-based effects (e.g., name tags, interaction highlights).
- `_VRC_RenderQueue` allows per-material render queue override for VRChat's queue sorting.

---

## 3. Implementation Phases

### Phase 1: Core Feature Stabilisation (v1.1.0-alpha)

1. Fix any remaining v1.0.3 issues found during testing.
2. Refactor shader property blocks into clear sections with comments.
3. Add `#pragma shader_feature` local keywords where material variety is low (e.g., outline) to reduce shader variant count.

### Phase 2: Lighting & Visual Enhancement (v1.1.0-beta)

1. Implement improved toon shading with shade ramp and shadow mask.
2. Implement MatCap mask and blend modes.
3. Implement outline vertex expansion pass.
4. Add fake light smoothness control.
5. Add rim light mask.

### Phase 3: Texture & Emission (v1.1.0-beta.2)

1. Implement emission ramp and bloom power.
2. Implement emission parallax.
3. Implement texture layer 2 and 3 with masks.
4. Add detail blend mode options.

### Phase 4: VRChat Integration (v1.1.0-rc)

1. Expose stencil in simple shader GUI.
2. Implement proximity-based effect.
3. Add render queue override.
4. VRChat SDK test pass with avatars.

### Phase 5: Polish (v1.1.0)

1. GUI updates for all new properties.
2. Presets for new features.
3. Documentation updates.
4. Performance profiling and keyword audit.

---

## 4. GUI Architecture Plan

### 4.1 Inspector Sections

The GUI will follow the existing foldout pattern. New sections:

```
MAIN TEXTURE
  - Main Texture, Tint, Tiling, Offset, Alpha

TEXTURE ENHANCEMENT
  - Brightness, Contrast, Saturation, Hue, Gamma, Vibrance, Sharpness

TEXTURE LAYERS
  - Detail Overlay (toggle + properties)
  - Detail Layer 2 (toggle + properties)
  - Detail Layer 3 (toggle + properties)

NORMAL MAP
  - Normal Map (toggle + strength)

LIGHTING
  - Toon Shading (toggle + shadow/ramp properties)
  - Fake Light (toggle + direction/intensity)
  - Rim Light (toggle + color/power/strength/mask)
  - Fresnel (toggle + color/power/strength)

MATERIAL
  - Metallic/Smoothness + maps
  - Occlusion Map

EMISSION
  - Emission Map (toggle + color/strength)
  - Emission Effects (pulse, flicker, scroll)
  - Emission Ramp (toggle)
  - Emission Bloom

EFFECTS
  - MatCap (toggle + texture/strength/mask/blend)
  - Gradient (toggle + texture/strength)
  - Outline (toggle + width/color/threshold)
  - Dissolve (toggle + amount/edge)

VRChat
  - Stencil (toggle + ref/comp/pass/fail)
  - AudioLink (toggle + band/strength)
  - Proximity (toggle + range/color/strength)
  - Render Queue

ADVANCED
  - Rendering (queue, cull, zwrite)
  - Performance
  - Debug Information
```

### 4.2 GUI Code Structure

- Keep existing `BlueShadeStudioGUI.cs` and `BlueShadeStudioSimpleGUI.cs` structure.
- Extract section drawing into shared helper methods where possible.
- Add tooltips for all new properties.
- Add missing texture warnings for new maps.
- Preserve all existing section states.

---

## 5. Shader Code Organisation

### 5.1 Current Problems to Fix

1. Duplicate colour adjustment code in toon/non-toon branches.
2. Emission code is long and hard to extend.
3. No clear section markers for future contributors.

### 5.2 Proposed Structure

```hlsl
// ============================================
// BLUEYSTEXTURE v1.1.0
// ============================================

// --- Helper Functions ---
// HueRotate, GammaAdjust, VibranceAdjust, ContrastAdjust, SaturationAdjust

// --- UV Utilities ---
// ParallaxUV, ScrolledUV

// --- Main Surface ---
void surf(Input IN, inout SurfaceOutputStandardSpecular o)
{
    // 1. Main Texture & UV
    // 2. Colour Adjustment
    // 3. Detail Layers
    // 4. Normal Map
    // 5. Lighting Model (toon / PBR)
    // 6. Effects (rim, fresnel, fake light)
    // 7. Emission
    // 8. MatCap
    // 9. Outline (handled in separate pass)
    // 10. Final Output
}
```

### 5.3 Keyword Audit

Before v1.1.0 release, audit all keywords:

- Count maximum variants per shader.
- Ensure no shader exceeds Unity's keyword limit (usually 256 per shader, but practical limit is lower).
- Use `shader_feature` for material-specific keywords (outline, detail 2, detail 3).
- Use `multi_compile` for global keywords (toon, emission).

---

## 6. Compatibility Matrix

| Feature | v1.0.3 Material Compatible? | New Properties | New Keywords |
|---------|----------------------------|----------------|--------------|
| Toon Shading | Yes | Yes | Yes |
| MatCap | Yes | Yes | No (existing) |
| Outline | Yes | Yes | No (existing) |
| Advanced Emission | Yes | Yes | No (existing) |
| Better Lighting | Yes | Yes | No (existing) |
| Texture Layers | Yes | Yes | Yes |
| VRChat Optimisation | Yes | Yes | Yes |

**Rule:** Any material created in v1.0.3 must open in v1.1.0 without errors and render identically with all new features disabled.

---

## 7. Testing Checklist

Before any feature is merged:

- [ ] New material creation works
- [ ] Existing v1.0.3 material opens without errors
- [ ] Texture assignment works
- [ ] VRChat avatar render test (PC)
- [ ] Shader compiles without warnings
- [ ] GUI opens without errors
- [ ] All keywords toggle correctly
- [ ] Performance impact measured (keyword count, instruction count)

---

## 8. Out of Scope for v1.1.0

These features are explicitly deferred:

- Vertex/Fragment shader rewrite
- Mobile/Quest support
- VRAM texture streaming
- Material property blocks
- Shader Graph conversion
- LOD crossfade
- Tessellation
- Subsurface scattering
- Parallax occlusion mapping (kept simple)
- Any breaking change to `_MainTex` UV system

---

## 9. Repository Structure Target

```
Packages/com.blueys.texture/
├── package.json
├── CHANGELOG.md
├── README.md
├── index.json
├── Documentation/
│   ├── ROADMAP_v1.1.0.md
│   ├── ARCHITECTURE.md
│   └── API.md
├── Runtime/
│   └── Shaders/
│       ├── BlueShadeStudio.shader
│       └── BlueShadeStudioSimple.shader
├── Editor/
│   ├── BlueShadeStudioGUI.cs
│   ├── BlueShadeStudioSimpleGUI.cs
│   ├── BlueShadeStudioDefine.cs
│   ├── BlueShadeStudioPresets.cs
│   ├── BlueShadeStudioBuiltinPresets.cs
│   └── BlueShadeStudioUtils.cs
└── Samples~
    └── Example Materials/
```

---

## 10. Decision Log

| Date | Decision | Rationale |
|------|----------|-----------|
| 2026-08-06 | Keep Surface Shader | Stability, VRChat compatibility, less code |
| 2026-08-06 | Use keyword toggles | Zero-cost disabled features |
| 2026-08-06 | Max 3 detail layers | Bounds shader complexity and instruction count |
| 2026-08-06 | Vertex outline first | Single-pass, cheaper than second pass |
| 2026-08-06 | No breaking UV changes | Existing material compatibility |
