using UnityEngine;

/// <summary>
/// Soul: A type of item earned when a ghost consumes a human. 
/// After a ghost consumes a human, max soul count is increased.
/// Souls regenerate over time based on SoulSettings.
/// This manager handles soul regeneration logic and keeps track of the current soul count.
/// </summary>
public class SoulManager : MonoBehaviour
{
    public int currentSoulCount { get; private set; } = 0;
    public int maxSoulCount { get; private set; } = 0;
    public float currentRegenTime { get; private set; } = 0f;
    public float regenerationDuration = 5f; // Time in seconds to regenerate 1 soul after consumption

    public void Initialize(LevelData levelData)
    {
        currentSoulCount = 0;
        maxSoulCount = levelData.initialSoulCount;
        currentRegenTime = 0f;
    }

    // This method can be used to handle soul regeneration logic
    // Calls every Tick() in GameManager
    public void UpdateSoulData(float deltaTime)
    {
        // Early out if no souls to regenerate
        if (currentSoulCount >= maxSoulCount || maxSoulCount == 0)
        {
            return;
        }

        currentRegenTime += deltaTime;
        int soulsToRegen = Mathf.FloorToInt(currentRegenTime / regenerationDuration);
        if (soulsToRegen > 0)
        {
            currentSoulCount += soulsToRegen; // Accounts for if we earn more than one soul at a time
            currentRegenTime -= soulsToRegen * regenerationDuration; // Reset the regen time by the amount of souls regenerated
        }
    }

    public bool CanConsumeSoul()
    {
        return currentSoulCount > 0;
    }

    // Decreases current soul count by 1 if possible
    // This is called when player consumes a soul to spawn a new ghost
    public void ConsumeSoul()
    {
        if (currentSoulCount > 0)
        {
            currentSoulCount--;
        }
        else
        {
            Debug.LogWarning("No souls available to consume.");
        }
    }

    // Increases the maximum soul count by a specified amount
    // This is called when a ghost consumes a human
    public void IncreaseMaxSoulCount(int amount)
    {
        maxSoulCount += amount;

    }
}
