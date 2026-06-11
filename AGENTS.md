<!-- AGENTS.md — IcoBeat (Super-Deng / Super 灯) -->

> **Purpose**: This file exists to orient AI coding agents who have zero prior knowledge of the project. Read this before modifying code, adding features, or refactoring.

---

## Project Overview

**IcoBeat** (also known as *Super-Deng* / *Super 灯*) is a rhythm-based action game developed by **DapokaStudio**. The player navigates a character across the triangular faces of an icosahedral sphere, dodging obstacles and interacting with beat-synchronized events.

- **Engine**: Unity 2022.3.62f3 LTS
- **Language**: C# 9.0 targeting .NET Standard 2.1
- **Primary Platform**: Standalone Windows 64-bit
- **Repo Root**: `d:/Games that I'm trying to make/Super 灯/Super-Deng`

---

## Technology Stack

### Core Unity Packages
| Package | Version | Purpose |
|---------|---------|---------|
| `com.unity.cinemachine` | 3.1.6 | Camera control |
| `com.unity.inputsystem` | 1.14.0 | Installed but **not used for gameplay** (see Input section) |
| `com.unity.localization` | 1.5.2 | Multi-language support |
| `com.unity.nuget.newtonsoft-json` | 3.2.1 | JSON serialization for save system |
| `com.unity.postprocessing` | 3.4.0 | Full-screen post-processing effects |
| `com.unity.textmeshpro` | 3.0.9 | Text rendering |
| `com.unity.timeline` | 1.7.7 | Cutscenes and scripted sequences |
| `com.unity.visualscripting` | 1.9.4 | Available; gameplay is code-driven |
| `com.unity.2d.animation` | 9.2.2 | 2D animation features |
| `com.unity.collab-proxy` | 2.7.1 | Version control integration |
| URP (Universal Render Pipeline) | — | Configured in `GraphicsSettings.asset`; not visible in `manifest.json` |

> **Note**: The Addressable Assets system is configured (`Assets/AddressableAssetsData/`) but only `Built In Data` and `Default Local Group` are set up.

### Third-Party / Asset Store Integrations
| Asset | Location in Project | Purpose |
|-------|---------------------|---------|
| **Steamworks.NET** | `Assets/com.rlabrecque.steamworks.net/` | Steam API integration (leaderboards, achievements) |
| **RetroTVFX** | `Packages/com.glairedaggers.retrotvfx` + `Assets/Retro 3D Shader Pack for Unity/` | Retro CRT / post-processing aesthetic |
| **NiceVibrations** | Separate `.csproj` in root | Haptic feedback |
| **MoreMountains.Tools** | Separate `.csproj` in root | Utility toolkit |
| **INab Studio** | `Assets/INab Studio/` | Advanced edge detection post-processing |
| **ShowLasers** | `Assets/ShowLasers/` | Laser VFX |
| **PeerPlay** | `Assets/PeerPlay/` | Procedural VFX (Phyllotaxis trails) |
| **VHS** | `Assets/VHS/` | VHS tape post-processing effects |
| **PostEffects** | `Assets/PostEffects/` | Custom image effects (Bloom, RGB shift, Scanlines, etc.) |

---

## Project Structure

### Top-Level Folders
```
Assets/
  Scripts/           # All C# gameplay and menu code (~224 .cs files)
  Scenes/            # Unity scenes
  ScriptableObjects/ # Data assets (scenarios, settings, material configs)
  Prefabs/           # GameObject prefabs (Player, faces, UI, etc.)
  Input/             # Input System action maps (currently empty)
  Animation/         # Legacy animations
  Animations/        # Animator controllers & clips
  Models/            # 3D meshes
  Materials/         # URP materials
  Sprites/           # 2D sprites & cursors
  Fonts/             # Custom fonts
  Music/             # OST / level music tracks
  Sounds/            # SFX
  StreamingAssets/   # Runtime-loaded files
  PostProcessing/    # Post-processing profiles
  TextMesh Pro/      # TMP fonts, examples, shaders
```

### Code Organization (`Assets/Scripts/`)
The codebase follows a **layered MVP-ish architecture**. Each gameplay domain has its own folder with sub-folders for layers:

