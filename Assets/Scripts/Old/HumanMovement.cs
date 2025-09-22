using UnityEngine;
using System;

/// <summary>
/// Responsible for random and deterministic movements of units (ghost, human)
/// Travels along 2D grid to random positions 
/// </summary>

// Improvement: Since ghost and humans move the same, consider inheriting from generic UnitMovement to reduce redundancy
public class HumanMovement : MonoBehaviour
{
    public Human human { get; private set; }
    private System.Random _rng;
    private Vector2 _currentGridPos;
    private Vector2 _targetGridPos;
    private Vector2 _stepGridPos; // best next step between current and target grid position
    private LevelData _levelData; // Reference to level data for grid size and spawn locations

    public float moveDuration = 0.5f; // how fast the Human moves
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
        human = GetComponent<Human>();
        _rng = new System.Random(GameManagerOld.Instance.seed + human.id + GameManagerOld.HUMAN_SEED_OFFSET);
        _levelData = human.levelData;

        /*
        TODO: v2 Moving away from Random position generation. Use predefined spawn points instead.
        _currentGridPos = GridUtil.GetRandomGridPosition(_rng, _levelData);
        _targetGridPos = GridUtil.GetRandomGridPosition(_rng, _levelData);
        _stepGridPos = GridUtil.GetNextStepGridPosition(_currentGridPos, _targetGridPos, directions);
        */

        // TODO: Replace with predefined spawn points per level or AI behavior
        _currentGridPos = new Vector2(0, 0);
        _targetGridPos = new Vector2(0, 0);
        _stepGridPos = new Vector2(0, 0);
    }

    // Wait for GhostManager to say it's ok to move
    public void Tick(float deltaTime, bool beginMove)
    {
        // Begin next step
        if (beginMove)
        {
            isMoving = true;
        }

        if (!isMoving)
        {
            return;
        }

        moveTimer += deltaTime;
        float t = Mathf.Clamp01(moveTimer / moveDuration);

        Vector3 startPos = GridUtil.GridToWorldPosition(_currentGridPos);
        Vector3 stepPos = GridUtil.GridToWorldPosition(_stepGridPos);
        Vector3 targetPos = GridUtil.GridToWorldPosition(_targetGridPos);

        transform.position = Vector3.Lerp(startPos, stepPos, t);

        Debug.DrawLine(startPos, targetPos, Color.white);

        // We reached the step position
        if (t >= 1f)
        {
            _currentGridPos = _stepGridPos; // Update current position to current step position
            GameManagerOld.Instance.gridManager.RegisterHumanArrival(this, _currentGridPos); // Notify ghost manager of new position for collision detection
            isMoving = false;

            if (_currentGridPos == _targetGridPos)
            {
                // TODO: v2 Moving away from Random positions. Stay still until human has a target action.
                // Replace 0,0 with target position
                _targetGridPos = new Vector2(0, 0);
                //_targetGridPos = GridUtil.GetRandomGridPosition(_rng, _levelData);
                _stepGridPos = GridUtil.GetNextStepGridPosition(_currentGridPos, _targetGridPos, directions);
                moveTimer = 0f;
            }
            else
            {
                // Get next step towards target position
                _stepGridPos = GridUtil.GetNextStepGridPosition(_currentGridPos, _targetGridPos, directions);
                moveTimer = 0f; // Reset timer for next movement
            }
        }
    }
}
