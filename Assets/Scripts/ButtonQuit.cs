using UnityEngine;

/// <summary>
/// Quits game
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
