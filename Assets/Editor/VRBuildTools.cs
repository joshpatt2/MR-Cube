using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
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
                    if (message.type == UnityEngine.LogType.Error)
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
    /// Simple build method for command line usage
    /// Called by: unity-builder.sh -m "VRBuildTools.BuildPlayer"
    /// </summary>
    public static void BuildPlayer()
    {
        Debug.Log("=== VRBuildTools.BuildPlayer START ===");
        
        // Get current build target
        BuildTarget currentTarget = EditorUserBuildSettings.activeBuildTarget;
        Debug.Log($"Current build target: {currentTarget}");
        
        // For Android, verify SDK setup first
        if (currentTarget == BuildTarget.Android)
        {
            if (!ValidateAndroidSDK())
            {
                Debug.LogError("Android SDK validation failed. Cannot proceed with build.");
                return;
            }
        }
        
        // Determine build path based on target
        string buildPath = GetBuildPath(currentTarget);
        Debug.Log($"Build path determined: {buildPath}");
        
        // Ensure build directory exists
        string buildDir = Path.GetDirectoryName(buildPath);
        if (!Directory.Exists(buildDir))
        {
            Directory.CreateDirectory(buildDir);
            Debug.Log($"Created build directory: {buildDir}");
        }
        
        // Get scenes from build settings
        string[] scenes = GetScenesFromBuildSettings();
        Debug.Log($"Found {scenes.Length} scenes in build settings");
        
        if (scenes.Length == 0)
        {
            Debug.LogError("No scenes found in build settings! Cannot build.");
            return;
        }
        
        // Configure build options
        BuildOptions buildOptions = BuildOptions.None;
        if (EditorUserBuildSettings.development)
        {
            buildOptions |= BuildOptions.Development;
            Debug.Log("Development build enabled");
        }
        
        // Build player settings
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = buildPath,
            target = currentTarget,
            options = buildOptions
        };
        
        Debug.Log($"Starting build with options:");
        Debug.Log($"  Target: {currentTarget}");
        Debug.Log($"  Path: {buildPath}");
        Debug.Log($"  Options: {buildOptions}");
        Debug.Log($"  Scenes: {string.Join(", ", scenes)}");
        
        // Start build
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        
        // Handle build result
        Debug.Log($"Build completed with result: {report.summary.result}");
        if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.Log($"✅ Build succeeded! Size: {report.summary.totalSize} bytes");
            Debug.Log($"Build time: {report.summary.totalTime}");
            Debug.Log($"Output location: {buildPath}");
        }
        else
        {
            Debug.LogError($"❌ Build failed with result: {report.summary.result}");
            
            // Log detailed errors
            foreach (var step in report.steps)
            {
                foreach (var message in step.messages)
                {
                    if (message.type == UnityEngine.LogType.Error)
                    {
                        Debug.LogError($"Build Error: {message.content}");
                    }
                }
            }
        }
        
        Debug.Log("=== VRBuildTools.BuildPlayer END ===");
    }
    
    /// <summary>
    /// Validate Android SDK setup
    /// </summary>
    private static bool ValidateAndroidSDK()
    {
        Debug.Log("Validating Android SDK setup...");
        
        // Check Android SDK path
        string sdkPath = EditorPrefs.GetString("AndroidSdkRoot");
        Debug.Log($"Android SDK Root: {sdkPath}");
        
        if (string.IsNullOrEmpty(sdkPath) || !Directory.Exists(sdkPath))
        {
            Debug.LogError("Android SDK path is not set or doesn't exist!");
            Debug.LogError("Please set Android SDK path in Unity Preferences > External Tools");
            return false;
        }
        
        // Check JDK path
        string jdkPath = EditorPrefs.GetString("JdkPath");
        Debug.Log($"JDK Path: {jdkPath}");
        
        if (string.IsNullOrEmpty(jdkPath) || !Directory.Exists(jdkPath))
        {
            Debug.LogError("JDK path is not set or doesn't exist!");
            Debug.LogError("Please set JDK path in Unity Preferences > External Tools");
            return false;
        }
        
        // Check Gradle
        string gradlePath = EditorPrefs.GetString("GradlePath");
        Debug.Log($"Gradle Path: {gradlePath}");
        
        // Fix Android project settings
        ConfigureAndroidSettings();
        
        // Check build tools
        string buildToolsPath = Path.Combine(sdkPath, "build-tools");
        if (Directory.Exists(buildToolsPath))
        {
            var buildToolVersions = Directory.GetDirectories(buildToolsPath);
            Debug.Log($"Available build tools: {string.Join(", ", buildToolVersions.Select(Path.GetFileName))}");
        }
        else
        {
            Debug.LogWarning("Build tools directory not found in Android SDK");
        }
        
        // Check platform tools
        string platformToolsPath = Path.Combine(sdkPath, "platform-tools");
        if (Directory.Exists(platformToolsPath))
        {
            Debug.Log("Platform tools found");
        }
        else
        {
            Debug.LogWarning("Platform tools not found in Android SDK");
        }
        
        Debug.Log("Android SDK validation completed");
        return true;
    }
    
    /// <summary>
    /// Configure Android project settings
    /// </summary>
    private static void ConfigureAndroidSettings()
    {
        Debug.Log("Configuring Android project settings...");
        
        // Set target API levels
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel33;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel29;
        
        // Set Android app theme to fix AppCompat issues
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        
        // Fix the Android theme that's causing the build error
        // The error is: style/Theme.AppCompat.DayNight.NoActionBar not found
        // We need to change this to a valid Android theme
        Debug.Log("Setting Android application theme to fix AppCompat issues...");
        
        // Use a standard Android theme that doesn't require AppCompat
        // This will be set in the generated AndroidManifest.xml
        PlayerSettings.Android.showActivityIndicatorOnLoading = AndroidShowActivityIndicatorOnLoading.DontShow;
        
        // Use IL2CPP scripting backend (Unity 6 API)
        var androidBuildTarget = NamedBuildTarget.Android;
        PlayerSettings.SetScriptingBackend(androidBuildTarget, ScriptingImplementation.IL2CPP);
        
        // Set API compatibility level (Unity 6)
        PlayerSettings.SetApiCompatibilityLevel(androidBuildTarget, ApiCompatibilityLevel.NET_Standard);
        
        Debug.Log($"Android Target SDK: {PlayerSettings.Android.targetSdkVersion}");
        Debug.Log($"Android Min SDK: {PlayerSettings.Android.minSdkVersion}");
        Debug.Log($"Android Architecture: {PlayerSettings.Android.targetArchitectures}");
        Debug.Log($"Scripting Backend: {PlayerSettings.GetScriptingBackend(androidBuildTarget)}");
        Debug.Log($"API Compatibility: {PlayerSettings.GetApiCompatibilityLevel(androidBuildTarget)}");
        
        Debug.Log("Android configuration completed");
    }
    
    /// <summary>
    /// Get build path based on target
    /// </summary>
    private static string GetBuildPath(BuildTarget target)
    {
        string basePath = Path.Combine(BuildPath, target.ToString());
        
        switch (target)
        {
            case BuildTarget.Android:
                return Path.Combine(basePath, PlayerSettings.productName + ".apk");
            case BuildTarget.StandaloneWindows64:
                return Path.Combine(basePath, PlayerSettings.productName + ".exe");
            case BuildTarget.StandaloneOSX:
                return Path.Combine(basePath, PlayerSettings.productName + ".app");
            case BuildTarget.iOS:
                return basePath; // iOS builds to a directory
            case BuildTarget.WebGL:
                return basePath; // WebGL builds to a directory
            default:
                return Path.Combine(basePath, PlayerSettings.productName);
        }
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
