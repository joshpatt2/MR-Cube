using System;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom Unity build tools for VR/AR projects
/// This script provides build methods that can be called from the unity-builder.sh script
/// Place this in your Unity project's Editor folder
/// </summary>
public static class VRBuildTools
{
    private static readonly string BuildPath = "Builds";
    
    [MenuItem("Build/Build for Quest (Android)")]
    public static void BuildForQuest()
    {
        BuildForAndroid(true);
    }
    
    [MenuItem("Build/Build for PC VR")]
    public static void BuildForPCVR()
    {
        BuildForWindows(true);
    }
    
    [MenuItem("Build/Build for Mac VR")]
    public static void BuildForMacVR()
    {
        BuildForMac(true);
    }
    
    /// <summary>
    /// Build for Android (Quest) with VR optimizations
    /// </summary>
    public static void BuildForAndroid(bool isVR = false)
    {
        Debug.Log("Starting Android build...");
        
        // Configure Android settings
        EditorUserBuildSettings.buildAppBundle = false; // APK for Quest
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        
        if (isVR)
        {
            ConfigureVRSettings();
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel23; // Required for Quest
        }
        
        // Set build target
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        
        // Build
        string buildPath = Path.Combine(BuildPath, "Android", PlayerSettings.productName + ".apk");
        BuildGeneric(BuildTarget.Android, buildPath, isVR ? "VR" : "Standard");
    }
    
    /// <summary>
    /// Build for Windows with optional VR support
    /// </summary>
    public static void BuildForWindows(bool isVR = false)
    {
        Debug.Log("Starting Windows build...");
        
        if (isVR)
        {
            ConfigureVRSettings();
        }
        
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
        
        string buildPath = Path.Combine(BuildPath, "Windows", PlayerSettings.productName + ".exe");
        BuildGeneric(BuildTarget.StandaloneWindows64, buildPath, isVR ? "VR" : "Standard");
    }
    
    /// <summary>
    /// Build for macOS with optional VR support
    /// </summary>
    public static void BuildForMac(bool isVR = false)
    {
        Debug.Log("Starting macOS build...");
        
        if (isVR)
        {
            ConfigureVRSettings();
        }
        
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX);
        
        string buildPath = Path.Combine(BuildPath, "macOS", PlayerSettings.productName + ".app");
        BuildGeneric(BuildTarget.StandaloneOSX, buildPath, isVR ? "VR" : "Standard");
    }
    
    /// <summary>
    /// Configure VR-specific settings
    /// </summary>
    private static void ConfigureVRSettings()
    {
        Debug.Log("Configuring VR settings...");
        
        // Enable VR
        PlayerSettings.virtualRealitySupported = true;
        
        // Configure XR settings for newer Unity versions
        try
        {
            // This requires XR Plugin Management
            var xrSettings = UnityEditor.XR.Management.XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(BuildTargetGroup.Android);
            if (xrSettings != null)
            {
                Debug.Log("XR Plugin Management detected, configuring for VR...");
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"XR Plugin Management not available: {e.Message}");
        }
        
        // Optimize for VR
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.use32BitDisplayBuffer = false;
        
        // Performance optimizations for Quest
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            PlayerSettings.Android.preferredInstallLocation = AndroidPreferredInstallLocation.Auto;
            PlayerSettings.gpuSkinning = true;
            PlayerSettings.graphicsJobs = true;
        }
    }
    
    /// <summary>
    /// Generic build method
    /// </summary>
    private static void BuildGeneric(BuildTarget buildTarget, string buildPath, string buildType = "Standard")
    {
        Debug.Log($"Building {buildType} build for {buildTarget}...");
        Debug.Log($"Output path: {buildPath}");
        
        // Ensure directory exists
        string directory = Path.GetDirectoryName(buildPath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        // Get scenes to build
        string[] scenes = GetScenesFromBuildSettings();
        
        if (scenes.Length == 0)
        {
            Debug.LogError("No scenes found in build settings!");
            return;
        }
        
        // Configure build options
        BuildOptions buildOptions = BuildOptions.None;
        
        // Check if this is a development build
        if (EditorUserBuildSettings.development)
        {
            buildOptions |= BuildOptions.Development;
            
            if (EditorUserBuildSettings.allowDebugging)
            {
                buildOptions |= BuildOptions.AllowDebugging;
            }
            
            if (EditorUserBuildSettings.connectProfiler)
            {
                buildOptions |= BuildOptions.ConnectWithProfiler;
            }
        }
        
        // Build player settings
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = buildPath,
            target = buildTarget,
            options = buildOptions
        };
        
        // Start build
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        
        // Handle build result
        if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.Log($"Build succeeded! Size: {report.summary.totalSize} bytes");
            Debug.Log($"Build time: {report.summary.totalTime}");
            
            // Open build folder on macOS
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                System.Diagnostics.Process.Start("open", $"-R \"{buildPath}\"");
            }
        }
        else
        {
            Debug.LogError($"Build failed with result: {report.summary.result}");
            
            // Log errors
            foreach (var step in report.steps)
            {
                foreach (var message in step.messages)
                {
                    if (message.type == LogType.Error)
                    {
                        Debug.LogError($"Build Error: {message.content}");
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// Get scenes from build settings
    /// </summary>
    private static string[] GetScenesFromBuildSettings()
    {
        var scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }
        return scenes;
    }
    
    /// <summary>
    /// Command line build method for CI/CD
    /// Called by: unity-builder.sh -m "VRBuildTools.CommandLineBuild"
    /// </summary>
    public static void CommandLineBuild()
    {
        Debug.Log("Starting command line build...");
        
        // Parse command line arguments
        string[] args = Environment.GetCommandLineArgs();
        string buildTarget = "Android";
        string buildPath = "";
        bool isVR = true;
        
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-buildTarget" && i + 1 < args.Length)
            {
                buildTarget = args[i + 1];
            }
            else if (args[i] == "-buildPath" && i + 1 < args.Length)
            {
                buildPath = args[i + 1];
            }
            else if (args[i] == "-isVR" && i + 1 < args.Length)
            {
                bool.TryParse(args[i + 1], out isVR);
            }
        }
        
        Debug.Log($"Command line build - Target: {buildTarget}, VR: {isVR}");
        
        // Execute appropriate build
        switch (buildTarget.ToLower())
        {
            case "android":
                BuildForAndroid(isVR);
                break;
            case "standalonewindows64":
                BuildForWindows(isVR);
                break;
            case "standaloneosx":
                BuildForMac(isVR);
                break;
            default:
                Debug.LogError($"Unsupported build target: {buildTarget}");
                break;
        }
    }
}
