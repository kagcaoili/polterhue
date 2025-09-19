using UnityEngine;

/// <summary>
/// Manages debug settings
/// </summary>
public class DebugManager : MonoBehaviour
{
    // Toggle debug mode on or off
    [SerializeField] private bool debugMode = false;

    // Index of the level to load when starting game
    [SerializeField] private int startLevelIndex = 0;
}
