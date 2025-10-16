using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BootLoader : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return SceneManager.LoadSceneAsync("GameController", LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameController"));

        GameController.Instance.GoToScene(SceneNames.MainMenu);

        yield return SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}