using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Handles collision resolutions for ghost and humans in game
/// GhostManager and HumanManager call into this class to track collisions
/// Improvement: Consider NavMesh for movement logic, esp for obstacle detection
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

    // TODO: If ghost collides with human, destroy human
    public void ResolveCollisions(Action<HumanMovement> humanDestroyAction)
    {
        // Check for collisions in the grid lookup
        foreach (var kvp in _ghostGridLookup)
        {
            Vector2 gridPosition = kvp.Key;
            List<GhostMovement> ghostsAtPosition = kvp.Value;

            // Get humans at the same position. This means a ghost and human exist on the same tile.
            List<HumanMovement> humansAtPosition = _humanGridLookup.ContainsKey(gridPosition) ? _humanGridLookup[gridPosition] : new List<HumanMovement>();

            // If a ghost collides with a human, destroy the human
            if (humansAtPosition.Count > 0)
            {
                foreach (var human in humansAtPosition)
                {
                    // Invoke the action to destroy the human
                    humanDestroyAction?.Invoke(human);
                    Debug.Log($"Human at {gridPosition} destroyed due to ghost collision with type {human.human.type}.");
                }
            }
        }

        // Clear the lookup for next frame
        _ghostGridLookup.Clear();
        _humanGridLookup.Clear();
    }
}
