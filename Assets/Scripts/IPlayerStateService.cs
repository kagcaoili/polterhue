using System;

/// <summary>
/// Interface for player state service
/// Allows for abstraction of player state management
/// Right now, supports player pref based state tracking
/// But could be extended to support more complex systems later like cloud
/// If player has a state set, it means they have achieved that state. For example, if Unlocked_CtrlMode is set, it means the player has unlocked Ctrl Mode and can use it in dialogues that require it.
/// Similar to a true/false flag system
/// </summary>
public interface IPlayerStateService
{
    bool HasStateSet(PlayerStateKey stateKey);
    event Action<PlayerStateKey> OnStateSet; // invoked with state key when setting a player's state
    void SetState(PlayerStateKey stateKey);
}

// Const state keys used throughout the game
// TODO: Make scalable by supporting data defined state keys
// Potentially use ScriptableObjects
// At the moment, need to hardcode state keys here and make code changes
// Because it's an enum, it also means ordering is set after being defined
public enum PlayerStateKey
{
    Unlocked_CtrlMode = 0,
}
