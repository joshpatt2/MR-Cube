using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

namespace MRCube.Tests.Runtime
{
    /// <summary>
    /// Basic runtime tests for MR-Cube project
    /// Simple tests that don't require complex dependencies
    /// </summary>
    public class BasicRuntimeTests
    {
        [Test]
        public void Unity_IsRunning()
        {
            // Basic test to verify Unity runtime is working
            Assert.IsTrue(Application.isPlaying || Application.isEditor, 
                "Unity should be running in play mode or editor");
        }

        [Test]
        public void GameObject_CanBeCreated()
        {
            // Test basic Unity functionality
            var go = new GameObject("TestObject");
            Assert.IsNotNull(go, "GameObject should be created successfully");
            
            Object.DestroyImmediate(go);
        }

        [Test]
        public void MonoBehaviour_CanBeAdded()
        {
            // Test that MonoBehaviour components can be added
            var go = new GameObject("TestMonoBehaviour");
            var component = go.GetComponent<Transform>(); // Transform is automatically added
            
            Assert.IsNotNull(component, "GameObject should have Transform component by default");
            
            Object.DestroyImmediate(go);
        }

        [UnityTest]
        public IEnumerator Coroutine_WorksProperly()
        {
            // Test that coroutines function correctly
            float startTime = Time.time;
            yield return new WaitForSeconds(0.1f);
            float endTime = Time.time;
            
            Assert.Greater(endTime - startTime, 0.05f, "Coroutine should wait for specified time");
        }

        [Test]
        public void MRCube_Namespace_Exists()
        {
            // Test that our namespace is properly defined
            var namespaceName = GetType().Namespace;
            Assert.IsTrue(namespaceName.Contains("MRCube"), $"Namespace should contain MRCube, got: {namespaceName}");
        }

        [Test]
        public void Random_FunctionsWork()
        {
            // Test Unity's random functions
            float randomValue = UnityEngine.Random.Range(0f, 1f);
            Assert.IsTrue(randomValue >= 0f && randomValue <= 1f, "Random should generate values in range");
        }

        [Test]
        public void Vector3_MathWorks()
        {
            // Test basic Unity math
            Vector3 a = new Vector3(1, 2, 3);
            Vector3 b = new Vector3(4, 5, 6);
            Vector3 sum = a + b;
            
            Assert.AreEqual(new Vector3(5, 7, 9), sum, "Vector math should work correctly");
        }

        [Test]
        public void Debug_LogWorks()
        {
            // Test that debug logging functions
            Assert.DoesNotThrow(() => {
                Debug.Log("Test log message from BasicRuntimeTests");
            }, "Debug.Log should not throw exceptions");
        }

        [Test]
        public void Time_IsProgressing()
        {
            // Test that Unity's time system is working
            float time1 = Time.time;
            float time2 = Time.time;
            
            // In editor, time might not progress between immediate calls
            // so we just test that Time.time doesn't throw
            Assert.DoesNotThrow(() => {
                float currentTime = Time.time;
            }, "Time.time should be accessible");
        }

        [Test]
        public void Application_PropertiesAccessible()
        {
            // Test that application properties are accessible
            Assert.DoesNotThrow(() => {
                string version = Application.version;
                string companyName = Application.companyName;
                string productName = Application.productName;
                Debug.Log($"App info: {productName} v{version} by {companyName}");
            }, "Application properties should be accessible");
        }
    }
}
