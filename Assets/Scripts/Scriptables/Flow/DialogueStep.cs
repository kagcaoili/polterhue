using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "DialogueStep", menuName = "Scriptable Objects/DialogueStep")]
public class DialogueStep : FlowStep
{
    [Header("Content")]
    public DialogueSequence sequence;

    bool finished = false;

    public override IEnumerator Run(FlowContext ctx)
    {
        finished = false; // reset for each run

        ctx.dialogueManager.OnDialogueComplete += HandleDialogueComplete;
        ctx.dialogueManager.PlayDialogue(sequence);

        // Wait until dialogue is finished
        while (!finished)
        {
            yield return null;
        }
    }
    private void HandleDialogueComplete()
    {
        Debug.Log("Dialogue complete");
        finished = true;
    }


}
