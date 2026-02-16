using UnityEngine;
using System;

/// <summary>
/// Responsible for random and deterministic movements of units (ghost, human)
/// Travels along 2D grid to random positions 
/// </summary>

// Improvement: Since ghost and humans move the same, consider inheriting from generic UnitMovement to reduce redundancy
public class GhostMovement : MonoBehaviour
{
    public Ghost ghost { get; private set; }
    private Vector2 _currentGridPos;
    private Vector2 _targetGridPos;
    private Vector2 _stepGridPos; // best next step between current and target grid position
    private LevelData _levelData; // Reference to level data for grid size and spawn locations

    public float moveDuration = 0.5f; // how fast the ghost moves
    private float moveTimer = 0f;
    public bool isMoving { get; private set; } = false;

    Vector2[] directions = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    public void Setup()
    {
        ghost = GetComponent<Ghost>();

        // TODO: Replace with predefined spawn points per level or AI behavior
        _currentGridPos = new Vector2(0, 0);
        _targetGridPos = new Vector2(0, 0);
        _stepGridPos = new Vector2(0, 0);
    }

    // Wait for GhostManager to say it's ok to move
    public void Tick(float deltaTime, bool beginMove)
    {
        
    }
}
