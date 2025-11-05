using UnityEngine;
using System.Collections.Generic;

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

    // Custom input settings for advancing dialogue during this sequence
    // Useful for tutorial prompts where player must use specific keys to advance
    // If not enabled, default for advancing dialogue is mouse left button and space key
    // TODO: Only support key inputs for now. No need for custom mouse inputs yet.
    [Header("Custom Input Settings")]
    [SerializeField] private bool useCustomKeyInputs = false;
    [SerializeField] private List<KeyCode> customDialogueKeys = new();

    /// <summary>
    /// Apply custom input settings to the InputManager if configured
    /// </summary>
    public void ApplyInputSettings(InputManager inputManager)
    {
        if (useCustomKeyInputs)
        {
            inputManager.SetDialogueInputs(false, 0, true, customDialogueKeys);
        }
        else
        {
            // Reset to default dialogue inputs
            inputManager.SetDialogueInputs();
        }
    }

}
