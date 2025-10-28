using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public GameObject GhostBasePrefab;

    // Reference to ghost object root in scene. When spawning ghost, this is the parent.
    private Transform _ghostRoot;

    // Reference to current level, should be reset when level changes
    private LevelData _currentLevelData;

    // Reference to active ghost in game
    private GhostMovement activeGhost;


    // Accessed by game manager
    public void Setup(Transform ghostRoot)
    {
        _ghostRoot = ghostRoot;
    }

    // Called by GameManager to initialize ghost per level
    // This should be called when the level is loaded or reset
    // Spawn ghost off screen for until ghost movement is ready
    // Only one type of ghost is spawned.
    // TODO: Ghost spawns at the same position in the map. Top center.
    // where the elevator is
    // Type will be the emotional response of the past level
    public void InitializeSpawn(LevelData levelData)
    {
        _currentLevelData = levelData;
        activeGhost = SpawnGhost((GhostType)0);
    }

    private GhostMovement SpawnGhost(GhostType type)
    {
        Vector3 position = GridUtil.offscreenPosition; // Start offscreen until ready to move
        GameObject ghost = Instantiate(GhostBasePrefab, position, Quaternion.identity);
        ghost.name = $"Ghost";
        ghost.transform.SetParent(_ghostRoot, false); // Set parent to ghostRoot

        Ghost ghostComponent = ghost.GetComponent<Ghost>();
        //ghostComponent.Setup(0, type);
        //ghostComponent.RegisterLevelData(_currentLevelData);

        GhostMovement ghostMovement = ghost.GetComponent<GhostMovement>();
        ghostMovement.Setup(); // Initialize ghost movement

        Debug.Log($"Spawned {ghost.name} at {position}");
        return ghostMovement;
    }

    // TODO: Can't happen in current game design
    // Instead of getting destroyed, should just kick the ghost back to start
    public void DestroyGhost()
    {
        Destroy(activeGhost.gameObject);
        activeGhost = null;
    }

    // todo: reset ghost position to start of level
    public void ResetPosition()
    {
        //activeGhost.ResetPosition();
    }

    // Called every frame by GameManager
    // Returns true if all ghosts have finished moving
    public bool Move(float deltaTime, bool beginMove)
    {

        bool allDone = true;

        // Call Tick on GhostMovement component
        // This will handle movement logic for the ghost
        activeGhost.Tick(deltaTime, beginMove);
        if (activeGhost.isMoving)
        {
            allDone = false;
        }

        return allDone;
    }
}
