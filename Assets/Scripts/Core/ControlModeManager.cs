using UnityEngine;

public class ControlModeManager : MonoBehaviour
{
    [SerializeField] private GameObject overlay;

    // Shake when player types with Ctrl mode enabled
    [SerializeField] private Screenshake screenshake;

    private bool active; // Is Ctrl mode currently active

    void OnEnable()
    {
        // Shake when player types a letter that is found in any ControlModeText
        ControlModeSignals.OnCtrlLetterFound += HandleCtrlLetterFound;
    }
    
    void OnDisable() 
    {
        ControlModeSignals.OnCtrlLetterFound -= HandleCtrlLetterFound;
    }

    void OnDestroy() 
    {
        ControlModeSignals.OnCtrlModeToggle -= HandleCtrlModeToggle;
        ControlModeSignals.OnCtrlModeChanged -= HandleCtrlModeChanged;
    }

    void Start()
    {
        ControlModeSignals.OnCtrlModeToggle += HandleCtrlModeToggle;
        ControlModeSignals.OnCtrlModeChanged += HandleCtrlModeChanged;
        SetActive(false); // default to inactive
    }

    // Callback function for when Ctrl mode is enabled
    private void HandleCtrlModeToggle()
    {
        Debug.Log("ControlModeManager: Ctrl mode toggled to " + !active);
        SetActive(!active);
    }

    private void HandleCtrlModeChanged(bool isActive)
    {
        Debug.Log("ControlModeManager: Ctrl mode changed to " + isActive);
        SetActive(isActive);
    }

    // Helper to hide all control mode UI elements
    private void SetActive(bool isActive)
    {
        active = isActive;

        if (overlay != null)
        {
            overlay.SetActive(isActive);
        }

    }

    void Update()
    {
        if (!active) return;

        string s = Input.inputString;
        for (int i = 0; i < s.Length; i++)
        {
            char c = char.ToLower(s[i]);
            if (char.IsLetter(c))
            {
                //screenshake.TriggerShake();
                Debug.Log("ControlModeManager: Typed letter '" + c + "'");

                // updates any listeners such as the texts themselves based on letter typed
                ControlModeSignals.RaiseCtrlLetterTyped(c);
            }
        }
    }

    // Callback function for when a correct letter is typed
    private void HandleCtrlLetterFound() {
        screenshake.TriggerShake();
    }
}
