# 🧪 MR-Cube Testing & Validation Guide

Welcome to the comprehensive testing strategy for the MR-Cube Unity VR/MR project! This guide follows the **Carmack-Letterkenny philosophy**: "Test the fundamentals first, automate the boring stuff, and figger it out before it becomes a problem."

## 🎯 **Testing Philosophy**

Following our coding mastery principles:
- **Clarity over cleverness** - Tests should be obvious and maintainable
- **Consistency over perfection** - Regular testing beats perfect coverage
- **Automation over repetition** - Let scripts handle routine validation
- **Learning over knowing** - Tests teach us about our code

## 🏗️ **Testing Architecture**

### **Test Structure**
```
Assets/Tests/
├── Editor/                    # Edit mode tests
│   ├── EditorTests.asmdef     # Assembly definition
│   ├── MRCubeEditorTests.cs   # Basic project validation
│   └── BuildValidationTests.cs # Build configuration checks
└── Runtime/                   # Play mode tests
    ├── RuntimeTests.asmdef    # Assembly definition
    ├── PassthroughControllerTests.cs # Component behavior
    └── PerformanceTests.cs    # Performance & stress tests
```

### **Assembly Definitions**
- **MRCube.asmdef** - Main project code
- **MRCube.Tests.Editor.asmdef** - Editor tests
- **MRCube.Tests.Runtime.asmdef** - Runtime tests

## 🧪 **Test Categories**

### **1. Unit Tests (Component Level)**
**Location:** `Assets/Tests/Runtime/PassthroughControllerTests.cs`

**What they test:**
- Component creation/destruction
- Method parameter validation
- Error handling
- State management

**Example:**
```csharp
[Test]
public void PassthroughController_SetOpacity_ClampsValues()
{
    controller.SetOpacity(-0.5f);  // Should clamp to 0
    controller.SetOpacity(1.5f);   // Should clamp to 1
    // Verify clamping behavior
}
```

### **2. Integration Tests (System Level)**
**Location:** `Assets/Tests/Runtime/PerformanceTests.cs`

**What they test:**
- OVR SDK integration
- Multiple component interaction
- Memory usage patterns
- Performance characteristics

### **3. Build Validation Tests (Configuration)**
**Location:** `Assets/Tests/Editor/BuildValidationTests.cs`

**What they test:**
- Project settings correctness
- Package dependencies
- Android manifest configuration
- Platform-specific settings

### **4. Performance Tests**
**Location:** `Assets/Tests/Runtime/PerformanceTests.cs`

**What they test:**
- Method execution time
- Memory leak detection
- Stress testing with multiple instances
- Boundary value handling

## 🚀 **Running Tests**

### **Option 1: Unity Editor (Manual)**
1. Open Unity Editor
2. Window > General > Test Runner
3. Select "EditMode" or "PlayMode"
4. Click "Run All" or select specific tests

### **Option 2: Command Line (Automated)**
```bash
# Run all tests
cd /Users/joshuapatterson/Coding/tools
./test-runner.sh

# Run specific test types
./test-runner.sh edit      # Editor tests only
./test-runner.sh play      # Runtime tests only
./test-runner.sh coverage  # Code coverage analysis
./test-runner.sh validate  # Build validation only
```

### **Option 3: Continuous Integration**
The `test-runner.sh` script is designed for CI/CD integration:
- Returns proper exit codes
- Generates XML test results
- Provides detailed logging
- Handles Unity licensing

## 📊 **Test Results & Coverage**

### **Test Results Location**
- `test-results-EditMode.xml` - Editor test results
- `test-results-PlayMode.xml` - Runtime test results
- `coverage-results/` - Code coverage reports

### **Understanding Results**
```bash
# Quick results summary (if xmllint is available)
xmllint --xpath "count(//test-case[@result='Passed'])" test-results-EditMode.xml
xmllint --xpath "count(//test-case[@result='Failed'])" test-results-EditMode.xml
```

## 🔧 **Build Validation**

### **Automated Checks**
The build validation ensures:
- ✅ Proper company name (not "DefaultCompany")
- ✅ Correct bundle identifier
- ✅ Android SDK versions (Min 26+, Target 33+)
- ✅ ARM64 architecture for Quest
- ✅ IL2CPP scripting backend
- ✅ Linear color space for VR
- ✅ AndroidManifest.xml configuration
- ✅ Required package dependencies

### **Manual Validation**
Use Unity menu: **Build > Validate Configuration**

## 🎭 **VR-Specific Testing Challenges**

### **Challenge 1: Hardware Dependencies**
**Problem:** Can't test Quest features without hardware
**Solution:** 
- Mock OVR components in tests
- Test logic separately from hardware
- Use Unity XR Device Simulator

### **Challenge 2: Performance Validation**
**Problem:** VR requires consistent 72/90/120 FPS
**Solution:**
- Automated performance benchmarks
- Memory usage monitoring
- Frame time measurement

### **Challenge 3: User Comfort**
**Problem:** Motion sickness and comfort validation
**Solution:**
- Automated comfort metric checks
- Performance regression detection
- Guidelines compliance testing

## 📝 **Writing New Tests**

### **For New Components:**
```csharp
[Test]
public void NewComponent_BasicFunctionality()
{
    // Arrange
    var gameObject = new GameObject("Test");
    var component = gameObject.AddComponent<NewComponent>();
    
    // Act
    component.DoSomething();
    
    // Assert
    Assert.IsNotNull(component);
    
    // Cleanup
    Object.DestroyImmediate(gameObject);
}
```

### **For VR Features:**
```csharp
[UnityTest]
public IEnumerator VRFeature_InitializationTest()
{
    // Wait for VR system initialization
    yield return new WaitForSeconds(1.0f);
    
    // Test VR-specific functionality
    Assert.IsTrue(VRSystem.IsInitialized());
}
```

## 🔄 **Test Maintenance**

### **Regular Tasks:**
1. **Weekly:** Run full test suite
2. **Before builds:** Run validation tests
3. **After changes:** Run affected test categories
4. **Monthly:** Review and update test coverage

### **Test Quality Guidelines:**
- Tests should be **fast** (< 1 second each)
- Tests should be **independent** (no shared state)
- Tests should be **deterministic** (same result every time)
- Tests should be **clear** (obvious what they're testing)

## 🚨 **Troubleshooting**

### **Common Issues:**

**Tests fail in batch mode but pass in editor:**
- Check for Unity licensing issues
- Verify package dependencies
- Check for editor-only code paths

**Performance tests are flaky:**
- Run on consistent hardware
- Account for system load
- Use statistical averaging

**VR tests fail:**
- Ensure XR packages are properly installed
- Check for missing Android SDK components
- Verify Quest developer settings

### **Getting Help:**
1. Check Unity console for detailed error messages
2. Review test runner logs in `/tmp/`
3. Validate build configuration first
4. Run individual tests to isolate issues

## 🎉 **Success Criteria**

Your MR-Cube project is ready for deployment when:
- ✅ All unit tests pass
- ✅ Build validation passes
- ✅ Performance tests meet targets
- ✅ No memory leaks detected
- ✅ Quest deployment succeeds

---

*"The best debugging session is the one you never have to have because your tests caught the issue first."* - The Carmack-Letterkenny Principle

**Now go run those tests and figger it out!** 🇨🇦
