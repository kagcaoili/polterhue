using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "DialogueStep", menuName = "Scriptable Objects/DialogueStep")]
public class DialogueStep : FlowStep
{
    [Header("Content")]
    public DialogueSequence sequence;

    bool dialogueFinished = false;

    public override IEnumerator Run(FlowContext ctx)
    {
        ctx.dialogueManager.OnDialogueComplete += HandleDialogueComplete;
        ctx.dialogueManager.PlayDialogue(sequence);

        // Wait until dialogue is finished
        while (dialogueFinished == false)
        {
            yield return null;
        }
    }
    private void HandleDialogueComplete()
    {
        dialogueFinished = true;
    }


}
