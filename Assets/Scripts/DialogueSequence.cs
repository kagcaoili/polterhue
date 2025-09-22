using UnityEngine;

/// <summary>
/// Holds a sequence of dialogue lines
/// Given to LevelData to define dialogues for that level
/// DialogueManager reaches into this to get the lines to display
/// </summary>
[CreateAssetMenu(fileName = "DialogueSequence", menuName = "Scriptable Objects/DialogueSequence")]
public class DialogueSequence : ScriptableObject
{
    public DialogueLine[] lines;

    // TODO: Implement this. Right now, it just always blocks because mouse button is the only input
    public bool isBlocking = true; // If true, player cannot move/act during dialogue

    [Header("Default Speaker Assets")]
    // Used to show active and inactive speakers before lines are read
    public Sprite defaultLeftPortrait; // if empty, hide left portrait
    public Sprite defaultRightPortrait; // if empty, hide right portrait

    // TODO Future: Add conditions or triggers for when this dialogue should play
    // For now, assume one dialogue sequence per level state (intro, outro)
}
