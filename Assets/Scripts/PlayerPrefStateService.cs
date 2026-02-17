using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefStateService : MonoBehaviour, IPlayerStateService
{
    // Event invoked when a state is set
    // Informs subscribers such as InputManager
    public event Action<PlayerStateKey> OnStateSet;

    // cached state keys, if contained, it means the player has the flag set to true
    // saved in memory so we don't have to query PlayerPrefs every time
    // e.g. 1 = completed tutorial, unlocked level, etc
    HashSet<PlayerStateKey> stateKeys = new HashSet<PlayerStateKey>();

    void Awake()
    {
        // Pre-load any persistent player states from PlayerPrefs
        // Improvement: Consider loading all keys dynamically if many states exist
        // For now, hardcode known state keys to load
        LoadState(PlayerStateKey.Unlocked_CtrlMode);

        // Note: Unlocked_IdString states are dynamic based on ids, so not pre-loaded here
        
    }

    void LoadState(PlayerStateKey stateKey)
    {
        // If state key exists and is set to 1, add to cached set
        if (PlayerPrefs.GetInt(stateKey.ToString(), 0) == 1)
        {
            stateKeys.Add(stateKey);
        }
    }

    public bool HasStateSet(PlayerStateKey stateKey)
    {
        return stateKeys.Contains(stateKey);
    }

    // Sets state with given key to true
    public void SetState(PlayerStateKey stateKey)
    {
        Debug.Log($"Setting player state: {stateKey}");

        PlayerPrefs.SetInt(stateKey.ToString(), 1);
        
        stateKeys.Add(stateKey);
        OnStateSet?.Invoke(stateKey);
    }
}