using UnityEngine;

/// <summary>
/// Handles displaying text for this action during control mode
/// Player can type the letters to interact with object
/// Highlights letters as they are typed correctly
/// When the action is complete, triggers event back to manager
/// </summary>
public class ControlModeText : MonoBehaviour
{
    [SerializeField] private GameObject rootObject; // used for enable/disable
    [SerializeField] private TMPro.TextMeshProUGUI textElement;
    [SerializeField] private Color highlight = Color.yellow;
    [SerializeField] private Color disabled = Color.gray;
    private Color original;
    
    // track typing progress
    private int currentIndex = 0; // current position in the sequence
    private string baseText; // original text without formatting

    #region Public function
    public void Setup(string text)
    {
        baseText = text.ToLower(); // Store clean lowercase text
        currentIndex = 0;
    }
    #endregion

    #region Mono Methods

    void Start()
    {
        if (textElement == null)
        {
            textElement = GetComponentInChildren<TMPro.TextMeshProUGUI>(true);
        }

        Debug.Log("ControlModeText: Start setting original color to " + textElement.color);
        original = textElement.color; // store original color
        UpdateDisplayText();
    }

    void OnEnable()
    {
        // Subscribe to control mode events
        ControlModeSignals.OnCtrlModeChanged += HandleCtrlModeChanged;
        ControlModeSignals.OnCtrlLetterTyped += HandleCtrlLetterTyped;
    }

    void OnDisable()
    {
        // Unsubscribe from control mode events
        ControlModeSignals.OnCtrlModeChanged -= HandleCtrlModeChanged;
        ControlModeSignals.OnCtrlLetterTyped -= HandleCtrlLetterTyped;
    }

    #endregion

    #region Event Handlers

    private void HandleCtrlModeChanged(bool isActive)
    {
        // Reset color and progress when disabling to "start over"
        if (!isActive)
        {
            Debug.Log("ControlModeText: Ctrl mode disabled, resetting text. Color: " + original);
            textElement.color = original;
            currentIndex = 0;
            UpdateDisplayText();
        }
    }

    // Assumes letter is lowercase
    // Highlights letter if it matches the text
    // e.g. if actionText is "jump" and player types 'j', highlights 'j'
    // if player then types 'u', highlights 'u'. thus 'ju' is highlighted, but 'mp' is not
    // if the player had typed 'x' instead of 'u', the text would turn gray
    private void HandleCtrlLetterTyped(char letter)
    {

        // check if we're expecting this letter at the current position
        if (currentIndex < baseText.Length && baseText[currentIndex] == letter)
        {
            // correct letter, advance to next position
            currentIndex++;
            ControlModeSignals.RaiseCtrlLetterFound();
            UpdateDisplayText();

            // check if word is complete
            if (currentIndex >= baseText.Length)
            {
                Debug.Log($"ControlModeText: Word '{baseText}' completed!");
                // TODO: Trigger action completion event
            }
        }
        else
        {
            // wrong letter or letter not found, disable text
            textElement.color = disabled;
            Debug.Log($"ControlModeText: Wrong letter '{letter}' at position {currentIndex}. Expected '{(currentIndex < baseText.Length ? baseText[currentIndex] : "none")}'");
        }
    }

    #endregion

    #region Private helpers

    // Updates the text display with current highlighting progress
    private void UpdateDisplayText()
    {
        Debug.Log("ControlModeText: Color: " + textElement.color);
        if (currentIndex == 0)
        {
            // No letters typed yet, show normal text
            textElement.text = baseText;
            textElement.color = original;
        }
        else if (currentIndex >= baseText.Length)
        {
            // All letters typed correctly
            string colorHex = ColorUtility.ToHtmlStringRGBA(highlight);
            textElement.text = $"<color=#{colorHex}>{baseText}</color>";
        }
        else
        {
            // Partial highlighting
            string highlightedPart = baseText.Substring(0, currentIndex);
            string normalPart = baseText.Substring(currentIndex);
            string colorHex = ColorUtility.ToHtmlStringRGBA(highlight);
            textElement.text = $"<color=#{colorHex}>{highlightedPart}</color>{normalPart}";
        }
    }
    #endregion
}