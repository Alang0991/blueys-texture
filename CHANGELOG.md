# Changelog

All notable changes to BlueysTexture will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.1] - 2024

### Added

- Shader keywords for all toggleable features (performance optimisation)
- Main texture tiling and offset controls
- Hue shift control
- Gamma control
- Vibrance control
- Sharpness control
- Metallic map support
- Smoothness map support
- Ambient occlusion map support
- Matcap reflection support
- Colour gradient support
- Outline system
- Dissolve effect with animated edge glow
- Improved Fresnel system
- Better transparency handling with edge alpha boost
- Pulse animation for emission
- Flicker effect for emission
- Scrolling emission support
- Reflection map support for fake reflections
- Search bar in custom inspector
- Material validator with auto-fix suggestions
- Texture information panel (resolution, format, VRAM, mipmaps)
- Performance information panel
- 10 material presets (Wet Fur, Plastic, Rubber, Latex, Metal, Skin, Toon, Glow, Matte, Fabric)
- Copy/Paste material settings
- Reset all button
- Tooltips for all properties
- Remembered section states per material
- CHANGELOG.md

### Changed

- Improved shader architecture with better feature organisation
- Enhanced custom ShaderGUI with better spacing and visual polish
- Optimised disabled features to add zero runtime cost via shader keywords
- Updated documentation with complete feature list
- Updated version to 1.0.1

### Fixed

- N/A (initial release was 1.0.0)

## [1.0.0] - 2024

### Added

- Initial release
- BlueysTexture shader with full feature set
- BlueysTextureSimple shader for lightweight use
- Custom BlueysTextureGUI inspector
- Custom BlueysTextureSimpleGUI inspector
- Basic emission system
- Basic rim glow
- Basic transparency support
- Basic texture controls (brightness, contrast, saturation)
