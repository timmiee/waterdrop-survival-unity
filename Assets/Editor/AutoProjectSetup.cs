using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Collections.Generic;

namespace WaterDropSurvival.Editor
{
    /// <summary>
    /// Automatically sets up the Unity project on first import.
    /// Creates scenes, prefabs, materials, and configures project settings.
    /// </summary>
    [InitializeOnLoad]
    public class AutoProjectSetup
    {
        private const string SETUP_COMPLETE_KEY = "WaterDropSurvival_SetupComplete";
        private const string SETUP_VERSION = "1.0";

        static AutoProjectSetup()
        {
            // Check if setup has been completed
            string completedVersion = EditorPrefs.GetString(SETUP_COMPLETE_KEY, "");
            
            if (completedVersion != SETUP_VERSION)
            {
                // Run setup on next editor update
                EditorApplication.update += RunSetupOnce;
            }
        }

        private static void RunSetupOnce()
        {
            EditorApplication.update -= RunSetupOnce;
            
            // Show dialog
            if (EditorUtility.DisplayDialog(
                "Water Drop Survival - Project Setup",
                "This appears to be the first time opening this project.\n\n" +
                "Would you like to automatically create all Unity assets?\n" +
                "(Scenes, Prefabs, Materials)\n\n" +
                "This will take a few moments.",
                "Yes, Set Up Project",
                "Skip"))
            {
                RunSetup();
            }
        }

        [MenuItem("Water Drop Survival/Run Project Setup", false, 1)]
        public static void RunSetupManually()
        {
            if (EditorUtility.DisplayDialog(
                "Water Drop Survival - Project Setup",
                "This will create/recreate all Unity assets.\n\n" +
                "Existing assets may be overwritten.\n\n" +
                "Continue?",
                "Yes",
                "Cancel"))
            {
                RunSetup();
            }
        }

        private static void RunSetup()
        {
            try
            {
                EditorUtility.DisplayProgressBar("Project Setup", "Starting setup...", 0f);

                // Create materials
                EditorUtility.DisplayProgressBar("Project Setup", "Creating materials...", 0.2f);
                CreateMaterials();

                // Create prefabs
                EditorUtility.DisplayProgressBar("Project Setup", "Creating prefabs...", 0.4f);
                CreatePrefabs();

                // Create scenes
                EditorUtility.DisplayProgressBar("Project Setup", "Creating scenes...", 0.6f);
                CreateScenes();

                // Configure project settings
                EditorUtility.DisplayProgressBar("Project Setup", "Configuring project settings...", 0.8f);
                ConfigureProjectSettings();

                EditorUtility.DisplayProgressBar("Project Setup", "Finalizing...", 0.9f);

                // Save assets
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                // Mark setup as complete
                EditorPrefs.SetString(SETUP_COMPLETE_KEY, SETUP_VERSION);

                EditorUtility.ClearProgressBar();

                EditorUtility.DisplayDialog(
                    "Setup Complete!",
                    "Project setup completed successfully!\n\n" +
                    "You can now:\n" +
                    "1. Open Scenes/MainGame.unity\n" +
                    "2. Press Play to test the game\n\n" +
                    "Note: Some prefab references may need to be wired manually in the Inspector.",
                    "OK");

                Debug.Log("[AutoProjectSetup] Setup completed successfully!");
            }
            catch (System.Exception e)
            {
                EditorUtility.ClearProgressBar();
                EditorUtility.DisplayDialog("Setup Error", $"An error occurred during setup:\n\n{e.Message}", "OK");
                Debug.LogError($"[AutoProjectSetup] Setup failed: {e}");
            }
        }

