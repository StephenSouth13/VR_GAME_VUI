# 🧁 Cuteness Overdose VR

**A kawaii VR game that progressively becomes darker**

A unique VR experience where you smash adorable cupcakes with a hammer in a pastel universe that becomes increasingly unsettling as the game progresses.

---

## 📖 Description

**Cuteness Overdose** is a virtual reality game developed for Meta Quest 3 that blends kawaii aesthetics with a creepy atmosphere. The player uses a hammer to destroy randomly appearing cupcakes while progressing through two scenes with distinct atmospheres.

### 🎮 Gameplay

- **Objective**: Reach 100 points by hitting cupcakes
- **Controls**: Grab the hammer with Quest controllers and smash the cupcakes
- **Progression**:
  - **GameScene (0-50 points)**: Kawaii atmosphere with soft music
  - **GameSceneIntermediaire (50-100 points)**: Darker, creepier atmosphere
  - **EndingScene**: Final scene after reaching 100 points

### ✨ Features

- 🎯 **Dynamic spawning system**: 8 positions with 5 cupcake types
- 🔨 **Realistic physics**: Collision detection with velocity threshold
- 🎨 **Visual effects**: Cupcake fade-out, impact particles
- 🎵 **3D spatial audio**: Ambient music and impact sounds
- 📊 **Score system**: Progress bar and automatic transitions
- 🌈 **2 distinct atmospheres**: From kawaii to slightly creepy

---

## 🛠️ Technologies Used

### Software
- **Unity**: 2022.3.37f1 (LTS)
- **Blender**: 4.1
- **Git**: Version control

### Unity Packages
- **XR Interaction Toolkit**: 2.5.4
- **XR Hands**: 1.7.2
- **OpenXR**: Meta Quest Plugin
- **TextMeshPro**: UI and text
- **XR Device Simulator**: Testing without headset

### Hardware
- **Meta Quest 3**: Primary VR headset
- **Quest Link**: PC connection for development

---

## 📂 Project Structure

```
Cuteness-Overdose-Vr/
├── Assets/
│   ├── Scenes/
│   │   ├── MenuScene.unity          # Main menu
│   │   ├── IntroScene.unity         # Introduction with mascot
│   │   ├── GameScene.unity          # First part (0-50 pts)
│   │   ├── GameSceneIntermediaire.unity  # Second part (50-100 pts)
│   │   └── EndingScene.unity        # Ending scene
│   ├── Scripts/
│   │   ├── MenuManager.cs           # Menu management
│   │   ├── MascotIntro.cs           # Mascot interactions
│   │   ├── HammerHit.cs             # Hammer collision detection
│   │   ├── CupcakeSpawner.cs        # Cupcake spawning
│   │   ├── ScoreManager.cs          # Score management (50-100)
│   │   ├── ScoreManagerFirstScene.cs # Score management (0-50)
│   │   └── FadeTransition.cs        # Scene transitions
│   ├── Prefabs/
│   │   ├── blue cream.prefab        # Blue cupcake
│   │   ├── brown cream.prefab       # Brown cupcake
│   │   ├── cherry.prefab            # Cherry cupcake
│   │   ├── chocolate.prefab         # Chocolate cupcake
│   │   └── strawberry.prefab        # Strawberry cupcake
│   ├── Materials/                   # Materials and textures
│   ├── Models/                      # 3D models (Blender)
│   ├── Sounds/                      # Audio files
│   └── XR/                          # XR configuration
├── README.md
└── .gitignore
```

---

## 🎯 Main Features

### 1. Interactive VR Menu
- Play and Quit buttons with animations
- VR support (raycast) and mouse
- Smooth transitions with fade

### 2. Introduction Scene
- Interactive mascot with dialogue
- Typewriter text animation
- 3D dialogue bubble

### 3. Game System
- **Smart spawning**: Cupcakes only appear 2 seconds after grabbing the hammer
- **5 cupcake types**: Each with adapted rotation and position
- **Collision detection**: Velocity threshold to avoid accidental destruction
- **Visual fade-out**: Cupcakes disappear with fade effect

### 4. Score System
- Visual progress bar
- +10 points per destroyed cupcake
- Automatic transition at 50 and 100 points
- Persistent score between scenes

### 5. 3D Audio
- Kawaii music (GameScene)
- Creepy music (GameSceneIntermediaire)
- Impact sounds on contact
- Fade-out during transitions

---

## 🚀 Installation and Deployment

### Prerequisites
- Unity 2022.3.37f1 (LTS)
- Meta Quest 3 with USB-C cable (optional, for headset testing)
- Meta Quest Developer Hub installed (optional, for Quest deployment)
- Developer Mode enabled on Quest (optional, for Quest deployment)

### Unity Setup
1. Clone the repository:
   ```bash
   git clone https://github.com/your-repo/Cuteness-Overdose-Vr.git
   cd Cuteness-Overdose-Vr
   ```

2. Open the project in Unity 2022.3.37f1

3. Verify XR settings:
   - Edit → Project Settings → XR Plug-in Management
   - Check **OpenXR** for Android and PC
   - Add **Meta Quest Feature** in OpenXR

### Testing with XR Device Simulator (No Headset Required)

**⭐ Important**: The XR Device Simulator is **already configured** in the project!

