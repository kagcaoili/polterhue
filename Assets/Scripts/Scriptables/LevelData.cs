using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// LevelData holds information about the level
/// Contains abstract data about the level
/// Prefab references spatial data such as entity spawn positions and tilemap
/// </summary>
[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    [Header("Level Settings")]
    public int levelIndex;
    public int initialHumanCount = 1; // Number of humans to spawn at start of
    public GameObject levelPrefab; // Tilemap and spawn markers

    [Header("Dialogue")]
    public DialogueSequence levelIntro;
    public DialogueSequence levelOutro;

    [Header("Flow")]
    public List<FlowStep> steps = new(); // list of all the cinematic/flow steps in this level
    
    // TODO: Assumes one dialogue sequence per level state
    // Dialogue sequence will contain multiple lines
    // Not scalable if we add more states or want behaviors to trigger dialogue
    // If so, consider an array of DialogueSequences. Each seq contains priority or condition
    // As part of update loop, check if condition met to trigger dialogue sequence
    // For now, keep it simple
}
