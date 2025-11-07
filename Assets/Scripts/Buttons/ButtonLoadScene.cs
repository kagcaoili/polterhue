using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Responsible for loading scenes from scene management
/// </summary>
public class ButtonLoadScene : MonoBehaviour
{
    public string sceneName;

    public void OnClick()
    {
        SceneManager.LoadScene(sceneName);
    }
}
