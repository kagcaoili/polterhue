using UnityEngine;

/// <summary>
/// Placed on gameobject to mark spawn point for humans in the level prefab
/// </summary>
public class HumanSpawnPoint : MonoBehaviour
{
    [SerializeField] private HumanType _humanType; // type of human to spawn here
    public HumanType humanType => _humanType;
}
