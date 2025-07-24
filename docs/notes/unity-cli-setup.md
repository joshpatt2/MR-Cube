# Unity Command Line Project Loading

## Preferred Method: Direct Unity Executable

Always use the direct Unity executable path to open projects from the command line, rather than using `open -a Unity` or Unity Hub.

### Step 1: Find Available Unity Versions

```bash
find /Applications -name "Unity" -type f 2>/dev/null | head -10
```

This will show all Unity editor executables, for example:
- `/Applications/Unity/Hub/Editor/6000.0.53f1/Unity.app/Contents/MacOS/Unity`
- `/Applications/Unity/Hub/Editor/2022.3.62f1/Unity.app/Contents/MacOS/Unity`

### Step 2: Open Project with Specific Unity Version

Use the full path to the Unity executable with the `-projectPath` parameter:

```bash
"/Applications/Unity/Hub/Editor/6000.0.53f1/Unity.app/Contents/MacOS/Unity" -projectPath "/Users/joshuapatterson/Coding/MR-Cube"
```

### For MR-Cube Project Specifically:

```bash
# Current recommended command for MR-Cube project:
"/Applications/Unity/Hub/Editor/6000.0.53f1/Unity.app/Contents/MacOS/Unity" -projectPath "/Users/joshuapatterson/Coding/MR-Cube"
```

## Why This Method is Better:

1. **Direct Control**: Bypasses Unity Hub launcher issues
2. **Version Specific**: You can choose exactly which Unity version to use
3. **Reliable**: Works consistently across different macOS configurations
4. **Background Process**: Runs in background without blocking terminal
5. **No Dependencies**: Doesn't rely on Unity Hub being properly configured

## Alternative Methods (Less Reliable):

### Unity Hub Method:
```bash
# These methods are less reliable:
open -a "Unity Hub" --args --projectPath "/path/to/project"
open -a Unity /path/to/project
```

## Troubleshooting:

- If Unity doesn't open, check that the Unity version path exists
- Make sure the project path is correct and accessible
- Verify Unity license is activated for the specific version
- Check that no other Unity instances are running and blocking the launch

## Additional Unity CLI Options:

```bash
# Open Unity with specific options:
"/path/to/Unity" -projectPath "/path/to/project" -batchmode -quit -logFile -
"/path/to/Unity" -projectPath "/path/to/project" -buildTarget Android
```

---

**Last Updated**: July 23, 2025  
**Unity Version Used**: 6000.0.53f1  
**Project**: MR-Cube VR/AR Project
