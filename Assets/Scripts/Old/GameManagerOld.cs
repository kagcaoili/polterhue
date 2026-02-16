using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManagerOld : MonoBehaviour
{
    // Singleton to manage game state, global references, and flow
    public static GameManagerOld Instance { get; private set; }

    public LevelManagerOld levelManager {get; private set; }
    public GhostManager ghostManager { get; private set; }
    public HumanManager humanManager { get; private set; }
    public GridManager gridManager { get; private set; }

    private SceneContext _sceneContext;
    public Tilemap tileMap { get; private set; }

    // Used for deterministic random behavior such as ghost and human movement
    // For server authoriative behavior, server should provide to client
    // Allows for debugging and replays
    public int seed = 1234;
    public const int GHOST_SEED_OFFSET = 0; // Offset for ghost movement to avoid collision with human seed
    public const int HUMAN_SEED_OFFSET = 1000; // Offset for human movement to avoid collision with ghost seed

    public int initialLevelIndex = 0; // Default to first level
    private int _currentLevelIndex = 0;

    private Coroutine _gameCoroutine;

    #region Singleton
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    private void SetupManagers()
    {
        if (ghostManager == null)
        {
            ghostManager = GetComponentInChildren<GhostManager>();
            if (ghostManager == null)
            {
                Debug.LogError("GhostManager not found in scene. Please add a GhostManager component.");
            }
        }

        if (levelManager == null)
        {
            levelManager = GetComponentInChildren<LevelManagerOld>();
            if (levelManager == null)
            {
                Debug.LogError("LevelManager not found in scene. Please add a LevelManager component.");
            }
        }

        if (humanManager == null)
        {
            humanManager = GetComponentInChildren<HumanManager>();
            if (humanManager == null)
            {
                Debug.LogError("HumanManager not found in scene. Please add a HumanManager component.");
            }
        }

        if (gridManager == null)
        {
            gridManager = GetComponentInChildren<GridManager>();
            if (gridManager == null)
            {
                Debug.LogError("GridManager not found in scene. Please add a GridManager component.");
            }
        }

        // Initialize managers
        ghostManager.Setup(_sceneContext.ghostRoot.transform);
        humanManager.Setup(_sceneContext.humanRoot.transform);
        levelManager.Setup(_sceneContext.tileMapRoot.transform);
    }

    // Called by SceneBootstrapper to provide references to relevant scene objects
    public void RegisterSceneContext(SceneContext sceneContext)
    {
        _sceneContext = sceneContext;

        // Managers need scene context for setup
        SetupManagers();
    }

    #region Event Button Callbacks
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");

        _gameCoroutine = StartCoroutine(RunGame(initialLevelIndex));
    }

    // TODO: Reset game to level 0
    // TODO: Create a new function for resetting the level
    public void ResetGame(int levelIndex = 0)
    {
        // Reset level index
        _currentLevelIndex = levelIndex;

        // Clean up any existing ghosts and humans
        ghostManager?.ResetPosition();
        humanManager?.CleanUpSpawn();

        // Reload the first level
        StopCoroutine(_gameCoroutine);
        _gameCoroutine = StartCoroutine(RunGame(levelIndex));
    }

    public void StartNextLevel()
    {
        _sceneContext.gameover.Hide();
        _currentLevelIndex++;
        ResetGame(_currentLevelIndex);
    }

    // Return to main menu
    public void ReturnToMainMenu()
    {
        _currentLevelIndex = 0;

        // Clean up any existing ghosts and humans
        ghostManager?.DestroyGhost();
        humanManager?.CleanUpSpawn();

        StopCoroutine(_gameCoroutine);

        SceneManager.LoadScene("TitleScene");
    }

    #endregion

    private IEnumerator RunGame(int levelIndex)
    {
        // Wait for the scene to load
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "GameScene");

        StartLevel(levelIndex);

        // Tick game loop that is called every frame
        // In the future, each tick can be associated with list of changes or events for debugging and replayability
        // Break out of game loop when we detect game over
        // Order of Operations:
        // 1. Move ghosts one tile
        // 2. Move humans one tile
        // 3. Resolve collisions
        // 4. Spawn new ghosts
        // 5. Update soul regen and count
        bool startTick = true;
        while (CheckGameover() == false)
        {
            float deltaTime = Time.deltaTime;

            // Move ghosts and humans
            bool ghostsReady = ghostManager.Move(deltaTime, startTick);
            bool humansReady = humanManager.Move(deltaTime, startTick);

            // After first tick, set to until all ghosts and humans have arrived
            startTick = false;

            if (ghostsReady && humansReady)
            {
                // Resolve collisions checking if ghosts landed on same tile
                gridManager.ResolveCollisions(humanManager.DestroyHuman);

                // if human was collided, need to show cutscene of memory encountered

                startTick = true;
            }

            yield return null;
        }

        HandleGameover();
    }

    public void StartLevel(int levelIndex)
    {
        if (levelManager == null)
        {
            Debug.LogError("LevelManager is not initialized. Cannot start level.");
            return;
        }

        _currentLevelIndex = levelIndex;
        
        (LevelData levelData, Tilemap map) = levelManager.LoadLevel(_currentLevelIndex);
        tileMap = map;

        if (levelData == null)
        {
            Debug.LogError($"Failed to load level {_currentLevelIndex}. Cannot start level.");
            return;
        }

        ghostManager.InitializeSpawn(levelData);
        humanManager.InitializeSpawn(levelData);
    }

    // If all humans are reached, level complete
    bool CheckGameover()
    {
        return humanManager.GetActiveHumanCount() == 0;
    }

    void HandleGameover()
    {
        bool endGame = levelManager.LastLevel(_currentLevelIndex);

        if (humanManager.GetActiveHumanCount() == 0)
        {
            _sceneContext.gameover.Setup(true, endGame);
        }
    }

    void Update()
    {
        // For debugging purposes, you can add key inputs to test level transitions
        if (Input.GetKeyDown(KeyCode.N)) // Press N to go to next level
        {
            if (levelManager.LastLevel(_currentLevelIndex))
            {
                ReturnToMainMenu();
            }
            else
            {
                StartNextLevel();
            }
        }
        else if (Input.GetKeyDown(KeyCode.R)) // Press R to reset the game
        {
            ResetGame(_currentLevelIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) // Return to main menu
        {
            ReturnToMainMenu();
        }
    }

}

