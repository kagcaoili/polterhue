using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "SpotlightStep", menuName = "Scriptable Objects/SpotlightStep")]
public class SpotlightStep : FlowStep
{
    public enum SpotlightTargetType
    {
        PlayerGhost,
        // Add other target types as needed, such as Human or tagged object
    }

    [Header("Content")]
    public SpotlightTargetType spotlightTargetType;

    public override IEnumerator Run(FlowContext ctx)
    {
        // Get the target position based on the selected spotlight target type
        Vector3 targetPosition = Vector3.zero;
        switch (spotlightTargetType)
        {
            case SpotlightTargetType.PlayerGhost:
                {
                    targetPosition = ctx.ghostTransform.position;
                    break;
                }
        }

        // Set the spotlight position
        ctx.spotlightEffect.SetSpotlightPosition(targetPosition);
        yield break;
    }
}
