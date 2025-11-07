using UnityEngine;

// Singleton Service Locator for app-wide services
// Improvement: Consider using a Dependency Injection pattern for better scalability
// It could be hard to manage dependencies as the app grows
public class AppManager : MonoBehaviour
{
    public static AppManager Instance { get; private set; }

    public AudioManager AudioManager { get; private set; }
    public SaveManager SaveManager { get; private set; }
    public DebugManager DebugManager { get; private set; }
    public InputManager InputManager { get; private set; }

    // Add other managers here as needed

    [SerializeField] private MonoBehaviour playerStateProvider;
    public IPlayerStateService PlayerState; // Abstracted player state service

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

        AudioManager = GetComponentInChildren<AudioManager>();
        SaveManager = GetComponentInChildren<SaveManager>();
        DebugManager = GetComponentInChildren<DebugManager>();
        InputManager = GetComponentInChildren<InputManager>();

        PlayerState = playerStateProvider as IPlayerStateService;
        if (PlayerState == null)
        {
            Debug.LogError("PlayerStateProvider not provided on AppManager.");
        }
    }
    #endregion

    void Start()
    {
        // TODO: Load Save Data
    }
}
