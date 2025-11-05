using UnityEngine;
using System.Collections;

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
    private Coroutine gameCoroutine;
    private LevelData currentLevelData; // TODO: Check for stale reference

    // Reference to local scene managers
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private FlowContext flowContext;

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
        //dialogueManager.OnDialogueComplete += HandleDialogueComplete;

        dialogueManager.Setup(AppManager.Instance.InputManager);
    }

    /// <summary>
    /// Starts the game by loading the first level and playing its intro dialogue
    /// </summary>
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

        //dialogueManager.PlayDialogue(currentLevelData.levelIntro);

        StartCoroutine(levelManager.RunFlow(currentLevelData.steps, flowContext));
    }

    private void RunGame()
    {
        currentState = GameState.Playing;

        // Check for existing game loop, could happen if player restarts level
        if (gameCoroutine != null)
        {
            StopCoroutine(gameCoroutine);
        }

        gameCoroutine = StartCoroutine(RunGameCR());
    }

    private IEnumerator RunGameCR()
    {
        // Remains in playing state until HandleLevelComplete is evoked by LevelManager 
        // LevelManager will directly StopCoroutine
        // But HandleLevelComplete transitions to Outro state which also stops this coroutine as fail safe
        while (currentState == GameState.Playing)
        {
            // Game loop logic here
            yield return null;
        }
    }

    /// <summary>
    /// Handles completion of dialogue, such as intro and outro
    /// TODO: Consider turning this into concrete states instead of enum control
    /// Temporarily disabled to focus on intro flow seq
    /// </summary>
    private void HandleDialogueComplete()
    {
        Debug.Log("Dialogue Complete!");
        if (currentState == GameState.Intro)
        {
            RunGame(); // Start main game loop after intro dialogue
        }
        else if (currentState == GameState.Outro)
        {
            currentState = GameState.Exit;
            // Transition to next level or end game
        }

    }

    private void HandleLevelComplete()
    {
        Debug.Log("Level Complete!");
        currentState = GameState.Outro;

        // Stop playing game loop CR
        if (gameCoroutine != null)
        {
            StopCoroutine(gameCoroutine);
            gameCoroutine = null;
        }
    }
}
