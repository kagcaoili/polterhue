using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    // Singleton to manage game state, global references, and flow
    public static GameManager Instance { get; private set; }

    [SerializeField]
    public LevelManager levelManager {get; private set; }
    [SerializeField]
    public GhostManager ghostManager { get; private set; }
    [SerializeField]
    public PortalManager portalManager { get; private set; }
    [SerializeField]
    public HumanManager humanManager { get; private set; }
    [SerializeField]
    public GridManager gridManager { get; private set; }
    [SerializeField]
    public SoulManager soulManager { get; private set; }

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
            levelManager = GetComponentInChildren<LevelManager>();
            if (levelManager == null)
            {
                Debug.LogError("LevelManager not found in scene. Please add a LevelManager component.");
            }
        }

        if (portalManager == null)
        {
            portalManager = GetComponentInChildren<PortalManager>();
            if (portalManager == null)
            {
                Debug.LogError("PortalManager not found in scene. Please add a PortalManager component.");
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

        if (soulManager == null)
        {
            soulManager = GetComponentInChildren<SoulManager>();
            if (soulManager == null)
            {
                Debug.LogError("SoulManager not found in scene. Please add a SoulManager component.");
            }
        }

        // Initialize managers
        ghostManager.Setup(_sceneContext.ghostRoot.transform);
        portalManager.Setup(_sceneContext.portalRoot.transform, ghostManager.QueueSpawnGhost);
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

    public void ResetGame(int levelIndex = 0)
    {
        // Reset level index
        _currentLevelIndex = levelIndex;

        // Clean up any existing ghosts and humans
        ghostManager?.CleanUpSpawn();
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
        ghostManager?.CleanUpSpawn();
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
                gridManager.ResolveCollisions(ghostManager.QueueSpawnGhost, ghostManager.DestroyGhost, humanManager.DestroyHuman);

                // Spawn new ghosts that were generated by portal or collisions
                ghostManager.SpawnGhostsQueued();

                startTick = true;
            }

            // Update soul regen and count
            soulManager.UpdateSoulData(deltaTime);
            portalManager.UpdatePortals();

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

        portalManager.InitializeSpawn(levelData);
        ghostManager.InitializeSpawn(levelData);
        humanManager.InitializeSpawn(levelData);
        soulManager.Initialize(levelData);
    }

    // If all ghosts are killed, player loses
    // If all humans are killed, player wins
    bool CheckGameover()
    {
        return ghostManager.GetActiveGhostCount() == 0 || humanManager.GetActiveHumanCount() == 0;
    }

    void HandleGameover()
    {
        bool endGame = levelManager.LastLevel(_currentLevelIndex);

        if (ghostManager.GetActiveGhostCount() == 0)
        {
            _sceneContext.gameover.Setup(false, endGame);
        }
        else if (humanManager.GetActiveHumanCount() == 0)
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

