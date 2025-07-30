using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ghost contains ghost type and sprite information. Referenced by GhostMovement.
/// </summary>
public class Ghost : MonoBehaviour
{
    public GhostTypeSpriteRegistry spriteRegistry;
    public Image spriteReference;

    // unique per ghost. assumes number of ghosts won't go past int.MAX_VALUE
    // otherwise, design game to never go past int.MAX_VALUE or
    // just reuse older numbers 
    public int id { get; private set; }
    public GhostType type { get; private set; }

    // Reference to level data for this ghost for grid movement information
    public LevelData levelData { get; private set; }

    public void Setup(int id, GhostType type)
    {
        this.id = id;
        this.type = type;

        // set sprite based on type
        Sprite sprite = spriteRegistry.GetSprite(type);
        if (sprite == null)
        {
            Debug.LogWarning($"No sprite found for ghost type {type}");
        }
        else
        {
            spriteReference.sprite = sprite;
            spriteReference.color = spriteRegistry.GetColor(type);
        }
    }

    public void RegisterLevelData(LevelData levelData)
    {
        this.levelData = levelData;
    }
}
