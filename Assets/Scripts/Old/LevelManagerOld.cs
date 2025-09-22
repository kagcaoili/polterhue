using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManagerOld : MonoBehaviour
{
    public List<LevelData> levels = new List<LevelData>();
    private Transform _tileMapRoot;

    public void Setup(Transform tileMapRoot)
    {
        _tileMapRoot = tileMapRoot;

        // Check if levels are set up
        if (levels == null || levels.Count == 0)
        {
            Debug.LogError("No LevelData found in LevelManager.");
            return;
        }
    }

    public (LevelData, Tilemap) LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Count)
        {
            Debug.LogError($"Invalid level index: {levelIndex}. Cannot load level.");
            return (null, null);
        }

        LevelData levelData = levels[levelIndex];
        GameObject tileMapObject = Instantiate(levelData.tileMapPrefab);

        // Set the parent to the tile map root
        tileMapObject.transform.SetParent(_tileMapRoot, false);

        Tilemap tilemap = tileMapObject.GetComponent<Tilemap>();
        tilemap.CompressBounds();
        //levelData.SetGridBounds(tilemap.origin, tilemap.size);
        //Debug.Log("Grid bounds set: " + levelData.gridOrigin + ", " + levelData.gridSize);

        Debug.Log($"Loading Level {levelData.levelIndex}");
        return (levelData, tilemap);
    }

    // Returns true if level index is the last level of the game
    // Used for handling gameover screen
    public bool LastLevel(int levelIndex)
    {
        return levelIndex == levels.Count - 1;
    }
}
