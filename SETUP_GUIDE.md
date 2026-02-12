# Water Drop Survival - Unity Setup Guide

This guide walks you through setting up the complete Unity project for Water Drop Survival.

## Prerequisites

- Unity 2021.3.31f1 LTS or newer
- Unity Hub installed
- Basic Unity knowledge

## Project Setup Steps

### 1. Open the Project

1. Open Unity Hub
2. Click "Add" button
3. Navigate to the cloned repository folder
4. Select the folder and click "Open"
5. Unity will import all packages automatically (this may take a few minutes)

### 2. Scene Setup

Since scenes are binary files that can't be included in text format, you'll need to create the main game scene:

#### Create MainGame Scene

1. In Unity Editor, go to **File > New Scene**
2. Select **2D** template
3. Save as `Assets/Scenes/MainGame.unity`

#### Setup Main Camera

1. Select the Main Camera in the Hierarchy
2. Add the `CameraController` component
3. Set position to (0, 0, -10)
4. Configure settings:
   - Portrait Orthographic Size: 6
   - Smooth Speed: 5
   - Find Player On Start: ✓

#### Create Player

1. Right-click in Hierarchy > **2D Object > Sprite > Circle**
2. Rename to "Player"
3. Tag as "Player"
4. Set Layer to "Player"
5. Add Components:
   - Rigidbody2D (Gravity Scale: 0)
   - Circle Collider 2D
   - PlayerStats
   - PlayerController
   - PlayerPhysics
   - DashController
