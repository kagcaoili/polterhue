using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Holds references to entity spawn positions in the level prefab
/// Used by LevelManager when instantiating entities
/// </summary>
public class LevelEntityConfig : MonoBehaviour
{
    [SerializeField] private Tilemap _tileMap;
    [SerializeField] private Transform _ghostSpawnPoint;
    [SerializeField] private HumanSpawnPoint[] _humanSpawnPoints;
    [SerializeField] private ControlModeTextSpawnPoint[] _controlModeTextSpawnPoints;

    public Tilemap tileMap => _tileMap;
    public Transform ghostSpawnPoint => _ghostSpawnPoint;
    public HumanSpawnPoint[] humanSpawnPoints => _humanSpawnPoints;
    public ControlModeTextSpawnPoint[] controlModeTextSpawnPoints => _controlModeTextSpawnPoints;
}
