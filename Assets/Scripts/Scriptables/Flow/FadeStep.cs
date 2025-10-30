using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "FadeStep", menuName = "Scriptable Objects/FadeStep")]
public class FadeStep : FlowStep
{
    [Header("Content")]
    public float targetAlpha = 1f;
    public float fadeDuration = 1f;

    public override IEnumerator Run(FlowContext ctx)
    {
        float startAlpha = ctx.fadeOverlay.color.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            Color color = ctx.fadeOverlay.color;
            color.a = newAlpha;
            ctx.fadeOverlay.color = color;
            yield return null;
        }

        // Ensure final alpha is set
        Color finalColor = ctx.fadeOverlay.color;
        finalColor.a = targetAlpha;
        ctx.fadeOverlay.color = finalColor;
    }

}
