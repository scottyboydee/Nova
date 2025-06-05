using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeCycleManager : MonoBehaviour
{
    // NOTE: TIGHT COUPLING! These Enum names MUST be the same as the Scene names in the project!
    public enum State
    {
        None,
        Loading,
        Splash,
        MainGame,
        GameComplete,
        Highscores,
        Max
    }
    public static LifeCycleManager Instance { get; private set; }

    private string SceneName(State state) => state.ToString();

    private State state;

    private Controls controls;
    public Controls Controls {  get { return controls; } }

    // ancillary scenes
    private State[] AncillaryScenes =
    {
        State.Splash,
        State.Highscores,
        State.GameComplete
    };

    private bool[] sceneLoaded = new bool[(int)State.Max];

    [SerializeField]
    private FadeController fadeController;

    private State nextState;

    private void Awake()
    {
        Screen.SetResolution(640, 400, false);

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
            case State.GameComplete:
                StateGameComplete();
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
        LoadAllAncillaryScenes();
    }

    private void LoadAllAncillaryScenes()
    {
        for (int i = 0; i < AncillaryScenes.Length; i++)
        {
            SceneManager.LoadScene(SceneName(AncillaryScenes[i]), LoadSceneMode.Additive);
        }
    }

    private void StateSplash()
    {
        Debug.Log("StateSplash");
        SceneUtils.SetSceneHierarchyActive(SceneName(State.Highscores), false);
        SceneUtils.SetSceneHierarchyActive(SceneName(State.Splash), true);
        fadeController.FadeToClear();
    }

    private void StateGameComplete()
    {
        Debug.Log("StateGameComplete");

        UnloadMainGame();
        SceneUtils.SetSceneHierarchyActive(SceneName(State.GameComplete), true);

        fadeController.FadeToClear();
    }

    private void StateMainGame()
    {
        Debug.Log("StateMainGame");
        SceneUtils.SetSceneHierarchyActive(SceneName(State.Splash), false);
        LoadGame();
    }

    private void StateHighscores()
    {
        Debug.Log("LifeCycleManager: StateHighscores");

        UnloadMainGame();
        SceneUtils.SetSceneHierarchyActive(SceneName(State.GameComplete), false);
        SceneUtils.SetSceneHierarchyActive(SceneName(State.Highscores), true);

        fadeController.FadeToClear();
    }

    private void UnloadMainGame()
    {
        Debug.Log("Unloading main game");
        if (SceneManager.GetSceneByName(SceneName(State.MainGame)).isLoaded)
        {
            SceneManager.UnloadSceneAsync(SceneName(State.MainGame));
        }
    }

    public void LoadGame()
    {
        Debug.Log("LifeCycleManager: LoadGame");
        SceneManager.LoadScene(SceneName(State.MainGame), LoadSceneMode.Additive);
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
            case State.GameComplete:
                SetStateAfterFadeToBlack(State.Highscores);
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

    private void CheckMainScenesAllLoaded()
    {
        for (int i = 0; i < AncillaryScenes.Length; i++)
        {
            string checkScene = SceneName(AncillaryScenes[i]);
            if (SceneManager.GetSceneByName(checkScene).isLoaded == false)
            {
                Debug.Log("CheckMainScenesAllLoaded: Haven't yet loaded: " + checkScene);
                return;
            }
        }

        Debug.Log("All ancillary scenes loaded, moving to splash.");
        SetState(State.Splash);
    }

    // called third
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("LifeCycleManager: OnSceneLoaded: " + scene.name + " mode: " + mode);

        if( scene.name == SceneName(State.Splash) )
        {
            CheckMainScenesAllLoaded();
        }
        else if (scene.name == SceneName(State.Highscores))
        {
            SceneUtils.SetSceneHierarchyActive(SceneName(State.Highscores), false);
            CheckMainScenesAllLoaded();
        }
        else if (scene.name == SceneName(State.GameComplete))
        {
            SceneUtils.SetSceneHierarchyActive(SceneName(State.GameComplete), false);
            CheckMainScenesAllLoaded();
        }
        else if( scene.name == SceneName(State.MainGame) ) 
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneName(State.MainGame)));
            fadeController.FadeToClear();
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
