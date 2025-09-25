using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Holds references to entity spawn positions in the level prefab
/// Used by LevelManager when instantiating entities
/// </summary>
public class LevelEntityConfig : MonoBehaviour
{
    [SerializeField] private Tilemap _tileMap;
    [SerializeField] private Transform _ghostSpawnRoot;
    [SerializeField] private Transform[] _humanSpawnRoot;

    public Tilemap tileMap => _tileMap;
    public Transform ghostSpawnRoot => _ghostSpawnRoot;
    public Transform[] humanSpawnRoots => _humanSpawnRoot;
}
