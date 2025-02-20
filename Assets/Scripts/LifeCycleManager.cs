using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeCycleManager : MonoBehaviour
{
    public enum State
    {
        None,
        Loading,
        Splash,
        MainGame,
        Highscores
    }

    private State state;

    private Controls controls;
    public Controls Controls {  get { return controls; } }

    private string splashScene = "Splash";
    private string mainGameScene = "MainGame";
    private string highscoresScene = "Highscores";

    [SerializeField]
    private FadeController fadeController;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        controls = new Controls();
        controls.Gameplay.Enable();

        LoadSplash();
    }

    public void SetState( State newState )
    {
        Debug.Log("LifeCycleManager: SetState: " + newState);

        switch( newState )
        {
            case State.Loading:
                StateLoading();
                break;
            case State.Splash:
                StateSplash();
                break;
            case State.MainGame:
                StateMainGame();
                break;
            case State.Highscores:
                StateHighscores();
                break;
            case State.None:
            default:
                break;
        }

        // Note: we only move on here, to avoid bad state sets, but this only happens on COMPLETION
        state = newState;
    }
    private void StateLoading()
    {
        LoadSplash();
    }

    private void StateSplash()
    {
        Debug.Log("Splash completed loading. Waiting for input to play.");
        fadeController.FadeToClear();
    }

    private void StateMainGame()
    {
        LoadGame();
    }

    private void StateHighscores()
    {

    }

    public void LoadSplash()
    {
        Debug.Log("LifeCycleManager: LoadSplash");
        // Start by loading splash scene additively
        SceneManager.LoadScene(splashScene, LoadSceneMode.Additive);
    }

    public void LoadGame()
    {
        Debug.Log("LifeCycleManager: LoadGame");
        // Load the game scene and optionally disable the splash screen
        SceneManager.LoadScene(mainGameScene, LoadSceneMode.Additive);
    }

    public void EndGame()
    {
        Debug.Log("LifeCycleManager: EndGame");

        // Unload MainGame scene and load Highscores
        SceneManager.UnloadSceneAsync(mainGameScene);
        SceneManager.LoadScene(highscoresScene, LoadSceneMode.Additive);
    }

    public void RestartGame()
    {
        Debug.Log("LifeCycleManager: RestartGame");

        // Unload Highscores and reload MainGame
        SceneManager.UnloadSceneAsync(highscoresScene);
        LoadGame();
    }

    private void CheckFire()
    {
        if (!controls.Gameplay.Fire.WasPressedThisFrame())
            return;

//        Debug.Log("LifeCycleManager: Fire pressed!");

        switch ( state )
        {
            case State.Splash:
                SetState(State.MainGame);
                break;
            default:
 //               Debug.Log("LifeCycleManager: Nothing to do with this input!");
                break;
        }
    }

    private void Update()
    {
        CheckFire();

    }

    // called third
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("LifeCycleManager: OnSceneLoaded: " + scene.name + " mode: " + mode);

        if( scene.name == splashScene )
        {
            SetState(State.Splash);
        }
        else if( scene.name == mainGameScene ) 
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(mainGameScene));
        }
        else if( scene.name == highscoresScene ) 
        {

        }
        else
        {
            Debug.LogError("LifeCycleManager: unhandled scene loaded: " +  scene.name);
        }

    }

    // called when the game is terminated
    void OnDisable()
    {
        Debug.Log("LifeCycleManager: OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
