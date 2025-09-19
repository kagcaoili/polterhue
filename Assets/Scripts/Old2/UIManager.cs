using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Contains event button callbacks for scene UI elements
/// </summary>
public class UIManager : MonoBehaviour
{

    /// <summary>
    /// Loads the game scene where the main gameplay occurs
    /// This is not responsible for loading levels.
    /// </summary>
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// Quits the application, either in the editor or a built app
    /// </summary>
    public void QuitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