| Folder | Role | Domains Inside |
|--------|------|----------------|
| `GameplayScripts/` | **Core gameplay** (~153 scripts) | Field, Player, Time&Rhythm, Actions, Abilities, Camera, Background, Input, DataBase/SaveSystem, Other Stuff |
| `MenuScripts/` | **Main menu stack** (~57 scripts) | Buttons, windows, settings, secrets, credits, camera, sound |
| `Editor/` | **Custom Inspector drawers** (~15 scripts) | Action settings editors (`ActionSettingsEditor` hierarchy) |
| `Steamworks.NET/` | **Steam wrapper** (1 script) | `SteamManager.cs` |
| `OldScripts/` | **Deprecated code** (`.txt` files) | Legacy implementations kept for reference |
| `GameScripts/` | **Legacy leftover** (1 script) | `FaceIdAssignerScript.cs` — do not add new code here |

#### Domain Layer Breakdown (inside `GameplayScripts/`)
Each domain typically contains:
- `Main/` — initializer / orchestrator scripts
- `Controller/` — input adapters
- `Interactor/` — business logic & state management
- `Presenter/` — translation from game state to view commands
- `View/` — visual feedback (material changes, UI updates, camera zoom)
- `Database/` — data definitions, save/load helpers

> **Important**: The old `GameScripts/` folder referenced in earlier documentation is **obsolete**. All active gameplay code lives in `GameplayScripts/`.

### Script Naming Conventions
| Construct | Convention | Example |
|-----------|------------|---------|
| Classes / Structs | PascalCase + `Script` suffix | `LevelTimeManagementScript` |
| Interfaces | `I` + PascalCase + `Script` suffix | `IPlayerMovementPresenterScript` |
| Methods | PascalCase | `InitializePlayer()` |
| Private fields | camelCase, `[SerializeField]` | `private float beatInterval;` |
| Public properties | PascalCase, expression-bodied when simple | `public float CurrentBeat => currentBeat;` |
| Enums | PascalCase members | `ActionType`, `BonusType` |
| ScriptableObjects | PascalCase, often end in `DataBase`, `Settings`, or `Script` | `ActionScenarioDataBase`, `RedFaceSettings` |

### Comments
- Comments are **predominantly in English**.
- Occasional **Russian** comments appear in some editor scripts and builder utilities (e.g. `ActionFaceSettingsEditor`, `FaceCylinderBuilderScript`, `GoodBananaScript`).
- Complex geometry includes ASCII art diagrams (see `FaceScript.cs`).

---

## Architecture Deep Dive

### 1. Game Initialization Flow
1. `LevelInitializerScript.Awake()` wires all serialized references.
2. Music starts; `LevelRhythmManagementScript` calculates beat intervals from BPM.
3. `FieldInitializerScript` calls a builder (`FaceIcosphereBuilderScript`, etc.) to generate the triangular face grid.
4. `PlayerInitializerScript` places the player on a starting face.
5. `ActionInitializerScript` loads the level's `ActionScenarioDataBase` + `ActionBasicSettingsDataBase` and schedules enemy/item/effect spawns.
6. Countdown starts (`StartCountDownInteractorScript`); gameplay begins.

### 2. Rhythm System
- `LevelRhythmManagementScript` implements `IRhythmableScript`.
- BPM drives `beatInterval = 60 / bpm`.
- Beat counting is sample-accurate: `timeSamples / (frequency * interval)`.
- Coroutine `SynchronizeAndTurnOn()` aligns gameplay start to the music beat.

### 3. Field & Faces
- The playable field is composed of **triangular faces** (`FaceScript`) arranged into various geometries.
- **Builders** generate geometry at runtime:
  - `FaceIcosahedronBuilderScript` — 20 faces
  - `FaceIcosphereBuilderScript` — subdivided icosphere (80 or 320 faces)
  - `FaceGridBuilderScript` — flat grid
  - `FaceCylinderBuilderScript` — cylindrical layout
  - `FaceTubeBuilderScript` — tube layout
- Each face has 3 sides (`side1`, `side2`, `side3`) and a `FaceStateScript` tracking properties (`HavePlayer`, `IsRight`, `IsTop`, `IsLeft`, `TransferInProgress`, etc.).
- `FieldAssemblerScript` / `FieldDisassemblerScript` handle dynamic restructuring.

