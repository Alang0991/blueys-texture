# BlueShade Studio

<p align="left">
  <img src="https://img.shields.io/badge/VRChat-VCC-blue?style=flat-square">
  <img src="https://img.shields.io/badge/Unity-2022.3+-black?style=flat-square">
  <img src="https://img.shields.io/badge/Platform-PC%20Only-red?style=flat-square">
  <img src="https://img.shields.io/badge/Package-VPM-purple?style=flat-square">
  <img src="https://img.shields.io/github/v/release/Alang0991/blueys-texture?style=flat-square">
  <img src="https://img.shields.io/github/downloads/Alang0991/blueys-texture/total?style=flat-square">
</p>

Professional VRChat texture shaders with emission, effects, and custom material inspectors.

> Currently intended for **PC Only**.


---

## Installation

### Add Repository

Add this repository to VRChat Creator Companion:

```text
https://raw.githubusercontent.com/Alang0991/blueys-texture/main/index.json
```

### Install Through VCC

1. Open VRChat Creator Companion
2. Open your project
3. Click Manage Project
4. Open Packages
5. Install BlueShade Studio

---

## Features

### BlueShade/Studio (Full Shader)

- **Texture Controls:** Tiling, offset, tint, brightness, contrast, saturation, hue shift, gamma, vibrance, sharpness
- **Detail Overlay:** Detail texture with tiling and offset controls
- **Normal Map:** Normal mapping with strength control
- **Wet Shine:** Smoothness, specular strength, metallic map, smoothness map support
- **Edge Glow:** Rim lighting with adjustable power and strength
- **Deep Color:** View-dependent depth colouring
- **Inner Glow:** Soft inner glow effect
- **Emission System:** Emission textures with pulse, flicker, and scrolling animation
- **Fake Reflection:** Fresnel-based reflections with reflection map support
- **Matcap:** Matcap reflection support
- **Gradient:** Colour gradient support
- **Dissolve:** Animated dissolve effect with edge glow
- **Outline:** Procedural outline system
- **Ambient Occlusion:** AO map support
- **Improved Fresnel:** Better edge detection system
- **Transparency:** Advanced alpha handling with edge alpha boost

### BlueShade/Studio Lite (Lightweight Shader)

- **Texture Controls:** Brightness, contrast, saturation, hue shift, gamma, vibrance, sharpness
- **Metallic & Smoothness:** Full PBR support with map inputs
- **Colour Overlay:** Solid colour blending
- **Emission System:** Emission with mask, pulse, flicker, and scrolling
- **Rim Glow:** Adjustable rim lighting
- **PNG Cutout:** Alpha cutout for PNG shapes
- **Matcap:** Matcap reflection support
- **Gradient:** Colour gradient support
- **Ambient Occlusion:** AO map support

### Custom Shader Inspector

- Search bar for quick property finding
- Better category organisation
- Dark blue professional inspector style
- Tooltips for all properties
- Reset buttons
- Copy/Paste material settings
- 10 Material presets
- Performance information panel
- Material validator with auto-fix
- Texture information display
- Remembered section states

### Material Presets

- Wet Fur
- Plastic
- Rubber
- Latex
- Metal
- Skin
- Toon
- Glow
- Matte
- Fabric

---

## Included Files

```text
Runtime/
└── Shaders/
    ├── BlueShadeStudio.shader
    └── BlueShadeStudioLite.shader

Editor/
├── BlueShadeStudioGUI.cs
└── BlueShadeStudioLiteGUI.cs
```

---

## Requirements

| Software | Version |
|----------|---------|
| Unity | 2022.3+ |
| VRChat SDK | Latest |
| VCC | Latest |
| Platform | PC Only |

---

## Version

Current Version: **1.2.0**

---

## Repository

https://github.com/Alang0991/blueys-texture

---

## Author

**BlueShade Studio**

Created for VRChat creators looking for simple but powerful texture shaders.

---

## Changelog

### v1.0.1

**Shader Improvements:**
- Added shader keywords for all toggleable features (better performance)
- Added UV tiling and offset controls for main texture
- Added hue shift, gamma, vibrance, and sharpness controls
- Added metallic map and smoothness map support
- Added ambient occlusion map support
- Added matcap reflection support
- Added colour gradient support
- Added outline system
- Added dissolve effect with edge glow
- Improved Fresnel system
- Better transparency handling with edge alpha boost
- Added pulse, flicker, and scrolling emission animations
- Improved reflection controls with reflection map support

**Inspector Improvements:**
- Added search bar for quick property finding
- Added material validator with auto-fix suggestions
- Added texture information panel (resolution, format, VRAM, mipmaps)
- Added performance information panel
- Added 10 material presets
- Added copy/paste material settings
- Added reset all button
- Added tooltips for all properties
- Improved spacing and visual polish
- Remembered section states per material

**Quality of Life:**
- Added CHANGELOG.md
- Updated documentation
- Better VRChat performance with shader keywords

---

## License

MIT License
