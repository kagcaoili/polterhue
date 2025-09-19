using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// SceneBootstrapper is a small bootstrap class responsible for referencing canvas and camera UI elements in scene
/// </summary>
public class SceneBootstrapper : MonoBehaviour
{
    public Camera mainCamera;
    public Canvas mainCanvas;
    public GameObject ghostRoot;
    public GameObject portalRoot;
    public GameObject humanRoot;
    public GameObject tileMapRoot;
    public GameoverController gameover;

    // Register elements to GameManager
    void Start()
    {
        // Improvement: Support starting from game scene for quicker testing
        if (GameManagerOld.Instance == null)
        {
            Debug.LogError("GameManager instance is not set. Must play from title screen.");
            return;
        }

        SceneContext sceneContext = new SceneContext(mainCamera, mainCanvas, ghostRoot, portalRoot, humanRoot, tileMapRoot, gameover);
        GameManagerOld.Instance.RegisterSceneContext(sceneContext);
    }
}

public struct SceneContext
{
    public Camera mainCamera;
    public Canvas mainCanvas;
    public GameObject ghostRoot;
    public GameObject portalRoot;
    public GameObject humanRoot;
    public GameObject tileMapRoot;
    public GameoverController gameover;

    public SceneContext(Camera camera, Canvas canvas, GameObject ghost, GameObject portal, GameObject human, GameObject tilemaproot, GameoverController gameover)
    {
        mainCamera = camera;
        mainCanvas = canvas;
        ghostRoot = ghost;
        portalRoot = portal;
        humanRoot = human;
        tileMapRoot = tilemaproot;
        this.gameover = gameover;
    }
}