using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;

public class SimpleBuildScript
{
    public static void BuildAndroid()
    {
        Debug.Log("SimpleBuildScript: Starting Android build...");
        
        string[] scenes = { "Assets/Scenes/SampleScene.unity" };
        string buildPath = "Builds/Android/MR-Cube.apk";
        
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = buildPath,
            target = BuildTarget.Android,
            options = BuildOptions.Development
        };
        
        Debug.Log($"SimpleBuildScript: Building to {buildPath}");
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        
        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log("SimpleBuildScript: Build succeeded!");
        }
        else
        {
            Debug.LogError($"SimpleBuildScript: Build failed: {report.summary.result}");
        }
    }
}
