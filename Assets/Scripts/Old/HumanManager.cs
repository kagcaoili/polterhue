using System.Collections.Generic;
using UnityEngine;

public class HumanManager : MonoBehaviour
{
    public GameObject HumanBasePrefab;

    // Reference to human object root in scene. When spawning ghost, this is the parent.
    private Transform _humanRoot;

    // Reference to current level, should be reset when level changes
    private LevelData _currentLevelData;

    // List of active human objects
    private List<HumanMovement> _humans = new List<HumanMovement>();

    // Accessed by game manager
    public void Setup(Transform root)
    {
        _humanRoot = root;
    }

    // Called by GameManager to initialize humans per level
    // This should be called when the level is loaded or reset
    public void InitializeSpawn(LevelData levelData)
    {
        _currentLevelData = levelData;
        int numberOfGhosts = levelData.initialHumanCount;
        for (int i = 0; i < numberOfGhosts; i++)
        {
            GhostType type = (GhostType)(i % System.Enum.GetValues(typeof(GhostType)).Length);
            SpawnHuman(type);
        }
    }

    private void SpawnHuman(GhostType type)
    {
        Vector3 position = GridUtil.offscreenPosition; // Start offscreen until ready to move
        GameObject human = Instantiate(HumanBasePrefab, position, Quaternion.identity);
        human.name = $"Human_{type}_{_humans.Count}";
        human.transform.SetParent(_humanRoot, false); // Set parent to root

        HumanOld humanComponent = human.GetComponent<HumanOld>();
        humanComponent.Setup(_humans.Count, type);
        humanComponent.RegisterLevelData(_currentLevelData);

        HumanMovement movement = human.GetComponent<HumanMovement>();
        movement.Setup(); // Initialize human movement

        _humans.Add(movement);
        Debug.Log($"Spawned {human.name} at {position}");
    }

    // Improvement: Consider object pooling
    public void CleanUpSpawn()
    {
        _humans.Clear();

        // Remove all humans from the scene
        foreach (Transform child in _humanRoot)
        {
            Destroy(child.gameObject);
        }
    }


    // Called every frame by GameManager
    public bool Move(float deltaTime, bool beginMove)
    {
        // Ensure deterministic ordering by sorting human by id
        _humans.Sort((a, b) => a.human.id.CompareTo(b.human.id));

        bool allDone = true;

        // Call Tick on each unit to trigger next movement
        for (int i = 0; i < _humans.Count; i++)
        {
            // This will handle movement logic for the unit
            _humans[i].Tick(deltaTime, beginMove);
            if (_humans[i].isMoving)
            {
                allDone = false;
            }
        }

        return allDone;
    }

    public void DestroyHuman(HumanMovement human)
    {
        _humans.Remove(human);
        Destroy(human.gameObject);
    }

    public int GetActiveHumanCount()
    {
        return _humans.Count;
    }
}
