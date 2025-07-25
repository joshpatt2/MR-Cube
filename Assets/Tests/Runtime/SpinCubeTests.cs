using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

namespace MRCube.Tests.Runtime
{
    /// <summary>
    /// Tests for cube spinning functionality
    /// </summary>
    public class SpinCubeTests
    {
        [Test]
        public void SpinCube_ScriptExists()
        {
            // Verify the SpinCube script can be found
            var spinCubeType = System.Type.GetType("MRCube.SpinCube, MRCube");
            if (spinCubeType == null)
            {
                // Try without assembly name
                spinCubeType = System.Type.GetType("MRCube.SpinCube");
            }
            Assert.IsNotNull(spinCubeType, "SpinCube script should exist in MRCube namespace");
        }

        [Test]
        public void SpinCube_CanBeCreatedOnGameObject()
        {
            // Test that SpinCube can be added to a GameObject
            var testObject = new GameObject("TestCube");
            var spinCube = testObject.AddComponent<MRCube.SpinCube>();
            
            Assert.IsNotNull(spinCube, "SpinCube component should be added successfully");
            Assert.IsTrue(spinCube.enabled, "SpinCube component should be enabled by default");
            
            Object.DestroyImmediate(testObject);
        }

        [UnityTest]
        public IEnumerator SpinCube_RotatesOverTime()
        {
            // Test that the cube actually rotates when SpinCube is attached
            var testObject = new GameObject("TestCube");
            var spinCube = testObject.AddComponent<MRCube.SpinCube>();
            
            // Record initial rotation
            Vector3 initialRotation = testObject.transform.eulerAngles;
            
            // Wait a frame for Update to be called
            yield return null;
            yield return null; // Wait an additional frame to ensure rotation has occurred
            
            // Check if rotation has changed
            Vector3 currentRotation = testObject.transform.eulerAngles;
            bool hasRotated = !Mathf.Approximately(initialRotation.x, currentRotation.x) ||
                             !Mathf.Approximately(initialRotation.y, currentRotation.y) ||
                             !Mathf.Approximately(initialRotation.z, currentRotation.z);
            
            Assert.IsTrue(hasRotated, "Cube should have rotated after Update calls");
            
            Object.DestroyImmediate(testObject);
        }

        [Test]
        public void SpinCube_HasConfigurableSpeed()
        {
            // Test that spin speeds can be configured
            var testObject = new GameObject("TestCube");
            var spinCube = testObject.AddComponent<MRCube.SpinCube>();
            
            // Test default values
            Assert.AreEqual(90f, spinCube.speedX, "Default speedX should be 90");
            Assert.AreEqual(90f, spinCube.speedY, "Default speedY should be 90");
            
            // Test that values can be changed
            spinCube.speedX = 45f;
            spinCube.speedY = 135f;
            
            Assert.AreEqual(45f, spinCube.speedX, "speedX should be configurable");
            Assert.AreEqual(135f, spinCube.speedY, "speedY should be configurable");
            
            Object.DestroyImmediate(testObject);
        }
    }
}
