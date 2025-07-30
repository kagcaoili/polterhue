using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Handles collision resolutions for ghost and humans in game
/// GhostManager and HumanManager call into this class to track collisions
/// </summary>
public class GridManager : MonoBehaviour
{
    private Dictionary<Vector2, List<GhostMovement>> _ghostGridLookup = new Dictionary<Vector2, List<GhostMovement>>();
    private Dictionary<Vector2, List<HumanMovement>> _humanGridLookup = new Dictionary<Vector2, List<HumanMovement>>();

    // Called by movement components when unit arrives at grid position
    public void RegisterGhostArrival(GhostMovement ghost, Vector2 gridPosition)
    {
        if (!_ghostGridLookup.ContainsKey(gridPosition))
        {
            _ghostGridLookup[gridPosition] = new List<GhostMovement>();
        }

        _ghostGridLookup[gridPosition].Add(ghost);
    }

    public void RegisterHumanArrival(HumanMovement human, Vector2 gridPosition)
    {
        if (!_humanGridLookup.ContainsKey(gridPosition))
        {
            _humanGridLookup[gridPosition] = new List<HumanMovement>();
        }

        _humanGridLookup[gridPosition].Add(human);
    }

    // Logic to handle collisions between ghosts
    // If two ghosts of same type collide, spawn third ghost
    //    e.g. if (1, red) and (2, red) ghost collide, spawn 1 new ghost (3, red)
    // If two ghosts of different types collide, destroy both
    //    e.g. if (1, red) and (2, blue) ghost collide, destroy both ghosts)
    // If multiple ghosts of same and different types collide, destroy all
    //    e.g. if (1, red), (2, blue), and (3, red) collide, destroys 1,2,3 ghosts
    // If four ghosts of same type collide, spawn 1 new ghost of that type
    //    e.g. if (1, red), (2, red), (3, red), and (4, red) collide, spawn 1 new ghost (5, red)
    //    It could be delightful to players if the more matches, the more ghosts spawn.

    // Collision: If two ghosts land on the same grid position. It is not a collision if they cross paths but land on different tiles.
    //    e.g. if two ghosts are next to each other and one moves left and the other moves right, they do not collide because they land on different tiles.
    //    e.g. if two ghosts arrive at the same tile, they collide.
    public void ResolveCollisions(Action<GhostType> ghostSpawnAction, Action<GhostMovement> ghostDestroyAction, Action<HumanMovement> humanDestroyAction)
    {
        // Check for collisions in the grid lookup
        foreach (var kvp in _ghostGridLookup)
        {
            Vector2 gridPosition = kvp.Key;
            List<GhostMovement> ghostsAtPosition = kvp.Value;

            // Get humans at the same position. This means a ghost and human exist on the same tile.
            List<HumanMovement> humansAtPosition = _humanGridLookup.ContainsKey(gridPosition) ? _humanGridLookup[gridPosition] : new List<HumanMovement>();

            // If a ghost collides with a human and the human has a matching type, destroy the human
            // If a ghost collides with a human and the human does not have a matching type, destroy the ghost.
            if (humansAtPosition.Count > 0)
            {
                foreach (var human in humansAtPosition)
                {
                    // If the human's type matches any of the ghost's type, destroy the human
                    if (ghostsAtPosition.Exists(g => g.ghost.type == human.human.type))
                    {
                        // Invoke the action to destroy the human
                        humanDestroyAction?.Invoke(human);
                        Debug.Log($"Human at {gridPosition} destroyed due to ghost collision with type {human.human.type}.");
                    }
                }
            }

            if (ghostsAtPosition.Count > 1)
            {
                bool sameTypeCollision = ghostsAtPosition.TrueForAll(g => g.ghost.type == ghostsAtPosition[0].ghost.type);
                if (sameTypeCollision)
                {
                    ghostSpawnAction?.Invoke(ghostsAtPosition[0].ghost.type);
                    Debug.Log($"Collision of same type ghosts at {gridPosition}. Spawned new ghost of type {ghostsAtPosition[0].ghost.type}.");
                }
                else
                {
                    // Different types collided, destroy all ghosts at this position
                    foreach (var ghost in ghostsAtPosition)
                    {
                        ghostDestroyAction?.Invoke(ghost);
                    }
                    Debug.Log($"Collision of different type ghosts at {gridPosition}. Destroyed all ghosts.");
                }
            }
        }

        // Clear the lookup for next frame
        _ghostGridLookup.Clear();
        _humanGridLookup.Clear();
    }
}
