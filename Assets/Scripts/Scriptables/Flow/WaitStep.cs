using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "WaitStep", menuName = "Scriptable Objects/WaitStep")]
public class WaitStep : FlowStep
{
    [Header("Content")]
    public float waitDuration = 1f;

    public override IEnumerator Run(FlowContext ctx)
    {
        yield return new WaitForSeconds(waitDuration);
    }

}
