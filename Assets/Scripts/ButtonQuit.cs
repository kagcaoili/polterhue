using UnityEngine;

/// <summary>
/// Quits the application, either in the editor or a built app
/// </summary>
public class ButtonQuit : MonoBehaviour
{
    public void OnClick()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
