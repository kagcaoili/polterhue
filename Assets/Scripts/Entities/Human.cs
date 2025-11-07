using UnityEngine;

/// <summary>
/// Base class for instanced human behavior
/// </summary>
public class Human : MonoBehaviour
{
    private HumanType type; // reference to scriptable object type for data

    public void Setup(HumanType type)
    {
        this.type = type;
    }
}