6. Scale to (1, 1, 1) or desired size
7. Set Sprite color to light blue (#6DB4F2)

#### Create Player Weapon Holder

1. Create Empty GameObject as child of Player
2. Rename to "WeaponHolder"
3. Add `Gun` component
4. Create another Empty GameObject as child named "FirePoint"
5. Position FirePoint slightly ahead of player (0.5, 0, 0)

#### Create Game Systems

1. Create Empty GameObject named "GameSystems"
2. Add components:
   - GameManager
   - ExperienceManager
   - LevelSystem
   - UpgradeSystem
   - WaveManager

#### Create Enemy Spawner

1. Create Empty GameObject named "EnemySpawner"
2. Add `EnemySpawner` component
3. Configure spawn settings

#### Create Map Generator

1. Create Empty GameObject named "MapGenerator"
2. Add `MapGenerator` component
3. Configure map size and biomes

#### Create Mobile Input

1. Right-click in Hierarchy > **UI > Canvas**
2. Set Canvas to Screen Space - Overlay
3. Set Canvas Scaler to Scale With Screen Size
   - Reference Resolution: 1080x1920
4. Create UI > Panel named "Joystick"
5. Add `VirtualJoystick` component
6. Create child Image named "Background"
7. Create child Image named "Handle"
8. Configure in Inspector

#### Create UI

1. Under Canvas, create:
   - Panel: "GameplayUI"
   - Panel: "PauseUI" (deactivate)
   - Panel: "GameOverUI" (deactivate)
   - Panel: "UpgradeMenuUI" (deactivate)

2. In GameplayUI, add:
   - Image with `HealthBar` component
   - Text (TextMeshPro) for level, wave, time
   - Panel with `StatsDisplay` for stats

3. Create Empty GameObject named "UIManager"
4. Add `UIManager` component
5. Assign all UI panels in Inspector

### 3. Create Prefabs

#### Player Prefab
1. Drag configured Player from Hierarchy to `Assets/Prefabs/Player/`
2. This creates a reusable player prefab

#### Enemy Prefabs

**Square Enemy:**
1. Create 2D Object > Sprite > Square
2. Rename to "SquareEnemy"
3. Tag as "Enemy", Layer "Enemy"
4. Add: Rigidbody2D, Box Collider 2D
5. Add: SquareEnemy, EnemyAI
6. Set color to red
7. Drag to `Assets/Prefabs/Enemies/`

**Triangle Enemy:**
1. Create 2D Object > Sprite > Triangle
2. Follow same steps as Square
3. Add: TriangleEnemy, EnemyAI
4. Set color to orange
5. Drag to `Assets/Prefabs/Enemies/`

**Round Enemy:**
1. Create 2D Object > Sprite > Circle
2. Follow same steps
3. Add: RoundEnemy, EnemyAI
4. Set color to purple
5. Drag to `Assets/Prefabs/Enemies/`

**Link Enemy Prefabs to Spawner:**
- Select EnemySpawner in scene
- Drag enemy prefabs to respective slots in Inspector

#### Projectile Prefab
1. Create 2D Object > Sprite > Circle
2. Rename to "Projectile"
3. Scale down (0.2, 0.2, 0.2)
4. Add: Rigidbody2D (Gravity: 0), Circle Collider 2D (Trigger: ✓)
5. Add: Projectile component (from Gun.cs)
6. Tag as "Projectile"
7. Set color to yellow
8. Drag to `Assets/Prefabs/Weapons/`

**Link Projectile to Gun:**
- Select Player > WeaponHolder
- In Gun component, assign Projectile prefab

#### Experience Pickup Prefab
1. Create 2D Object > Sprite > Circle (or star shape)
2. Rename to "ExperiencePickup"
3. Scale (0.3, 0.3, 0.3)
4. Add: Circle Collider 2D (Trigger: ✓)
5. Add: ExperiencePickup component
6. Set color to gold/yellow
7. Tag as "Experience"
8. Drag to `Assets/Prefabs/`

**Link to GameManager and Enemies:**
- Select GameSystems > GameManager
- Assign ExperiencePickup prefab
- For each enemy prefab, assign ExperiencePickup in Inspector

### 4. Materials Setup

#### Water Drop Material
1. Right-click in `Assets/Materials/`
2. Create > Material
3. Rename to "WaterDropMaterial"
4. Set Shader to "Custom/SquishyWaterDrop"
5. Adjust properties:
   - Color: Light blue with alpha
   - Glossiness: 0.8
   - Fresnel Power: 2
6. Assign to Player sprite

#### Enemy Materials
1. Create materials for each enemy type
2. Use "Custom/GelEffect" shader
3. Set appropriate colors
4. Assign to enemy prefabs

### 5. Input System Setup

#### Enable New Input System
1. Go to **Edit > Project Settings > Player**
2. Under "Active Input Handling", select "Both" or "Input System Package (New)"
3. Unity will restart

#### Setup Touch Input
1. Create `MobileInput` GameObject
2. Add components:
   - MobileInput
   - SwipeDetector
3. Assign VirtualJoystick reference

### 6. Build Settings

#### Configure for Mobile

**Android:**
1. **File > Build Settings**
2. Click "Add Open Scenes" to include MainGame
3. Select "Android" platform
4. Click "Switch Platform"
5. **Player Settings**:
   - Company Name: Your name
   - Product Name: Water Drop Survival
   - Default Orientation: Portrait
   - Minimum API Level: Android 7.0 (API 24)
   - Target API Level: Automatic (highest installed)
   - Graphics APIs: OpenGLES3, OpenGLES2
6. **Quality Settings**: Set to Medium/High
7. Click "Build" when ready

**iOS:**
1. Follow similar steps but select iOS platform
2. Set minimum iOS version to 12.0
3. Build and open in Xcode

### 7. Testing

#### In-Editor Testing
1. Press Play in Unity Editor
2. Use WASD or Arrow keys to move
3. Right mouse button to dash
4. Click on virtual joystick with mouse

#### Device Testing
1. Connect Android device via USB
2. Enable Developer Mode and USB Debugging
3. Click "Build and Run" in Build Settings
4. Test touch controls on device

## Troubleshooting

### Common Issues

**Scripts have compilation errors:**
- Check that all required packages are installed
- Check that Unity version is 2021.3 or newer
- Reimport all assets: Assets > Reimport All

**Player doesn't move:**
- Ensure PlayerController has MobileInput reference
- Check that VirtualJoystick is properly configured
- Verify Rigidbody2D has Gravity Scale = 0

**Enemies don't spawn:**
- Ensure enemy prefabs are assigned to EnemySpawner
- Check that Player has "Player" tag
- Verify enemy prefabs have required components

**UI doesn't appear:**
- Ensure Canvas is set to Screen Space - Overlay
- Check that EventSystem exists in scene
- Verify UI Manager has all panel references

**Touch controls don't work:**
- Enable New Input System in Project Settings
- Ensure EventSystem has Standalone Input Module
- Check that Canvas has Graphic Raycaster

## Next Steps

### Enhancing the Game

1. **Add Particle Effects:**
   - Import Unity particle system
   - Create effects for hits, deaths, level ups
   - Assign to appropriate components

2. **Add Audio:**
   - Import sound effects
   - Add Audio Source components
   - Assign clips to weapons, UI, enemies

3. **Polish Visuals:**
   - Create sprite sheets for animations
   - Add post-processing effects
   - Improve UI design

4. **Add Content:**
   - Create more enemy types
   - Design additional weapons
   - Add more upgrade options
   - Create boss enemies

5. **Optimize Performance:**
   - Use object pooling for projectiles
   - Optimize draw calls
   - Test on target devices

## Resources

- [Unity 2D Documentation](https://docs.unity3d.com/Manual/2D.html)
- [Unity Mobile Optimization](https://docs.unity3d.com/Manual/MobileOptimization.html)
- [Unity Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.6/manual/index.html)

## Support

For issues or questions:
- Check the GitHub Issues page
- Review Unity documentation
- Ask in Unity forums or Discord

---

**Happy Game Development! 🎮**
