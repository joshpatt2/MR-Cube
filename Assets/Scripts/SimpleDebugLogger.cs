using UnityEngine;

namespace MRCube
{
    /// <summary>
    /// Simple debug logger to verify basic MonoBehaviour execution
    /// This is a baseline test to ensure scripts are running properly
    /// </summary>
    public class SimpleDebugLogger : MonoBehaviour
    {
        void Start()
        {
            Debug.Log("SimpleDebugLogger: Start() called - Basic MonoBehaviour execution confirmed!");
        }
        
        void Update()
        {
            // Log every 120 frames (roughly every 2 seconds) to avoid spam
            if (Time.frameCount % 120 == 0)
            {
                Debug.Log($"SimpleDebugLogger: Update() frame {Time.frameCount} - Still running!");
            }
        }
    }
}
