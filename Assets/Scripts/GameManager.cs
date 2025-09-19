using UnityEngine;

/// <summary>
/// Manages high level game loop state and delegates to other managers as needed
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Different states of the game loop
    /// </summary>
    private enum GameState
    {
        Intro,      // start of level, primarily dialogue and tutorial
        Playing,    // player has control
        Outro,      // end of level, primarily dialogue and cutscenes
        Paused      // game is paused
    }
    private GameState currentState;

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
        //dialogueManager.OnDialogueComplete += HandleIntroComplete;
    }

    private void StartGame()
    {
        currentState = GameState.Intro;
        levelManager.LoadLevel(1);
        //dialogueManager.StartDialogue("IntroDialogue");
    }

    private void HandleIntroComplete()
    {
        Debug.Log("Intro Complete!");
        currentState = GameState.Playing;
        // Give player control
    }

    private void HandleLevelComplete()
    {
        Debug.Log("Level Complete!");
        currentState = GameState.Outro;
        //dialogueManager.StartDialogue("LevelCompleteDialogue");
    }
}
