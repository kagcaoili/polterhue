using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// LevelData holds information about the level, including grid size and spawn locations for ghosts and humans.
/// </summary>
[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    [Header("Level Settings")]
    public int levelIndex;
    public int initialSoulCount = 3; // Max souls available at start of level
    public int initialGhostCount = 4; // Number of ghosts to spawn at start of level
    public int initialHumanCount = 1; // Number of humans to spawn at start of
    public GameObject tileMapPrefab; // Tilemap used for this level

    [Header("Grid Settings")]
    public Vector2 gridOrigin { get; private set; }
    public Vector2 gridSize { get; private set; }

    // Used when loading level after instantiating the tilemap
    public void SetGridBounds(Vector3Int origin, Vector3Int size)
    {
        gridOrigin = new Vector2(origin.x, origin.y);
        gridSize = new Vector2(size.x, size.y);
    }
}
