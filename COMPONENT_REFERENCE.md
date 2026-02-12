# Water Drop Survival - Component Reference Guide

Complete reference for all C# scripts/components in the project.

## Core Scripts

### GameManager.cs
**Location:** `Assets/Scripts/`  
**Purpose:** Main game coordinator, manages game state and system references  
**Key Methods:**
- `StartGame()` - Initialize and start the game
- `PauseGame()` - Pause game and freeze time
- `ResumeGame()` - Resume from pause
- `GameOver()` - Trigger game over state
- `RestartGame()` - Reload current scene

**Usage:** Attach to GameSystems GameObject in scene

---

### CameraController.cs
**Location:** `Assets/Scripts/`  
**Purpose:** Follow player with smooth camera movement in portrait mode  
**Settings:**
- Smooth Speed: 5
- Offset: (0, 0, -10)
- Portrait Orthographic Size: 6
- Use Bounds: ✓

**Usage:** Attach to Main Camera

---

## Player Scripts

### PlayerStats.cs
**Location:** `Assets/Scripts/Player/`  
**Purpose:** Manages all player statistics and progression  
**Stats Managed:**
- Health (100), Damage (1), Speed (5)
- Crit Chance (10%), Crit Damage (1.5x)
- Armor (0), Dodge (0), Evasion (0)
- Level (1), Experience (0)

**Key Methods:**
- `TakeDamage(float)` - Apply damage with armor reduction
- `Heal(float)` - Restore health
- `AddExperience(int)` - Add XP and check for level up
- `ApplyUpgrade(string)` - Apply stat upgrade
- `CalculateDamage(float)` - Calculate final damage with crits

**Events:**
- `OnHealthChanged` - Health changed
- `OnLevelUp` - Player leveled up
- `OnDeath` - Player died

---

### PlayerController.cs
**Location:** `Assets/Scripts/Player/`  
**Purpose:** Handle player movement with physics-based inertia  
**Settings:**
- Acceleration: 20
- Deceleration: 15
- Requires: Rigidbody2D, PlayerStats

**Key Methods:**
- `MovePlayer()` - Apply movement with smooth acceleration
- `ApplyKnockback(Vector2, float)` - Push player back on hit

**Properties:**
- `LastMoveDirection` - Last movement direction for weapon aiming
- `IsMoving` - Is player currently moving

---

### PlayerPhysics.cs
**Location:** `Assets/Scripts/Player/`  
**Purpose:** Squishy visual deformation and particle effects  
**Settings:**
- Squish Speed: 10
- Squish Amount: 0.2
- Bounce Back Speed: 8

**Key Methods:**
- `ApplyImpactSquish(Vector2)` - Squish on damage
- `ApplyDashSquish(Vector2)` - Squish during dash
- `SpawnDropletParticles(Vector2)` - Create water droplets

---

### DashController.cs
**Location:** `Assets/Scripts/Player/`  
**Purpose:** Dash ability with swipe detection and invulnerability  
**Settings:**
- Dash Speed: 15
- Dash Duration: 0.3s
- Dash Cooldown: 1.5s
- Invulnerable During Dash: ✓

**Key Methods:**
- `StartDash(Vector2)` - Initiate dash
- `TriggerDash(Vector2)` - Manual dash trigger

**Properties:**
- `IsDashing` - Currently dashing
- `DashCooldownPercent` - Cooldown progress (0-1)

---

## Input Scripts

### MobileInput.cs
**Location:** `Assets/Scripts/Input/`  
**Purpose:** Central mobile input management  
**Properties:**
- `MoveInput` - Current joystick input direction

---

### VirtualJoystick.cs
**Location:** `Assets/Scripts/Input/`  
**Purpose:** On-screen joystick for mobile controls  
**Settings:**
- Handle Range: 50
- Dynamic Joystick: Optional

**Properties:**
- `InputDirection` - Current input direction (Vector2)

**Usage:** Attach to UI Panel with Background and Handle images

---

### SwipeDetector.cs
**Location:** `Assets/Scripts/Input/`  
**Purpose:** Detect swipe gestures for dash  
**Settings:**
- Min Swipe Distance: 50 pixels
- Max Swipe Time: 0.5 seconds

**Events:**
- `OnSwipe(Vector2)` - Swipe detected with direction

---

## Weapon Scripts

### WeaponBase.cs (Abstract)
**Location:** `Assets/Scripts/Weapons/`  
**Purpose:** Base class for all weapons  
**Common Properties:**
- Base Damage
- Fire Rate
- Weapon Level

