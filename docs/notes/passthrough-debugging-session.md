# MR-Cube Passthrough Debugging Session

**Date:** July 24, 2025  
**Unity Version:** 6000.0.53f1  
**Target Device:** Meta Quest 3 (ID: 2G0YC1ZF8B09YT)  
**Issue:** Passthrough functionality not working despite proper component setup

## 🔍 Problem Statement

The MR-Cube VR application has a PassthroughController component configured in the scene, but passthrough functionality is not activating on the Meta Quest 3 device. The app builds and deploys successfully, but passthrough remains off.

## 🛠️ Debugging Steps Completed

### 1. Android Manifest Configuration ✅
**File:** `Assets/Plugins/Android/AndroidManifest.xml`

**Changes Made:**
```xml
<!-- Added passthrough feature requirement -->
<uses-feature android:name="com.oculus.feature.PASSTHROUGH" android:required="true" />

<!-- Added scene permission -->
<uses-permission android:name="com.oculus.permission.USE_SCENE" />

<!-- Added VR metadata -->
<meta-data android:name="com.oculus.application.mode" android:value="vr" />
<meta-data android:name="com.oculus.intent.category.VR" android:value="vr_only" />
```

### 2. PassthroughController Script Enhancement ✅
**File:** `Assets/Scripts/PassthroughController.cs`

**Changes Made:**
- Added extensive debug logging in `Start()` method
- Enhanced component detection and error reporting
- Removed Input System dependency (keyboard input)
- Added initialization status logging

**Debug Messages Added:**
```csharp
Debug.Log("PassthroughController: Start() called");
Debug.Log("PassthroughController: OVRPassthroughLayer found");
Debug.Log($"PassthroughController: Initialized - Enabled: {startWithPassthroughEnabled}");
Debug.Log($"Passthrough set to: {enabled}");
```

### 3. Build System Integration ✅
**Tool:** `unity-builder.sh`

**Improvements Made:**
- Auto-installation feature added
- macOS compatibility fixes (`numfmt` fallback)
- Enhanced APK validation and deployment
- Automatic ADB device detection

**Build Results:**
- APK Size: ~59MB
- Build Time: ~1 minute
- Deployment: Successful via ADB
- App Launch: Successful

### 4. Runtime Debugging Setup ✅
**Method:** ADB logcat monitoring

**Commands Used:**
```bash
# Monitor Unity logs
adb logcat -s Unity

# Monitor PassthroughController specifically
adb logcat | grep PassthroughController

# Monitor VR runtime logs
adb logcat | grep -i passthrough
```

**Key Finding:** VR runtime logs show "PT is: OFF numLayers: 0"

## 📊 Current Status

### ✅ Working Components
- Unity 6 build system with Android VR target
- APK generation and deployment
- App launch on Meta Quest 3
- Android manifest permissions configured
- PassthroughController script attached to scene GameObject

### ❌ Issues Identified
- **No Debug Output**: PassthroughController debug logs not appearing in logcat
- **Passthrough Inactive**: VR runtime shows passthrough is OFF
- **Component Execution**: Unclear if PassthroughController.Start() is being called

### 🔧 Scene Configuration
**GameObject:** PassthroughLayer (ID: 2100000000)
- **OVRPassthroughLayer Component:** overlayType: 1, enabled
- **PassthroughController Component:** startWithPassthroughEnabled: true
- **Transform:** Position (0, 0, 0), default scale and rotation

## 🎯 Next Investigation Steps

### Priority 1: Script Execution Verification
- Check Unity build logs for script compilation errors
- Verify PassthroughLayer GameObject is active in scene hierarchy
- Test with simpler debug script to confirm MonoBehaviour execution

### Priority 2: OVR Manager Configuration
- Examine OVR Manager settings in Unity project
- Verify XR Plugin Management configuration
- Check if additional OVR/OpenXR setup is required

### Priority 3: Alternative Debugging Approaches
- Try manual passthrough activation via OVR SDK methods
- Test with minimal passthrough scene
- Compare with working Meta Quest passthrough examples

## 📁 Modified Files

1. **`Assets/Plugins/Android/AndroidManifest.xml`**
   - Added passthrough features and permissions

2. **`Assets/Scripts/PassthroughController.cs`** 
   - Enhanced debug logging and error handling

3. **`tools/unity-builder.sh`**
   - Added auto-installation and macOS compatibility

4. **Build Configuration**
   - Unity 6 VR project targeting Android
   - Development build with debugging enabled

## 🧠 Lessons Learned

1. **VR Debugging Requires Multi-Layer Approach:**
   - Android manifest configuration
   - Unity script debugging
   - Runtime VR service monitoring
   - Device-specific testing

2. **Debug Logging is Critical:**
   - Component initialization can silently fail
   - VR runtime services provide valuable status information
   - ADB logcat monitoring essential for mobile VR

3. **Build System Automation Improves Iteration:**
   - Auto-installation reduces development friction
   - Consistent build/deploy/test cycle
   - Enhanced validation catches issues early

## 🔗 Related Documentation

- [Unity VR Development Guide](/Mastering%20Coding%2C%20tips%2C%20tricks%20and%20methods/working%20with%20unity/unity-development-guide.md)
- [Unity Build Error Debugging](/Mastering%20Coding%2C%20tips%2C%20tricks%20and%20methods/debug-unity-build-errors.md)
- [Unity Builder Documentation](/tools/README-Unity-Builder.md)

---
**Status:** In Progress - Debugging passthrough activation  
**Last Updated:** July 24, 2025  
**Next Session:** Focus on script execution verification and OVR Manager settings
