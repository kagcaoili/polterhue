using System;
using UnityEngine;

/// <summary>
/// Manages level loading and progression
/// </summary>
public class LevelManager : MonoBehaviour
{
    public event Action OnLevelComplete;
    public void LoadLevel(int levelIndex)
    {
        Debug.Log($"Loading level {levelIndex}");
        // Implement level loading logic here
    }

    private void PlayerCompletedLevel()
    {
        OnLevelComplete?.Invoke();
    }
}
