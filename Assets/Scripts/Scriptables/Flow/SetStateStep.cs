using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "SetStateStep", menuName = "Scriptable Objects/SetStateStep")]
public class SetStateStep : FlowStep
{
    [Header("Content")]
    public PlayerStateKey stateKey; // The state to set. Setting it means the player has achieved this state.

    public override IEnumerator Run(FlowContext ctx)
    {
        AppManager.Instance.PlayerState.SetState(stateKey);
        yield break;
    }

}
