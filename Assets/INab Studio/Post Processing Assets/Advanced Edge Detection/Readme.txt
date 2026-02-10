Support: Discord (https://discord.gg/K88zmyuZFD)
Online Documentation: https://inabstudios.gitbook.io/advanced-edge-detection/
You can find more specific information on the asset functionality and setup process.
 
## Quick Start

### IMPORTING

After downloading the asset, import the appropriate .unitypackage file based on your Unity version and SRP (either Built-in.unitypackage, URP 6.unitypackage, or URP 2022.unitypackage).
Include shaders that are used by the asset in the Always Included Shaders tab in Project Settings/Graphics.
Depending on the rendering pipeline shaders may vary. You can find all the shaders you need to include in the list in \Core(URP or Built-In)\Shaders

#### BUILT-IN
Ensure you have the Shader Graph installed in your project to easily explore demo scenes and examples.
Navigate to your main camera object in your scene.
Add the AdvancedEdgeDetection.cs script to your camera.
Use the 'Update' button when changing the settings of the stencil mask or adding new game objects to the stencil mask layer.

#### URP
Make sure your URP asset has depth texture enabled.
Add AdvancedEdgeDetection feature to your URP data asset.

### Demo Scenes

Demo scenes can be found: \Common (URP or Built-In)\Demo Scenes

If you encounter issues setting up, you can use the provided URP settings asset. Adjust it via the Graphics section in your project settings. Ensure that the render asset is not overridden in the Quality tab. Make sure in the Quality tab render asset is not overridden.

### ISSUES
If you encounter any difficulties using, implementing, or understanding the asset, please feel free to contact me.
