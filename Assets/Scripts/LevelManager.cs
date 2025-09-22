using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages level loading and progression
/// </summary>
public class LevelManager : MonoBehaviour
{
    public List<LevelData> levels = new List<LevelData>();

    public event Action OnLevelComplete;
    public LevelData LoadLevel(int levelIndex)
    {
        Debug.Log($"Loading level {levelIndex}");
        // Implement level loading logic here

        return new LevelData(); // TODO: Placeholder return
    }

    private void PlayerCompletedLevel()
    {
        OnLevelComplete?.Invoke();
    }
}
