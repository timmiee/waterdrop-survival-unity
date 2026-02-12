using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace WaterDropSurvival.Editor
{
    /// <summary>
    /// Helper utilities for the Water Drop Survival project.
    /// </summary>
    public class ProjectUtilities
    {
        [MenuItem("Water Drop Survival/Open MainGame Scene", false, 100)]
        public static void OpenMainGameScene()
        {
            string scenePath = "Assets/Scenes/MainGame.unity";
            if (System.IO.File.Exists(scenePath))
            {
                EditorSceneManager.OpenScene(scenePath);
                Debug.Log("[ProjectUtilities] Opened MainGame scene");
            }
            else
            {
                EditorUtility.DisplayDialog("Scene Not Found", 
                    "MainGame.unity not found. Run 'Run Project Setup' first.", "OK");
            }
        }

        [MenuItem("Water Drop Survival/Open MainMenu Scene", false, 101)]
        public static void OpenMainMenuScene()
        {
            string scenePath = "Assets/Scenes/MainMenu.unity";
            if (System.IO.File.Exists(scenePath))
            {
                EditorSceneManager.OpenScene(scenePath);
                Debug.Log("[ProjectUtilities] Opened MainMenu scene");
            }
            else
            {
                EditorUtility.DisplayDialog("Scene Not Found", 
                    "MainMenu.unity not found. Run 'Run Project Setup' first.", "OK");
            }
        }

        [MenuItem("Water Drop Survival/Wire Scene References", false, 200)]
        public static void WireSceneReferences()
        {
            if (!EditorUtility.DisplayDialog("Wire Scene References",
                "This will attempt to automatically wire up references in the current scene.\n\n" +
                "Make sure you have the correct scene open (MainGame.unity).\n\n" +
                "Continue?",
                "Yes", "Cancel"))
            {
                return;
            }

            try
            {
                int wiredCount = 0;

                // Find GameManager and wire references
                var gameManager = Object.FindObjectOfType<GameManager>();
                if (gameManager != null)
                {
                    SerializedObject so = new SerializedObject(gameManager);
                    
                    // Wire PlayerStats
                    if (so.FindProperty("playerStats").objectReferenceValue == null)
                    {
                        var playerStats = Object.FindObjectOfType<WaterDropSurvival.Player.PlayerStats>();
                        if (playerStats != null)
                        {
                            so.FindProperty("playerStats").objectReferenceValue = playerStats;
                            wiredCount++;
                        }
                    }

                    // Wire ExperienceManager
                    if (so.FindProperty("experienceManager").objectReferenceValue == null)
                    {
                        var expManager = Object.FindObjectOfType<WaterDropSurvival.Systems.ExperienceManager>();
                        if (expManager != null)
                        {
                            so.FindProperty("experienceManager").objectReferenceValue = expManager;
                            wiredCount++;
                        }
                    }

                    // Wire LevelSystem
                    if (so.FindProperty("levelSystem").objectReferenceValue == null)
                    {
                        var levelSystem = Object.FindObjectOfType<WaterDropSurvival.Systems.LevelSystem>();
                        if (levelSystem != null)
                        {
                            so.FindProperty("levelSystem").objectReferenceValue = levelSystem;
                            wiredCount++;
                        }
                    }

                    // Wire UpgradeSystem
                    if (so.FindProperty("upgradeSystem").objectReferenceValue == null)
                    {
                        var upgradeSystem = Object.FindObjectOfType<WaterDropSurvival.Systems.UpgradeSystem>();
                        if (upgradeSystem != null)
                        {
                            so.FindProperty("upgradeSystem").objectReferenceValue = upgradeSystem;
                            wiredCount++;
                        }
                    }

                    // Wire WaveManager
                    if (so.FindProperty("waveManager").objectReferenceValue == null)
                    {
                        var waveManager = Object.FindObjectOfType<WaterDropSurvival.Systems.WaveManager>();
                        if (waveManager != null)
                        {
                            so.FindProperty("waveManager").objectReferenceValue = waveManager;
                            wiredCount++;
                        }
                    }

                    // Wire EnemySpawner
                    if (so.FindProperty("enemySpawner").objectReferenceValue == null)
                    {
                        var enemySpawner = Object.FindObjectOfType<WaterDropSurvival.Enemies.EnemySpawner>();
                        if (enemySpawner != null)
                        {
                            so.FindProperty("enemySpawner").objectReferenceValue = enemySpawner;
                            wiredCount++;
                        }
                    }

                    // Wire Prefabs
                    GameObject playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");
                    if (playerPrefab != null && so.FindProperty("playerPrefab").objectReferenceValue == null)
                    {
                        so.FindProperty("playerPrefab").objectReferenceValue = playerPrefab;
                        wiredCount++;
                    }

                    GameObject expPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ExperiencePickup.prefab");
                    if (expPrefab != null && so.FindProperty("experiencePickupPrefab").objectReferenceValue == null)
                    {
                        so.FindProperty("experiencePickupPrefab").objectReferenceValue = expPrefab;
                        wiredCount++;
                    }

                    so.ApplyModifiedProperties();
                    EditorUtility.SetDirty(gameManager);
                }

                // Wire EnemySpawner prefabs
                var spawner = Object.FindObjectOfType<WaterDropSurvival.Enemies.EnemySpawner>();
                if (spawner != null)
                {
                    SerializedObject so = new SerializedObject(spawner);

                    GameObject squareEnemy = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/SquareEnemy.prefab");
                    if (squareEnemy != null && so.FindProperty("squareEnemyPrefab").objectReferenceValue == null)
                    {
                        so.FindProperty("squareEnemyPrefab").objectReferenceValue = squareEnemy;
                        wiredCount++;
                    }

                    GameObject triangleEnemy = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/TriangleEnemy.prefab");
                    if (triangleEnemy != null && so.FindProperty("triangleEnemyPrefab").objectReferenceValue == null)
                    {
                        so.FindProperty("triangleEnemyPrefab").objectReferenceValue = triangleEnemy;
                        wiredCount++;
                    }

                    GameObject roundEnemy = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/RoundEnemy.prefab");
                    if (roundEnemy != null && so.FindProperty("roundEnemyPrefab").objectReferenceValue == null)
                    {
                        so.FindProperty("roundEnemyPrefab").objectReferenceValue = roundEnemy;
                        wiredCount++;
                    }

                    so.ApplyModifiedProperties();
                    EditorUtility.SetDirty(spawner);
                }

                // Save scene
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                EditorSceneManager.SaveOpenScenes();

                EditorUtility.DisplayDialog("Wiring Complete",
                    $"Successfully wired {wiredCount} references.\n\n" +
                    "Scene has been saved.",
                    "OK");

                Debug.Log($"[ProjectUtilities] Wired {wiredCount} references");
            }
            catch (System.Exception e)
            {
                EditorUtility.DisplayDialog("Error",
                    $"An error occurred while wiring references:\n\n{e.Message}",
                    "OK");
                Debug.LogError($"[ProjectUtilities] Error wiring references: {e}");
            }
        }

        [MenuItem("Water Drop Survival/Validate Project Setup", false, 300)]
        public static void ValidateProjectSetup()
        {
            System.Text.StringBuilder report = new System.Text.StringBuilder();
            report.AppendLine("=== Water Drop Survival - Project Validation ===\n");

            int issueCount = 0;

            // Check scenes
            report.AppendLine("SCENES:");
            if (System.IO.File.Exists("Assets/Scenes/MainGame.unity"))
                report.AppendLine("✅ MainGame.unity exists");
            else
            {
                report.AppendLine("❌ MainGame.unity NOT FOUND");
                issueCount++;
            }

            if (System.IO.File.Exists("Assets/Scenes/MainMenu.unity"))
                report.AppendLine("✅ MainMenu.unity exists");
            else
            {
                report.AppendLine("❌ MainMenu.unity NOT FOUND");
                issueCount++;
            }

            // Check materials
            report.AppendLine("\nMATERIALS:");
            string[] materials = {
                "Assets/Materials/WaterDropMaterial.mat",
                "Assets/Materials/GelMaterial.mat",
                "Assets/Materials/GroundMaterial.mat",
                "Assets/Materials/Enemies/SquareEnemyMaterial.mat",
                "Assets/Materials/Enemies/TriangleEnemyMaterial.mat",
                "Assets/Materials/Enemies/RoundEnemyMaterial.mat"
            };

            foreach (var mat in materials)
            {
                if (System.IO.File.Exists(mat))
                    report.AppendLine($"✅ {System.IO.Path.GetFileName(mat)}");
                else
                {
                    report.AppendLine($"❌ {System.IO.Path.GetFileName(mat)} NOT FOUND");
                    issueCount++;
                }
            }

            // Check prefabs
            report.AppendLine("\nPREFABS:");
            string[] prefabs = {
                "Assets/Prefabs/Player.prefab",
                "Assets/Prefabs/Enemies/SquareEnemy.prefab",
                "Assets/Prefabs/Enemies/TriangleEnemy.prefab",
                "Assets/Prefabs/Enemies/RoundEnemy.prefab",
                "Assets/Prefabs/Projectile.prefab",
                "Assets/Prefabs/ExperiencePickup.prefab"
            };

            foreach (var prefab in prefabs)
            {
                if (System.IO.File.Exists(prefab))
                    report.AppendLine($"✅ {System.IO.Path.GetFileName(prefab)}");
                else
                {
                    report.AppendLine($"❌ {System.IO.Path.GetFileName(prefab)} NOT FOUND");
                    issueCount++;
                }
            }

            // Summary
            report.AppendLine($"\n=== SUMMARY ===");
            if (issueCount == 0)
            {
                report.AppendLine("✅ All assets found! Project is ready.");
            }
            else
            {
                report.AppendLine($"❌ {issueCount} assets missing.");
                report.AppendLine("\nRun 'Water Drop Survival > Run Project Setup' to create missing assets.");
            }

            Debug.Log(report.ToString());
            EditorUtility.DisplayDialog("Project Validation",
                report.ToString(),
                "OK");
        }

        [MenuItem("Water Drop Survival/Documentation/Open Setup Guide", false, 400)]
        public static void OpenSetupGuide()
        {
            string path = System.IO.Path.Combine(Application.dataPath, "../UNITY_SETUP_README.md");
            if (System.IO.File.Exists(path))
            {
                Application.OpenURL("file://" + path);
            }
            else
            {
                EditorUtility.DisplayDialog("File Not Found", "UNITY_SETUP_README.md not found", "OK");
            }
        }

        [MenuItem("Water Drop Survival/Documentation/Open Component Reference", false, 401)]
        public static void OpenComponentReference()
        {
            string path = System.IO.Path.Combine(Application.dataPath, "../COMPONENT_REFERENCE.md");
            if (System.IO.File.Exists(path))
            {
                Application.OpenURL("file://" + path);
            }
            else
            {
                EditorUtility.DisplayDialog("File Not Found", "COMPONENT_REFERENCE.md not found", "OK");
            }
        }
    }
}
