using System;

/// <summary>
/// Interface for player state service
/// Allows for abstraction of player state management
/// Right now, supports player pref based state tracking
/// But could be extended to support more complex systems later like cloud
/// </summary>
public interface IPlayerStateService
{
    bool HasStateSet(PlayerStateKey stateKey);
    event Action<PlayerStateKey> OnStateSet; // invoked with state key when setting a player's state
    void SetState(PlayerStateKey stateKey);
}

// Const state keys used throughout the game
public enum PlayerStateKey
{
    Unlocked_CtrlMode
}
