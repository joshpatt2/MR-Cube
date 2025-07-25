using UnityEngine;

namespace MRCube
{
    /// <summary>
    /// Simple debug logger to verify basic MonoBehaviour execution
    /// This is a baseline test to ensure scripts are running properly
    /// Only logs in editor and development builds
    /// </summary>
    public class SimpleDebugLogger : MonoBehaviour
    {
        void Start()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log("SimpleDebugLogger: Start() called - Basic MonoBehaviour execution confirmed!");
#endif
        }
        
        void Update()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            // Log every 120 frames (roughly every 2 seconds) to avoid spam
            if (Time.frameCount % 120 == 0)
            {
                Debug.Log($"SimpleDebugLogger: Update() frame {Time.frameCount} - Still running!");
            }
#endif
        }
    }
}
