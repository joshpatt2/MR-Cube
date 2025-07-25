using UnityEngine;

namespace MRCube
{
    /// <summary>
    /// Simple passthrough controller for MR Cube project
    /// Enables/disables passthrough and provides visual feedback
    /// </summary>
    public class PassthroughController : MonoBehaviour
    {
        [Header("Passthrough Settings")]
        [SerializeField] private bool startWithPassthroughEnabled = true;
        [SerializeField] private float opacity = 1.0f;
        
        private OVRPassthroughLayer passthroughLayer;
        
        void Start()
        {
            Debug.Log("PassthroughController: Start() called");
            
            // Get the passthrough layer component
            passthroughLayer = GetComponent<OVRPassthroughLayer>();
            
            if (passthroughLayer == null)
            {
                Debug.LogError("PassthroughController: No OVRPassthroughLayer component found!");
                return;
            }
            
            Debug.Log("PassthroughController: OVRPassthroughLayer found");
            
            // Initialize passthrough state
            SetPassthroughEnabled(startWithPassthroughEnabled);
            
            Debug.Log($"PassthroughController: Initialized - Enabled: {startWithPassthroughEnabled}");
        }
        
        /// <summary>
        /// Enable or disable passthrough
        /// </summary>
        /// <param name="enabled">True to enable passthrough, false to disable</param>
        public void SetPassthroughEnabled(bool enabled)
        {
            if (passthroughLayer != null)
            {
                passthroughLayer.enabled = enabled;
                Debug.Log($"Passthrough set to: {enabled}");
            }
        }
        
        /// <summary>
        /// Toggle passthrough on/off
        /// </summary>
        public void TogglePassthrough()
        {
            if (passthroughLayer != null)
            {
                SetPassthroughEnabled(!passthroughLayer.enabled);
            }
        }
        
        /// <summary>
        /// Set passthrough opacity
        /// </summary>
        /// <param name="newOpacity">Opacity value between 0 and 1</param>
        public void SetOpacity(float newOpacity)
        {
            opacity = Mathf.Clamp01(newOpacity);
            if (passthroughLayer != null)
            {
                passthroughLayer.textureOpacity = opacity;
                Debug.Log($"Passthrough opacity set to: {opacity}");
            }
        }
        
        void Update()
        {
            // For now, just log that Update is called to verify the script is running
            // TODO: Add controller input for passthrough toggle
        }
    }
}
