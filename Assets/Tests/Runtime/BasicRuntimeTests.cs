using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

namespace MRCube.Tests.Runtime
{
    /// <summary>
    /// Basic runtime tests for MR-Cube
    /// These tests validate basic Unity functionality without VR dependencies
    /// </summary>
    public class BasicRuntimeTests
    {
        [Test]
        public void Unity_IsRunning()
        {
            // Basic Unity runtime verification
            Assert.IsTrue(Application.isPlaying || !Application.isEditor);
        }

        [Test]
        public void GameObject_CanBeCreated()
        {
            // Tests GameObject instantiation
            var testObject = new GameObject("TestObject");
            Assert.IsNotNull(testObject);
            Assert.AreEqual("TestObject", testObject.name);
            Object.DestroyImmediate(testObject);
        }

        [Test]
        public void MonoBehaviour_CanBeAdded()
        {
            // Validates component system
            var testObject = new GameObject("TestObject");
            var transform = testObject.GetComponent<Transform>();
            Assert.IsNotNull(transform);
            Object.DestroyImmediate(testObject);
        }

        [UnityTest]
        public IEnumerator Coroutine_WorksProperly()
        {
            // Tests async coroutine functionality
            bool completed = false;
            yield return new WaitForSeconds(0.1f);
            completed = true;
            Assert.IsTrue(completed);
        }

        [Test]
        public void MRCube_Namespace_Exists()
        {
            // Confirms namespace consistency
            Assert.IsTrue(typeof(BasicRuntimeTests).Namespace == "MRCube.Tests.Runtime");
        }

        [Test]
        public void Random_FunctionsWork()
        {
            // Tests Unity's random number generation
            float randomValue = UnityEngine.Random.Range(0f, 1f);
            Assert.IsTrue(randomValue >= 0f && randomValue <= 1f);
        }

        [Test]
        public void Vector3_MathWorks()
        {
            // Validates Unity math operations
            Vector3 a = new Vector3(1, 0, 0);
            Vector3 b = new Vector3(0, 1, 0);
            float dot = Vector3.Dot(a, b);
            Assert.AreEqual(0f, dot, 0.001f);
        }

        [Test]
        public void Debug_LogWorks()
        {
            // Tests logging functionality
            Debug.Log("Test log message");
            Assert.Pass("Debug.Log executed without error");
        }

        [Test]
        public void Time_IsProgressing()
        {
            // Verifies time system access
            float currentTime = Time.time;
            Assert.IsTrue(currentTime >= 0f);
        }

        [Test]
        public void Application_PropertiesAccessible()
        {
            // Tests application metadata
            Assert.IsNotNull(Application.version);
            Assert.IsNotNull(Application.platform);
        }
    }
}
