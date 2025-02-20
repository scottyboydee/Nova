using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeCycleManager : MonoBehaviour
{
    private string splashScene = "Splash";
    private string mainGameScene = "MainGame";
    private string highscoresScene = "Highscores";

    private void Start()
    {
        // Start by loading splash scene additively
        SceneManager.LoadScene(splashScene, LoadSceneMode.Additive);
    }

    public void StartGame()
    {
        // Load the game scene and optionally disable the splash screen
        SceneManager.LoadScene(mainGameScene, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mainGameScene));
    }

    public void EndGame()
    {
        // Unload MainGame scene and load Highscores
        SceneManager.UnloadSceneAsync(mainGameScene);
        SceneManager.LoadScene(highscoresScene, LoadSceneMode.Additive);
    }

    public void RestartGame()
    {
        // Unload Highscores and reload MainGame
        SceneManager.UnloadSceneAsync(highscoresScene);
        StartGame();
    }
}
