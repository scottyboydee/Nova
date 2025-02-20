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
    public static LifeCycleManager Instance { get; private set; }

    private State state;

    private Controls controls;
    public Controls Controls {  get { return controls; } }

    private string splashScene = "Splash";
    private string mainGameScene = "MainGame";
    private string highscoresScene = "Highscores";

    [SerializeField]
    private FadeController fadeController;

    private bool splashLoaded = false;
    private bool highscoresLoaded = false;

    private State nextState;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        // TODO: clean up the two instantiations of controls
        controls = new Controls();
        controls.Gameplay.Enable();

        SetState(State.Loading);
    }

    public void SetStateAfterFadeToBlack(State newState)
    {
        Debug.Log("SetStateAfterFades: " + newState);

        nextState = newState;

        fadeController.FadeToBlack(() => FadeToBlackCompleted());
    }

    private void FadeToBlackCompleted()
    {
        Debug.Log("FadeToBlackCompleted: state: " + state + " nextState: " + nextState);

        SetState(nextState);
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
        Debug.Log("StateLoading");
        SceneManager.LoadScene(splashScene, LoadSceneMode.Additive);
        SceneManager.LoadScene(highscoresScene, LoadSceneMode.Additive);
    }

    private void StateSplash()
    {
        Debug.Log("StateSplash");
        SceneUtils.SetSceneHierarchyActive(highscoresScene, false);
        SceneUtils.SetSceneHierarchyActive(splashScene, true);
        fadeController.FadeToClear();
    }

    private void StateMainGame()
    {
        Debug.Log("StateMainGame");
        LoadGame();
    }

    private void StateHighscores()
    {
        Debug.Log("LifeCycleManager: StateHighscores");

        // Unload MainGame scene and load Highscores
        SceneManager.UnloadSceneAsync(mainGameScene);
        //        SceneManager.LoadScene(highscoresScene, LoadSceneMode.Additive);
        SceneUtils.SetSceneHierarchyActive(highscoresScene, true);

        fadeController.FadeToClear();
    }

    public void LoadGame()
    {
        Debug.Log("LifeCycleManager: LoadGame");
        // Load the game scene and optionally disable the splash screen
        SceneManager.LoadScene(mainGameScene, LoadSceneMode.Additive);
    }

    private void CheckFire()
    {
        if (!controls.Gameplay.Fire.WasPressedThisFrame())
            return;

//        Debug.Log("LifeCycleManager: Fire pressed!");

        switch ( state )
        {
            case State.Splash:
                SetStateAfterFadeToBlack(State.MainGame);
                break;
            case State.Highscores:
                SetStateAfterFadeToBlack(State.Splash);
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
            splashLoaded = true;
            if(highscoresLoaded)
            {
                Debug.Log("Got highscores, then splash loaded, moving on to splash state");
                SetState(State.Splash);
            }
            else
            {
                Debug.Log("Got splash loaded, waiting on highscores");
            }
        }
        else if( scene.name == mainGameScene ) 
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(mainGameScene));
            fadeController.FadeToClear();
        }
        else if( scene.name == highscoresScene ) 
        {
            SceneUtils.SetSceneHierarchyActive(highscoresScene, false);
            highscoresLoaded = true;
            if (splashLoaded)
            {
                SetState(State.Splash);
            }
            else
            {
                Debug.Log("Got highscores loaded, waiting on splash");
            }
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
