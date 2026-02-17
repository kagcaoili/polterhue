using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Persistent manager for low level input events
/// DialogueSequence can configure custom input settings for advancing dialogue
/// InputManager applies these settings when a dialogue sequence starts
/// Default example: SetDialogueInputs(true, 0, true, null)
/// Ctrl example: SetDialogueInputs(false, 0, true, new List<KeyCode> { KeyCode.LeftControl, KeyCode.RightControl });
/// </summary>
public class InputManager : MonoBehaviour
{
    // Events accessible by other systems
    // Improvement: Update to match ControlModeSignal pattern
    public event Action OnAdvanceDialogue;

    // Default advance inputs are left mouse button and space bar
    [Header("Default Dialogue Input Settings")]
    [SerializeField] private bool enableMouseInput = true;
    [SerializeField] private int mouseButton = 0; // 0 = left, 1 = right, 2 = middle
    [SerializeField] private bool enableKeyboardInput = true;
    [SerializeField] private List<KeyCode> defaultDialogueKeys = new List<KeyCode> { KeyCode.Space };

    // Current dialogue input settings
    private List<KeyCode> dialogueKeys = new List<KeyCode> { KeyCode.Space };

    // Player state service references
    private IPlayerStateService playerState;
    private bool unlocked_CtrlMode = false;

    #region MonoBehaviour Methods
    void Awake()
    {
        playerState = AppManager.Instance.PlayerState;
        playerState.OnStateSet += HandlePlayerStateSet; // InputManager listens for player state changes
    }
    
    void Update()
    {
        bool inputDetected = false;

        // Check mouse input if enabled
        if (enableMouseInput && Input.GetMouseButtonDown(mouseButton))
        {
            inputDetected = true;
        }

        // Check keyboard input if enabled
        if (enableKeyboardInput && !inputDetected)
        {
            // Only allow configured keys to advance dialogue
            foreach (KeyCode key in dialogueKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    inputDetected = true;
                    break;
                }
            }
        }

        // Trigger event if any dialogue advance input detected
        if (inputDetected)
        {
            OnAdvanceDialogue?.Invoke();
        }

        // Trigger Ctrl mode toggle event
        if (unlocked_CtrlMode && (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)))
        {
            Debug.Log("InputManager: Ctrl mode toggled");
            ControlModeSignals.RaiseCtrlModeToggle();
        }
        
    }
    #endregion



    #region Setting Custom Inputs

    /// <summary>
    /// Set dialogue inputs to default settings
    /// </summary>
    public void SetDialogueInputs()
    {
        SetDialogueInputs(true, 0, true, null);
    }

    /// <summary>
    /// Configure dialogue input settings
    /// If no keys are provided, will allow no keyboard inputs
    /// </summary>
    public void SetDialogueInputs(bool enableMouse, int mouseBtn, bool enableKeyboard, List<KeyCode> keys)
    {
        SetMouseInput(enableMouse, mouseBtn);
        SetKeyboardInput(enableKeyboard);
        if (keys != null)
        {
            SetDialogueKeys(keys);
        }
        else
        {
            // No keys provided, clear the list
            SetDialogueKeys(null);
        }
    }

    /// <summary>
    /// Configure mouse input settings for dialogue
    /// </summary>
    private void SetMouseInput(bool enabled, int mouseBtn = 0)
    {
        enableMouseInput = enabled;
        mouseButton = mouseBtn;
    }

    /// <summary>
    /// Configure keyboard input settings for dialogue
    /// </summary>
    private void SetKeyboardInput(bool enabled)
    {
        enableKeyboardInput = enabled;
    }

    /// <summary>
    /// Replace all dialogue keys with custom list of keycodes
    /// </summary>
    private void SetDialogueKeys(List<KeyCode> keys)
    {
        if (keys != null)
        {
            dialogueKeys = new List<KeyCode>(keys);
        }
        else
        {
            dialogueKeys.Clear();
        }
    }

    #endregion

    #region Player State Handling

    /// <summary>
    /// If state has changed, update local flags accordingly
    /// </summary>
    private void HandlePlayerStateSet(PlayerStateKey stateKey)
    {
        if (stateKey == PlayerStateKey.Unlocked_CtrlMode)
        {
            unlocked_CtrlMode = true;
        }
    }

    #endregion
}
