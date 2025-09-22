using System;
using UnityEngine;

/// <summary>
/// Persistent manager for low level input event 
/// TODO: Expand to handle more complex input scenarios
/// </summary>
public class InputManager : MonoBehaviour
{
    public event Action OnAdvanceDialogue;

    void Update()
    {
        // TODO: Refine to handle cases like spamming input or holding down keys
        // See Game Programming Patterns book for input handling patterns
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            OnAdvanceDialogue?.Invoke();
        }
    }
}