1. **Open MenuScene** in Unity
2. **Click Play** in Unity Editor
3. **The simulator activates automatically** and works throughout the entire game (all scenes)
4. Use keyboard/mouse controls:
   - **Tab**: Cycle between HMD/Controller/Hand input
   - **Right Shift + Mouse**: Move right hand
   - **Left Shift + Mouse**: Move left hand
   - **Space + Mouse**: Move both hands
   - **G**: Grip (grab hammer and objects)
   - **Mouse Movement**: Look around (when HMD is selected)
   - **Z, S, Q, D**: Move forward, backward, left, right
   - **E/A**: Move up/down
   - **Mouse Scroll**: Adjust movement speed

5. **XR Device Simulator UI** (bottom-left corner):
   - Shows current input mode (Controller/Hand/HMD)
   - Displays button mappings
   - Can be minimized by clicking the arrow icon

**Note**: Once activated in MenuScene, the XR Device Simulator remains active for all subsequent scenes (IntroScene, GameScene, GameSceneIntermediaire, EndingScene). You don't need to reconfigure it!

### Deploying to Meta Quest 3

1. Connect Quest 3 via USB
2. Enable Quest Link in headset
3. **Option A - Play in Unity with Quest Link**:
   - Press Play in Unity Editor
   - The game runs directly in your headset
   
4. **Option B - Build standalone APK**:
   - File → Build Settings → Android
   - Switch Platform → Build And Run
   - Install APK on Quest 3

---

## 🎨 Design and Aesthetics

### Color Palette
- **GameScene**: Pastel pink (#FFB3D9), Sky blue (#87CEEB), Soft yellow (#FFFACD)
- **GameSceneIntermediaire**: Dark pink (#D87093), Purple (#9370DB), Gray (#808080)

### Visual Style
- **Low-poly 3D models**: Optimized for VR performance
- **Pastel colors**: Kawaii aesthetic in first scene
- **Gradual darkening**: Visual transition to creepy atmosphere
- **Particle effects**: Impact feedback and visual polish

### 3D Modeling
- All models created in Blender 4.1
- Low-poly style for VR optimization (targeting 90 FPS on Quest 3)
- Materials with vibrant colors and Standard shaders
- Custom cupcake models with unique shapes and toppings

### UI/UX Design
- **Minimalist interface**: No cluttered HUD
- **Diegetic UI**: Score bar integrated in the environment
- **Clear progress feedback**: Visual and audio cues
- **Spatial audio**: 3D sound sources for immersion
- **Comfortable VR experience**: No artificial locomotion, standing experience

---

## 🎮 Game Mechanics Details

### Hammer Physics
- **Grab system**: Using XR Grab Interactable
- **Attach point**: Handle positioned for natural grip
- **Movement type**: Velocity Tracking for realistic physics
- **Collision detection**: Continuous Dynamic for accurate hits
- **Impact threshold**: Minimum velocity of 0.5 m/s required to destroy cupcakes

### Cupcake Spawning
- **8 spawn positions**: Arranged around the player
- **Random selection**: Prevents pattern memorization
- **Spawn timing**: 1.5 seconds between spawns
- **Initial delay**: 2 seconds after grabbing the hammer
- **Lifecycle**: Rise up (2s) → Stay visible (3s) → Descend (2s)
- **Physics state**: Kinematic during movement, dynamic when at peak

### Score Progression
- **First scene (0-50)**: 5 cupcakes to complete
- **Second scene (50-100)**: 5 more cupcakes to complete
- **Scene transition**: Automatic when reaching threshold
- **Visual feedback**: Progress bar + text display

---

## 🐛 Troubleshooting

### Hammer goes through walls
- Verify Rigidbody has **Collision Detection = Continuous Dynamic**
- Verify **Movement Type = Velocity Tracking** in XR Grab Interactable
- Add a collider to the platform with **Is Trigger = false**
- Check that Platform has a Box Collider or Mesh Collider

### Cupcakes don't spawn
- Verify the hammer has been grabbed (wait 2 seconds after grab)
- Check **Delay After Hammer Grab = 2s** in CupcakeSpawner
- Verify all 8 spawn positions are assigned in Inspector
- Check Console for spawn-related errors

### Score doesn't increment
- Verify ScoreManager (or ScoreManagerFirstScene) is in the scene
- Verify cupcakes have the Tag **"Cupcake"** (case-sensitive)
- Check **Min Velocity To Hit = 0.5** in HammerHit script
- Verify you're hitting with enough force (swing the hammer)

### Ray doesn't detect objects (in menu)
- Verify **Interaction Layer Mask = Everything** on interactable objects
- Verify Left/Right Hand have **XR Ray Interactor** component
- Check that XR Origin is properly configured in the scene

### XR Device Simulator not working
- Verify **XR Device Simulator** is enabled in Project Settings
- Check that you started from **MenuScene** (simulator activates there)
- Press **Tab** to cycle input modes if controls aren't responding
- Look for the simulator UI panel in the bottom-left corner

### Music not playing
- Verify audio files are assigned in ScoreManager Inspector
- Check **Music Volume** is not set to 0
- Ensure AudioListener is on the Main Camera
- Verify the 3D audio source is positioned correctly (0, 2, 0)

---

## 🎯 Controls Reference

### Meta Quest 3 Controllers
- **Grip Button**: Grab/Release hammer
- **Trigger**: Interact with UI buttons
- **Thumbstick**: Navigation (menu only)
- **Physical Movement**: Move your body to dodge/position
---

**Made with ❤️ and Unity**

*Experience the cuteness... until it becomes too much.* 🧁💀
