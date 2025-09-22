using UnityEngine;

/// <summary>
/// Manages debug settings
/// </summary>
public class DebugManager : MonoBehaviour
{
    // Toggle debug mode on or off
    public bool DebugMode = false;

    // Index of the level to load when starting game
    // TODO: How to only set via editor and not in code? Set to public for quick access
    public int startLevelIndex = 0;
}
