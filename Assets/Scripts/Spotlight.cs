using UnityEngine;

public class Spotlight : MonoBehaviour
{
    [SerializeField] private GameObject blackOverlay;
    [SerializeField] private GameObject circleMask;

    public void SetSpotlightPosition(Vector3 targetWorldPosition)
    {
        circleMask.transform.position = targetWorldPosition;
    }
}
