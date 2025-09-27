using UnityEngine;

/// <summary>
/// Data class for different human types. Represents concept of each type, not instance in the world
/// Decouples data from prefab. Can reference human type data separately without instantiating prefab
/// </summary>
[CreateAssetMenu(fileName = "HumanType", menuName = "Scriptable Objects/HumanType")]
public class HumanType : ScriptableObject
{
    public string id;
    public Sprite portrait; // For dialogue UI. Sprite is defined in the Human prefab
    public GameObject prefab; // For instantiated human object
}
