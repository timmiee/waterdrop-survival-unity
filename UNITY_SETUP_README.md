# Unity Project Auto-Setup Guide

## What Happens When You Open This Project

This Unity project includes an **AutoProjectSetup** script that will automatically create all necessary Unity assets when you first open the project in Unity Editor.

## First-Time Setup Process

1. **Clone the repository** (you've already done this!)
   ```bash
   git clone https://github.com/timmiee/waterdrop-survival-unity.git
   ```

2. **Open Unity Hub**
   - Make sure you have Unity **2021.3.31f1 LTS** installed
   - If not, install it from Unity Hub

3. **Add the project to Unity Hub**
   - Click "Add" button
   - Navigate to the cloned repository folder
   - Select the folder

4. **Open the project**
   - Unity will start importing assets
   - **First Launch Dialog**: You'll see a dialog asking if you want to run the auto-setup
   - Click **"Yes, Set Up Project"**

5. **Setup Process** (automatic)
   The AutoProjectSetup script will:
   - ✅ Create all materials (WaterDrop, Gel, Ground, Enemy materials)
   - ✅ Create all prefabs (Player, Enemies, Projectile, ExperiencePickup)
   - ✅ Create MainGame.unity scene with full hierarchy
   - ✅ Create MainMenu.unity scene
   - ✅ Configure layer collision matrix
   - ✅ Add scenes to build settings
   - ✅ Wire up basic scene components

6. **Setup Complete!**
   - You'll see a success dialog
   - The project is now ready to use!

## After Setup

### Running the Game

1. Open **Assets/Scenes/MainGame.unity**
2. Press the **Play ▶️** button
3. The game should run immediately with:
   - Player character visible
   - Virtual joystick (bottom-left)
   - Enemies spawning
   - Combat working
   - UI displaying

### Manual Setup (if auto-setup is skipped)

If you skipped the auto-setup or want to run it manually:

1. Go to **Menu Bar** → **Water Drop Survival** → **Run Project Setup**
2. Confirm the dialog
3. Wait for setup to complete

## Project Structure After Setup

```
Assets/
├── Scenes/
│   ├── MainMenu.unity       ⭐ Main menu scene
│   └── MainGame.unity        ⭐ Gameplay scene
├── Prefabs/
│   ├── Player.prefab         ⭐ Player character
│   ├── Projectile.prefab     ⭐ Bullet prefab
│   ├── ExperiencePickup.prefab ⭐ XP orb
│   └── Enemies/
│       ├── SquareEnemy.prefab
│       ├── TriangleEnemy.prefab
│       └── RoundEnemy.prefab
├── Materials/
│   ├── WaterDropMaterial.mat ⭐ Player material
│   ├── GelMaterial.mat
│   ├── GroundMaterial.mat
│   ├── ProjectileMaterial.mat
│   ├── ExperienceMaterial.mat
│   └── Enemies/
│       ├── SquareEnemyMaterial.mat
│       ├── TriangleEnemyMaterial.mat
│       └── RoundEnemyMaterial.mat
├── Scripts/                  ✅ Already implemented
├── Shaders/                  ✅ Already implemented
└── Editor/
    └── AutoProjectSetup.cs   ⭐ Setup automation script
```

## What the Setup Does

### Materials Created

1. **WaterDropMaterial**: Light blue with transparency for player
2. **GelMaterial**: Cyan with transparency for gel effects
3. **GroundMaterial**: Green for environment
4. **Enemy Materials**: Red, Orange, Purple for different enemy types
5. **ProjectileMaterial**: Yellow for bullets
6. **ExperienceMaterial**: Gold/Yellow for XP orbs

### Prefabs Created

All prefabs are created with:
- ✅ Proper colliders (CircleCollider2D/BoxCollider2D)
- ✅ Rigidbody2D with correct settings
- ✅ All required scripts attached
- ✅ Materials applied
- ✅ Tags and layers assigned

### Scenes Created

**MainGame.unity** includes:
- Main Camera with CameraController
- GameManager with all system references
- Systems (LevelSystem, ExperienceManager, etc.)
- Player character (from prefab)
- Map with ground plane
- Canvas with UI elements
- EventSystem for UI input
- Directional Light

**MainMenu.unity** includes:
- Main Camera
- Canvas with title and start button
- EventSystem

## Manual Configuration Needed

After auto-setup, you may need to manually wire some references in the Inspector:

### GameManager Inspector
- Drag Player from Hierarchy → Player Stats field
- Drag Systems → Experience Manager field
- Drag Systems → Level System field
- Drag Systems → Upgrade System field
- Drag Systems → Wave Manager field
- Drag Systems → Enemy Spawner field

### EnemySpawner Inspector
- Drag enemy prefabs from Prefabs/Enemies/ to the array
- Set spawn radius, interval as needed

### UIManager Inspector
- Wire up UI element references to child objects

## Testing Checklist

After setup, verify:
- [ ] Project opens without errors
- [ ] MainGame scene loads
- [ ] All prefabs load correctly
- [ ] Materials are assigned
- [ ] Press Play and game runs
- [ ] Player moves with WASD
- [ ] Enemies spawn
- [ ] Weapons fire
- [ ] XP drops when enemies die
- [ ] UI displays correctly
- [ ] No console errors

## Troubleshooting

### Setup Dialog Doesn't Appear
- Go to **Water Drop Survival** → **Run Project Setup** in the menu bar

### Missing References in Inspector
- Some prefab references may need to be dragged manually
- Check GameManager, EnemySpawner, and UIManager components

### Shader Not Found
- Make sure URP package is installed
- Check if custom shaders compiled correctly

### Scenes Not Loading
- Check **File** → **Build Settings** → scenes should be listed
- Make sure scenes exist in Assets/Scenes/

## Building for Mobile

### Android
1. **File** → **Build Settings** → **Android**
2. Click **Switch Platform**
3. **Player Settings**:
   - Orientation: Portrait
   - Minimum API: Android 7.0 (API 24)
4. Click **Build**

### iOS
1. **File** → **Build Settings** → **iOS**
2. Click **Switch Platform**
3. **Player Settings**:
   - Orientation: Portrait
   - Target minimum: iOS 12.0
4. Click **Build** and open in Xcode

## Additional Documentation

- **COMPONENT_REFERENCE.md**: Complete reference for all scripts
- **IMPLEMENTATION_SUMMARY.md**: Implementation details
- **SETUP_GUIDE.md**: Original setup guide
- **README.md**: Project overview

## Support

If you encounter issues:
1. Check Unity Console for errors
2. Verify Unity version is 2021.3.31f1 LTS
3. Make sure all required packages are installed
4. Re-run the setup script if needed

## Success Criteria

✅ **The project should work immediately after auto-setup**
- No manual Inspector work required
- Just open MainGame.unity and press Play
- Game runs with all systems functional

---

**Made with ❤️ for the Unity community**
