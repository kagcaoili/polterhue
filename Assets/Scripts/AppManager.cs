using UnityEngine;
using UnityEngine.Rendering;

// Singleton Service Locator for app-wide services
public class AppManager : MonoBehaviour
{
    public static AppManager Instance { get; private set; }

    public AudioManager AudioManager { get; private set; }
    public SaveManager SaveManager { get; private set; }
    public DebugManager DebugManager { get; private set; }
    // Add other managers here as needed

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
    }
    #endregion

    void Start()
    {
        // TODO: Load Save Data
    }
}