### 4. Action System (Enemies, Effects, Items)
- **ActionScenarioDataBase** (ScriptableObject) holds an ordered array of `ActionSettingsScript` entries defining *what* spawns and *when*.
- **ActionBasicSettingsDataBase** holds default parameters for each action type.
- Concrete actions inherit from `ActionScript`:
  - `SpawnerActionScript` — enemy spawners (`RedFace`, `RedWave`, `FallFace`, `BonusFace`)
  - `FieldEffect` scripts — dynamic field modifications (`FaceDance`, `StripDance`)
  - `NonFieldEffect` scripts — camera / screen effects (`RGBSuddenEffect`, `CameraRotationSuddenEffect`)
  - `ItemSpawner` — portals, bonuses
- **Custom Editors** (`ActionSettingsEditor` subclasses) provide polished inspector UIs for designers to tweak these ScriptableObjects.

### 5. Player Movement & Beat Sync
- `InputKeyBoardControllerScript` polls the legacy `Input.GetKeyDown` API for A–Z keys and forwards to `InputHandlerScript` subclasses.
- `PlayerMovementInteractorScript` validates moves against adjacent faces and beat timing.
- `PlayerBeatSyncValidatorScript` checks whether the player pressed on-beat.
- `PlayerMovementPresenterScript` / `PlayerMovementViewScript` handle visual feedback.
- **Abilities**: `PlayerAbilityTauntInteractorScript`, `PlayerAbilityRedFaceInteractorScript`, `PlayerAbilityPortalFaceInteractorScript`, `PlayerAbilityJumpFaceInteractorScript`. Some are fully implemented; `JumpFace` and `PortalFace` abilities currently throw `NotImplementedException`.

### 6. Menu Architecture
- `MenuController` is the central orchestrator for the main menu.
- `WindowInteractorScript` / `WindowViewScript` implement draggable/settings/credits windows.
- Secret inputs (Konami code, annihilation password) have dedicated `*SecretRepositoryScript` and `*InteractorScript` classes.
- Menu buttons use a transition system with configurable `AnimationCurve`s.

---

## Build & Development Workflow

### Opening the Project
1. Launch **Unity Hub** → Add project from `d:/Games that I'm trying to make/Super 灯/Super-Deng`
2. Open with **Unity 2022.3.62f3** (exact version recommended).

### Scenes
| Scene | Purpose | In Build? |
|-------|---------|-----------|
| `StartMenu.unity` | Main menu, level select, settings, credits | ✅ Yes (index 0) |
| `IcoScene.unity` | Core gameplay scene | ✅ Yes (index 1) |
| `TutorialScene.unity` | Tutorial | ❌ No |
| `TestScene.unity` | Development / experiment scene | ❌ No |
| `BlackApple.unity` | Secret level | ❌ No |
| `CaramelDansen.unity` | Secret level | ❌ No |
| `HitogataCutScene.unity` | Cutscene / secret | ❌ No |
| `PasswordHintScene.unity` | Secret / hint scene | ❌ No |

Secret levels are loaded at runtime via `SceneManager.LoadScene(index)` from menu logic, not via `EditorBuildSettings`.

### Building
- **Target**: Standalone Windows 64-bit (configured in Build Settings).
- **Scenes in build**: `StartMenu.unity` (0), `IcoScene.unity` (1).
- Burst compilation is enabled for Android, StandaloneWindows, WebGL, and WSAPlayer.
- No automated build scripts / CI pipelines are present in the repository. Builds are produced manually via the Unity Editor (`File → Build Settings → Build`).

### Generated Files
- `.csproj`, `.sln`, and `.csproj.lscache` files are **auto-generated by Unity**; do not hand-edit them. They are already in `.gitignore`.
- `Library/`, `Temp/`, `obj/`, `Logs/`, `UserSettings/` are also gitignored.

---

## Code Style Guidelines

### Naming
Follow the table in **Script Naming Conventions** above. All classes must end with `Script`. Interfaces must end with `Script` as well (`I...Script`).

