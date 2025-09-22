using UnityEngine;

/// <summary>
/// A single line of dialogue in a DialogueSequence
/// A DialogueSequence is made up of an array of DialogueLines
/// TODO: Delightful addition would be sound effect or color control per character or character name
/// </summary>
[System.Serializable]
public class DialogueLine
{
    [TextArea]
    public string text; // The dialogue body

    public bool isLeftSide = true; // If true, line is spoken as if from left side of screen. Otherwise right
    
}
