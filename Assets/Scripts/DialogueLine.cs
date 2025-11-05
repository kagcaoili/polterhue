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

    public enum Alignment
    {
        Left = 0,
        Right = 1,
        Center = 2
    }
    public Alignment alignment = Alignment.Left; // Alignment of the dialogue line
    
}
