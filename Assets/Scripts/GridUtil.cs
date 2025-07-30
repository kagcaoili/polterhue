using UnityEngine;
using UnityEngine.Tilemaps;

public static class GridUtil
{
    public static Vector2 offscreenPosition = new Vector2(-1000, -1000); // Offscreen position for spawning items

    public static Vector3 GridToWorldPosition(Vector2 gridPosition)
    {
        // Convert grid position to world position
        Vector3Int cellPos = new Vector3Int((int)gridPosition.x, (int)gridPosition.y, 0);
        Tilemap tileMap = GameManager.Instance.tileMap;
        return tileMap.CellToWorld(cellPos) + tileMap.cellSize * 0.5f; // Center the position in the cell
    }

    public static bool IsValidGridPosition(Vector2 gridPosition)
    {
        Vector3Int cell = new Vector3Int((int)gridPosition.x, (int)gridPosition.y, 0);
        Tilemap tileMap = GameManager.Instance.tileMap;
        return tileMap.HasTile(cell);
    }

    public static Vector2 GetNextStepGridPosition(Vector2 current, Vector2 target, Vector2[] directions)
    {
        // Greedy: Pick a valid direction that takes ghost closest to endPos
        // Improvement: Consider using BFS for more optimal pathfinding
        float closestDistance = float.MaxValue;
        Vector2 bestStep = current;
        foreach (Vector2 dir in directions)
        {
            Vector2 stepGridPos = current + dir;
            if (GridUtil.IsValidGridPosition(stepGridPos))
            {
                float distance = Vector2.Distance(stepGridPos, target);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestStep = stepGridPos;
                }
            }
        }

        return bestStep;
    }

    public static Vector2 GetRandomGridPosition(System.Random rng, LevelData levelData)
    {
        if (rng == null)
        {
            Debug.LogError("Random number generator not initialized. Call Setup() first.");
            return Vector2.zero;
        }

        // Generate a random position within the grid bounds
        int x = rng.Next((int)levelData.gridOrigin.x, (int)levelData.gridOrigin.x + (int)levelData.gridSize.x);
        int y = rng.Next((int)levelData.gridOrigin.y, (int)levelData.gridOrigin.y + (int)levelData.gridSize.y);
        return new Vector2(x, y);
    }
}
