[![Publish to NPM Registry](https://github.com/kovanci/Package_Unity3D_ChimpSolar/actions/workflows/publish_npm.yml/badge.svg)](https://github.com/kovanci/Package_Unity3D_ChimpSolar/actions/workflows/publish_npm.yml)

### Chimp Solar

Unity3D package for precise sun position calculations and sunrise/sunset time estimations.

Designed to facilitate the process of determining the sun's position and timing for game developers, visual effects artists, and other Unity3D users. Understanding the position of the sun in the sky can be a crucial factor when creating game worlds and atmospheric effects, and this package simplifies such tasks.

- Includes 2 usage samples. Both samples can be imported via the `Package Manager`.
  - [Day-Night Cycle](https://github.com/kovanci/Package_Unity3D_ChimpSolar/tree/master/Samples~/Day-Night%20Cycle)
  - [Basic Data Visualization](https://github.com/kovanci/Package_Unity3D_ChimpSolar/tree/master/Samples~/Data%20Visualization)


### 📦 Dependencies
- [Unity.Mathematics][1]

### 💿 Installation
This package uses the scoped registry feature. Open the `Package Manager` page in the `Project Settings` window and add the following entry to the `Scoped Registries` list:

![install_1](https://github.com/emrekovanci/emrekovanci.github.io/assets/13253356/7a31050a-5369-4436-8805-9f56cc1f9513)

- Name: `Kovanci Packages`
- URL: `https://registry.npmjs.com`
- Scope: `com.kovanci`

Now you can install the package from `My Registries` page in the `Package Manager` window.

![install_2](https://github.com/emrekovanci/emrekovanci.github.io/assets/13253356/9a43304f-e05f-4ad2-9806-80858ab7f754)

[1]: https://docs.unity3d.com/Packages/com.unity.mathematics@1.3/manual/index.html
