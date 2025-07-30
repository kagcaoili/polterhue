using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class PortalManager : MonoBehaviour
{
    public GameObject PortalPrefab;

    // Reference to portal object root in scene. When spawning portals, this is the parent.
    private Transform _portalRoot;

    // Reference to current level, should be reset when level changes
    private LevelData _currentLevelData;

    // List of active portal objects
    private List<Portal> _portals;

    // To generate a ghost at the portal on click
    private Action<GhostType> _spawnGhostCallback;

    // Accessed by game manager
    public void Setup(Transform root, Action<GhostType> spawnGhostCallback)
    {
        _portalRoot = root;
        _spawnGhostCallback = spawnGhostCallback;
    }

    // Called by GameManager to initialize items per level
    // This should be called when the level is loaded or reset
    public void InitializeSpawn(LevelData levelData)
    {
        _currentLevelData = levelData;
        _portals = _portalRoot.GetComponentsInChildren<Portal>().ToList();

        for (int i = 0; i < _portals.Count; i++)
        {
            _portals[i].gameObject.name = $"Portal_{_portals[i].type}_{i}";
            GhostType type = (GhostType)(i % System.Enum.GetValues(typeof(GhostType)).Length);
            _portals[i].Setup(this, i, type);
        }
    }

    // Called as part of tick loop in GameManager every frame
    public void UpdatePortals()
    {
        // Ensure deterministic ordering by sorting portal by id
        _portals.Sort((a, b) => a.id.CompareTo(b.id));

        // Call tick function on each portal to update UI
        for (int i = 0; i < _portals.Count; i++)
        {
            // This will handle updating soul logic for the portal
            _portals[i].UpdateSoulUI();
        }
    }

    // Spawns ghost associated with the given type
    public void SpawnGhostCallback(GhostType type)
    {
        _spawnGhostCallback?.Invoke(type);
    }
}