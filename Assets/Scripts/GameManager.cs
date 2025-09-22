using UnityEngine;

/// <summary>
/// Manages high level game loop state and delegates to other managers as needed
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Different states of the game loop
    /// Per Level State Machine
    /// TODO: Look at Game Programming Patterns book for state machine implementation
    /// </summary>
    private enum GameState
    {
        Intro,      // start of level, primarily dialogue and tutorial
        Playing,    // player has control
        Outro,      // end of level, primarily dialogue and cutscenes
        Exit,       // transitioning out of level
        Paused      // game is paused
    }
    private GameState currentState;

    private LevelData currentLevelData;

    // Reference to local scene managers
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private DialogueManager dialogueManager;

    private void Start()
    {
        Setup();
        StartGame();
    }

    /// <summary>
    /// Sets up event listeners
    /// </summary>
    private void Setup()
    {
        levelManager.OnLevelComplete += HandleLevelComplete;
        dialogueManager.OnDialogueComplete += HandleDialogueComplete;

        dialogueManager.Setup(AppManager.Instance.InputManager);
    }

    private void StartGame()
    {
        currentState = GameState.Intro;

        if (AppManager.Instance.DebugManager.DebugMode)
        {
            int levelIndex = AppManager.Instance.DebugManager.startLevelIndex;
            Debug.Log("Debug overriding start level with level index " + levelIndex);
            currentLevelData = levelManager.LoadLevel(levelIndex);
        }
        else
        {
            currentLevelData = levelManager.LoadLevel(0); // Load first level by default
            // TODO: Load last saved level
        }
        
        //dialogueManager.StartDialogue("IntroDialogue");
    }

    /// <summary>
    /// Handles completion of dialogue, such as intro and outro
    /// </summary>
    private void HandleDialogueComplete()
    {
        Debug.Log("Dialogue Complete!");
        if (currentState == GameState.Intro)
        {
            currentState = GameState.Playing;
            // Give player control
        } else if (currentState == GameState.Outro)
        {
            currentState = GameState.Exit;
            // Transition to next level or end game
        }

    }

    private void HandleLevelComplete()
    {
        Debug.Log("Level Complete!");
        currentState = GameState.Outro;
        //dialogueManager.StartDialogue("LevelCompleteDialogue");
    }
}
