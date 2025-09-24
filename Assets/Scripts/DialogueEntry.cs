/// <summary>
/// Wrapper for a single line in a dialogue sequence. Includes the DialogueLine and the DialogueSequence it belongs to.
/// This allows us to keep track of which sequence a line belongs to, useful for DialogueManager queue
/// Struct for performance so there isn't heap allocation
/// </summary>
public struct DialogueEntry
{
    public DialogueLine line;
    public DialogueSequence sequence;

    public DialogueEntry(DialogueLine line, DialogueSequence sequence)
    {
        this.line = line;
        this.sequence = sequence;
    }
}
