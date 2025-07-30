using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public GameObject GhostBasePrefab;

    // Reference to ghost object root in scene. When spawning ghost, this is the parent.
    private Transform _ghostRoot;

    // Reference to current level, should be reset when level changes
    private LevelData _currentLevelData;

    // List of all active ghosts in game
    private List<GhostMovement> _ghosts = new List<GhostMovement>();

    // Queue ghosts waiting to spawn until next tick
    private Queue<GhostType> waitingToSpawn = new Queue<GhostType>();


    // Accessed by game manager
    public void Setup(Transform ghostRoot)
    {
        _ghostRoot = ghostRoot;
    }

    // Called by GameManager to initialize ghosts and humans per level
    // This should be called when the level is loaded or reset
    // Spawn ghost off screen for until ghost movement is ready
    // Loop through ghost enum types spawning one of each type
    public void InitializeSpawn(LevelData levelData)
    {
        _currentLevelData = levelData;
        int numberOfGhosts = levelData.initialGhostCount;
        for (int i = 0; i < numberOfGhosts; i++)
        {
            GhostType type = (GhostType)(i % System.Enum.GetValues(typeof(GhostType)).Length);
            SpawnGhost(type);
        }
    }

    // Queue to spawn at next tick
    public void QueueSpawnGhost(GhostType type)
    {
        waitingToSpawn.Enqueue(type);
    }

    public void SpawnGhostsQueued()
    {
        while (waitingToSpawn.Count > 0)
        {
            GhostType type = waitingToSpawn.Dequeue();
            SpawnGhost(type);
        }
    }

    private void SpawnGhost(GhostType type)
    {
        Vector3 position = GridUtil.offscreenPosition; // Start offscreen until ready to move
        GameObject ghost = Instantiate(GhostBasePrefab, position, Quaternion.identity);
        ghost.name = $"Ghost_{type}_{_ghosts.Count}";
        ghost.transform.SetParent(_ghostRoot, false); // Set parent to ghostRoot

        Ghost ghostComponent = ghost.GetComponent<Ghost>();
        ghostComponent.Setup(_ghosts.Count, type);
        ghostComponent.RegisterLevelData(_currentLevelData);

        GhostMovement ghostMovement = ghost.GetComponent<GhostMovement>();
        ghostMovement.Setup(); // Initialize ghost movement

        _ghosts.Add(ghostMovement);
        Debug.Log($"Spawned {ghost.name} at {position}");
    }

    public void DestroyGhost(GhostMovement ghost)
    {
        _ghosts.Remove(ghost);
        Destroy(ghost.gameObject);
    }

    // Improvement: Consider object pooling
    public void CleanUpSpawn()
    {
        _ghosts.Clear();

        // Remove all ghosts and humans from the scene
        foreach (Transform child in _ghostRoot)
        {
            Destroy(child.gameObject);
        }
    }


    // Called every frame by GameManager
    // Returns true if all ghosts have finished moving
    public bool Move(float deltaTime, bool beginMove)
    {
        // Ensure deterministic ordering by sorting ghosts by id
        _ghosts.Sort((a, b) => a.ghost.id.CompareTo(b.ghost.id));

        bool allDone = true;

        // Call Tick on each ghost to trigger next movement
        for (int i = 0; i < _ghosts.Count; i++)
        {
            // Call Tick on GhostMovement component
            // This will handle movement logic for the ghost
            _ghosts[i].Tick(deltaTime, beginMove);
            if (_ghosts[i].isMoving)
            {
                allDone = false;
            }
        }

        return allDone;
    }

    public int GetActiveGhostCount()
    {
        return _ghosts.Count;
    }
}