        private static void CreateMaterials()
        {
            Debug.Log("[AutoProjectSetup] Creating materials...");

            // Water Drop Material
            CreateMaterial("Assets/Materials/WaterDropMaterial.mat", 
                new Color(0.3f, 0.7f, 1.0f, 0.8f), 
                "Assets/Shaders/SquishyWaterDrop.shader");

            // Gel Material
            CreateMaterial("Assets/Materials/GelMaterial.mat", 
                new Color(0f, 1f, 1f, 0.7f), 
                "Assets/Shaders/GelEffect.shader");

            // Ground Material (Standard shader)
            CreateMaterial("Assets/Materials/GroundMaterial.mat", 
                new Color(0.3f, 0.8f, 0.3f, 1f), 
                "Standard");

            // Enemy Materials
            CreateMaterial("Assets/Materials/Enemies/SquareEnemyMaterial.mat", 
                new Color(1f, 0.2f, 0.2f, 1f), 
                "Standard");

            CreateMaterial("Assets/Materials/Enemies/TriangleEnemyMaterial.mat", 
                new Color(1f, 0.6f, 0.2f, 1f), 
                "Standard");

            CreateMaterial("Assets/Materials/Enemies/RoundEnemyMaterial.mat", 
                new Color(0.8f, 0.3f, 0.8f, 1f), 
                "Standard");

            // Projectile Material
            CreateMaterial("Assets/Materials/ProjectileMaterial.mat", 
                new Color(1f, 1f, 0.5f, 1f), 
                "Standard");

            // Experience Material
            CreateMaterial("Assets/Materials/ExperienceMaterial.mat", 
                new Color(1f, 0.9f, 0.2f, 1f), 
                "Standard");

            Debug.Log("[AutoProjectSetup] Materials created.");
        }

        private static void CreateMaterial(string path, Color color, string shaderName)
        {
            // Check if directory exists
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            Material mat = new Material(Shader.Find(shaderName) ?? Shader.Find("Standard"));
            mat.color = color;

            // For transparent materials
            if (color.a < 1f && shaderName == "Standard")
            {
                mat.SetFloat("_Mode", 3); // Transparent mode
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;
            }

            AssetDatabase.CreateAsset(mat, path);
            Debug.Log($"[AutoProjectSetup] Created material: {path}");
        }

        private static void CreatePrefabs()
        {
            Debug.Log("[AutoProjectSetup] Creating prefabs...");

            // Create Player Prefab
            CreatePlayerPrefab();

            // Create Enemy Prefabs
            CreateEnemyPrefab("SquareEnemy", "Assets/Materials/Enemies/SquareEnemyMaterial.mat", 
                "WaterDropSurvival.Enemies.SquareEnemy");
            CreateEnemyPrefab("TriangleEnemy", "Assets/Materials/Enemies/TriangleEnemyMaterial.mat", 
                "WaterDropSurvival.Enemies.TriangleEnemy");
            CreateEnemyPrefab("RoundEnemy", "Assets/Materials/Enemies/RoundEnemyMaterial.mat", 
                "WaterDropSurvival.Enemies.RoundEnemy");

            // Create Projectile Prefab
            CreateProjectilePrefab();

            // Create Experience Pickup Prefab
            CreateExperiencePickupPrefab();

            Debug.Log("[AutoProjectSetup] Prefabs created.");
        }

        private static void CreatePlayerPrefab()
        {
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            player.name = "Player";
            player.tag = "Player";
            player.layer = LayerMask.NameToLayer("Player");
            player.transform.localScale = Vector3.one * 0.8f;

            // Apply material
            Material mat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/WaterDropMaterial.mat");
            if (mat != null)
            {
                player.GetComponent<Renderer>().material = mat;
            }

            // Add Rigidbody2D
            Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.drag = 0f;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            // Replace 3D collider with 2D
            Object.DestroyImmediate(player.GetComponent<SphereCollider>());
            CircleCollider2D col = player.AddComponent<CircleCollider2D>();
            col.radius = 0.5f;

            // Add player scripts
            AddScriptComponent(player, "WaterDropSurvival.Player.PlayerStats");
            AddScriptComponent(player, "WaterDropSurvival.Player.PlayerController");
            AddScriptComponent(player, "WaterDropSurvival.Player.PlayerPhysics");
            AddScriptComponent(player, "WaterDropSurvival.Player.DashController");

            // Save prefab
            PrefabUtility.SaveAsPrefabAsset(player, "Assets/Prefabs/Player.prefab");
            Object.DestroyImmediate(player);

            Debug.Log("[AutoProjectSetup] Created Player prefab");
        }

