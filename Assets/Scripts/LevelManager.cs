using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

/// <summary>
/// Manages level loading and progression
/// </summary>
public class LevelManager : MonoBehaviour
{
    public List<LevelData> levels = new List<LevelData>();
    public Transform levelMapRoot; // Acts as parent for instantiated tilemap
    public event Action OnLevelComplete;

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

        if (!ValidLevelIndex(levelIndex))
        {
            Debug.LogError($"Invalid level index: {levelIndex}. Cannot load level.");
            return null;
        }

        LevelData levelData = levels[levelIndex];
        GameObject tileMapObject = Instantiate(levelData.tileMapPrefab);

        // Set the parent to the tile map root
        tileMapObject.transform.SetParent(levelMapRoot, false);

        Tilemap tilemap = tileMapObject.GetComponent<Tilemap>();
        tilemap.CompressBounds();
        //levelData.SetGridBounds(tilemap.origin, tilemap.size);
        //Debug.Log("Grid bounds set: " + levelData.gridOrigin + ", " + levelData.gridSize);

        Debug.Log($"Loading Level {levelData.levelIndex}");
        return levelData;
    }

    private void PlayerCompletedLevel()
    {
        OnLevelComplete?.Invoke();
    }
    
    /// <summary>
    /// Validates if the level index is within the bounds of available levels
    /// </summary>
    /// <param name="levelIndex"></param>
    /// <returns></returns>
    private bool ValidLevelIndex(int levelIndex)
    {
        return levelIndex >= 0 && levelIndex < levels.Count;
    }
}
