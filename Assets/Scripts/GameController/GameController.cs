using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] SessionManager _sessionManager;
    public SessionManager SessionManager { get { return _sessionManager; } }

    [SerializeField] WorldGenController _worldGenController;
    public WorldGenController WorldGenController { get { return _worldGenController; } }

    string _currentScene;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator GoToSceneAsync(string sceneName)
    {
        if (_currentScene == sceneName)
            yield break;

        if (!string.IsNullOrEmpty(_currentScene))
            yield return SceneManager.UnloadSceneAsync(_currentScene);

        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        _currentScene = sceneName;
    }

    public void GoToScene(string sceneName)
    {
        StartCoroutine(GoToSceneAsync(sceneName));
    }
}

public static class SceneNames
{
    public static string MainMenu = "MainMenuScene";
    public static string Game = "GameScene";
}