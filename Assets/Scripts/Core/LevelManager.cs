using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Manages level loading and progression and creating entities in the level
/// Subscribed to events that entities may emit
/// Responsible for deciding when the level ends and notifying GameManager
/// Improvement: Tightly coupled roots for instantiated objects. Consider having separated classes manage each type of entity
/// </summary>
public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<LevelData> levels = new List<LevelData>();
    [SerializeField] private Transform levelMapRoot; // Acts as parent for instantiated tilemap
    [SerializeField] private Transform humanSpawnRoot; // Acts as parent for instantiated humans
    [SerializeField] private Transform controlModeTextRoot; // Acts as parent for instantiated control mode texts
    [SerializeField] private Ghost mainGhost; // Reference to the main player ghost in scene

    public event Action OnLevelComplete; // Notifies listeners (GameManager) that level is complete

    private Ghost ghost;
    private List<Human> humans;

    private void Start()
    {
        // Without this, tileMap objects will be instantiated at root level
        if (levelMapRoot == null)
        {
            Debug.LogError("Level Map Root is not assigned in LevelManager.");
        }
    }

    public LevelData LoadLevel(int levelIndex)
    {
        Debug.Log($"Loading level {levelIndex}");
        CleanLevelScene();

        if (!ValidLevelIndex(levelIndex))
        {
            Debug.LogError($"Invalid level index: {levelIndex}. Cannot load level.");
            return null;
        }

        LevelData levelData = levels[levelIndex]; // scriptable object data
        LevelEntityConfig levelConfig = Instantiate(levelData.levelPrefab).GetComponent<LevelEntityConfig>();

        // Set the parent to the tile map root to clean hierarchy
        levelConfig.transform.SetParent(levelMapRoot, false);

        Tilemap tilemap = levelConfig.tileMap;
        tilemap.CompressBounds();

        LoadGhost(levelConfig);

        LoadHumans(levelConfig, levelData);

        LoadControlModeTexts(levelConfig);

        Debug.Log($"Loading Level {levelData.levelIndex}");
        return levelData;
    }

    public IEnumerator RunFlow(List<FlowStep> steps, FlowContext context)
    {
        foreach (var step in steps)
        {
            Debug.Log($"Starting flow step: {step.id}");
            yield return StartCoroutine(step.Run(context));
            Debug.Log($"Completed flow step: {step.id}");
        }
    }

    private void PlayerCompletedLevel()
    {
        OnLevelComplete?.Invoke();
    }

    /// <summary>
    /// Removes humans and cleans up the tilemap
    /// Don't destroy ghost as it persists between levels
    /// </summary>
    private void CleanLevelScene()
    {
        foreach (Transform child in humanSpawnRoot)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in levelMapRoot)
        {
            Destroy(child.gameObject);
        }

        humans = null;
    }

    #region LoadLevel Helpers

    private void LoadGhost(LevelEntityConfig levelConfig)
    {
        // Reposition ghost to new spawn point in this level
        // Convert world position to screen position since ghost prefabs use screen space coordinates
        // Improvement: Awkward coupling between world space and screen space here
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(levelConfig.ghostSpawnPoint.position);
        mainGhost.transform.position = screenPosition;

        Debug.Log($"Spawned ghost");
    }

    private void LoadHumans(LevelEntityConfig levelConfig, LevelData levelData)
    {
        // Instantiate humans at spawn points
        // Validate that the number of spawn points in the level is greater than the human count set in the data
        if (!ValidHumanSpawnToCount(levelConfig, levelData))
        {
            Debug.LogError($"Mismatch between human spawn points ({levelConfig.humanSpawnPoints.Length}) and initial human count ({levelData.initialHumanCount}) in LevelData {levelData.levelIndex}");
            return;
        }
        humans = new List<Human>();
        // Improvement: Consider having Human be responsible for positioning themselves at the spawn point
        for (int i = 0; i < levelData.initialHumanCount; i++)
        {
            Transform worldSpawnPoint = levelConfig.humanSpawnPoints[i].transform;
            Vector3 spawnPoint = Camera.main.WorldToScreenPoint(worldSpawnPoint.position);
            HumanType humanType = levelConfig.humanSpawnPoints[i].humanType;
            GameObject humanObject = Instantiate(humanType.prefab, spawnPoint, Quaternion.identity, humanSpawnRoot);
            Debug.Log($"Spawned human {humanType.id}");
            Human human = humanObject.GetComponent<Human>();
            if (human == null)
            {
                Debug.LogError($"Human prefab for type {humanType.id} does not have a Human component.");
                continue;
            }
            human.Setup(humanType);
            humans.Add(human);
        }
    }

    private void LoadControlModeTexts(LevelEntityConfig levelConfig)
    {
        // Instantiate control mode texts at designated points
        // Improvement: Similar to humans, consider having ControlModeText be responsible for positioning itself
        foreach (var textPoint in levelConfig.controlModeTextSpawnPoints)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(textPoint.transform.position);
            GameObject textObject = Instantiate(textPoint.controlModeTextPrefab, screenPosition, Quaternion.identity, controlModeTextRoot);
            Debug.Log($"Spawned ControlModeText at {screenPosition}");
            ControlModeText controlModeText = textObject.GetComponent<ControlModeText>();
            if (controlModeText == null)
            {
                Debug.LogError($"ControlModeText prefab does not have a ControlModeText component.");
                continue;
            }
            controlModeText.Setup(textPoint.text);
        }
    }

    #endregion


    #region Validation

    /// <summary>
    /// Validates if the level index is within the bounds of available levels
    /// </summary>
    /// <param name="levelIndex"></param>
    /// <returns></returns>
    private bool ValidLevelIndex(int levelIndex)
    {
        return levelIndex >= 0 && levelIndex < levels.Count;
    }

    /// <summary>
    /// Validates if the LevelEntityConfig has enough human spawn points for LevelData
    /// </summary>
    /// <param name="config"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    private bool ValidHumanSpawnToCount(LevelEntityConfig config, LevelData data)
    {
        return config.humanSpawnPoints.Length >= data.initialHumanCount;
    }


    #endregion
}
