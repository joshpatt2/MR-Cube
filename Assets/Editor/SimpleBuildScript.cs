using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;

public class SimpleBuildScript
{
    public static void BuildAndroid()
    {
        Debug.Log("SimpleBuildScript: Starting Android build...");
        
        // Configure Android settings properly
        EditorUserBuildSettings.buildAppBundle = false; // Build APK, not AAB
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24; // Fix: Use API level 24 for OpenXR
        
        // Fix: Configure splash screen for MR apps
        PlayerSettings.SplashScreen.showUnityLogo = false;
        PlayerSettings.SplashScreen.backgroundColor = Color.clear; // Transparent background for MR
        
        // Set build target (this is crucial!)
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
        {
            Debug.Log("Switching to Android build target...");
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        }
        
        string[] scenes = { "Assets/Scenes/SampleScene.unity" };
        string buildPath = "Builds/Android/MR-Cube.apk";
        
        // Create directory if it doesn't exist
        System.IO.Directory.CreateDirectory("Builds/Android");
        
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = buildPath,
            target = BuildTarget.Android,
            options = BuildOptions.Development
        };
        
        Debug.Log($"SimpleBuildScript: Building to {buildPath}");
        Debug.Log($"SimpleBuildScript: Target architecture: {PlayerSettings.Android.targetArchitectures}");
        Debug.Log($"SimpleBuildScript: Min SDK: {PlayerSettings.Android.minSdkVersion}");
        
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        
        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log("SimpleBuildScript: Build succeeded!");
            Debug.Log($"SimpleBuildScript: APK size: {report.summary.totalSize} bytes");
        }
        else
        {
            Debug.LogError($"SimpleBuildScript: Build failed: {report.summary.result}");
            
            // Log detailed error information
            foreach (var step in report.steps)
            {
                foreach (var message in step.messages)
                {
                    if (message.type == LogType.Error)
                    {
                        Debug.LogError($"Build Error: {message.content}");
                    }
                    else if (message.type == LogType.Warning)
                    {
                        Debug.LogWarning($"Build Warning: {message.content}");
                    }
                }
            }
        }
    }
}
