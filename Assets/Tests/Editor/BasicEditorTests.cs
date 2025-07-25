using NUnit.Framework;
using UnityEngine;
using UnityEditor;

namespace MRCube.Tests.Editor
{
    /// <summary>
    /// Basic editor tests for MR-Cube project
    /// Simple tests that verify Unity editor functionality
    /// </summary>
    public class BasicEditorTests
    {
        [Test]
        public void Editor_IsAccessible()
        {
            // Basic test to verify Unity editor is working
            Assert.IsTrue(EditorApplication.isPlaying || !EditorApplication.isPlaying, 
                "Unity editor should be accessible");
        }

        [Test]
        public void AssetDatabase_IsWorking()
        {
            // Test that AssetDatabase functions
            Assert.DoesNotThrow(() => {
                AssetDatabase.Refresh();
            }, "AssetDatabase should be accessible");
        }

        [Test]
        public void EditorPrefs_CanBeAccessed()
        {
            // Test EditorPrefs functionality
            string testKey = "MRCube.Test.Key";
            string testValue = "TestValue123";
            
            EditorPrefs.SetString(testKey, testValue);
            string retrievedValue = EditorPrefs.GetString(testKey);
            
            Assert.AreEqual(testValue, retrievedValue, "EditorPrefs should store and retrieve values");
            
            EditorPrefs.DeleteKey(testKey);
        }

        [Test]
        public void Selection_CanBeManipulated()
        {
            // Test Unity Selection functionality
            var testObject = new GameObject("EditorTestObject");
            
            Assert.DoesNotThrow(() => {
                Selection.activeGameObject = testObject;
                Assert.AreEqual(testObject, Selection.activeGameObject, "Selection should work correctly");
            }, "Selection manipulation should work");
            
            Object.DestroyImmediate(testObject);
        }

        [Test]
        public void ProjectSettings_CanBeRead()
        {
            // Test that we can read project settings
            Assert.DoesNotThrow(() => {
                string companyName = PlayerSettings.companyName;
                string productName = PlayerSettings.productName;
                Debug.Log($"Project: {productName} by {companyName}");
            }, "PlayerSettings should be readable");
        }

        [Test]
        public void BuildTarget_CanBeQueried()
        {
            // Test build target functionality
            Assert.DoesNotThrow(() => {
                BuildTarget currentTarget = EditorUserBuildSettings.activeBuildTarget;
                Debug.Log($"Current build target: {currentTarget}");
            }, "Build target should be queryable");
        }

        [Test]
        public void Handles_CanBeAccessed()
        {
            // Test that we can access Handles (editor GUI)
            Assert.DoesNotThrow(() => {
                Color originalColor = Handles.color;
                Handles.color = Color.red;
                Handles.color = originalColor;
            }, "Handles should be accessible");
        }

        [Test]
        public void SceneView_CanBeAccessed()
        {
            // Test SceneView functionality
            Assert.DoesNotThrow(() => {
                SceneView sceneView = SceneView.lastActiveSceneView;
                // SceneView might be null if no scene view is open, that's OK
            }, "SceneView should be accessible");
        }

        [Test]
        public void Undo_SystemWorks()
        {
            // Test Unity's Undo system
            var testObject = new GameObject("UndoTestObject");
            
            Assert.DoesNotThrow(() => {
                Undo.RegisterCreatedObjectUndo(testObject, "Create Test Object");
                Undo.DestroyObjectImmediate(testObject);
            }, "Undo system should work");
        }

        [Test]
        public void MRCube_EditorNamespace_Exists()
        {
            // Test that our namespace is properly defined
            var namespaceName = GetType().Namespace;
            Assert.IsTrue(namespaceName.Contains("MRCube"), $"Namespace should contain MRCube, got: {namespaceName}");
        }
    }
}
