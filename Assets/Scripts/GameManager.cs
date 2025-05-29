using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool invulnerability = false;
    public bool Invulnerability => invulnerability;

    [SerializeField]
    private bool muteSFX = false;
    public bool MuteSFX => muteSFX;

    [SerializeField]
    private float scaleTime = 0.5f;

    [SerializeField]
    private bool useScaleTime;

    [SerializeField]
    public bool useSceneView = false;

    [SerializeField]
    private float pauseAfterPlayerDeath;

    private float playerDeadPauseRemaining = 0;

    [SerializeField]
    private Player thePlayer;
    private Vector3 playerSpawnPos;
    public Player Player { get { return thePlayer; } }

    [SerializeField]
    private LivesManager livesManager;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("EEK! GameManager Singleton already existed!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

#if UNITY_EDITOR
        if (useSceneView)
        {
            UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
        }
#endif
    }

    public void PlayerDied()
    {
        Debug.Log("GameManager: PlayerDied!");
        // TODO: move this inside the player, so it controls itself rather than GameManager doing so
        thePlayer.gameObject.SetActive(false);

        bool stillAlive = livesManager.RemoveLife();
        if(stillAlive == false)
        {
            Debug.Log("PLAYER COMPLETELY OUT OF LIVES!!");
            if(LifeCycleManager.Instance != null ) 
            {
                LifeCycleManager.Instance.SetStateAfterFadeToBlack(LifeCycleManager.State.Highscores);
                return;
            }

            Debug.Log("But we're in testing mode, so I guess we'll keep going!");
            return;
        }

        playerDeadPauseRemaining = pauseAfterPlayerDeath;
    }


    // Start is called before the first frame update
    void Start()
    {
        if( useScaleTime )
        {
            Debug.Log("GameManager: Overriding time speed to: " + scaleTime);
            Time.timeScale = scaleTime;
        }

        playerSpawnPos = thePlayer.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if( useScaleTime )
        {
            Time.timeScale = scaleTime;
        }
        else
        {
            Time.timeScale = 1.0f;
        }

        if (playerDeadPauseRemaining > 0)
        {
//            Debug.Log("Next Wave timer remain: " + playerDeadPauseRemaining);
            playerDeadPauseRemaining -= Time.deltaTime;

            if (playerDeadPauseRemaining < 0)
            {
//                Debug.Log("Next Wave timer depleted! Spawning!");
                playerDeadPauseRemaining = 0;
                ResetPlayer();
            }
        }

    }

    private void ResetPlayer()
    {
        Debug.Log("ResetPlayer");
        thePlayer.transform.position = playerSpawnPos;
        // TODO: move this inside the player, so it controls itself rather than GameManager doing so
        thePlayer.gameObject.SetActive(true);
        WaveManager.Instance.RestartCurrentWave();

        WaveManager.Instance.BulletManager.CleanUpAllBullets();
    }
}
