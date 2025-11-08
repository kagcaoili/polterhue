using UnityEngine;

/// <summary>
/// Placed on gameobject to mark spawn point for control mode texts in the level prefab
/// </summary>
public class ControlModeTextSpawnPoint : MonoBehaviour
{
    // Improvement: Weird to have prefab reference here, but for consistency with other spawn point types
    // There's only one control mode prefab. Tedious to keep having to reference it.
    // Consider registry pattern or scriptable object to hold common references instead
    [SerializeField] private GameObject _controlModeTextPrefab;
    [SerializeField] private string _text;

    public GameObject controlModeTextPrefab => _controlModeTextPrefab;
    public string text => _text;
}