        private static void CreateEnemyPrefab(string enemyName, string materialPath, string scriptTypeName)
        {
            GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Cube);
            enemy.name = enemyName;
            enemy.tag = "Enemy";
            enemy.layer = LayerMask.NameToLayer("Enemy");
            enemy.transform.localScale = Vector3.one * 0.6f;

            // Apply material
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            if (mat != null)
            {
                enemy.GetComponent<Renderer>().material = mat;
            }

            // Add Rigidbody2D
            Rigidbody2D rb = enemy.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            // Replace 3D collider with 2D
            Object.DestroyImmediate(enemy.GetComponent<BoxCollider>());
            BoxCollider2D col = enemy.AddComponent<BoxCollider2D>();
            col.size = Vector2.one * 0.6f;

            // Add enemy scripts
            AddScriptComponent(enemy, scriptTypeName);
            AddScriptComponent(enemy, "WaterDropSurvival.Enemies.EnemyAI");

            // Save prefab
            PrefabUtility.SaveAsPrefabAsset(enemy, $"Assets/Prefabs/Enemies/{enemyName}.prefab");
            Object.DestroyImmediate(enemy);

            Debug.Log($"[AutoProjectSetup] Created {enemyName} prefab");
        }

        private static void CreateProjectilePrefab()
        {
            GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            projectile.name = "Projectile";
            projectile.tag = "Projectile";
            projectile.layer = LayerMask.NameToLayer("Projectile");
            projectile.transform.localScale = Vector3.one * 0.2f;

            // Apply material
            Material mat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/ProjectileMaterial.mat");
            if (mat != null)
            {
                projectile.GetComponent<Renderer>().material = mat;
            }

            // Add Rigidbody2D
            Rigidbody2D rb = projectile.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            // Replace 3D collider with 2D
            Object.DestroyImmediate(projectile.GetComponent<SphereCollider>());
            CircleCollider2D col = projectile.AddComponent<CircleCollider2D>();
            col.radius = 0.1f;
            col.isTrigger = true;

            // Add Projectile script (nested class in Gun.cs)
            AddScriptComponent(projectile, "WaterDropSurvival.Weapons.Projectile");

            // Save prefab
            PrefabUtility.SaveAsPrefabAsset(projectile, "Assets/Prefabs/Projectile.prefab");
            Object.DestroyImmediate(projectile);

            Debug.Log("[AutoProjectSetup] Created Projectile prefab");
        }

        private static void CreateExperiencePickupPrefab()
        {
            GameObject exp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            exp.name = "ExperiencePickup";
            exp.tag = "Experience";
            exp.layer = LayerMask.NameToLayer("Experience");
            exp.transform.localScale = Vector3.one * 0.3f;

            // Apply material
            Material mat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/ExperienceMaterial.mat");
            if (mat != null)
            {
                exp.GetComponent<Renderer>().material = mat;
            }

            // Replace 3D collider with 2D
            Object.DestroyImmediate(exp.GetComponent<SphereCollider>());
            CircleCollider2D col = exp.AddComponent<CircleCollider2D>();
            col.radius = 0.15f;
            col.isTrigger = true;

            // Add ExperiencePickup script
            AddScriptComponent(exp, "WaterDropSurvival.Systems.ExperiencePickup");

            // Save prefab
            PrefabUtility.SaveAsPrefabAsset(exp, "Assets/Prefabs/ExperiencePickup.prefab");
            Object.DestroyImmediate(exp);

            Debug.Log("[AutoProjectSetup] Created ExperiencePickup prefab");
        }

