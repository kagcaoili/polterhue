using UnityEngine;
using System.Collections;

/// <summary>
/// Handles screenshake effects
/// </summary>
public class Screenshake : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.2f;
    private Vector3 originalPosition;
    private Coroutine shakeCr;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        originalPosition = mainCamera.transform.localPosition;
    }

    /// <summary>
    /// Public function to trigger a screenshake effect
    /// </summary>
    public void TriggerShake(float durationOverride = -1f, float magnitudeOverride = -1f)
    {
        // If a shake is in progress, 
        if (shakeCr != null)
        {
            StopCoroutine(shakeCr);
            mainCamera.transform.localPosition = originalPosition;
        }

        float duration = durationOverride > 0 ? durationOverride : shakeDuration;
        float magnitude = magnitudeOverride > 0 ? magnitudeOverride : shakeMagnitude;

        shakeCr = StartCoroutine(Shake(duration, magnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            mainCamera.transform.localPosition = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        mainCamera.transform.localPosition = originalPosition;
    }
}