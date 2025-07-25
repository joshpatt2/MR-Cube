using UnityEngine;
using System.Collections;

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
            Debug.Log($"PassthroughController: Initial layer enabled state = {passthroughLayer.enabled}");
            Debug.Log($"PassthroughController: Overlay type = {passthroughLayer.overlayType}");
            Debug.Log($"PassthroughController: Texture opacity = {passthroughLayer.textureOpacity}");
            
            // Initialize passthrough state
            SetPassthroughEnabled(startWithPassthroughEnabled);
            
            Debug.Log($"PassthroughController: Initialized - Enabled: {startWithPassthroughEnabled}");
            
            // Additional debugging: Check OVR Manager passthrough settings
            var ovrManager = FindFirstObjectByType<OVRManager>();
            if (ovrManager != null)
            {
                Debug.Log($"PassthroughController: OVRManager found - isInsightPassthroughEnabled = {ovrManager.isInsightPassthroughEnabled}");
            }
            else
            {
                Debug.LogError("PassthroughController: No OVRManager found in scene!");
            }
            
            // Try delayed activation in case the VR system needs time to initialize
            StartCoroutine(DelayedPassthroughActivation());
            
            // Check OVR SDK status
            CheckOVRPassthroughStatus();
        }
        
        /// <summary>
        /// Try to activate passthrough after a delay to allow VR system initialization
        /// </summary>
        private System.Collections.IEnumerator DelayedPassthroughActivation()
        {
            Debug.Log("PassthroughController: DelayedPassthroughActivation starting...");
            yield return new WaitForSeconds(2.0f); // Wait 2 seconds
            
            Debug.Log("PassthroughController: Attempting delayed passthrough activation");
            ForcePassthroughActivation();
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
        
        /// <summary>
        /// Force passthrough activation using lower-level OVR SDK
        /// </summary>
        public void ForcePassthroughActivation()
        {
            Debug.Log("PassthroughController: ForcePassthroughActivation() called");
            
            if (passthroughLayer == null)
            {
                Debug.LogError("PassthroughController: Cannot force activation - no passthroughLayer!");
                return;
            }
            
            // Try to explicitly create and start the passthrough layer
            try
            {
                passthroughLayer.enabled = false;
                passthroughLayer.enabled = true;
                passthroughLayer.textureOpacity = 1.0f;
                Debug.Log("PassthroughController: Forced layer reset and activation");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"PassthroughController: Exception during force activation: {e.Message}");
            }
        }
        
        /// <summary>
        /// Check OVR SDK passthrough status directly
        /// </summary>
        public void CheckOVRPassthroughStatus()
        {
            Debug.Log("PassthroughController: CheckOVRPassthroughStatus() called");
            
            // Check if OVR Plugin is available and passthrough is supported
            if (OVRPlugin.GetSystemHeadsetType() != OVRPlugin.SystemHeadset.None)
            {
                Debug.Log($"PassthroughController: OVR headset detected: {OVRPlugin.GetSystemHeadsetType()}");
                
                // Check basic OVR runtime status
                Debug.Log($"PassthroughController: OVR initialized: {OVRPlugin.initialized}");
                Debug.Log($"PassthroughController: OVR version: {OVRPlugin.version}");
            }
            else
            {
                Debug.LogWarning("PassthroughController: No OVR headset detected or OVR Plugin not available");
            }
        }
        
        void Update()
        {
            // Log every 60 frames (roughly once per second) to verify script execution
            if (Time.frameCount % 60 == 0)
            {
                Debug.Log($"PassthroughController: Update() frame {Time.frameCount} - Script is running!");
                
                if (passthroughLayer != null)
                {
                    Debug.Log($"PassthroughController: Layer enabled = {passthroughLayer.enabled}, opacity = {passthroughLayer.textureOpacity}");
                }
            }
            
            // For now, just log that Update is called to verify the script is running
            // TODO: Add controller input for passthrough toggle
        }
    }
}