        private static void AddScriptComponent(GameObject obj, string scriptTypeName)
        {
            System.Type scriptType = System.Type.GetType(scriptTypeName + ", Assembly-CSharp");
            if (scriptType != null)
            {
                obj.AddComponent(scriptType);
            }
            else
            {
                Debug.LogWarning($"[AutoProjectSetup] Could not find script type: {scriptTypeName}");
            }
        }

        private static void CreateScenes()
        {
            Debug.Log("[AutoProjectSetup] Creating scenes...");

            // Create MainGame scene
            CreateMainGameScene();

            // Create MainMenu scene
            CreateMainMenuScene();

            Debug.Log("[AutoProjectSetup] Scenes created.");
        }

        private static void CreateMainGameScene()
        {
            // Create new scene
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // Create Main Camera
            GameObject camera = new GameObject("Main Camera");
            camera.tag = "MainCamera";
            Camera cam = camera.AddComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = 10f;
            cam.backgroundColor = new Color(0.95f, 0.9f, 0.95f); // Lavender blush
            camera.transform.position = new Vector3(0, 0, -10);
            AddScriptComponent(camera, "WaterDropSurvival.CameraController");
            camera.AddComponent<AudioListener>();

            // Create Directional Light
            GameObject light = new GameObject("Directional Light");
            light.transform.rotation = Quaternion.Euler(50, -30, 0);
            Light lightComp = light.AddComponent<Light>();
            lightComp.type = LightType.Directional;
            lightComp.color = Color.white;
            lightComp.intensity = 1f;

            // Create GameManager
            GameObject gameManager = new GameObject("GameManager");
            AddScriptComponent(gameManager, "WaterDropSurvival.GameManager");

            // Create Systems
            GameObject systems = new GameObject("Systems");
            AddScriptComponent(systems, "WaterDropSurvival.Systems.LevelSystem");
            AddScriptComponent(systems, "WaterDropSurvival.Systems.ExperienceManager");
            AddScriptComponent(systems, "WaterDropSurvival.Systems.UpgradeSystem");
            AddScriptComponent(systems, "WaterDropSurvival.Systems.WaveManager");
            AddScriptComponent(systems, "WaterDropSurvival.Enemies.EnemySpawner");

            // Create Player (from prefab if available)
            GameObject playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");
            if (playerPrefab != null)
            {
                PrefabUtility.InstantiatePrefab(playerPrefab);
            }
            else
            {
                GameObject player = new GameObject("Player");
                player.tag = "Player";
            }

            // Create Map
            GameObject map = new GameObject("Map");
            AddScriptComponent(map, "WaterDropSurvival.Map.MapGenerator");
            AddScriptComponent(map, "WaterDropSurvival.Map.BiomeManager");
            
            // Create ground plane
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.SetParent(map.transform);
            ground.transform.localScale = new Vector3(10, 1, 10);
            ground.layer = LayerMask.NameToLayer("Ground");
            Material groundMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/GroundMaterial.mat");
            if (groundMat != null)
            {
                ground.GetComponent<Renderer>().material = groundMat;
            }

            // Create Canvas
            CreateUICanvas();

            // Create EventSystem
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

            // Save scene
            EditorSceneManager.SaveScene(scene, "Assets/Scenes/MainGame.unity");
            Debug.Log("[AutoProjectSetup] Created MainGame scene");
        }

