using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using MRCube;

namespace MRCube.Tests.Runtime
{
    /// <summary>
    /// Performance and stress tests for MR-Cube
    /// These tests validate performance characteristics and memory usage
    /// </summary>
    public class PerformanceTests
    {
        private GameObject testGameObject;
        private PassthroughController controller;
        
        [SetUp]
        public void SetUp()
        {
            testGameObject = new GameObject("PerformanceTest");
            controller = testGameObject.AddComponent<PassthroughController>();
        }

        [TearDown]
        public void TearDown()
        {
            if (testGameObject != null)
            {
                Object.DestroyImmediate(testGameObject);
            }
        }

        [Test]
        public void PassthroughController_SetOpacity_PerformanceTest()
        {
            // Simple performance test without Unity.PerformanceTesting package
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            // Measure performance of opacity changes
            for (int i = 0; i < 1000; i++)
            {
                controller.SetOpacity(UnityEngine.Random.Range(0f, 1f));
            }
            
            stopwatch.Stop();
            Debug.Log($"SetOpacity performance: {stopwatch.ElapsedMilliseconds}ms for 1000 calls");
            
            // Basic assertion - should complete in reasonable time
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 5000, "SetOpacity should complete quickly");
        }

        [Test]
        public void PassthroughController_Toggle_PerformanceTest()
        {
            // Simple performance test without Unity.PerformanceTesting package
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            // Measure performance of rapid toggling
            for (int i = 0; i < 100; i++)
            {
                controller.TogglePassthrough();
            }
            
            stopwatch.Stop();
            Debug.Log($"TogglePassthrough performance: {stopwatch.ElapsedMilliseconds}ms for 100 calls");
            
            // Basic assertion - should complete in reasonable time
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 2000, "TogglePassthrough should complete quickly");
        }

        [UnityTest]
        public IEnumerator PassthroughController_MemoryLeak_Test()
        {
            // Create and destroy controllers to check for memory leaks
            int initialMemory = (int)(UnityEngine.Profiling.Profiler.GetTotalAllocatedMemory(0) / 1024);
            
            for (int i = 0; i < 50; i++)
            {
                var go = new GameObject($"TempController_{i}");
                var tempController = go.AddComponent<PassthroughController>();
                yield return null; // Let one frame pass
                Object.DestroyImmediate(go);
            }
            
            // Force garbage collection
            System.GC.Collect();
            yield return new WaitForSeconds(0.1f);
            
            int finalMemory = (int)(UnityEngine.Profiling.Profiler.GetTotalAllocatedMemory(0) / 1024);
            int memoryDiff = finalMemory - initialMemory;
            
            // Allow some memory growth but not excessive
            Assert.IsTrue(memoryDiff < 1024, $"Memory leak detected: {memoryDiff}KB increase");
        }

        [Test]
        public void PassthroughController_StressTest_MultipleInstances()
        {
            // Test multiple controllers existing simultaneously
            var controllers = new PassthroughController[10];
            var gameObjects = new GameObject[10];
            
            for (int i = 0; i < 10; i++)
            {
                gameObjects[i] = new GameObject($"StressTest_{i}");
                controllers[i] = gameObjects[i].AddComponent<PassthroughController>();
                Assert.IsNotNull(controllers[i], $"Controller {i} failed to create");
            }
            
            // Test all controllers can be operated simultaneously
            for (int i = 0; i < 10; i++)
            {
                Assert.DoesNotThrow(() => controllers[i].SetOpacity(0.5f));
                Assert.DoesNotThrow(() => controllers[i].TogglePassthrough());
            }
            
            // Cleanup
            for (int i = 0; i < 10; i++)
            {
                Object.DestroyImmediate(gameObjects[i]);
            }
        }
        
        [Test]
        public void PassthroughController_EdgeCase_BoundaryValues()
        {
            // Test boundary values for opacity
            Assert.DoesNotThrow(() => controller.SetOpacity(-999f), "Negative opacity should not throw");
            Assert.DoesNotThrow(() => controller.SetOpacity(999f), "High opacity should not throw");
            Assert.DoesNotThrow(() => controller.SetOpacity(float.NaN), "NaN opacity should not throw");
            Assert.DoesNotThrow(() => controller.SetOpacity(float.PositiveInfinity), "Infinity should not throw");
        }
    }
}