**Key Methods:**
- `Fire()` - Abstract, must implement
- `Upgrade()` - Increase weapon level
- `FindNearestEnemy()` - Auto-targeting helper

---

### Gun.cs
**Location:** `Assets/Scripts/Weapons/`  
**Purpose:** Starting ranged weapon  
**Stats:**
- Damage: 15
- Fire Rate: 1/sec
- Max Range: 15

**Requires:** Projectile prefab

---

### Sword.cs
**Location:** `Assets/Scripts/Weapons/`  
**Purpose:** Melee weapon unlocked at level 5  
**Stats:**
- Damage: 25
- Fire Rate: 1.5/sec
- Attack Range: 2
- Attack Arc: 90°

---

### DoubleBarrel.cs
**Location:** `Assets/Scripts/Weapons/`  
**Purpose:** Dual-shot weapon at level 10  
**Stats:**
- Damage: 20 per shot
- Fire Rate: 0.8/sec
- Spread Angle: 15°

---

### EnergyAura.cs
**Location:** `Assets/Scripts/Weapons/`  
**Purpose:** Rotating orbs at level 10  
**Stats:**
- Damage: 15 per tick
- Tick Rate: 2/sec
- Aura Radius: 3
- Number of Orbs: 3

---

## Enemy Scripts

### EnemyBase.cs (Abstract)
**Location:** `Assets/Scripts/Enemies/`  
**Purpose:** Base class for all enemies  
**Common Properties:**
- Max Health: 100
- Move Speed: 2.5
- Attack Damage: 33
- Experience Value: 1

**Key Methods:**
- `TakeDamage(float)` - Apply damage
- `Die()` - Handle death and spawn XP
- `AttackPlayer()` - Damage player

---

### EnemyAI.cs
**Location:** `Assets/Scripts/Enemies/`  
**Purpose:** Chase and attack AI  
**Settings:**
- Detection Range: 15
- Attack Range: 1
- Avoidance Radius: 0.5

---

### SquareEnemy.cs
**Location:** `Assets/Scripts/Enemies/`  
**Stats:** HP: 100, Speed: 2.5, Damage: 33, XP: 1

---

### TriangleEnemy.cs
**Location:** `Assets/Scripts/Enemies/`  
**Stats:** HP: 80, Speed: 3, Damage: 30, XP: 1

---

### RoundEnemy.cs
**Location:** `Assets/Scripts/Enemies/`  
**Stats:** HP: 120, Speed: 2, Damage: 35, XP: 2

---

### EnemySpawner.cs
**Location:** `Assets/Scripts/Enemies/`  
**Purpose:** Wave-based enemy spawning  
**Settings:**
- Spawn Radius: 15
- Spawn Interval: 2 seconds
- Initial Enemies Per Wave: 5
- Wave Scaling: 1.2x
- Max Active Enemies: 50

**Key Methods:**
- `SpawnEnemy()` - Spawn single enemy
- `GetCurrentWave()` - Get wave number
- `GetActiveEnemyCount()` - Count active enemies

---

## System Scripts

### ExperiencePickup.cs
**Location:** `Assets/Scripts/Systems/`  
**Purpose:** Collectible XP orbs  
**Settings:**
- Attraction Range: 3
- Attraction Speed: 5
- Pickup Range: 0.5
- Rotation Speed: 180

---

### ExperienceManager.cs
**Location:** `Assets/Scripts/Systems/`  
**Purpose:** Manage XP collection and distribution  
**Key Methods:**
- `AddExperience(int)` - Add XP to player

**Events:**
- `OnExperienceGained(int, int)` - XP gained with total

---

### LevelSystem.cs
**Location:** `Assets/Scripts/Systems/`  
**Purpose:** Handle level progression and weapon unlocks  
**Weapon Unlocks:**
- Level 5: Sword
- Level 10: Double Barrel & Energy Aura

---

### UpgradeSystem.cs
**Location:** `Assets/Scripts/Systems/`  
**Purpose:** Manage stat upgrades on level up  
**Available Upgrades:**
- Attack +10%
- Attack Speed +10%
- Armor +25%
- Health +10%
- Move Speed +10%
- Crit Chance +5%
- Crit Damage +25%
- Health Regen +1/sec

**Key Methods:**
- `ShowUpgradeMenu()` - Display 3 random choices
- `ApplyUpgrade(Upgrade)` - Apply selected upgrade

---

