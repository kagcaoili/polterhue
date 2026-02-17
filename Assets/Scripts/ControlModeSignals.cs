using System;

/// <summary>
/// Static class to hold signals/events for control mode changes
/// Lightweight alternative to using events in Manager directly 
/// Instead of manager having to reference listeners, listeners can subscribe to these static events
/// </summary>
public static class ControlModeSignals
{
    public static event Action OnCtrlModeToggle; // when player toggles ctrl mode on/off
    public static event Action<bool> OnCtrlModeChanged; // bool indicates state of ctrl mode
    public static event Action<char> OnCtrlLetterTyped; // char indicates which letter was typed
    public static event Action OnCtrlLetterFound; // when a letter is correctly typed matching a text

    public static void RaiseCtrlModeToggle()
    {
        OnCtrlModeToggle?.Invoke();
    }

    public static void RaiseCtrlModeChanged(bool isActive)
    {
        OnCtrlModeChanged?.Invoke(isActive);
    }

    public static void RaiseCtrlLetterTyped(char letter)
    {
        OnCtrlLetterTyped?.Invoke(letter);
    }

    public static void RaiseCtrlLetterFound()
    {
        OnCtrlLetterFound?.Invoke();
    }
}