### Inspector Best Practices
- Use `[Header("...")]` and `[Space]` to organize serialized fields.
- Use `[SerializeReference]` for polymorphic arrays in ScriptableObjects (e.g., `ActionSettingsScript[]`).
- Wire dependencies via `[SerializeField]` rather than `FindObjectOfType` or `GameObject.Find` where possible.
- Use `[Tooltip("...")]` for non-obvious fields.

### Input
- **Do not use the new Input System for gameplay** — the project relies on legacy `Input.GetKeyDown` polling inside `InputKeyBoardControllerScript`.
- Key bindings are stored in `KeyBindingDataScript` / `MovementKeyBindingDataScript` / `AbilityKeyBindingDataScript` assets.
- If you need to add input, extend `InputHandlerScript` and register it in `InputKeyBoardControllerScript`.

---

## Testing

- **There is no automated test suite** (no NUnit, no Test Runner assemblies, no PlayMode/EditMode tests).
- All validation is manual / play-mode testing inside the Unity Editor.
- When adding new features, test through the `IcoScene` or `TestScene`.

---

## Data & Persistence

### Save System
- `JsonDataServiceScript` implements `IDataServiceScript`.
- Saves are stored as JSON inside `Application.persistentDataPath`.
- `LevelSaveData` tracks level progression.
- Key bindings are saved via `KeyBindingDataScript`.

### ScriptableObject Assets
Designer-configurable data lives in `Assets/ScriptableObjects/`:
- `ActionScenarioDataBase` — level spawn timelines
- `ActionBasicSettingsDataBase` — default action parameters
- `MaterialSettings` — runtime material references
- Sub-folders (`RedFace/`, `BonusFace/`, `Portal/`, etc.) hold per-action setting assets.

---

## Steam Integration

- `SteamManager.cs` (from Steamworks.NET) is a singleton that initializes the Steam API on startup.
- Conditionally compiled with `#if !DISABLESTEAMWORKS`.
- If Steam is not running or the platform is unsupported, the game still launches in offline mode.
- **Do not** remove or rename `SteamManager` — other scripts may depend on `SteamManager.Initialized`.

---

## Security & Deployment Considerations

- **No secrets / API keys** are stored in plain text in the repository.
- Steam App ID is managed by the Steamworks.NET plugin (usually `steam_appid.txt` in the build output, not in source control).
- `.gitignore` is the standard GitHub Unity template; it correctly excludes `Library/`, `Temp/`, build artifacts, and IDE caches.
- Addressable asset bundles may contain large binary data — ensure they are tracked via Git LFS if they move into version control.

---

## Quick Reference for Agents

### I want to add a new enemy type
1. Create a new `ActionScript` subclass in `Assets/Scripts/GameplayScripts/Actions/Interactor/EnemySpawner/<YourEnemy>/`.
2. Create matching `*Settings` and `*BasicSettings` ScriptableObject classes.
3. Add a custom editor in `Assets/Scripts/Editor/` inheriting from `ActionSettingsEditor`.
4. Add the new `ActionType` enum value in `Assets/Scripts/GameplayScripts/Other Stuff/Types/ActionType.cs`.
5. Create prefabs in `Assets/Prefabs/GamePrefabs/`.
6. Add entries to `ActionScenarioDataBase` assets for levels that use it.

### I want to add a new level
1. Duplicate `IcoScene.unity` or use the existing one as a base.
2. Create a new `ActionScenarioDataBase` asset describing the spawn sequence.
3. Assign the scenario asset to the scene's `LevelInitializerScript`.
4. Add the scene to `EditorBuildSettings.asset` if it needs to be reachable from the menu.

### I want to change how the player moves
- Modify `PlayerMovementControllerScript` (input reading) and/or `PlayerMovementInteractorScript` (move validation & execution).
- Update `PlayerMovementPresenterScript` / `PlayerMovementViewScript` if visuals need to change.

### I want to tweak post-processing / visual effects
- URP post-processing volumes are in `Assets/PostProcessing/`.
- Retro effects (CRT, VHS, scanlines) are handled by **RetroTVFX** and custom shaders in `Assets/PostEffects/` and `Assets/VHS/`.
- **Do not** directly edit third-party shader packages unless you fork them — prefer overriding via material instances.

---

*Last updated: 2026-05-31 by agent exploration.*