### WaveManager.cs
**Location:** `Assets/Scripts/Systems/`  
**Purpose:** Manage wave progression and difficulty  
**Settings:**
- Wave Duration: 60 seconds
- Difficulty Scaling: 1.1x
- Boss Wave Interval: Every 5 waves

---

## UI Scripts

### UIManager.cs
**Location:** `Assets/Scripts/UI/`  
**Purpose:** Coordinate all UI elements  
**Manages:**
- Gameplay UI (HUD)
- Pause UI
- Game Over UI
- Level/Wave/Time displays

**Key Methods:**
- `ShowGameplayUI()`
- `ShowPauseUI()`
- `ShowGameOverUI()`
- `ResumeGame()`

---

### HealthBar.cs
**Location:** `Assets/Scripts/UI/`  
**Purpose:** Visual health bar display  
**Features:**
- Smooth transition
- Color gradient (red → yellow → green)
- Auto-updates on health change

---

### StatsDisplay.cs
**Location:** `Assets/Scripts/UI/`  
**Purpose:** Show player stats  
**Displays:**
- Damage, Attack Speed, Move Speed
- Armor, Crit Chance, Crit Damage

---

### UpgradeMenu.cs
**Location:** `Assets/Scripts/UI/`  
**Purpose:** Display and handle upgrade selection  
**Usage:** Attach to UpgradeMenuUI panel

---

## Map Scripts

### MapGenerator.cs
**Location:** `Assets/Scripts/Map/`  
**Purpose:** Generate game map with biomes  
**Settings:**
- Map Width: 100
- Map Height: 100
- Tree Count: 50

**Biomes Generated:**
- Forest (upper left)
- Lake (center)
- Grassland (default)
- Structures: Cabin, Windmill, Mine

---

### BiomeManager.cs
**Location:** `Assets/Scripts/Map/`  
**Purpose:** Manage biome properties and effects  
**Biome Properties:**
- Ground Color
- Ambient Color
- Enemy Spawn Multiplier

---

## Shader Files

### SquishyWaterDrop.shader
**Location:** `Assets/Shaders/`  
**Purpose:** Water drop visual effect with fresnel and deformation  
**Properties:**
- Color, Glossiness, Metallic
- Fresnel Power, Fresnel Color
- Squish Amount, Squish Direction

---

### GelEffect.shader
**Location:** `Assets/Shaders/`  
**Purpose:** Jelly/gel effect with wobble animation  
**Properties:**
- Color, Transparency
- Rim Color, Rim Power
- Wobble Amount, Wobble Speed

---

## Helper Classes

### Projectile (in Gun.cs)
**Purpose:** Bullet/projectile behavior  
**Key Methods:**
- `SetDamage(float)` - Set projectile damage
- `OnTriggerEnter2D()` - Handle enemy hit

---

## Component Dependencies

### Player GameObject Requires:
1. PlayerStats
2. PlayerController
3. PlayerPhysics
4. DashController
5. Rigidbody2D
6. Collider2D

### Enemy GameObject Requires:
1. EnemyBase (or derived)
2. EnemyAI
3. Rigidbody2D
4. Collider2D

### Weapon Holder Requires:
1. Weapon component (Gun, Sword, etc.)
2. FirePoint child transform

### Canvas Requires:
1. Canvas
2. Canvas Scaler
3. Graphic Raycaster
4. EventSystem (in scene)

---

## Script Execution Order

Default order works, but for optimization:
1. GameManager (early)
2. Input systems
3. Player components
4. Enemy AI
5. Weapon systems
6. UI updates (late)

Configure in **Edit > Project Settings > Script Execution Order**

---

## Events & Delegates

### PlayerStats Events:
- `HealthChanged(float current, float max)`
- `LevelUp(int newLevel)`
- `Death()`

### SwipeDetector Events:
- `OnSwipe(Vector2 direction)`

### ExperienceManager Events:
- `ExperienceGained(int amount, int total)`

### WaveManager Events:
- `WaveChanged(int waveNumber)`

---

## Quick Reference: Common Tasks

### Add New Weapon:
1. Create class inheriting WeaponBase
2. Implement Fire() method
3. Add unlock in LevelSystem

### Add New Enemy:
1. Create class inheriting EnemyBase
2. Set stats in Start()
3. Add to EnemySpawner selection

### Add New Upgrade:
1. Add to UpgradeSystem.allUpgrades
2. Handle type in PlayerStats.ApplyUpgrade()

### Add New UI Panel:
1. Create Panel in Canvas
2. Add reference to UIManager
3. Add show/hide methods

---

**For detailed usage, see SETUP_GUIDE.md**
