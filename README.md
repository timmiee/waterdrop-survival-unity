# Water Drop Survival - Unity Game

A Vampire Survivors-style horde survival game featuring a cute water drop character with squishy physics, wave-based enemy spawning, and progressive weapon unlocks.

## 🎮 Game Features

### Core Gameplay
- **Survivor/Horde Gameplay**: Endless waves of enemies with increasing difficulty
- **Squishy Water Drop Character**: Player character with soft-body physics and particle effects
- **Mobile-First Design**: Portrait mode with virtual joystick and swipe controls
- **Progressive Weapon System**: Unlock new weapons as you level up
- **RPG Progression**: Level up, gain experience, and choose stat upgrades

### Player Features
- **Squishy Physics**: Dynamic deformation based on movement and impacts
- **Dash Ability**: Swipe to dash in any direction with invulnerability frames
- **Starting Stats**:
  - Health: 100
  - Damage: 1
  - Speed: 25% bonus
  - Crit Chance: 10%
  - Crit Damage: 1.5x

### Weapon System
- **Gun** (Starting): Auto-targeting projectile weapon (15 damage, 1 shot/sec)
- **Sword** (Level 5): Frontal melee slash attack
- **Double Barrel** (Level 10): Dual projectiles with spread
- **Energy Aura** (Level 10): Rotating orbs that damage on contact

### Enemy Types
- **Square Enemy**: Standard balanced enemy (100 HP, 33 damage)
- **Triangle Enemy**: Fast but weaker (80 HP, 30 damage, faster)
- **Round Enemy**: Slow tank (120 HP, 35 damage, tankier)

### Progression System
- **Fast Leveling**: 2 kills for first level, scales up
- **Stat Upgrades**:
  - Attack +10%
  - Attack Speed +10%
  - Armor +25%
  - Health +10%
  - Move Speed +10%
  - Crit Chance +5%
  - Crit Damage +25%
  - Health Regen +1/sec

### Map & Environment
- **Large Open World**: 100x100 unit map
- **Multiple Biomes**:
  - Forest (trees with shadows)
  - Lake (blue water area)
  - Grassland (green ground)
- **Structures**:
  - Cabin
  - Windmill
  - Mine
  - Forest roads

## 🛠️ Technical Details

### Unity Version
- **Unity 2021.3.31f1 LTS**

### Required Packages
- Universal Render Pipeline (URP) 12.1.12
- Input System 1.6.3
- TextMeshPro 3.0.6
- Post Processing 3.2.2

### Project Structure
```
Assets/
├── Scripts/
│   ├── Player/          # Player controller, stats, physics, dash
│   ├── Weapons/         # Weapon system and implementations
│   ├── Enemies/         # Enemy AI and types
│   ├── Systems/         # XP, leveling, upgrades, waves
│   ├── UI/              # HUD, menus, health bars
│   ├── Input/           # Mobile controls and gestures
│   └── Map/             # Map generation and biomes
├── Materials/           # Materials for player, enemies, environment
├── Shaders/             # Custom shaders for squishy effects
├── Prefabs/             # Game object prefabs
├── Scenes/              # Game scenes
└── Resources/           # Sprites and particles
```

## 🚀 Getting Started

### Prerequisites
- Unity 2021.3.31f1 or newer
- Basic knowledge of Unity Editor

### Installation
1. Clone this repository
2. Open Unity Hub
3. Click "Add" and select the project folder
4. Open the project (Unity will import packages automatically)
5. Open `Assets/Scenes/MainGame.unity`

### Building for Mobile

#### Android
1. Go to **File > Build Settings**
2. Select **Android** platform
3. Click **Switch Platform**
4. Configure **Player Settings**:
   - Set orientation to **Portrait**
   - Set minimum API level to **Android 7.0 (API 24)**
5. Click **Build** or **Build and Run**

#### iOS
1. Go to **File > Build Settings**
2. Select **iOS** platform
3. Click **Switch Platform**
4. Configure **Player Settings**:
   - Set orientation to **Portrait**
   - Set target minimum iOS version to **12.0**
5. Click **Build** and open in Xcode

## 🎯 Controls

### Mobile (Touch)
- **Virtual Joystick**: Move the water drop
- **Swipe**: Dash in swipe direction

### Desktop (Testing)
- **WASD / Arrow Keys**: Move
- **Right Mouse Button**: Dash (simulate swipe)

## 📝 Code Architecture

### Key Classes

#### Player System
- `PlayerStats.cs`: Manages health, damage, and stat progression
- `PlayerController.cs`: Handles movement with physics-based inertia
- `PlayerPhysics.cs`: Squishy deformation and particle effects
- `DashController.cs`: Dash ability with invulnerability

#### Weapon System
- `WeaponBase.cs`: Abstract base for all weapons
- Individual weapon classes inherit and implement firing behavior
- Auto-targeting system for ranged weapons

#### Enemy System
- `EnemyBase.cs`: Health, damage, death handling
- `EnemyAI.cs`: Follow player with collision avoidance
- `EnemySpawner.cs`: Wave-based spawning around player

#### Progression System
- `ExperienceManager.cs`: XP collection and distribution
- `LevelSystem.cs`: Level-up triggers and weapon unlocks
- `UpgradeSystem.cs`: Random upgrade choices on level up
- `WaveManager.cs`: Wave progression and difficulty scaling

## 🎨 Visual Style

### Color Palette
- **Background**: Lavender Blush (#FFF0F5)
- **Ground**: Green (#4EB333)
- **Water**: Blue (#5599DD)
- **Player**: Light blue water drop with transparency

### Shaders
- **SquishyWaterDrop.shader**: Fresnel effect and vertex deformation
- **GelEffect.shader**: Wobble animation and rim lighting

## 🔧 Customization

### Adjusting Difficulty
Edit values in `EnemySpawner.cs`:
- `spawnInterval`: Time between spawns
- `initialEnemiesPerWave`: Starting enemy count
- `waveScaling`: Difficulty increase per wave

### Modifying Player Stats
Edit values in `PlayerStats.cs` inspector:
- Base health, damage, speed
- Crit chance and damage multipliers

### Adding New Weapons
1. Create new class inheriting from `WeaponBase`
2. Implement `Fire()` method
3. Add unlock condition in `LevelSystem.cs`

### Adding New Enemies
1. Create new class inheriting from `EnemyBase`
2. Set unique stats (health, speed, damage)
3. Add to enemy type selection in `EnemySpawner.cs`

## 🐛 Known Issues
- Unity scene files and prefabs need to be created in the Unity Editor
- Materials need to be assigned to objects in the Inspector
- Audio clips need to be imported and assigned

## 📄 License
This project is for educational purposes. Feel free to use and modify.

## 🤝 Contributing
This is a portfolio/learning project. Suggestions and feedback are welcome!

## 📞 Contact
- Repository: [timmiee/waterdrop-survival-unity](https://github.com/timmiee/waterdrop-survival-unity)
- Original HTML Version: [timmiee/0.2-NewVersion-Waterdrop-](https://github.com/timmiee/0.2-NewVersion-Waterdrop-)

## 🙏 Acknowledgments
- Inspired by Vampire Survivors gameplay
- Original HTML5/JavaScript version provided game design reference
- Unity Asset Store for potential asset resources

---

**Made with ❤️ and Unity** 
