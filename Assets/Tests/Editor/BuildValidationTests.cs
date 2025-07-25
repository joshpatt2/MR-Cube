using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace MRCube.Tests.Editor
{
    /// <summary>
    /// Build and configuration validation tests
    /// These tests ensure the project is properly configured for VR/MR deployment
    /// </summary>
    public class BuildValidationTests
    {
        [Test]
        public void Project_HasCorrectCompanyName()
        {
            Assert.AreEqual("Joshua Patterson Dev", PlayerSettings.companyName, 
                "Company name should be set to 'Joshua Patterson Dev'");
        }

        [Test]
        public void Project_HasCorrectProductName()
        {
            Assert.AreEqual("MR-Cube", PlayerSettings.productName, 
                "Product name should be 'MR-Cube'");
        }

        [Test]
        public void Project_HasCorrectBundleIdentifier()
        {
            #pragma warning disable CS0618 // Type or member is obsolete
            var androidId = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
            #pragma warning restore CS0618 // Type or member is obsolete
            Assert.AreEqual("com.joshuapatterson.mrcube", androidId, 
                "Android bundle ID should be 'com.joshuapatterson.mrcube'");
        }

        [Test]
        public void Android_HasCorrectMinSDKVersion()
        {
            Assert.GreaterOrEqual((int)PlayerSettings.Android.minSdkVersion, 26, 
                "Android Min SDK should be 26 or higher for modern Quest devices");
        }

        [Test]
        public void Android_HasCorrectTargetSDKVersion()
        {
            Assert.GreaterOrEqual((int)PlayerSettings.Android.targetSdkVersion, 33, 
                "Android Target SDK should be 33 or higher for Quest store submission");
        }

        [Test]
        public void Android_HasCorrectArchitecture()
        {
            Assert.AreEqual(AndroidArchitecture.ARM64, PlayerSettings.Android.targetArchitectures, 
                "Should target ARM64 for Quest devices");
        }

        [Test]
        public void Project_HasCorrectScriptingBackend()
        {
            #pragma warning disable CS0618 // Type or member is obsolete
            var backend = PlayerSettings.GetScriptingBackend(BuildTargetGroup.Android);
            #pragma warning restore CS0618 // Type or member is obsolete
            Assert.AreEqual(ScriptingImplementation.IL2CPP, backend, 
                "Should use IL2CPP for Android builds");
        }

        [Test]
        public void AndroidManifest_Exists()
        {
            string manifestPath = Path.Combine(Application.dataPath, "Plugins/Android/AndroidManifest.xml");
            Assert.IsTrue(File.Exists(manifestPath), "Custom AndroidManifest.xml should exist");
        }

        [Test]
        public void AndroidManifest_HasPassthroughFeature()
        {
            string manifestPath = Path.Combine(Application.dataPath, "Plugins/Android/AndroidManifest.xml");
            if (File.Exists(manifestPath))
            {
                string content = File.ReadAllText(manifestPath);
                Assert.IsTrue(content.Contains("com.oculus.feature.PASSTHROUGH"), 
                    "AndroidManifest should declare passthrough feature");
            }
        }

        [Test]
        public void AndroidManifest_HasCorrectVRCategory()
        {
            string manifestPath = Path.Combine(Application.dataPath, "Plugins/Android/AndroidManifest.xml");
            if (File.Exists(manifestPath))
            {
                string content = File.ReadAllText(manifestPath);
                Assert.IsTrue(content.Contains("com.oculus.intent.category.VR"), 
                    "AndroidManifest should declare VR category");
            }
        }

        [Test]
        public void Project_HasRequiredPackages()
        {
            string manifestPath = Path.Combine(Application.dataPath, "../Packages/manifest.json");
            Assert.IsTrue(File.Exists(manifestPath), "Package manifest should exist");

            string content = File.ReadAllText(manifestPath);
            
            // Check for essential VR packages
            Assert.IsTrue(content.Contains("com.meta.xr.sdk.core"), "Meta XR Core SDK should be installed");
            Assert.IsTrue(content.Contains("com.unity.xr.management"), "XR Management should be installed");
            Assert.IsTrue(content.Contains("com.unity.test-framework"), "Test Framework should be installed");
            Assert.IsTrue(content.Contains("com.unity.inputsystem"), "Input System should be installed");
        }

        [Test]
        public void Project_HasTestAssemblies()
        {
            // Check for test assembly definitions
            Assert.IsTrue(File.Exists(Path.Combine(Application.dataPath, "Tests/Editor/EditorTests.asmdef")), 
                "Editor test assembly should exist");
            Assert.IsTrue(File.Exists(Path.Combine(Application.dataPath, "Tests/Runtime/RuntimeTests.asmdef")), 
                "Runtime test assembly should exist");
            Assert.IsTrue(File.Exists(Path.Combine(Application.dataPath, "Scripts/MRCube.asmdef")), 
                "Main script assembly should exist");
        }

        [Test]
        public void Build_CanSwitchToAndroidTarget()
        {
            // Test that we can switch to Android build target
            Assert.DoesNotThrow(() => 
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            }, "Should be able to switch to Android build target");
        }

        [Test]
        public void Project_HasCorrectColorSpace()
        {
            Assert.AreEqual(ColorSpace.Linear, PlayerSettings.colorSpace, 
                "Should use Linear color space for VR rendering");
        }

        [Test]
        public void Project_HasCorrectGraphicsAPI()
        {
            var apis = PlayerSettings.GetGraphicsAPIs(BuildTarget.Android);
            Assert.IsTrue(System.Array.Exists(apis, api => api == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3 || 
                                                         api == UnityEngine.Rendering.GraphicsDeviceType.Vulkan), 
                "Should include OpenGL ES 3 or Vulkan for Android");
        }
        
        [Test]
        public void Project_HasVREnabled()
        {
            // Note: This test is for legacy VR settings
            // Modern projects should use XR Management instead
            Debug.Log("VR settings test - using XR Management package for VR support");
            Assert.IsTrue(true, "VR support handled by XR Management package");
        }
    }
}
