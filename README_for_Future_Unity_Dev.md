# Welcome, Future Unity Developer!

This note is your guide to understanding the MR-Cube project and the broader coding mastery system it lives within. Here’s what you need to know to get up to speed, avoid common mistakes, and build on the strong foundation already in place.

## 1. Philosophy & Workflow
- **Carmack-Letterkenny Principle:** Always understand the existing system before trying to improve it. Investigate, read, and learn before acting.
- **Automation First:** Use the provided tools (especially `commit-all` for git) instead of manual commands. Check the `tools/` directory and read all tool documentation before scripting anything new.
- **Documentation is Key:** The main `README.md` and the `READ THIS FIRST.md` file set the tone and expectations for all work. Read them fully.

## 2. Required Reading
- `READ THIS FIRST.md` — Philosophy, learning order, and the Carmack-Letterkenny conversation.
- `code-quality.md`, `git-mastery.md`, `shell-scripting.md`, `private-repo-security.md` — Core best practices.
- `Journal/` — Especially the current month’s file (e.g., `2025-07-July.md`). This is where all recent problems, solutions, and lessons are documented.
- `working with unity/` — Contains deep dives, case studies, and textbook-level Unity knowledge.

## 3. MR-Cube Project Structure
- **Scripts:** Main logic is in `Assets/Scripts/PassthroughController.cs` (handles passthrough for VR/MR, uses reflection for OVR compatibility).
- **Scenes:** Main scene is `Assets/Scenes/SampleScene.unity`. Investigate GameObjects and components directly in YAML if needed.
- **Tests:** See `Assets/Tests/` and `test-validation-summary.md` for test coverage and validation commands.
- **Builds:** Use the `tools/build` and `tools/unity-builder.sh` scripts for building and deploying to Quest devices. These scripts are robust and handle most edge cases (see journal for troubleshooting).

## 4. Debugging & Problem Solving
- **Always check the journal** for similar issues before troubleshooting.
- **Use logcat and debug logging** for runtime issues, especially with VR passthrough and OVR components.
- **Direct YAML editing** of scene files is sometimes faster and more reliable than using the Unity Editor for bulk or precise changes.

## 5. Testing & Validation
- All tests must pass before merging changes. See `test-validation-summary.md` for the current test suite and validation commands.
- Tests are split into Editor and Runtime (PlayMode) categories, and are CI/CD ready.

## 6. Learning Resources
- The `working with unity/` directory contains:
  - `unity-development-guide.md` — Practical Unity tips and YAML editing.
  - `Unity Game Design Textbook Outline.md` — Deep dive into Unity’s philosophy and architecture.
  - `carmack-master-class.md` — Game design and engineering principles.
  - `scene-investigation-case-study.md` — Real-world debugging and scene editing.
  - `test-validation-summary.md` — Test results and project evolution.

## 7. Final Advice
- **Read before you act.** Most mistakes come from skipping documentation or not checking the journal.
- **Embrace the philosophy:** Clarity, consistency, automation, and learning are valued above cleverness or speed.
- **Document your work** in the journal for future devs (and yourself).

---

*If you follow these principles and use the resources provided, you’ll be able to understand, maintain, and extend the MR-Cube project—and contribute to a culture of mastery and continuous improvement.*

— GitHub Copilot, July 2025