        private static void CreateUICanvas()
        {
            GameObject canvas = new GameObject("Canvas");
            Canvas c = canvas.AddComponent<Canvas>();
            c.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();
            AddScriptComponent(canvas, "WaterDropSurvival.UI.UIManager");

            // Create basic UI elements as placeholders
            // HealthBar
            CreateUIElement(canvas.transform, "HealthBar");
            
            // StatsDisplay
            CreateUIElement(canvas.transform, "StatsDisplay");
            
            // LevelDisplay
            CreateUIElement(canvas.transform, "LevelDisplay");
            
            // WaveDisplay
            CreateUIElement(canvas.transform, "WaveDisplay");
            
            // UpgradeMenu (hidden by default)
            GameObject upgradeMenu = CreateUIElement(canvas.transform, "UpgradeMenu");
            upgradeMenu.SetActive(false);
            
            // VirtualJoystick
            GameObject joystick = CreateUIElement(canvas.transform, "VirtualJoystick");
            AddScriptComponent(joystick, "WaterDropSurvival.Input.VirtualJoystick");

            // MobileInput
            GameObject mobileInput = new GameObject("MobileInput");
            mobileInput.transform.SetParent(canvas.transform);
            AddScriptComponent(mobileInput, "WaterDropSurvival.Input.MobileInput");
        }

        private static GameObject CreateUIElement(Transform parent, string name)
        {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(parent);
            obj.AddComponent<RectTransform>();
            return obj;
        }

        private static void CreateMainMenuScene()
        {
            // Create new scene
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // Create Main Camera
            GameObject camera = new GameObject("Main Camera");
            camera.tag = "MainCamera";
            Camera cam = camera.AddComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = 10f;
            cam.backgroundColor = new Color(0.95f, 0.9f, 0.95f);
            camera.transform.position = new Vector3(0, 0, -10);
            camera.AddComponent<AudioListener>();

            // Create Canvas
            GameObject canvas = new GameObject("Canvas");
            Canvas c = canvas.AddComponent<Canvas>();
            c.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();

            // Create Title
            GameObject title = CreateUIElement(canvas.transform, "Title");

            // Create Start Button
            GameObject startButton = CreateUIElement(canvas.transform, "StartButton");

            // Create EventSystem
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

            // Save scene
            EditorSceneManager.SaveScene(scene, "Assets/Scenes/MainMenu.unity");
            Debug.Log("[AutoProjectSetup] Created MainMenu scene");
        }

        private static void ConfigureProjectSettings()
        {
            Debug.Log("[AutoProjectSetup] Configuring project settings...");

            // Configure layer collision matrix
            ConfigureLayerCollisions();

            // Add scenes to build settings
            AddScenesToBuildSettings();

            Debug.Log("[AutoProjectSetup] Project settings configured.");
        }

        private static void ConfigureLayerCollisions()
        {
            // Get layer indices
            int playerLayer = LayerMask.NameToLayer("Player");
            int enemyLayer = LayerMask.NameToLayer("Enemy");
            int projectileLayer = LayerMask.NameToLayer("Projectile");
            int experienceLayer = LayerMask.NameToLayer("Experience");
            int groundLayer = LayerMask.NameToLayer("Ground");

            // Disable collisions between certain layers
            if (playerLayer >= 0 && enemyLayer >= 0)
            {
                Physics2D.IgnoreLayerCollision(playerLayer, experienceLayer, true);
                Physics2D.IgnoreLayerCollision(enemyLayer, experienceLayer, true);
                Physics2D.IgnoreLayerCollision(projectileLayer, experienceLayer, true);
                Physics2D.IgnoreLayerCollision(enemyLayer, projectileLayer, false); // Enable
                Physics2D.IgnoreLayerCollision(playerLayer, projectileLayer, true);
            }

            Debug.Log("[AutoProjectSetup] Layer collision matrix configured.");
        }

        private static void AddScenesToBuildSettings()
        {
            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();

            // Add MainMenu scene
            string mainMenuPath = "Assets/Scenes/MainMenu.unity";
            if (File.Exists(mainMenuPath))
            {
                scenes.Add(new EditorBuildSettingsScene(mainMenuPath, true));
            }

            // Add MainGame scene
            string mainGamePath = "Assets/Scenes/MainGame.unity";
            if (File.Exists(mainGamePath))
            {
                scenes.Add(new EditorBuildSettingsScene(mainGamePath, true));
            }

            EditorBuildSettings.scenes = scenes.ToArray();
            Debug.Log("[AutoProjectSetup] Scenes added to build settings.");
        }
    }
}
